using System;

namespace MadReflection.Osmotic.Tests
{
	public class TestClassWithExplicitIFormattable : IFormattable
	{
		private string _value;


		public TestClassWithExplicitIFormattable(string value)
		{
			if (value == null)
				throw new ArgumentNullException(nameof(value));
			if (value == "")
				throw new ArgumentException("Empty string is invalid for this purpose.");

			_value = value;
		}


		public string Value => _value;


		public static TestClassWithExplicitIFormattable Parse(string s)
		{
			if (s == null)
				throw new ArgumentNullException(nameof(s));
			if (s.Length == 0)
				throw new ArgumentException("Invalid input.");

			return new TestClassWithExplicitIFormattable(s);
		}

		string IFormattable.ToString(string format, IFormatProvider formatProvider) => _value;

		public override string ToString() => _value;
	}
}
