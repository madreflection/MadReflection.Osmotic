namespace MadReflection.Osmotic
{
	/// <summary>
	/// Represents the functionality of an object that can format objects of type <typeparamref name="T"/> into strings.
	/// </summary>
	/// <typeparam name="T">The type of object which the formatter can format into strings.</typeparam>
	public interface IFormatter<T> : IFormatter
	{
		/// <summary>
		/// Converts the object to a string.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The resulting string.</returns>
		string Format(T value);

		/// <summary>
		/// Converts the object to a string in the specified format.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <param name="format">The format specification.</param>
		/// <returns>The resulting string.</returns>
		string Format(T value, string format);
	}
}
