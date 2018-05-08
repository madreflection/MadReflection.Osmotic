using System;
using System.Reflection;

namespace MadReflection.Osmotic
{
	internal static class ReflectionHelper
	{
#if !NEW_REFLECTION
		public static Delegate CreateDelegate(this MethodInfo method, Type delegateType) => Delegate.CreateDelegate(delegateType, method);

		public static Delegate CreateDelegate(this MethodInfo method, Type delegateType, object firstArgument) => Delegate.CreateDelegate(delegateType, firstArgument, method);
#endif

		public static T CreateDelegate<T>(this MethodInfo method) where T : class => (T)(object)method.CreateDelegate(typeof(T));

		public static T CreateDelegate<T>(this MethodInfo method, object firstArgument) where T : class => (T)(object)method.CreateDelegate(typeof(T), firstArgument);

		public static MethodInfo GetPrivateGenericMethod(Type declaringType, string methodName, Type typeParameter) => declaringType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(typeParameter);
	}
}
