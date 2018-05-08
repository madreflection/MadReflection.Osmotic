using System;

namespace MadReflection.Osmotic.Tests
{
	public class TestClassWithImplicitIFormattable : IFormattable
	{
		private string _value;


		public TestClassWithImplicitIFormattable(string value)
		{
			if (value == null)
				throw new ArgumentNullException(nameof(value));
			if (value == "")
				throw new ArgumentException("Empty string is invalid for this purpose.");

			_value = value;
		}


		public string Value => _value;


		public static TestClassWithImplicitIFormattable Parse(string s)
		{
			if (s == null)
				throw new ArgumentNullException(nameof(s));
			if (s.Length == 0)
				throw new ArgumentException("Invalid input.");

			return new TestClassWithImplicitIFormattable(s);
		}

		public string ToString(string format, IFormatProvider formatProvider) => _value;

		public override string ToString() => _value;
	}
}
