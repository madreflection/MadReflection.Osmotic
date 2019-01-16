using System;

namespace MadReflection.Osmotic.Tests
{
	public class TestClassWithSolitaryToStringFormat
	{
		private string _value;


		public TestClassWithSolitaryToStringFormat(string value)
		{
			if (value is null)
				throw new ArgumentNullException(nameof(value));
			if (value == "")
				throw new ArgumentException("Empty string is invalid for this purpose.");

			_value = value;
		}


		public string Value => _value;


		public static TestClassWithSolitaryToStringFormat Parse(string s)
		{
			if (s is null)
				throw new ArgumentNullException(nameof(s));
			if (s.Length == 0)
				throw new ArgumentException("Invalid input.");

			return new TestClassWithSolitaryToStringFormat(s);
		}

		public string ToString(string format) => _value;

		public override string ToString() => _value;
	}
}
