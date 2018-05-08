namespace MadReflection.Osmotic
{
	/// <summary>
	/// Indicates the course of action to take when parsing to a type that does not implement a TryParse with the expected signature.
	/// </summary>
	public enum MissingTryParseHandling
	{
		/// <summary>
		/// Throw <see cref="System.NotSupportedException"/>.
		/// </summary>
		ThrowNotSupportedException,

		/// <summary>
		/// Return false.
		/// </summary>
		ReturnFalse,

		/// <summary>
		/// Wrap a call to the Parse method in a try/catch block.
		/// </summary>
		WrapParseInTryCatch
	}
}
