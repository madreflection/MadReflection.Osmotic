using System;
using NUnit.Framework;

namespace MadReflection.Osmotic.Tests
{
	[TestFixture]
	public class DefaultFormatter_Tests
	{
		private static readonly FormatterContainer DefaultFormatter = FormatterContainer.Default;


		[TestCase(null)]
		[TestCase("")]
		[TestCase("anything")]
		public void Format_String_Is_Idempotent(string input)
		{
			// Arrange

			// Act
			string result = DefaultFormatter.For<string>().Format(input);

			// Assert
			Assert.That(result, Is.EqualTo(input));
		}

		[TestCase(1, "1")]
		public void Format_Int32_Succeeds_On_Valid_Inputs(int input, string expected)
		{
			// Arrange

			// Act
			string result = DefaultFormatter.For<int>().Format(input);

			// Assert
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase(null, typeof(ArgumentNullException))]
		public void Format_Int32_Fails_On_Invalid_Inputs(object input, Type expectedExceptionType)
		{
			// Arrange

			// Act
			TestDelegate test = () => DefaultFormatter.For<int>().Format(input);

			// Assert
			Assert.That(test, Throws.TypeOf(expectedExceptionType));
		}

		[TestCase(1, "1")]
		public void Format_NullableInt32_Succeeds_On_Valid_Inputs(int? input, string expected)
		{
			// Arrange

			// Act
			string result = DefaultFormatter.For<int?>().Format(input);

			// Assert
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase(null, typeof(ArgumentNullException))]
		public void Format_NullableInt32_Fails_On_Invalid_Inputs(int? input, Type expectedExceptionType)
		{
			// Arrange

			// Act
			TestDelegate test = () => DefaultFormatter.For<int?>().Format(input);

			// Assert
			Assert.That(test, Throws.TypeOf(expectedExceptionType));
		}

		[TestCase(TestEnum.Fourth, "Fourth")]
		[TestCase((TestEnum)100, "100")]
		public void Format_TestEnum_Succeeds_On_Valid_Inputs(TestEnum input, string expected)
		{
			// Arrange

			// Act
			string result = DefaultFormatter.For<TestEnum>().Format(input);

			// Assert
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase(null, typeof(ArgumentNullException))]
		public void Format_TestEnum_Fails_On_Invalid_Inputs(object input, Type expectedExceptionType)
		{
			// Arrange

			// Act
			TestDelegate test = () => DefaultFormatter.For<TestEnum>().Format(input);

			// Assert
			Assert.That(test, Throws.TypeOf(expectedExceptionType));
		}

		[TestCase(TestEnum.Fourth, "Fourth")]
		public void Format_NullableTestEnum_Succeeds_On_Valid_Inputs(TestEnum input, string expected)
		{
			// Arrange

			// Act
			string result = DefaultFormatter.For<TestEnum?>().Format(input);

			// Assert
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase(null, typeof(ArgumentNullException))]
		public void Format_NullableTestEnum_Fails_On_Invalid_Inputs(object input, Type expectedExceptionType)
		{
			// Arrange

			// Act
			TestDelegate test = () => DefaultFormatter.For<TestEnum?>().Format(input);

			// Assert
			Assert.That(test, Throws.TypeOf(expectedExceptionType));
		}

		[TestCase(3, 1, 9, "3.1.9")]
		public void Format_Version_Succeeds_On_Valid_Inputs(int inputMajor, int inputMinor, int inputBuild, string expected)
		{
			// Arrange
			Version input = new Version(inputMajor, inputMinor, inputBuild);

			// Act
			string result = DefaultFormatter.For<Version>().Format(input);

			// Assert
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase(null, typeof(ArgumentNullException))]
		public void Format_Version_Fails_On_Invalid_Inputs(Version input, Type expectedExceptionType)
		{
			// Arrange

			// Act
			TestDelegate test = () => DefaultFormatter.For<Version>().Format(input);

			// Assert
			Assert.That(test, Throws.TypeOf(expectedExceptionType));
		}

		[TestCase("anything")]
		public void Format_TestClassWithoutTryParse(string input)
		{
			// Arrange
			TestClassWithoutTryParse obj = new TestClassWithoutTryParse(input);

			// Act
			string result = DefaultFormatter.For<TestClassWithoutTryParse>().Format(obj);

			// Assert
			Assert.That(result, Is.EqualTo(input));
		}

		[TestCase("anything", "blah")]
		public void FormatSpecific_String_Is_Idempotent(string input, string format)
		{
			// Arrange

			// Act
			string result = DefaultFormatter.For<string>().Format(input, format);

			// Assert
			Assert.That(result, Is.EqualTo(input));
		}

		[TestCase(1, "00", "01")]
		public void FormatSpecific_Int32_Succeeds_On_Valid_Inputs(int input, string format, string expected)
		{
			// Arrange

			// Act
			string result = DefaultFormatter.For<int>().Format(input, format);

			// Assert
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase(1, "00", "01")]
		public void FormatSpecific_NullableInt32_Succeeds_On_Valid_Inputs(int? input, string format, string expected)
		{
			// Arrange

			// Act
			string result = DefaultFormatter.For<int?>().Format(input, format);

			// Assert
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase(null, "00", typeof(ArgumentNullException))]
		public void FormatSpecific_NullableInt32_Fails_On_Invalid_Inputs(int? input, string format, Type expectedExceptionType)
		{
			// Arrange

			// Act
			TestDelegate test = () => DefaultFormatter.For<int?>().Format(input, format);

			// Assert
			Assert.That(test, Throws.TypeOf(expectedExceptionType));
		}

		[TestCase(TestEnum.Fifth, "D", "5")]
		public void FormatSpecific_TestEnum_Succeeds_On_Valid_Inputs(TestEnum input, string format, string expected)
		{
			// Arrange

			// Act
			string result = DefaultFormatter.For<TestEnum>().Format(input, format);

			// Assert
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase(null, "D", typeof(ArgumentNullException))]
		public void FormatSpecific_TestEnum_Succeeds_On_Valid_Inputs(object input, string format, Type expectedExceptionType)
		{
			// Arrange

			// Act
			TestDelegate test = () => DefaultFormatter.For<TestEnum>().Format(input, format);

			// Assert
			Assert.That(test, Throws.TypeOf(expectedExceptionType));
		}

		[TestCase(TestEnum.Fifth, "D", "5")]
		public void FormatSpecific_NullableTestEnum_Succeeds_On_Valid_Inputs(TestEnum? input, string format, string expected)
		{
			// Arrange

			// Act
			string result = DefaultFormatter.For<TestEnum?>().Format(input, format);

			// Assert
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase(null, typeof(ArgumentNullException))]
		public void FormatSpecific_NullableTestEnum_Fails_On_Invalid_Inputs(TestEnum? input, Type expectedExceptionType)
		{
			// Arrange

			// Act
			TestDelegate test = () => DefaultFormatter.For<TestEnum?>().Format(input);

			// Assert
			Assert.That(test, Throws.TypeOf(expectedExceptionType));
		}


		// The following tests are designed to test the ability to bind and call the ToString methods
		// with all the possible permutations.

		[TestCase]
		public void Format_ReferenceType_WithExplicitIFormattable()
		{
			// Arrange
			TestClassWithExplicitIFormattable input = new TestClassWithExplicitIFormattable("...");

			// Act
			string result1 = DefaultFormatter.For<TestClassWithExplicitIFormattable>().Format(input);
			string result2 = DefaultFormatter.For<TestClassWithExplicitIFormattable>().Format(input, null);

			// Assert
			Assert.That(result1, Is.EqualTo("..."));
			Assert.That(result2, Is.EqualTo("..."));
		}

		[TestCase]
		public void Format_ReferenceType_WithImplicitIFormattable()
		{
			// Arrange
			TestClassWithImplicitIFormattable input = new TestClassWithImplicitIFormattable("...");

			// Act
			string result1 = DefaultFormatter.For<TestClassWithImplicitIFormattable>().Format(input);
			string result2 = DefaultFormatter.For<TestClassWithImplicitIFormattable>().Format(input, null);

			// Assert
			Assert.That(result1, Is.EqualTo("..."));
			Assert.That(result2, Is.EqualTo("..."));
		}

		[TestCase]
		public void Format_ReferenceType_WithSolitaryToStringFormatAndProvider()
		{
			// Arrange
			TestClassWithSolitaryToStringFormatAndProvider input = new TestClassWithSolitaryToStringFormatAndProvider("...");

			// Act
			string result1 = DefaultFormatter.For<TestClassWithSolitaryToStringFormatAndProvider>().Format(input);
			string result2 = DefaultFormatter.For<TestClassWithSolitaryToStringFormatAndProvider>().Format(input, null);

			// Assert
			Assert.That(result1, Is.EqualTo("..."));
			Assert.That(result2, Is.EqualTo("..."));
		}

		[TestCase]
		public void Format_ReferenceType_WithSolitaryToStringFormat()
		{
			// Arrange
			TestClassWithSolitaryToStringFormat input = new TestClassWithSolitaryToStringFormat("...");

			// Act
			string result1 = DefaultFormatter.For<TestClassWithSolitaryToStringFormat>().Format(input);
			string result2 = DefaultFormatter.For<TestClassWithSolitaryToStringFormat>().Format(input, null);

			// Assert
			Assert.That(result1, Is.EqualTo("..."));
			Assert.That(result2, Is.EqualTo("..."));
		}

		[TestCase]
		public void Format_ValueType_WithExplicitIFormattable()
		{
			// Arrange
			TestStructWithExplicitIFormattable input = new TestStructWithExplicitIFormattable("...");

			// Act
			string result1 = DefaultFormatter.For<TestStructWithExplicitIFormattable>().Format(input);
			string result2 = DefaultFormatter.For<TestStructWithExplicitIFormattable>().Format(input, null);

			// Assert
			Assert.That(result1, Is.EqualTo("..."));
			Assert.That(result2, Is.EqualTo("..."));
		}

		[TestCase]
		public void Format_ValueType_WithImplicitIFormattable()
		{
			// Arrange
			TestStructWithImplicitIFormattable input = new TestStructWithImplicitIFormattable("...");

			// Act
			string result1 = DefaultFormatter.For<TestStructWithImplicitIFormattable>().Format(input);
			string result2 = DefaultFormatter.For<TestStructWithImplicitIFormattable>().Format(input, null);

			// Assert
			Assert.That(result1, Is.EqualTo("..."));
			Assert.That(result2, Is.EqualTo("..."));
		}

		[TestCase]
		public void Format_ValueType_WithSolitaryToStringFormatAndProvider()
		{
			// Arrange
			TestStructWithSolitaryToStringFormatAndProvider input = new TestStructWithSolitaryToStringFormatAndProvider("...");

			// Act
			string result1 = DefaultFormatter.For<TestStructWithSolitaryToStringFormatAndProvider>().Format(input);
			string result2 = DefaultFormatter.For<TestStructWithSolitaryToStringFormatAndProvider>().Format(input, null);

			// Assert
			Assert.That(result1, Is.EqualTo("..."));
			Assert.That(result2, Is.EqualTo("..."));
		}

		[TestCase]
		public void Format_ValueType_WithSolitaryToStringFormat()
		{
			// Arrange
			TestStructWithSolitaryToStringFormat input = new TestStructWithSolitaryToStringFormat("...");

			// Act
			string result1 = DefaultFormatter.For<TestStructWithSolitaryToStringFormat>().Format(input);
			string result2 = DefaultFormatter.For<TestStructWithSolitaryToStringFormat>().Format(input, null);

			// Assert
			Assert.That(result1, Is.EqualTo("..."));
			Assert.That(result2, Is.EqualTo("..."));
		}

		[TestCase]
		public void Format_NullableValueType_WithExplicitIFormattable()
		{
			// Arrange
			TestStructWithExplicitIFormattable? input = new TestStructWithExplicitIFormattable("...");

			// Act
			string result1 = DefaultFormatter.For<TestStructWithExplicitIFormattable?>().Format(input);
			string result2 = DefaultFormatter.For<TestStructWithExplicitIFormattable?>().Format(input, null);

			// Assert
			Assert.That(result1, Is.EqualTo("..."));
			Assert.That(result2, Is.EqualTo("..."));
		}

		[TestCase]
		public void Format_NullableValueType_WithImplicitIFormattable()
		{
			// Arrange
			TestStructWithImplicitIFormattable input = new TestStructWithImplicitIFormattable("...");

			// Act
			string result1 = DefaultFormatter.For<TestStructWithImplicitIFormattable?>().Format(input);
			string result2 = DefaultFormatter.For<TestStructWithImplicitIFormattable?>().Format(input, null);

			// Assert
			Assert.That(result1, Is.EqualTo("..."));
			Assert.That(result2, Is.EqualTo("..."));
		}

		[TestCase]
		public void Format_NullableValueType_WithSolitaryToStringFormatAndProvider()
		{
			// Arrange
			TestStructWithSolitaryToStringFormatAndProvider input = new TestStructWithSolitaryToStringFormatAndProvider("...");

			// Act
			string result1 = DefaultFormatter.For<TestStructWithSolitaryToStringFormatAndProvider?>().Format(input);
			string result2 = DefaultFormatter.For<TestStructWithSolitaryToStringFormatAndProvider?>().Format(input, null);

			// Assert
			Assert.That(result1, Is.EqualTo("..."));
			Assert.That(result2, Is.EqualTo("..."));
		}

		[TestCase]
		public void Format_NullableValueType_WithSolitaryToStringFormat()
		{
			// Arrange
			TestStructWithSolitaryToStringFormat input = new TestStructWithSolitaryToStringFormat("...");

			// Act
			string result1 = DefaultFormatter.For<TestStructWithSolitaryToStringFormat?>().Format(input);
			string result2 = DefaultFormatter.For<TestStructWithSolitaryToStringFormat?>().Format(input, null);

			// Assert
			Assert.That(result1, Is.EqualTo("..."));
			Assert.That(result2, Is.EqualTo("..."));
		}
	}
}
