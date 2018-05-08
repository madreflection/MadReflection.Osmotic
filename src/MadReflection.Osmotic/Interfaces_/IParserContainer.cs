using System;

namespace MadReflection.Osmotic
{
	/// <summary>
	/// Represents an object that contains parsing objects selectable by type.
	/// </summary>
	public interface IParserContainer
	{
		/// <summary>
		/// Gets a parsing interface for the specified type.
		/// </summary>
		/// <param name="type">The type that the requested object parses.</param>
		/// <returns>An <see cref="IParser"/> reference to a parsing object for type <paramref name="type"/> that also implements <see cref="IParser{T}"/> where 'T' is <paramref name="type"/>.</returns>
		IParser For(Type type);

		/// <summary>
		/// Gets a parsing interface for the specified type.
		/// </summary>
		/// <typeparam name="T">The type that the requested object parses.</typeparam>
		/// <returns>An <see cref="IParser{T}"/> reference to a parsing object for <typeparamref name="T"/>.</returns>
		IParser<T> For<T>();
	}
}
