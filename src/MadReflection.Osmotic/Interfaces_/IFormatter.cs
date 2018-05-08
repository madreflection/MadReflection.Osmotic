namespace MadReflection.Osmotic
{
	/// <summary>
	/// Represents the functionality of an object that can format objects of a certain, but unspecified, type into strings.
	/// </summary>
	public interface IFormatter
	{
		/// <summary>
		/// Converts the object to a string.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The resulting string.</returns>
		string Format(object value);

		/// <summary>
		/// Converts the object to a string in the specified format.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <param name="format">The format specifier.</param>
		/// <returns>The resulting string.</returns>
		string Format(object value, string format);
	}
}
