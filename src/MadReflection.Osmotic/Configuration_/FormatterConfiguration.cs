using System.Globalization;

namespace MadReflection.Osmotic
{
	/// <summary>
	/// An object that can be used to configure <see cref="FormatterContainer"/> objects.
	/// </summary>
	public abstract class FormatterConfiguration
	{
		internal FormatterConfiguration()
		{
		}


		/// <summary>
		/// Gets or sets a value indicating how to handle types that do not have a ToString method with one of the supported signatures taking a format string.
		/// </summary>
		public abstract MissingFormatSpecificHandling MissingFormatSpecific { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether null (Nothing in VB) creates a null (Nothing in VB) string for reference types.
		/// </summary>
		public abstract bool ReferenceTypesFormatNullToNull { get; set; }

		/// <summary>
		/// Gets or sets a value indicating how to generate strings from null (Nothing in VB) for nullable value types.
		/// </summary>
		public abstract NullFormatHandling NullableValueTypesFormatNull { get; set; }

		/// <summary>
		/// Gets or sets the default culture to use with <see cref="System.IFormatProvider"/>.
		/// </summary>
		internal abstract CultureInfo DefaultCultureInfo { get; set; }   // Not exposed (yet?).


		internal abstract void Lock();

		/// <summary>
		/// Configures formatting of <typeparamref name="T"/> to explicitly use the default implementation.
		/// </summary>
		/// <typeparam name="T">The type for which to configure formatting.</typeparam>
		public void UseDefault<T>() => UseDefault<T>(MissingFormatSpecificHandling.ThrowNotSupportedException);

		/// <summary>
		/// Configures formatting of <typeparamref name="T"/> to explicitly use the default implementation.
		/// </summary>
		/// <typeparam name="T">The type for which to configure formatting.</typeparam>
		/// <param name="missingFormatSpecific">A value indicating how to configure format functionality if the type does not have a ToString overload that accepts a format specification.</param>
		public abstract void UseDefault<T>(MissingFormatSpecificHandling missingFormatSpecific);

		/// <summary>
		/// Configures formatting of <typeparamref name="T"/> to use the provided function.
		/// </summary>
		/// <typeparam name="T">The type for which to configure formatting.</typeparam>
		/// <param name="formatFunc">A function that implements formatting functionality for <typeparamref name="T"/>.</param>
		public void UseFunc<T>(FormatFunc<T> formatFunc) => UseFunc<T>(formatFunc, MissingFormatSpecificHandling.ThrowNotSupportedException);

		/// <summary>
		/// Configures formatting of <typeparamref name="T"/> to use the provided function.
		/// </summary>
		/// <typeparam name="T">The type for which to configure formatting.</typeparam>
		/// <param name="formatFunc">A function that implements formatting functionality for <typeparamref name="T"/>.</param>
		/// <param name="missingFormatSpecific">A value indicating how to configure format functionality if the type does not have a ToString overload that accepts a format specification.</param>
		public abstract void UseFunc<T>(FormatFunc<T> formatFunc, MissingFormatSpecificHandling missingFormatSpecific);

		/// <summary>
		/// Configures formatting of <typeparamref name="T"/> to use the provided functions.
		/// </summary>
		/// <typeparam name="T">The type for which to configure formatting.</typeparam>
		/// <param name="formatFunc">A function that implements formatting functionality for <typeparamref name="T"/>.</param>
		/// <param name="formatSpecificFunc">A function that implements formatting functionality with a format specifier for <typeparamref name="T"/>.</param>
		public abstract void UseFunc<T>(FormatFunc<T> formatFunc, FormatSpecificFunc<T> formatSpecificFunc);

		/// <summary>
		/// Configures formatting of <typeparamref name="T"/> to use the provided implementation of <see cref="IFormatter{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type for which to configure formatting.</typeparam>
		/// <param name="formatter">An object that implements formatting functionality for <typeparamref name="T"/>.</param>
		public abstract void UseFormatterObject<T>(IFormatter<T> formatter);

		/// <summary>
		/// Configures formatting of <typeparamref name="T"/> to use its type converter.
		/// </summary>
		/// <typeparam name="T">The type for which to configure formatting.</typeparam>
		public void UseTypeConverter<T>() => UseTypeConverter<T>(MissingFormatSpecificHandling.ThrowNotSupportedException);

		/// <summary>
		/// Configures formatting of <typeparamref name="T"/> to use its type converter.
		/// </summary>
		/// <typeparam name="T">The type for which to configure formatting.</typeparam>
		/// <param name="missingFormatSpecific">A value indicating how to configure format functionality if the type does not have a ToString overload that accepts a format specification.</param>
		public abstract void UseTypeConverter<T>(MissingFormatSpecificHandling missingFormatSpecific);
	}
}
