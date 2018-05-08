using System;
using System.ComponentModel;
using System.Reflection;

namespace MadReflection.Osmotic
{
	partial class ParserContainer
	{
		private sealed class Configuration : ParserConfiguration
		{
			private bool _locked;
			private readonly ParserContainer _container;


			public Configuration(ParserContainer container)
			{
				_container = container;
			}


			public override MissingTryParseHandling MissingTryParse
			{
				get => _container._missingTryParse;
				set
				{
					ThrowIfLocked();

					if (value < MissingTryParseHandling.ThrowNotSupportedException || value > MissingTryParseHandling.WrapParseInTryCatch)
						throw new ArgumentOutOfRangeException(nameof(value), "Invalid MissingTryParseHandling value.");

					ThrowIfAnyTypesConfigured();

					_container._missingTryParse = value;
				}
			}

			public override bool ReferenceTypesParseNullToNull
			{
				get => _container._referenceTypesParseNullToNull;
				set
				{
					ThrowIfLocked();
					ThrowIfAnyTypesConfigured();

					_container._referenceTypesParseNullToNull = value;
				}
			}

			public override bool NullableValueTypesParseNullToNull
			{
				get => _container._nullableValueTypesParseNullToNull;
				set
				{
					ThrowIfLocked();
					ThrowIfAnyTypesConfigured();

					_container._nullableValueTypesParseNullToNull = value;
				}
			}

			public override bool NullableValueTypesParseEmptyStringToNull
			{
				get => _container._nullableValueTypesParseEmptyStringToNull;
				set
				{
					ThrowIfLocked();
					ThrowIfAnyTypesConfigured();

					_container._nullableValueTypesParseEmptyStringToNull = value;
				}
			}


			internal override void Lock()
			{
				_locked = true;
			}

			public override void UseDefault<T>(MissingTryParseHandling missingTryParse)
			{
				ThrowIfLocked();

				if (missingTryParse < MissingTryParseHandling.ThrowNotSupportedException || missingTryParse > MissingTryParseHandling.WrapParseInTryCatch)
					throw new ArgumentOutOfRangeException(nameof(missingTryParse), "Invalid MissingTryParseHandling value.");

				Type type = typeof(T);

				ThrowIfTypeNotConfigurable(type);
				ThrowIfTypeAlreadyConfigured(type);

				_container._parsers[type] = _container.CreateParserObject<T>();
			}

			public override void UseFunc<T>(ParseFunc<T> parseFunc, MissingTryParseHandling missingTryParse)
			{
				ThrowIfLocked();

				if (parseFunc == null)
					throw new ArgumentNullException(nameof(parseFunc));
				if (missingTryParse < MissingTryParseHandling.ThrowNotSupportedException || missingTryParse > MissingTryParseHandling.WrapParseInTryCatch)
					throw new ArgumentOutOfRangeException(nameof(missingTryParse), "Invalid MissingTryParseHandling value.");

				Type type = typeof(T);

				ThrowIfTypeNotConfigurable(type);
				ThrowIfTypeAlreadyConfigured(type);

				_container._parsers[type] = _container.AltCreateParserObject(parseFunc, null, missingTryParse);
			}

			public override void UseFunc<T>(ParseFunc<T> parseFunc, TryParseFunc<T> tryParseFunc)
			{
				ThrowIfLocked();

				if (parseFunc == null)
					throw new ArgumentNullException(nameof(parseFunc));
				if (tryParseFunc == null)
					throw new ArgumentNullException(nameof(tryParseFunc));

				Type type = typeof(T);

				ThrowIfTypeNotConfigurable(type);
				ThrowIfTypeAlreadyConfigured(type);

				_container._parsers[type] = _container.AltCreateParserObject(parseFunc, tryParseFunc, _container._missingTryParse);
			}

			public override void UseParserObject<T>(IParser<T> parserObject)
			{
				ThrowIfLocked();

				if (parserObject == null)
					throw new ArgumentNullException(nameof(parserObject));

				Type type = typeof(T);

				ThrowIfTypeNotConfigurable(type);
				ThrowIfTypeAlreadyConfigured(type);

				_container._parsers[type] = parserObject;
			}

			public override void UseTypeConverter<T>(MissingTryParseHandling missingTryParse)
			{
				ThrowIfLocked();

				Type type = typeof(T);

				ThrowIfTypeNotConfigurable(type);
				ThrowIfTypeAlreadyConfigured(type);

				TypeConverter converter = TypeDescriptor.GetConverter(type);
				if (converter == null)
					throw new OsmoticConfigurationException("The type does not have a type converter.");

				if (!converter.CanConvertFrom(typeof(string)))
					throw new OsmoticConfigurationException("The type converter does not support conversion from string.");

				ParseFunc<T> parseFunc = CreateParseFuncForTypeConverter<T>(converter);
				TryParseFunc<T> tryParseFunc = CreateTryParseFuncForTypeConverter<T>(converter, missingTryParse);

				_container._parsers[type] = _container.AltCreateParserObject(parseFunc, tryParseFunc, missingTryParse);
			}


			private void ThrowIfLocked()
			{
				if (_locked)
					throw new OsmoticConfigurationException("Configuration is locked.");
			}

			private void ThrowIfTypeNotConfigurable(Type type)
			{
				if (Nullable.GetUnderlyingType(type) is Type underlyingValueType)
					throw new ArgumentException($"Cannot accept configuration for nullable of '{underlyingValueType.FullName}'.  Configure '{underlyingValueType.FullName}' instead.");

#pragma warning disable IDE0008 // Use explicit type: Declared type differs by framework/version so 'var' is necessary.
				var typeInfo = type.GetTypeInfo();
#pragma warning restore IDE0008 // Use explicit type

				if (typeInfo.IsInterface)
					throw new ArgumentException($"Cannot accept configuration for '{type.FullName}' because interfaces are not supported.");

				if (typeInfo.IsSubclassOf(typeof(Delegate)))
					throw new ArgumentException($"Cannot accept configuration for '{type.FullName}' because delegates are not supported.");
			}

			private void ThrowIfAnyTypesConfigured()
			{
				if (_container._parsers.Count > 0)
					throw new OsmoticConfigurationException("Cannot modify option properties after a type has been configured.");
			}

			private void ThrowIfTypeAlreadyConfigured(Type type)
			{
				if (_container._parsers.ContainsKey(type))
					throw new OsmoticConfigurationException("Type is already configured.");
			}
		}
	}
}
