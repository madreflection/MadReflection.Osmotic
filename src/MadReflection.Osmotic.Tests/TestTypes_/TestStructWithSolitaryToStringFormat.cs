using System;

namespace MadReflection.Osmotic.Tests
{
	public struct TestStructWithSolitaryToStringFormat
	{
		private string _value;


		public TestStructWithSolitaryToStringFormat(string value)
		{
			if (value == null)
				throw new ArgumentNullException(nameof(value));
			if (value == "")
				throw new ArgumentException("Empty string is invalid for this purpose.");

			_value = value;
		}


		public string Value => _value;


		public static TestStructWithSolitaryToStringFormat Parse(string s)
		{
			if (s == null)
				throw new ArgumentNullException(nameof(s));
			if (s.Length == 0)
				throw new ArgumentException("Invalid winput.");

			return new TestStructWithSolitaryToStringFormat(s);
		}

		public string ToString(string format) => _value;

		public override string ToString() => _value;
	}
}
