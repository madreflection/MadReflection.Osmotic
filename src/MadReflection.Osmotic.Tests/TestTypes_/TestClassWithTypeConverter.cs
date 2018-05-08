using System;
using System.ComponentModel;
using System.Globalization;

namespace MadReflection.Osmotic.Tests
{
	[TypeConverter(typeof(TestClassWithTypeConverterTypeConverter))]
	public class TestClassWithTypeConverter
	{
		private string _value;


		public TestClassWithTypeConverter(string value)
		{
			if (value == null)
				throw new ArgumentNullException(nameof(value));
			if (value == "")
				throw new ArgumentException("Empty string is invalid for this purpose.");

			_value = value;
		}


		public string Value => _value;
	}

	public class TestClassWithTypeConverterTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;

			return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
				return true;

			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string s)
				return new TestClassWithTypeConverter(s);

			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is TestClassWithTypeConverter tc)
			{
				if (destinationType == typeof(string))
					return tc.Value;
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
