using System;

namespace MadReflection.Osmotic
{
	/// <summary>
	/// Represents an object that contains formatting objects selectable by type.
	/// </summary>
	public interface IFormatterContainer
	{
		/// <summary>
		/// Gets a formatting interface for the specified type.
		/// </summary>
		/// <param name="type">The type that the requested object formats.</param>
		/// <returns>An <see cref="IFormatter"/> reference to a formatting object for type <paramref name="type"/> that also implements <see cref="IFormatter{T}"/> where 'T' is <paramref name="type"/>.</returns>
		IFormatter For(Type type);

		/// <summary>
		/// Gets a formatting interface for the specified type.
		/// </summary>
		/// <typeparam name="T">The type that the requested object formats.</typeparam>
		/// <returns>An <see cref="IFormatter{T}"/> reference to a parsing object for <typeparamref name="T"/>.</returns>
		IFormatter<T> For<T>();
	}
}
