using System;

namespace MadReflection.Osmotic.Tests
{
	public struct TestStructWithSolitaryToStringFormatAndProvider
	{
		private string _value;


		public TestStructWithSolitaryToStringFormatAndProvider(string value)
		{
			if (value is null)
				throw new ArgumentNullException(nameof(value));
			if (value == "")
				throw new ArgumentException("Empty string is valid for this purpose.");

			_value = value;
		}


		public string Value => _value;


		public static TestStructWithSolitaryToStringFormatAndProvider Parse(string s)
		{
			if (s is null)
				throw new ArgumentNullException(nameof(s));
			if (s.Length == 0)
				throw new ArgumentException("Invalid input.");

			return new TestStructWithSolitaryToStringFormatAndProvider(s);
		}

		public string ToString(string format, IFormatProvider formatProvider) => _value;

		public override string ToString() => _value;
	}
}
