using System;

namespace MadReflection.Osmotic.Tests
{
	public class TestClass : IFormattable
	{
		private string _value;


		public TestClass(string value)
		{
			if (value is null)
				throw new ArgumentNullException(nameof(value));
			if (value == "")
				throw new ArgumentException("Empty string is invalid for this purpose.");

			_value = value;
		}


		public string Value => _value;


		public static TestClass Parse(string s)
		{
			if (s is null)
				throw new ArgumentNullException(nameof(s));
			if (s.Length == 0)
				throw new FormatException("Invalid input.");

			return new TestClass(s);
		}

		public static bool TryParse(string s, out TestClass result)
		{
			result = default(TestClass);

			if (s is null)
				return false;

			if (s.Length == 0)
				return false;

			result = new TestClass(s);
			return true;
		}

		public override string ToString() => _value;

		public string ToString(string format, IFormatProvider formatProvider) => _value;
	}
}
