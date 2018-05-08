using System.Reflection;
using NUnitLite;

namespace MadReflection.Osmotic.Tests
{
	internal static class Program
	{
		private static int Main(string[] args)
		{
			return new AutoRun(typeof(Program).GetTypeInfo().Assembly).Execute(args);
		}
	}
}
