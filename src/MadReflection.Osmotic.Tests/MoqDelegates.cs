namespace MadReflection.Osmotic.Tests
{
	internal delegate void ParseCallback<T>(string s);
	internal delegate string ParseReturns<T>(string s);

	internal delegate void TryParseCallback<T>(string s, out T result);
	internal delegate bool TryParseReturns<T>(string s, ref T result);

	internal delegate string FormatReturns<T>(T obj);

	internal delegate string FormatSpecificReturns<T>(T obj, string format);
}
