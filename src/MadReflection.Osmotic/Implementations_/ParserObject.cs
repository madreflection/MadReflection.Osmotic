namespace MadReflection.Osmotic
{
	internal abstract class ParserObject : IParser
	{
		protected ParserObject()
		{
		}


		protected abstract object InternalParse(string s);

		protected abstract bool InternalTryParse(string s, out object result);


		object IParser.Parse(string s) => InternalParse(s);

		bool IParser.TryParse(string s, out object result) => InternalTryParse(s, out result);
	}
}
