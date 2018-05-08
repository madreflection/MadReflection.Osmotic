using System;

namespace MadReflection.Osmotic
{
	internal class FunctorFormatterObject<T> : FormatterObject, IFormatter<T>
	{
		private FormatFunc<T> _formatFunc;
		private FormatSpecificFunc<T> _formatSpecificFunc;


		public FunctorFormatterObject(FormatFunc<T> formatFunc, FormatSpecificFunc<T> formatSpecificFunc)
		{
			_formatFunc = formatFunc;
			_formatSpecificFunc = formatSpecificFunc;
		}


		protected override string InternalFormat(object value) => _formatFunc((T)value);

		protected override string InternalFormat(object value, string format) => _formatSpecificFunc((T)value, format);


		string IFormatter<T>.Format(T value) => _formatFunc(value);

		string IFormatter<T>.Format(T value, string format) => _formatSpecificFunc(value, format);
	}
}
