using System;
using Moq;
using NUnit.Framework;

namespace MadReflection.Osmotic.Tests
{
	[TestFixture]
	public class ConfiguredFormatter_Tests
	{
		[TestCase]
		public void MissingFormatSpecific_ThrowNotSupportedException_Option()
		{
			// Arrange
			FormatterContainer formatter = FormatterContainer.Create(config =>
			{
				config.MissingFormatSpecific = MissingFormatSpecificHandling.ThrowNotSupportedException;
			});
			var input = new TestClassWithoutFormatSpecific("anything");

			// Act
			TestDelegate test = () => formatter.For<TestClassWithoutFormatSpecific>().Format(input, null);

			// Assert
			Assert.That(test, Throws.TypeOf<NotSupportedException>());
		}

		[TestCase]
		public void MissingFormatSpecific_ReturnEmptyString_Option()
		{
			// Arrange
			FormatterContainer formatter = FormatterContainer.Create(config =>
			{
				config.MissingFormatSpecific = MissingFormatSpecificHandling.ReturnEmptyString;
			});
			var input = new TestClassWithoutFormatSpecific("anything");

			// Act
			string result = formatter.For<TestClassWithoutFormatSpecific>().Format(input, null);

			// Assert
			Assert.That(result, Is.EqualTo(""));
		}

		[TestCase]
		public void MissingFormatSpecific_ReturnNull_Option()
		{
			// Arrange
			FormatterContainer formatter = FormatterContainer.Create(config =>
			{
				config.MissingFormatSpecific = MissingFormatSpecificHandling.ReturnNull;
			});
			var input = new TestClassWithoutFormatSpecific("anything");

			// Act
			string result = formatter.For<TestClassWithoutFormatSpecific>().Format(input, null);

			// Assert
			Assert.That(result, Is.Null);
		}

		[TestCase]
		public void MissingFormatSpecific_UseToString_Option()
		{
			// Arrange
			FormatterContainer formatter = FormatterContainer.Create(config =>
			{
				config.MissingFormatSpecific = MissingFormatSpecificHandling.UseToString;
			});
			var input = new TestClassWithoutFormatSpecific("anything");

			// Act
			string result = formatter.For<TestClassWithoutFormatSpecific>().Format(input, null);

			// Assert
			Assert.That(result, Is.EqualTo("anything"));
		}

		[TestCase]
		public void ReferenceTypesFormatNullToNull_Option()
		{
			// Arrange
			FormatterContainer formatter = FormatterContainer.Create(config =>
			{
				config.ReferenceTypesFormatNullToNull = true;
			});

			// Act
			string result = formatter.For<TestClass>().Format(null);

			// Assert
			Assert.That(result, Is.Null);
		}

		[TestCase]
		public void NullableValueTypesFormatNull_ThrowArgumentNullException_Option()
		{
			// Arrange
			FormatterContainer formatter = FormatterContainer.Create(config =>
			{
				config.NullableValueTypesFormatNull = NullFormatHandling.ThrowArgumentNullException;
			});

			// Act
			TestDelegate test = () => formatter.For<int?>().Format(null);

			// Assert
			Assert.That(test, Throws.TypeOf<ArgumentNullException>());
		}

		[TestCase]
		public void NullableValueTypesFormatNull_ReturnNull_Option()
		{
			// Arrange
			FormatterContainer formatter = FormatterContainer.Create(config =>
			{
				config.NullableValueTypesFormatNull = NullFormatHandling.ReturnNull;
			});

			// Act
			string result = formatter.For<int?>().Format(null);

			// Assert
			Assert.That(result, Is.Null);
		}

		[TestCase]
		public void NullableValueTypesFormatNull_ReturnEmptyString_Option()
		{
			// Arrange
			FormatterContainer formatter = FormatterContainer.Create(config =>
			{
				config.NullableValueTypesFormatNull = NullFormatHandling.ReturnEmptyString;
			});

			// Act
			string result = formatter.For<int?>().Format(null);

			// Assert
			Assert.That(result, Is.EqualTo(""));
		}

		[TestCase("hello?", "HELLO?")]
		public void Format_String_Func_Can_Replace_Default(string input, string expected)
		{
			// Arrange
			FormatterContainer formatter = FormatterContainer.Create(config =>
			{
				config.UseFunc<string>(s => s.ToUpper());
			});

			// Act
			string result = formatter.For<string>().Format("hello?");

			// Assert
			Assert.That(result, Is.EqualTo("HELLO?"));
		}

		[TestCase("hello?", "HELLO?")]
		public void FormatSpecific_String_Func_Can_Replace_Default(string input, string expected)
		{
			// Arrange
			FormatterContainer formatter = FormatterContainer.Create(config =>
			{
				config.UseFunc<string>(s => s.ToUpper(), (s, f) => s.ToUpper());
			});

			// Act
			string result = formatter.For<string>().Format("hello?");

			// Assert
			Assert.That(result, Is.EqualTo("HELLO?"));
		}

		[TestCase("hello?", "HELLO?")]
		public void Format_FormatSpecific_String_FormatterObject_Can_Replace_Default(string input, string expected)
		{
			// Arrange
			Mock<IFormatter<string>> stringFormatterMock = new Mock<IFormatter<string>>();

			stringFormatterMock.Setup(m => m.Format(It.IsAny<string>()))
				.Returns(new FormatReturns<string>(s => s.ToUpper()));

			stringFormatterMock.Setup(m => m.Format(It.IsAny<string>(), It.IsAny<string>()))
				.Returns(new FormatSpecificReturns<string>((s, f) => s.ToUpper()));

			FormatterContainer formatter = FormatterContainer.Create(config =>
			{
				config.UseFormatterObject(stringFormatterMock.Object);
			});

			// Act
			string result1 = formatter.For<string>().Format(input);
			string result2 = formatter.For<string>().Format(input, null);

			// Assert
			Assert.That(result1, Is.EqualTo(expected));
			Assert.That(result2, Is.EqualTo(expected));
		}

		[TestCase("anything")]
		public void Format_Uses_Configured_Type_Converter(string input)
		{
			// Arrange
			FormatterContainer formatter = FormatterContainer.Create(config =>
			{
				config.UseTypeConverter<TestClassWithTypeConverter>();
			});
			TestClassWithTypeConverter obj = new TestClassWithTypeConverter(input);

			// Act
			string result = formatter.For<TestClassWithTypeConverter>().Format(obj);

			// Assert
			Assert.That(result, Is.EqualTo(input));
		}
	}
}
