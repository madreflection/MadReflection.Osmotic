using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace MadReflection.Osmotic
{
	partial class FormatterContainer
	{
		private class Configuration : FormatterConfiguration
		{
			private bool _locked;
			private readonly FormatterContainer _container;


			public Configuration(FormatterContainer container)
			{
				_container = container;
			}


			public override MissingFormatSpecificHandling MissingFormatSpecific
			{
				get => _container._missingFormatSpecific;
				set
				{
					ThrowIfLocked();

					if (value < MissingFormatSpecificHandling.ThrowNotSupportedException || value > MissingFormatSpecificHandling.UseToString)
						throw new ArgumentOutOfRangeException(nameof(value), "Invalid MissingFormatSpecificHandling value.");

					ThrowIfAnyTypesConfigured();

					_container._missingFormatSpecific = value;
				}
			}

			public override bool ReferenceTypesFormatNullToNull
			{
				get => _container._referenceTypesFormatNullToNull;
				set
				{
					ThrowIfLocked();
					ThrowIfAnyTypesConfigured();

					_container._referenceTypesFormatNullToNull = value;
				}
			}

			public override NullFormatHandling NullableValueTypesFormatNull
			{
				get => _container._nullableValueTypesFormatNull;
				set
				{
					ThrowIfLocked();

					if (value < NullFormatHandling.ThrowArgumentNullException || value > NullFormatHandling.ReturnEmptyString)
						throw new ArgumentOutOfRangeException(nameof(value), "Invalid NullFormatHandling value.");

					ThrowIfAnyTypesConfigured();

					_container._nullableValueTypesFormatNull = value;
				}
			}

			internal override CultureInfo DefaultCultureInfo
			{
				get => _container._defaultCultureInfo;
				set
				{
					ThrowIfLocked();

					if (value is null)
						throw new ArgumentNullException(nameof(value));

					ThrowIfAnyTypesConfigured();

					_container._defaultCultureInfo = value;
				}
			}


			internal override void Lock()
			{
				_locked = true;
			}

			public override void UseDefault<T>(MissingFormatSpecificHandling missingFormatSpecific)
			{
				ThrowIfLocked();

				if (missingFormatSpecific < MissingFormatSpecificHandling.ThrowNotSupportedException || missingFormatSpecific > MissingFormatSpecificHandling.UseToString)
					throw new ArgumentOutOfRangeException(nameof(missingFormatSpecific), "Invalid MissingFormatSpecificHandling value.");

				Type type = typeof(T);

				ThrowIfTypeNotConfigurable(type);
				ThrowIfTypeAlreadyConfigured(type);

				_container._formatters[type] = _container.CreateFormatterObject<T>();
			}

			public override void UseFunc<T>(FormatFunc<T> formatFunc, MissingFormatSpecificHandling missingFormatSpecific)
			{
				ThrowIfLocked();

				if (formatFunc is null)
					throw new ArgumentNullException(nameof(formatFunc));
				if (missingFormatSpecific < MissingFormatSpecificHandling.ThrowNotSupportedException || missingFormatSpecific > MissingFormatSpecificHandling.UseToString)
					throw new ArgumentOutOfRangeException(nameof(missingFormatSpecific), "Invalid MissingFormatSpecificHandling value.");

				Type type = typeof(T);

				ThrowIfTypeNotConfigurable(type);
				ThrowIfTypeAlreadyConfigured(type);

				_container._formatters[type] = _container.AltCreateFormatterObject(formatFunc, null, missingFormatSpecific);
			}

			public override void UseFunc<T>(FormatFunc<T> formatFunc, FormatSpecificFunc<T> formatSpecificFunc)
			{
				ThrowIfLocked();

				if (formatFunc is null)
					throw new ArgumentNullException(nameof(formatFunc));
				if (formatSpecificFunc is null)
					throw new ArgumentNullException(nameof(formatSpecificFunc));

				Type type = typeof(T);

				ThrowIfTypeNotConfigurable(type);
				ThrowIfTypeAlreadyConfigured(type);

				_container._formatters[type] = _container.AltCreateFormatterObject(formatFunc, formatSpecificFunc, _container._missingFormatSpecific);
			}

			public override void UseFormatterObject<T>(IFormatter<T> formatterObject)
			{
				ThrowIfLocked();

				if (formatterObject is null)
					throw new ArgumentNullException(nameof(formatterObject));

				Type type = typeof(T);

				ThrowIfTypeNotConfigurable(type);
				ThrowIfTypeAlreadyConfigured(type);

				_container._formatters[type] = formatterObject;
			}

			public override void UseTypeConverter<T>(MissingFormatSpecificHandling missingFormatSpecific)
			{
				ThrowIfLocked();

				Type type = typeof(T);

				ThrowIfTypeNotConfigurable(type);
				ThrowIfTypeAlreadyConfigured(type);

				TypeConverter converter = TypeDescriptor.GetConverter(type);
				if (converter is null)
					throw new InvalidOperationException($"A type converter is not available for '{type.Name}'.");

				if (!converter.CanConvertTo(typeof(string)))
					throw new InvalidOperationException("The type converter does not support conversion to string.");

				FormatFunc<T> formatFunc = CreateFormatFuncForTypeConverter<T>(converter);
				FormatSpecificFunc<T> formatSpecificFunc = CreateFormatSpecificFuncForTypeConverter<T>(converter, missingFormatSpecific);

				_container._formatters[type] = _container.AltCreateFormatterObject(formatFunc, formatSpecificFunc, missingFormatSpecific);
			}


			private void ThrowIfLocked()
			{
				if (_locked)
					throw new OsmoticConfigurationException("Configuration is locked.");
			}

			private void ThrowIfTypeNotConfigurable(Type type)
			{
				if (Nullable.GetUnderlyingType(type) is Type underlyingValueType)
					throw new OsmoticConfigurationException($"Cannot accept configuration for nullable of '{underlyingValueType.FullName}'.  Configure '{underlyingValueType.FullName}' instead.");

#pragma warning disable IDE0008 // Use explicit type: Declared type differs by framework/version so 'var' is necessary.
				var typeInfo = type.GetTypeInfo();
#pragma warning restore IDE0008 // Use explicit type

				if (typeInfo.IsInterface)
					throw new OsmoticConfigurationException($"Cannot accept configuration for '{type.FullName}' because interfaces are not supported.");

				if (typeInfo.IsSubclassOf(typeof(Delegate)))
					throw new OsmoticConfigurationException($"Cannot accept configuration for '{type.FullName}' because delegates are not supported.");
			}

			private void ThrowIfAnyTypesConfigured()
			{
				if (_container._formatters.Count > 0)
					throw new OsmoticConfigurationException("Cannot modify option properties after a type has been configured.");
			}

			private void ThrowIfTypeAlreadyConfigured(Type type)
			{
				if (_container._formatters.ContainsKey(type))
					throw new OsmoticConfigurationException("Type is already configured.");
			}
		}
	}
}
