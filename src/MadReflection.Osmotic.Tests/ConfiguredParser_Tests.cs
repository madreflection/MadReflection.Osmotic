using System;
using Moq;
using NUnit.Framework;

namespace MadReflection.Osmotic.Tests
{
	[TestFixture]
	public class ConfiguredParser_Tests
	{
		[TestCase]
		public void MissingTryParse_ReturnFalse_Option()
		{
			// Arrange
			ParserContainer parser = ParserContainer.Create(config =>
			{
				config.MissingTryParse = MissingTryParseHandling.ReturnFalse;
			});

			// Act
			bool result = parser.For<TestClassWithoutTryParse>().TryParse("anything", out var disregard);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test(Description = "")]
		[TestCase]
		public void MissingTryParse_WrapParseInTryCatch_Option()
		{
			// Arrange
			ParserContainer parser = ParserContainer.Create(config =>
			{
				config.MissingTryParse = MissingTryParseHandling.WrapParseInTryCatch;
			});

			// Act
			bool result = parser.For<TestClassWithoutTryParse>().TryParse("anything", out var output);

			// Assert
			Assert.That(result, Is.True);
			Assert.That(output.Value, Is.EqualTo("anything"));
		}

		[Test(Description = "")]
		[TestCase]
		public void ReferenceTypesParseNullToNull_Option()
		{
			// Arrange
			ParserContainer parser = ParserContainer.Create(config =>
			{
				config.ReferenceTypesParseNullToNull = true;
			});

			// Act
			Version result = parser.For<Version>().Parse(null);

			// Assert
			Assert.That(result, Is.Null);
		}

		[Test(Description = "")]
		[TestCase]
		public void NullableValueTypesParseNullToNull_Option()
		{
			// Arrange
			ParserContainer parser = ParserContainer.Create(config =>
			{
				config.NullableValueTypesParseNullToNull = true;
			});

			// Act
			int? result = parser.For<int?>().Parse(null);

			// Assert
			Assert.That(result, Is.Null);
		}

		[Test(Description = "")]
		[TestCase]
		public void NullableValueTypesParseEmptyStringToNull_Option()
		{
			// Arrange
			ParserContainer parser = ParserContainer.Create(config =>
			{
				config.NullableValueTypesParseEmptyStringToNull = true;
			});

			// Act
			int? result = parser.For<int?>().Parse("");

			// Assert
			Assert.That(result, Is.Null);
		}

		[TestCase]
		public void Uri_ParseFunc_And_TryParseFunc_Proxy_Constructor_And_TryCreate()
		{
			// Arrange
			ParserContainer parser = ParserContainer.Create(config =>
			{
				// Show that this option can be applied on top of custom delegates.
				config.ReferenceTypesParseNullToNull = true;

				// Proxy Uri::.ctor and Uri::TryCreate and use UriKind.RelativeOrAbsolute with both.
				config.UseFunc(
					s => new Uri(s, UriKind.RelativeOrAbsolute),
					(string s, out Uri result) =>
					{
						return Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out result);
					});
			});
			IParser<Uri> uriParser = parser.For<Uri>();

			// Act
			Uri uri1 = uriParser.Parse("https://www.google.com/");
			Uri uri2 = uriParser.Parse(null);
			bool result1 = uriParser.TryParse("https://www.google.com/", out Uri output1);
			bool result2 = uriParser.TryParse(null, out Uri output2);
			bool result3 = uriParser.TryParse("http://", out Uri output3);

			// Assert
			Assert.That(uri1, Is.Not.Null);
			Assert.That(uri2, Is.Null);
			Assert.That(result1, Is.True);
			Assert.That(result2, Is.True);
			Assert.That(result3, Is.False);
		}

		[TestCase]
		public void Parse_String_Func_Can_Replace_Default()
		{
			// Arrange
			ParserContainer parser = ParserContainer.Create(config =>
			{
				config.UseFunc(s => s.ToLower());
			});

			// Act
			string result = parser.For<string>().Parse("Hello!");

			// Assert
			Assert.That(result, Is.EqualTo("hello!"));
		}

		[TestCase]
		public void TryParse_String_Func_Can_Replace_Default()
		{
			// Arrange
			ParserContainer parser = ParserContainer.Create(config =>
			{
				config.UseFunc(
					s => s.ToLower(),
					(string s, out string r) =>
					{
						r = s.ToLower();
						return true;
					});
			});

			// Act
			bool result = parser.For<string>().TryParse("Hello!", out string output);

			// Assert
			Assert.That(result, Is.True);
			Assert.That(output, Is.EqualTo("hello!"));
		}

		[TestCase("asdf", "asdf...", false)]
		[TestCase("asdf", "asdf...", true)]
		public void Parse_TryParse_String_ParserObject_Can_Replace_Default(string input, string expectedParseResult, bool expectedTryParseResult)
		{
			// Arrange
			Mock<IParser<string>> stringParserMock = new Mock<IParser<string>>();

			stringParserMock.Setup(m => m.Parse(It.IsAny<string>()))
				.Returns(new ParseReturns<string>(s => s + "..."));

			stringParserMock.Setup(m => m.TryParse(It.IsAny<string>(), out It.Ref<string>.IsAny))
				.Callback(new TryParseCallback<string>((string s, out string result) => result = s + "..."))
				.Returns(new TryParseReturns<string>((string s, ref string result) => expectedTryParseResult));

			ParserContainer parser = ParserContainer.Create(config =>
			{
				config.UseParserObject(stringParserMock.Object);
			});

			// Act
			string parseResult = parser.For<string>().Parse(input);
			bool tryParseResult = parser.For<string>().TryParse(input, out string tryParseOutput);

			// Assert
			Assert.That(parseResult, Is.EqualTo(expectedParseResult));
			Assert.That(tryParseResult, Is.EqualTo(expectedTryParseResult));
			Assert.That(tryParseOutput, Is.EqualTo(expectedParseResult));
		}

		[TestCase("anything")]
		public void Parse_Uses_Configured_Type_Converter(string input)
		{
			// Arrange
			ParserContainer parser = ParserContainer.Create(config =>
			{
				config.UseTypeConverter<TestClassWithTypeConverter>();
			});

			// Act
			TestClassWithTypeConverter obj = parser.For<TestClassWithTypeConverter>().Parse(input);

			// Assert
			Assert.That(obj.Value, Is.EqualTo(input));
		}
	}
}
