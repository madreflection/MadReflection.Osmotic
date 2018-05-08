using System;

namespace MadReflection.Osmotic.Tests
{
	public class TestClassWithoutTryParse
	{
		private string _value;


		public TestClassWithoutTryParse(string value)
		{
			if (value == null)
				throw new ArgumentNullException(nameof(value));
			if (value == "")
				throw new ArgumentException("Empty string is invalid for this purpose.");

			_value = value;
		}


		public string Value => _value;


		public static TestClassWithoutTryParse Parse(string s)
		{
			if (s == null)
				throw new ArgumentNullException(nameof(s));
			if (s.Length == 0)
				throw new ArgumentException("Invalid input.");

			return new TestClassWithoutTryParse(s);
		}

		public override string ToString() => _value;
	}
}
