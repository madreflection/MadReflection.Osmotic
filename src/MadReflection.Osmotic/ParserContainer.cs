using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MadReflection.Osmotic
{
	/// <summary>
	/// Provides parsing functionality.
	/// </summary>
	public sealed partial class ParserContainer : IParserContainer
	{
		private MissingTryParseHandling _missingTryParse = MissingTryParseHandling.ThrowNotSupportedException;
		private bool _referenceTypesParseNullToNull = false;
		private bool _nullableValueTypesParseNullToNull = false;
		private bool _nullableValueTypesParseEmptyStringToNull = false;
		private Dictionary<Type, IParser> _parsers = new Dictionary<Type, IParser>();


		private ParserContainer()
		{
		}


		/// <summary>
		/// Gets a <see cref="ParserContainer"/> instance with the default configuration.
		/// </summary>
		public static ParserContainer Default { get; } = Create(config => { });

		/// <summary>
		/// Creates a <see cref="ParserContainer"/> instance configured using the provided configuration method.
		/// </summary>
		/// <param name="configurator">A method that sets configuration options for a new <see cref="ParserContainer"/> instance.</param>
		/// <returns>A new <see cref="ParserContainer"/> instance.</returns>
		public static ParserContainer Create(ParserConfigurator configurator)
		{
			if (configurator == null)
				throw new ArgumentNullException(nameof(configurator));

			ParserContainer container = new ParserContainer();

			ParserConfiguration configuration = new Configuration(container);

			configurator(configuration);

			configuration.Lock();

			return container;
		}


		/// <summary>
		/// Gets a parsing interface for the specified type.
		/// </summary>
		/// <param name="type">The type that the requested object parses.</param>
		/// <returns>An <see cref="IParser"/> reference to a parsing object for type <paramref name="type"/> that also implements <see cref="IParser{T}"/> where 'T' is <paramref name="type"/>.</returns>
		public IParser For(Type type)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));

			return ForInternal(type);
		}

		/// <summary>
		/// Gets a parsing interface for the specified type.
		/// </summary>
		/// <typeparam name="T">The type that the requested object parses.</typeparam>
		/// <returns>An <see cref="IParser{T}"/> reference to a parsing object for <typeparamref name="T"/>.</returns>
		public IParser<T> For<T>() => (IParser<T>)ForInternal(typeof(T));

		private IParser ForInternal(Type type)
		{
			if (!_parsers.TryGetValue(type, out IParser parser))
			{
				lock (_parsers)
				{
					if (!_parsers.TryGetValue(type, out parser))
					{
						MethodInfo method = ReflectionHelper.GetPrivateGenericMethod(typeof(ParserContainer), nameof(CreateParserObject), type);
						Func<IParser> parserObjectFactory = method.CreateDelegate<Func<IParser>>(this);
						parser = parserObjectFactory();

						_parsers[type] = parser;
					}
				}
			}

			return parser;
		}

		private IParser<T> CreateParserObject<T>()
		{
			Type type = typeof(T);

			MethodInfo factoryMethod;
			if (Nullable.GetUnderlyingType(type) is Type underlyingValueType)
				factoryMethod = ReflectionHelper.GetPrivateGenericMethod(typeof(ParserContainer), nameof(GetNullableValueTypeParserObject), underlyingValueType);
			else if (type.GetTypeInfo().IsValueType)
				factoryMethod = ReflectionHelper.GetPrivateGenericMethod(typeof(ParserContainer), nameof(GetValueTypeParserObject), type);
			else
				factoryMethod = ReflectionHelper.GetPrivateGenericMethod(typeof(ParserContainer), nameof(GetReferenceTypeParserObject), type);

			Func<IParser<T>> factory = factoryMethod.CreateDelegate<Func<IParser<T>>>(this);

			return factory();
		}

		private IParser<T?> GetNullableValueTypeParserObject<T>()
			where T : struct
		{
			IParser<T> valueTypeParser = (IParser<T>)ForInternal(typeof(T));

			ParseFunc<T?> parseFunc = s =>
			{
				if (s == null)
				{
					if (_nullableValueTypesParseNullToNull)
						return null;
				}
				else if (s.Length == 0)
				{
					if (_nullableValueTypesParseEmptyStringToNull)
						return null;
				}

				return valueTypeParser.Parse(s);
			};

			TryParseFunc<T?> tryParseFunc = (string s, out T? result) =>
			{
				if (s == null)
				{
					if (_nullableValueTypesParseNullToNull)
					{
						result = null;
						return true;
					}
				}
				else if (s.Length == 0)
				{
					if (_nullableValueTypesParseEmptyStringToNull)
					{
						result = null;
						return true;
					}
				}

				bool returnValue = valueTypeParser.TryParse(s, out T temp);
				result = temp;
				return returnValue;
			};

			return new FunctorParserObject<T?>(parseFunc, tryParseFunc);
		}

		private IParser<T> GetValueTypeParserObject<T>()
			where T : struct
		{
			Type type = typeof(T);

			if (type.GetTypeInfo().IsEnum)
				return GetEnumTypeParserObject<T>();

			ParseFunc<T> parseFunc = GetDefaultParseFunc<T>();
			TryParseFunc<T> tryParseFunc = GetDefaultTryParseFunc<T>();

			return ApplyOptionsAndGetValueTypeParserObject(parseFunc, tryParseFunc, _missingTryParse);
		}

		private IParser<T> GetEnumTypeParserObject<T>()
			where T : struct
		{
			Type type = typeof(T);

			// .NET Core 2.0 has this method.
			MethodInfo genericParseMethod = (
				from m in typeof(Enum).GetMethods()
				where m.Name == Constants.ParseName
				where m.IsGenericMethodDefinition
				let parameters = m.GetParameters()
				where parameters.Length == 1
				where parameters[0].ParameterType == typeof(string)
				select m.MakeGenericMethod(typeof(T))
				).SingleOrDefault();

			ParseFunc<T> parseFunc;
			if (genericParseMethod != null)
				parseFunc = genericParseMethod.CreateDelegate<ParseFunc<T>>();
			else
				parseFunc = s => (T)Enum.Parse(typeof(T), s);

			// .NET 4.0 has this method.
			MethodInfo genericTryParseMethod = (
				from m in typeof(Enum).GetMethods()
				where m.Name == Constants.TryParseName
				where m.IsGenericMethodDefinition
				let parameters = m.GetParameters()
				where parameters.Length == 2
				where parameters[0].ParameterType == typeof(string)
				where parameters[1].ParameterType.IsByRef
				select m.MakeGenericMethod(type)
				).SingleOrDefault();

			TryParseFunc<T> tryParseFunc;
			if (genericTryParseMethod != null)
			{
				tryParseFunc = genericTryParseMethod.CreateDelegate<TryParseFunc<T>>();
			}
			else
			{
				tryParseFunc = (string s, out T result) =>
				{
					try
					{
						result = (T)Enum.Parse(typeof(T), s);
						return true;
					}
					catch
					{
						result = default(T);
						return false;
					}
				};
			}

			return new FunctorParserObject<T>(parseFunc, tryParseFunc);
		}

		private IParser<T> GetReferenceTypeParserObject<T>()
			where T : class
		{
			if (typeof(T) == typeof(string))
				return (IParser<T>)new StringParser();

			ParseFunc<T> parseFunc = GetDefaultParseFunc<T>();
			TryParseFunc<T> tryParseFunc = GetDefaultTryParseFunc<T>();

			return ApplyOptionsAndGetReferenceTypeParserObject(parseFunc, tryParseFunc, _missingTryParse);
		}

		private IParser<T> AltCreateParserObject<T>(ParseFunc<T> parseFunc, TryParseFunc<T> tryParseFunc, MissingTryParseHandling missingTryParse)
		{
			Type type = typeof(T);

			string methodName = type.GetTypeInfo().IsValueType ? nameof(ApplyOptionsAndGetValueTypeParserObject) : nameof(ApplyOptionsAndGetReferenceTypeParserObject);
			MethodInfo factoryMethod = ReflectionHelper.GetPrivateGenericMethod(typeof(ParserContainer), methodName, type);

			Func<ParseFunc<T>, TryParseFunc<T>, MissingTryParseHandling, IParser<T>> factory = factoryMethod.CreateDelegate<Func<ParseFunc<T>, TryParseFunc<T>, MissingTryParseHandling, IParser<T>>>(this);
			return factory(parseFunc, tryParseFunc, missingTryParse);
		}

		private IParser<T> ApplyOptionsAndGetValueTypeParserObject<T>(ParseFunc<T> parseFunc, TryParseFunc<T> tryParseFunc, MissingTryParseHandling missingTryParse)
			where T : struct
		{
			if (parseFunc == null)
				return new FunctorParserObject<T>(ParseThrowsNotSupportedException<T>, TryParseThrowsNotSupportedException);

			ParseFunc<T> valueTypeParseFunc = parseFunc;  // Just keeping with the pattern.
			TryParseFunc<T> valueTypeTryParseFunc = tryParseFunc;

			if (tryParseFunc == null)
			{
				if (missingTryParse == MissingTryParseHandling.WrapParseInTryCatch)
					valueTypeTryParseFunc = CreateWrapperForTryParse(valueTypeParseFunc);
				else if (missingTryParse == MissingTryParseHandling.ReturnFalse)
					valueTypeTryParseFunc = TryParseReturnsFalse;
				else
					valueTypeTryParseFunc = TryParseThrowsNotSupportedException;
			}

			return new FunctorParserObject<T>(valueTypeParseFunc, valueTypeTryParseFunc);
		}

		private IParser<T> ApplyOptionsAndGetReferenceTypeParserObject<T>(ParseFunc<T> parseFunc, TryParseFunc<T> tryParseFunc, MissingTryParseHandling missingTryParse)
			where T : class
		{
			if (parseFunc == null)
				return new FunctorParserObject<T>(ParseThrowsNotSupportedException<T>, TryParseThrowsNotSupportedException);

			ParseFunc<T> referenceTypeParseFunc = parseFunc;
			TryParseFunc<T> referenceTypeTryParseFunc = tryParseFunc;

			if (_referenceTypesParseNullToNull)
			{
				referenceTypeParseFunc = s =>
				{
					if (s == null)
						return null;

					return parseFunc(s);
				};
			}

			if (tryParseFunc == null)
			{
				if (missingTryParse == MissingTryParseHandling.WrapParseInTryCatch)
					referenceTypeTryParseFunc = CreateWrapperForTryParse(referenceTypeParseFunc);
				else if (missingTryParse == MissingTryParseHandling.ReturnFalse)
					referenceTypeTryParseFunc = TryParseReturnsFalse;
				else
					referenceTypeTryParseFunc = TryParseThrowsNotSupportedException;
			}
			else
			{
				if (_referenceTypesParseNullToNull)
				{
					referenceTypeTryParseFunc = (string s, out T result) =>
					{
						if (s == null)
						{
							result = null;
							return true;
						}

						return tryParseFunc(s, out result);
					};
				}
			}

			return new FunctorParserObject<T>(referenceTypeParseFunc, referenceTypeTryParseFunc);
		}

		private static ParseFunc<T> GetDefaultParseFunc<T>()
		{
			Type type = typeof(T);

			MethodInfo method = (
				from m in type.GetMethods(BindingFlags.Public | BindingFlags.Static)
				where m.Name == Constants.ParseName
				where m.ReturnType == type
				let parameters = m.GetParameters()
				where parameters.Length == 1
				where parameters[0].ParameterType == typeof(string)
				select m
				).SingleOrDefault();

			if (method == null)
				return null;

			return method.CreateDelegate<ParseFunc<T>>();
		}

		private static TryParseFunc<T> GetDefaultTryParseFunc<T>()
		{
			Type type = typeof(T);

			MethodInfo method = (
				from m in type.GetMethods(BindingFlags.Public | BindingFlags.Static)
				where m.Name == Constants.TryParseName
				where m.ReturnType == typeof(bool)
				let parameters = m.GetParameters()
				where parameters.Length == 2
				where parameters[0].ParameterType == typeof(string)
				where parameters[1].ParameterType == type.MakeByRefType()
				select m
				).SingleOrDefault();

			if (method == null)
				return null;

			return method.CreateDelegate<TryParseFunc<T>>();
		}

		private static TryParseFunc<T> CreateWrapperForTryParse<T>(ParseFunc<T> parseFunc)
		{
			return (string s, out T result) =>
			{
				try
				{
					result = parseFunc(s);
					return true;
				}
				catch
				{
					result = default(T);
					return false;
				}
			};
		}

		private static ParseFunc<T> CreateParseFuncForTypeConverter<T>(TypeConverter converter) => s => (T)converter.ConvertFrom(s);

		private static TryParseFunc<T> CreateTryParseFuncForTypeConverter<T>(TypeConverter converter, MissingTryParseHandling missingTryParse) => null;

		private static T ParseThrowsNotSupportedException<T>(string s) => throw new NotSupportedException();

		private static bool TryParseThrowsNotSupportedException<T>(string s, out T result) => throw new NotSupportedException();

		private static bool TryParseReturnsFalse<T>(string s, out T result)
		{
			result = default(T);
			return false;
		}
	}
}
