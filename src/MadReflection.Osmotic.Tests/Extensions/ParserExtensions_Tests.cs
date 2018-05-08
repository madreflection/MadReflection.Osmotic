using NUnit.Framework;
using MadReflection.Osmotic.Extensions;

namespace MadReflection.Osmotic.Tests.Extensions
{
	[TestFixture]
	public class ParserExtensions_Tests
	{
		private static ParserContainer _parser = ParserContainer.Default;


		[TestCase("1", 0, 1)]
		[TestCase("asdf", 0, 0)]
		public void ParseDefaultValue_Int32(string input, int defaultValue, int expected)
		{
			// Arrange

			// Act
			int result = _parser.For<int>().ParseDefault(input, defaultValue);

			// Assert
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1", 0, 1)]
		[TestCase("asdf", 0, 0)]
		public void ParseDefaultFactory_Int32(string input, int defaultValue, int expected)
		{
			// Arrange

			// Act
			int result = _parser.For<int>().ParseDefault(input, () => defaultValue);

			// Assert
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1", 0, true, 1)]
		[TestCase("asdf", 0, false, 0)]
		public void TryParseDefaultValue_Int32(string input, int defaultValue, bool expectedResult, int expectedOutput)
		{
			// Arrange

			// Act
			bool result = _parser.For<int>().TryParseDefault(input, defaultValue, out int output);

			// Assert
			Assert.That(result, Is.EqualTo(expectedResult));
			Assert.That(output, Is.EqualTo(expectedOutput));
		}

		[TestCase("1", 0, true, 1)]
		[TestCase("asdf", 0, false, 0)]
		public void TryParseDefaultFactory_Int32(string input, int defaultValue, bool expectedResult, int expectedOutput)
		{
			// Arrange

			// Act
			bool result = _parser.For<int>().TryParseDefault(input, () => defaultValue, out int output);

			// Assert
			Assert.That(result, Is.EqualTo(expectedResult));
			Assert.That(output, Is.EqualTo(expectedOutput));
		}
	}
}
