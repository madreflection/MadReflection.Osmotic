namespace MadReflection.Osmotic
{
	/// <summary>
	/// Represents the functionality of an object that can parse strings to objects of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of object to which the parser can parse strings.</typeparam>
	public interface IParser<T> : IParser
	{
		/// <summary>
		/// Parses a string to an object instance.
		/// </summary>
		/// <param name="s">The string to parse.</param>
		/// <returns>The parsed object instance.</returns>
		new T Parse(string s);

		/// <summary>
		/// Parses a string to an object instance.
		/// </summary>
		/// <param name="s">The string to parse.</param>
		/// <param name="result">When this method returns, the parsed object instance.</param>
		/// <returns>A value indicating whether the parsing succeeded.</returns>
		bool TryParse(string s, out T result);
	}
}
