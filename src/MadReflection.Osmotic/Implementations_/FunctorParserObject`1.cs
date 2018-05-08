using System;

namespace MadReflection.Osmotic
{
	internal class FunctorParserObject<T> : ParserObject, IParser<T>
	{
		private ParseFunc<T> _parseFunc;
		private TryParseFunc<T> _tryParseFunc;


		public FunctorParserObject(ParseFunc<T> parseFunc, TryParseFunc<T> tryParseFunc)
		{
			_parseFunc = parseFunc;
			_tryParseFunc = tryParseFunc;
		}


		protected override object InternalParse(string s) => _parseFunc(s);

		protected override bool InternalTryParse(string s, out object result)
		{
			bool returnValue = _tryParseFunc(s, out T actualResult);
			result = actualResult;
			return returnValue;
		}


		T IParser<T>.Parse(string s) => _parseFunc(s);

		bool IParser<T>.TryParse(string s, out T result) => _tryParseFunc(s, out result);
	}
}
