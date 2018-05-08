namespace MadReflection.Osmotic
{
	internal abstract class FormatterObject : IFormatter
	{
		protected FormatterObject()
		{
		}


		protected abstract string InternalFormat(object value);

		protected abstract string InternalFormat(object value, string format);


		string IFormatter.Format(object value) => InternalFormat(value);

		string IFormatter.Format(object value, string format) => InternalFormat(value, format);
	}
}
