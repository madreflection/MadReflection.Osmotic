using System;

namespace MadReflection.Osmotic.Extensions
{
	/// <summary>
	/// Provides extension methods for the <see cref="IParser{T}"/> interface.
	/// </summary>
	public static class ParserExtensions
	{
		/// <summary>
		/// Parses a string to an object instance, returning <paramref name="defaultValue"/> if parsing fails.
		/// </summary>
		/// <typeparam name="T">The type of object to which to parse.</typeparam>
		/// <param name="parser">The parser to use.</param>
		/// <param name="s">The string to parse.</param>
		/// <param name="defaultValue">The value to return if parsing fails.</param>
		/// <returns>The parsed object instance.</returns>
		/// <exception cref="NotSupportedException">A TryParse is not available.</exception>
		/// <remarks>
		/// It leverages <see cref="IParser{T}.TryParse(string, out T)"/> so either 1) a TryParse
		/// method must be implemented by the type, or 2) a Parse method must be implemented by the
		/// type and Osmotic must be configured to either return false or wrap Parse if TryParse is
		/// not implemented by the type.
		/// </remarks>
		public static T ParseDefault<T>(this IParser<T> parser, string s, T defaultValue)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));

			if (parser.TryParse(s, out object result))
				return (T)result;

			return defaultValue;
		}

		/// <summary>
		/// Parses a string to an object instance, returning the result of <paramref name="defaultFactory"/> if parsing fails.
		/// </summary>
		/// <typeparam name="T">The type of object to which to parse.</typeparam>
		/// <param name="parser">The parser to use.</param>
		/// <param name="s">The string to parse.</param>
		/// <param name="defaultFactory">A function that creates the value to return if parsing fails.</param>
		/// <returns>The parsed object instance.</returns>
		/// <exception cref="NotSupportedException">A TryParse is not available.</exception>
		/// <remarks>
		/// It leverages <see cref="IParser{T}.TryParse(string, out T)"/> so either 1) a TryParse
		/// method must be implemented by the type, or 2) a Parse method must be implemented by the
		/// type and Osmotic must be configured to either return false or wrap Parse if TryParse is
		/// not implemented by the type.
		/// </remarks>
		public static T ParseDefault<T>(this IParser<T> parser, string s, Func<T> defaultFactory)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			if (defaultFactory == null)
				throw new ArgumentNullException(nameof(defaultFactory));

			if (parser.TryParse(s, out object result))
				return (T)result;

			return defaultFactory();
		}

		/// <summary>
		/// Parses a string to an object instance, returning <paramref name="defaultValue"/> if parsing fails.
		/// </summary>
		/// <typeparam name="T">The type of object to which to parse.</typeparam>
		/// <param name="parser">The parser to use.</param>
		/// <param name="s">The string to parse.</param>
		/// <param name="defaultValue">The value to return if parsing fails.</param>
		/// <param name="result">Upon return, this parameter contains the parsed result in the case of success or the value of <paramref name="defaultValue"/> in the case of failure.</param>
		/// <returns></returns>
		/// <exception cref="NotSupportedException">A TryParse is not available.</exception>
		/// <remarks>
		/// It leverages <see cref="IParser{T}.TryParse(string, out T)"/> so either 1) a TryParse
		/// method must be implemented by the type, or 2) a Parse method must be implemented by the
		/// type and Osmotic must be configured to either return false or wrap Parse if TryParse is
		/// not implemented by the type.
		/// </remarks>
		public static bool TryParseDefault<T>(this IParser<T> parser, string s, T defaultValue, out T result)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));

			if (parser.TryParse(s, out result))
				return true;

			result = defaultValue;
			return false;
		}

		/// <summary>
		/// Parses a string to an object instance, returning the result of <paramref name="defaultFactory"/> if parsing fails.
		/// </summary>
		/// <typeparam name="T">The type of object to which to parse.</typeparam>
		/// <param name="parser">The parser to use.</param>
		/// <param name="s">The string to parse.</param>
		/// <param name="defaultFactory">A function that creates the value to return if parsing fails.</param>
		/// <param name="result">Upon return, this parameter contains the parsed result in the case of success or the result of calling <paramref name="defaultFactory"/> in the case of failure.</param>
		/// <returns></returns>
		/// <exception cref="NotSupportedException">A TryParse is not available.</exception>
		/// <remarks>
		/// It leverages <see cref="IParser{T}.TryParse(string, out T)"/> so either 1) a TryParse
		/// method must be implemented by the type, or 2) a Parse method must be implemented by the
		/// type and Osmotic must be configured to either return false or wrap Parse if TryParse is
		/// not implemented by the type.
		/// </remarks>
		public static bool TryParseDefault<T>(this IParser<T> parser, string s, Func<T> defaultFactory, out T result)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			if (defaultFactory == null)
				throw new ArgumentNullException(nameof(defaultFactory));

			if (parser.TryParse(s, out result))
				return true;

			result = defaultFactory();
			return false;
		}
	}
}
