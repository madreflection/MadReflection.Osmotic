using System;

namespace MadReflection.Osmotic.Tests
{
	public class TestClassWithoutParse
	{
		private string _value;


		public TestClassWithoutParse(string value)
		{
			if (value is null)
				throw new ArgumentNullException(nameof(value));
			if (value == "")
				throw new ArgumentException("Empty string is invalid for this purpose.");

			_value = value;
		}


		public string Value => _value;
	}
}
