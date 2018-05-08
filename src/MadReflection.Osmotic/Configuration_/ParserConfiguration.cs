namespace MadReflection.Osmotic
{
	/// <summary>
	/// An object that can be used to configure <see cref="ParserContainer"/> objects.
	/// </summary>
	public abstract class ParserConfiguration
	{
		internal ParserConfiguration()
		{
		}


		/// <summary>
		/// Gets or sets a value indicating how to handle types that do not have a TryParse method.
		/// </summary>
		public abstract MissingTryParseHandling MissingTryParse { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether null (Nothing in VB) parses to null (Nothing in VB) for reference types.
		/// </summary>
		public abstract bool ReferenceTypesParseNullToNull { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether null (Nothing in VB) parses to null (Nothing in VB) for nullable value types.
		/// </summary>
		public abstract bool NullableValueTypesParseNullToNull { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether an empty string parses to null (Nothing in VB) for nullable value types, allowing null to round-trip.
		/// </summary>
		public abstract bool NullableValueTypesParseEmptyStringToNull { get; set; }


		internal abstract void Lock();

		/// <summary>
		/// Configures parsing of <typeparamref name="T"/> to explicitly use the default implementation.
		/// </summary>
		/// <typeparam name="T">The type for which to configure parsing.</typeparam>
		public void UseDefault<T>() => UseDefault<T>(MissingTryParseHandling.ThrowNotSupportedException);

		/// <summary>
		/// Configures parsing of <typeparamref name="T"/> to explicitly use the default implementation.
		/// </summary>
		/// <typeparam name="T">The type for which to configure parsing.</typeparam>
		/// <param name="missingTryParse">A value indicating how to configure TryParse functionality if the type does not have a TryParse method</param>
		public abstract void UseDefault<T>(MissingTryParseHandling missingTryParse);

		/// <summary>
		/// Configures parsing of <typeparamref name="T"/> to use the provided function.
		/// </summary>
		/// <typeparam name="T">The type for which to configure parsing.</typeparam>
		/// <param name="parseFunc">A function that implements Parse method functionality for <typeparamref name="T"/>.</param>
		public void UseFunc<T>(ParseFunc<T> parseFunc) => UseFunc(parseFunc, MissingTryParseHandling.ThrowNotSupportedException);

		/// <summary>
		/// Configures parsing of <typeparamref name="T"/> to use the provided function.
		/// </summary>
		/// <typeparam name="T">The type for which to configure parsing.</typeparam>
		/// <param name="parseFunc">A function that implements Parse method functionality for <typeparamref name="T"/>.</param>
		/// <param name="missingTryParse">A value indicating how to configure TryParse functionality if the type does not have a TryParse method</param>
		public abstract void UseFunc<T>(ParseFunc<T> parseFunc, MissingTryParseHandling missingTryParse);

		/// <summary>
		/// Configures parsing of <typeparamref name="T"/> to use the provided functions.
		/// </summary>
		/// <typeparam name="T">The type for which to configure parsing.</typeparam>
		/// <param name="parseFunc">A function that implements Parse method functionality for <typeparamref name="T"/>.</param>
		/// <param name="tryParseFunc">A function that implements the TryParse method functionality for <typeparamref name="T"/>.</param>
		public abstract void UseFunc<T>(ParseFunc<T> parseFunc, TryParseFunc<T> tryParseFunc);

		/// <summary>
		/// Configures parsing of <typeparamref name="T"/> to use the provided implementation of <see cref="IParser{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type for which to configure parsing.</typeparam>
		/// <param name="parserObject">An object that implements parsing functionality for <typeparamref name="T"/>.</param>
		public abstract void UseParserObject<T>(IParser<T> parserObject);

		/// <summary>
		/// Configures parsing of <typeparamref name="T"/> to use its type converter.
		/// </summary>
		/// <typeparam name="T">The type for which to configure parsing.</typeparam>
		public void UseTypeConverter<T>() => UseTypeConverter<T>(MissingTryParseHandling.ThrowNotSupportedException);

		/// <summary>
		/// Configures parsing of <typeparamref name="T"/> to use its type converter.
		/// </summary>
		/// <typeparam name="T">The type for which to configure parsing.</typeparam>
		/// <param name="missingTryParse">A value indicating how to configure TryParse functionality if the type does not have a TryParse method</param>
		public abstract void UseTypeConverter<T>(MissingTryParseHandling missingTryParse);
	}
}
