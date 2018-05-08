namespace MadReflection.Osmotic
{
	internal class StringFormatter : IFormatter<string>
	{
		string IFormatter.Format(object value) => (string)value;

		string IFormatter.Format(object value, string format) => (string)value;

		public string Format(string value) => value;

		public string Format(string value, string format) => value;
	}
}
