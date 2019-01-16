using System;

namespace MadReflection.Osmotic.Tests
{
	public struct TestStruct : IFormattable
	{
		private string _value;


		public TestStruct(string value)
		{
			if (value is null)
				throw new ArgumentNullException(nameof(value));
			if (value == "")
				throw new ArgumentException("Empty string is invalid for this purpose.");

			_value = value;
		}


		public string Value => _value;


		public static TestStruct Parse(string s)
		{
			if (s is null)
				throw new ArgumentNullException(nameof(s));
			if (s.Length == 0)
				throw new FormatException("Invalid input.");

			return new TestStruct(s);
		}

		public static bool TryParse(string s, out TestStruct result)
		{
			result = default(TestStruct);

			if (s is null)
				return false;

			if (s.Length == 0)
				return false;

			result = new TestStruct(s);
			return true;
		}

		public override string ToString() => _value;

		public string ToString(string format, IFormatProvider formatProvider) => _value ?? "(null)";
	}
}
