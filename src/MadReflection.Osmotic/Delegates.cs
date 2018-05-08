using System;

namespace MadReflection.Osmotic
{
	/// <summary>
	/// A delegate type for a parser configuration function.
	/// </summary>
	/// <param name="configuration">The configuration object that configures a parser container.</param>
	public delegate void ParserConfigurator(ParserConfiguration configuration);

	/// <summary>
	/// A delegate type for a formatter configuration function.
	/// </summary>
	/// <param name="configuration">The configuration object that configures a formatter container.</param>
	public delegate void FormatterConfigurator(FormatterConfiguration configuration);


	/// <summary>
	/// A delegate type that represents the signature of the Parse method contract.
	/// </summary>
	/// <typeparam name="T">The type to be parsed from strings.</typeparam>
	/// <param name="s">The string to parse.</param>
	/// <returns>The parsed object instance.</returns>
	public delegate T ParseFunc<T>(string s);

	/// <summary>
	/// A delegate type that represents the signature of the TryParse method contract.
	/// </summary>
	/// <typeparam name="T">The type to be parsed from strings.</typeparam>
	/// <param name="s">The string to parse.</param>
	/// <param name="result">When the target method returns, the parsed object instance, or the default value of <typeparamref name="T"/> if parsing failed.</param>
	/// <returns>A value that indicates whether the parsing succeeded.</returns>
	public delegate bool TryParseFunc<T>(string s, out T result);

	/// <summary>
	/// A delegate type that represents the signature of the ToString method contract as a static-like method.
	/// </summary>
	/// <typeparam name="T">The type to convert to string.</typeparam>
	/// <param name="value">The object to convert.</param>
	/// <returns>The resulting string.</returns>
	public delegate string FormatFunc<T>(T value);

	/// <summary>
	/// A delegate type that represents the signature of the ToString method contract as a static-like method, taking a format specifier.
	/// </summary>
	/// <typeparam name="T">The type to convert to string.</typeparam>
	/// <param name="value">The object to convert.</param>
	/// <param name="format">The format specification.</param>
	/// <returns>The resulting string.</returns>
	public delegate string FormatSpecificFunc<T>(T value, string format);


	// Internal delegates for ToString overloads that aren't implementations of an interface member.
	internal delegate string ValueTypeSolitaryToStringFormatFunc<T>(ref T value, string format) where T : struct;
	internal delegate string ValueTypeSolitaryToStringFormatWithProviderFunc<T>(ref T value, string format, IFormatProvider formatProvider) where T : struct;
}
