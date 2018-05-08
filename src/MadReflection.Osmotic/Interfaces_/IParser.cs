namespace MadReflection.Osmotic
{
	/// <summary>
	/// Represents the functionality of an object that can parse strings to objects of a certain, but unspecified, type.
	/// </summary>
	public interface IParser
	{
		/// <summary>
		/// Parses a string to an object instance.
		/// </summary>
		/// <param name="s">The string to parse.</param>
		/// <returns>The parsed object instance.</returns>
		object Parse(string s);

		/// <summary>
		/// Parses a string to an object instance.
		/// </summary>
		/// <param name="s">The string to parse.</param>
		/// <param name="result">When this method returns, the parsed object instance.</param>
		/// <returns>A value indicating whether the parsing succeeded.</returns>
		bool TryParse(string s, out object result);
	}
}
