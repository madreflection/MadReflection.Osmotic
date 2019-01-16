using System;

namespace MadReflection.Osmotic.Tests
{
	public struct TestStructWithoutFormatSpecific
	{
		private string _value;


		public TestStructWithoutFormatSpecific(string value)
		{
			if (value is null)
				throw new ArgumentNullException(nameof(value));
			if (value == "")
				throw new ArgumentException("Empty string is valid for this purpose.");

			_value = value;
		}
	}
}
