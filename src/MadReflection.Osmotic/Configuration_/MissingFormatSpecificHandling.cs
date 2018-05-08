namespace MadReflection.Osmotic
{
	/// <summary>
	/// Indicates the course of action to take when a type does not implement a ToString method with a supported signature that takes a format string.
	/// </summary>
	public enum MissingFormatSpecificHandling
	{
		/// <summary>
		/// Throw <see cref="System.NotSupportedException"/>
		/// </summary>
		ThrowNotSupportedException,

		/// <summary>
		/// Return an empty string.
		/// </summary>
		ReturnEmptyString,

		/// <summary>
		/// Return null (Nothing in VB).
		/// </summary>
		ReturnNull,

		/// <summary>
		/// Call the parameterless ToString method, thereby ignoring the format string parameter.
		/// </summary>
		UseToString
	}
}
