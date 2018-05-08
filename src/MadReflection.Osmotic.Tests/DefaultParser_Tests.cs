using System;
using NUnit.Framework;

namespace MadReflection.Osmotic.Tests
{
	[TestFixture]
	public class DefaultParser_Tests
	{
		private static readonly IParserContainer DefaultParser = ParserContainer.Default;


		[TestCase(null)]
		[TestCase("")]
		[TestCase("anything")]
		public void Parse_String_Is_Idempotent(string input)
		{
			// Arrange

			// Act
			string result = DefaultParser.For<string>().Parse(input);

			// Assert
			Assert.That(result, Is.EqualTo(input));
		}

		[TestCase("1", 1)]
		public void Parse_Int32_Succeeds_On_Valid_Inputs(string input, int expected)
		{
			// Arrange

			// Act
			int result = DefaultParser.For<int>().Parse(input);

			// Assert
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase(null, typeof(ArgumentNullException))]
		[TestCase("", typeof(FormatException))]
		public void Parse_Int32_Fails_On_Invalid_Inputs(string input, Type expectedExceptionType)
		{
			// Arrange

			// Act
			TestDelegate test = () => DefaultParser.For<int>().Parse(input);

			// Assert
			Assert.That(test, Throws.TypeOf(expectedExceptionType));
		}

		[TestCase("1", 1)]
		public void Parse_NullableInt32_Succeeds_On_Valid_Inputs(string input, int? expected)
		{
			// Arrange

			// Act
			int? result = DefaultParser.For<int?>().Parse(input);

			// Assert
			Assert.That(result, Is.EqualTo(result));
		}

		[TestCase(null, typeof(ArgumentNullException))]
		[TestCase("", typeof(FormatException))]
		public void Parse_NullableInt32_Fails_On_Invalid_Inputs(string input, Type exceptionExceptionType)
		{
			// Arrange

			// Act
			TestDelegate test = () => DefaultParser.For<int?>().Parse(input);

			// Assert
			Assert.That(test, Throws.TypeOf(exceptionExceptionType));
		}

		[TestCase("Second", TestEnum.Second)]
		public void Parse_TestEnum_Succeeds_On_Valid_Inputs(string input, TestEnum expected)
		{
			// Arrange

			// Act
			TestEnum result = DefaultParser.For<TestEnum>().Parse(input);

			// Assert
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase(null, typeof(ArgumentNullException))]
		[TestCase("", typeof(ArgumentException))]
		public void Parse_TestEnum_Fails_On_Invalid_Inputs(string input, Type expectedExceptionType)
		{
			// Arrange

			// Act
			TestDelegate test = () => DefaultParser.For<TestEnum>().Parse(input);

			// Assert
			Assert.That(test, Throws.TypeOf(expectedExceptionType));
		}

		[TestCase("Third", TestEnum.Third)]
		public void Parse_NullableTestEnum_Succeeds_On_Valid_Inputs(string input, TestEnum? expected)
		{
			// Arrange

			// Act
			TestEnum? result = DefaultParser.For<TestEnum?>().Parse(input);

			// Assert
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase(null, typeof(ArgumentNullException))]
		[TestCase("", typeof(ArgumentException))]
		public void Parse_NullableTestEnum_Fails_On_Invalid_Inputs(string input, Type expectedExceptionType)
		{
			// Arrange

			// Act
			TestDelegate test = () => DefaultParser.For<TestEnum?>().Parse(input);

			// Assert
			Assert.That(test, Throws.TypeOf(expectedExceptionType));
		}

		[TestCase("1.2.3")]
		public void Parse_Version_Succeeds_On_Valid_Inputs(string input)
		{
			// Arrange

			// Act
			Version result = DefaultParser.For<Version>().Parse(input);

			// Assert
			Assert.That(result, Is.Not.Null);
		}

		[TestCase(null, typeof(ArgumentNullException))]
		[TestCase("", typeof(ArgumentException))]
		[TestCase("0.x.0.0", typeof(FormatException))]
		public void Parse_Version_Fails_On_Invalid_Inputs(string input, Type expectedExceptionType)
		{
			// Arrange

			// Act
			TestDelegate test = () => DefaultParser.For<Version>().Parse(input);

			// Assert
			Assert.That(test, Throws.TypeOf(expectedExceptionType));
		}

		[TestCase("anything")]
		public void Parse_TestClassWithoutParse_Throws_NotSupportedException(string input)
		{
			// Arrange

			// Act
			TestDelegate test = () => DefaultParser.For<TestClassWithoutParse>().Parse(input);

			// Assert
			Assert.That(test, Throws.TypeOf<NotSupportedException>());
		}

		[TestCase("anything")]
		public void Parse_TestClassWithoutTryParse_Succeeds_On_Valid_Inputs(string input)
		{
			// Arrange

			// Act
			TestClassWithoutTryParse result = DefaultParser.For<TestClassWithoutTryParse>().Parse(input);

			// Assert
			Assert.That(result, Is.Not.Null);
		}

		[TestCase(null, typeof(ArgumentNullException))]
		[TestCase("", typeof(ArgumentException))]
		public void Parse_TestClassWithoutTryParse_Throws_On_Invalid_Inputs(string input, Type expectedExceptionType)
		{
			// Arrange

			// Act
			TestDelegate test = () => DefaultParser.For<TestClassWithoutTryParse>().Parse(input);

			// Assert
			Assert.That(test, Throws.TypeOf(expectedExceptionType));
		}

		[TestCase(null)]
		[TestCase("")]
		[TestCase("anything")]
		public void TryParse_String_Is_Idempotent(string input)
		{
			// Arrange

			// Act
			bool result = DefaultParser.For<string>().TryParse(input, out string output);

			// Assert
			Assert.That(result, Is.True);
			Assert.That(output, Is.EqualTo(input));
		}

		[Test(Description = "")]
		[TestCase("1", 1)]
		public void TryParse_Int32_Succeeds_On_Valid_Inputs(string input, int expected)
		{
			// Arrange

			// Act
			bool result = DefaultParser.For<int>().TryParse(input, out int output);

			// Assert
			Assert.That(result, Is.True);
			Assert.That(output, Is.EqualTo(expected));
		}

		[Test(Description = "")]
		[TestCase(null)]
		[TestCase("")]
		public void TryParse_Int32_Fails_On_Invalid_Inputs(string input)
		{
			// Arrange

			// Act
			bool result = DefaultParser.For<int>().TryParse(input, out int output);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test(Description = "")]
		[TestCase("1", 1)]
		public void TryParse_NullableInt32_Succeeds_On_Valid_Inputs(string input, int? expected)
		{
			// Arrange

			// Act
			bool result = DefaultParser.For<int?>().TryParse(input, out int? output);

			// Assert
			Assert.That(result, Is.True);
			Assert.That(output, Is.EqualTo(expected));
		}

		[Test(Description = "")]
		[TestCase(null)]
		[TestCase("")]
		public void TryParse_NullableInt32_Fails_On_Invalid_Inputs(string input)
		{
			// Arrange

			// Act
			bool result = DefaultParser.For<int?>().TryParse(input, out int? output);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test(Description = "")]
		[TestCase("Fourth", TestEnum.Fourth)]
		public void TryParse_TestEnum_Succeeds_On_Valid_Inputs(string input, TestEnum expected)
		{
			// Arrange

			// Act
			bool result = DefaultParser.For<TestEnum>().TryParse(input, out TestEnum output);

			// Assert
			Assert.That(result, Is.True);
			Assert.That(output, Is.EqualTo(expected));
		}

		[Test(Description = "")]
		[TestCase(null)]
		[TestCase("")]
		public void TryParse_TestEnum_Fails_On_Invalid_Inputs(string input)
		{
			// Arrange

			// Act
			bool result = DefaultParser.For<TestEnum>().TryParse(input, out TestEnum output);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test(Description = "")]
		[TestCase("Fifth", TestEnum.Fifth)]
		public void TryParse_NullableTestEnum_Succeeds_On_Valid_Inputs(string input, TestEnum? expected)
		{
			// Arrange

			// Act
			bool result = DefaultParser.For<TestEnum?>().TryParse(input, out TestEnum? output);

			// Assert
			Assert.That(result, Is.True);
			Assert.That(output, Is.EqualTo(expected));
		}

		[Test(Description = "")]
		[TestCase(null)]
		[TestCase("")]
		public void TryParse_NullableTestEnum_Fails_On_Invalid_Inputs(string input)
		{
			// Arrange

			// Act
			bool result = DefaultParser.For<TestEnum?>().TryParse(input, out TestEnum? output);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test(Description = "")]
		[TestCase("1.2.3")]
		public void TryParse_Version_Succeeds_On_Valid_Inputs(string input)
		{
			// Arrange

			// Act
			bool result = DefaultParser.For<Version>().TryParse(input, out Version output);

			// Assert
			Assert.That(result, Is.True);
			Assert.That(output, Is.Not.Null);
		}

		[Test(Description = "")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase("0.x.0.0")]
		public void TryParse_Version_Fails_On_Invalid_Inputs(string input)
		{
			// Arrange

			// Act
			bool result = DefaultParser.For<Version>().TryParse(input, out Version output);

			// Assert
			Assert.That(result, Is.False);
		}

		[TestCase("anything")]
		public void TryParse_TestClassWithoutTryParse_Throws_NotSupportedException(string input)
		{
			// Arrange

			// Act
			TestDelegate test = () => DefaultParser.For<TestClassWithoutTryParse>().TryParse(input, out TestClassWithoutTryParse output);

			// Assert
			Assert.That(test, Throws.TypeOf<NotSupportedException>());
		}
	}
}
