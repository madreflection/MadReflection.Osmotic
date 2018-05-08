using System;
using System.Runtime.Serialization;

namespace MadReflection.Osmotic
{
	/// <summary>
	/// Base class for Osmotic exceptions.
	/// </summary>
	[Serializable]
	public abstract class OsmoticException : Exception
	{
		#region OsmoticException members
		/// <summary>
		/// Initializes a new instance of <see cref="OsmoticException"/>.
		/// </summary>
		public OsmoticException()
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="OsmoticException"/>.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public OsmoticException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="OsmoticException"/>.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public OsmoticException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
		#endregion


		#region ISerializable members
#if !NETSTANDARD1_3
		/// <summary>
		/// Initializes a new instance of <see cref="OsmoticException"/>.
		/// </summary>
		/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
		protected OsmoticException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}


		/// <summary>
		/// Sets the <see cref="SerializationInfo"/> with information about the exception.
		/// </summary>
		/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
#endif
		#endregion
	}
}
