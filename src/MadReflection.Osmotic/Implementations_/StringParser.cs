namespace MadReflection.Osmotic
{
	internal class StringParser : IParser<string>
	{
		object IParser.Parse(string s) => s;

		bool IParser.TryParse(string s, out object result)
		{
			result = s;
			return true;
		}

		string IParser<string>.Parse(string s) => s;

		bool IParser<string>.TryParse(string s, out string result)
		{
			result = s;
			return true;
		}
	}
}
