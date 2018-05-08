using System;

namespace MadReflection.Osmotic
{
	internal class ValueTypeFunctorFormatterObject<T> : FunctorFormatterObject<T>
	{
		public ValueTypeFunctorFormatterObject(FormatFunc<T> formatFunc, FormatSpecificFunc<T> formatSpecificFunc)
			: base(formatFunc, formatSpecificFunc)
		{
		}

		protected override string InternalFormat(object value)
		{
			if (value == null)
				throw new ArgumentNullException(nameof(value));

			return base.InternalFormat(value);
		}

		protected override string InternalFormat(object value, string format)
		{
			if (value == null)
				throw new ArgumentNullException(nameof(value));

			return base.InternalFormat(value, format);
		}
	}
}
