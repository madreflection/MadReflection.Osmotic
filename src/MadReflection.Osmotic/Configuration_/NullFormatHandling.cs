namespace MadReflection.Osmotic
{
	/// <summary>
	/// Indicates the course of action to take when formatting a null (Nothing in VB) instance to a string.
	/// </summary>
	public enum NullFormatHandling
	{
		/// <summary>
		/// Throw <see cref="System.ArgumentNullException"/> .
		/// </summary>
		ThrowArgumentNullException,

		/// <summary>
		/// Return null (Nothing in VB).
		/// </summary>
		ReturnNull,

		/// <summary>
		/// Return an empty string.
		/// </summary>
		ReturnEmptyString
	}
}
