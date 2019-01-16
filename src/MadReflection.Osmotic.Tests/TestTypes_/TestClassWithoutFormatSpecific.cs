using System;

namespace MadReflection.Osmotic.Tests
{
	public class TestClassWithoutFormatSpecific
	{
		private string _value;


		public TestClassWithoutFormatSpecific(string value)
		{
			if (value is null)
				throw new ArgumentNullException(nameof(value));
			if (value == "")
				throw new ArgumentException("Empty string is invalid for this purpose.");

			_value = value;
		}


		public string Value => _value;


		public override string ToString() => _value;
	}
}
