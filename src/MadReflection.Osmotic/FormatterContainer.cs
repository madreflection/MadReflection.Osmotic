using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace MadReflection.Osmotic
{
	/// <summary>
	/// Provides formatting functionality.
	/// </summary>
	public sealed partial class FormatterContainer : IFormatterContainer
	{
		private MissingFormatSpecificHandling _missingFormatSpecific = MissingFormatSpecificHandling.ThrowNotSupportedException;
		private bool _referenceTypesFormatNullToNull = false;
		private NullFormatHandling _nullableValueTypesFormatNull = NullFormatHandling.ThrowArgumentNullException;
		private CultureInfo _defaultCultureInfo = CultureInfo.InvariantCulture;
		private Dictionary<Type, IFormatter> _formatters = new Dictionary<Type, IFormatter>();


		private FormatterContainer()
		{
		}


		/// <summary>
		/// Gets a <see cref="FormatterContainer"/> instance with the default configuration.
		/// </summary>
		public static FormatterContainer Default { get; } = Create(config => { });

		/// <summary>
		/// Creates a <see cref="FormatterContainer"/> instance with the default configuration.  Alternatively, use <see cref="Default"/>.
		/// </summary>
		/// <returns>A new <see cref="FormatterContainer"/> instance.</returns>
		public static FormatterContainer Create(FormatterConfigurator configurator)
		{
			if (configurator == null)
				throw new ArgumentNullException(nameof(configurator));

			FormatterContainer container = new FormatterContainer();

			FormatterConfiguration configuration = new Configuration(container);

			configurator(configuration);

			configuration.Lock();

			return container;
		}


		/// <summary>
		/// Gets a formatting interface for the specified type.
		/// </summary>
		/// <param name="type">The type that the requested object formats.</param>
		/// <returns>An <see cref="IFormatter"/> reference to a formatting object for type <paramref name="type"/> that also implements <see cref="IFormatter{T}"/> where 'T' is <paramref name="type"/>.</returns>
		public IFormatter For(Type type)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));

			return ForInternal(type);
		}

		/// <summary>
		/// Gets a formatting interface for the specified type.
		/// </summary>
		/// <typeparam name="T">The type that the requested object formats.</typeparam>
		/// <returns>An <see cref="IFormatter{T}"/> reference to a parsing object for <typeparamref name="T"/>.</returns>
		public IFormatter<T> For<T>() => (IFormatter<T>)ForInternal(typeof(T));

		private IFormatter ForInternal(Type type)
		{
			if (!_formatters.TryGetValue(type, out IFormatter formatter))
			{
				lock (_formatters)
				{
					if (!_formatters.TryGetValue(type, out formatter))
					{
						MethodInfo method = ReflectionHelper.GetPrivateGenericMethod(typeof(FormatterContainer), nameof(CreateFormatterObject), type);
						Func<IFormatter> formatterObjectFactory = method.CreateDelegate<Func<IFormatter>>(this);
						formatter = formatterObjectFactory();

						_formatters[type] = formatter;
					}
				}
			}

			return formatter;
		}

		private IFormatter<T> CreateFormatterObject<T>()
		{
			Type type = typeof(T);

			MethodInfo factoryMethod;
			if (Nullable.GetUnderlyingType(type) is Type underlyingValueType)
				factoryMethod = ReflectionHelper.GetPrivateGenericMethod(typeof(FormatterContainer), nameof(GetNullableValueTypeFormatterObject), underlyingValueType);
			else if (type.GetTypeInfo().IsValueType)
				factoryMethod = ReflectionHelper.GetPrivateGenericMethod(typeof(FormatterContainer), nameof(GetValueTypeFormatterObject), type);
			else
				factoryMethod = ReflectionHelper.GetPrivateGenericMethod(typeof(FormatterContainer), nameof(GetReferenceTypeFormatterObject), type);

			Func<IFormatter<T>> factory = factoryMethod.CreateDelegate<Func<IFormatter<T>>>(this);

			return factory();
		}

		private IFormatter<T?> GetNullableValueTypeFormatterObject<T>()
			where T : struct
		{
			IFormatter<T> valueTypeformatter = (IFormatter<T>)ForInternal(typeof(T));

			FormatFunc<T?> formatFunc;
			if (_nullableValueTypesFormatNull == NullFormatHandling.ReturnNull)
				formatFunc = value => value != null ? valueTypeformatter.Format(value) : null;
			else if (_nullableValueTypesFormatNull == NullFormatHandling.ReturnEmptyString)
				formatFunc = value => value != null ? valueTypeformatter.Format(value) : "";
			else
				formatFunc = value => value != null ? valueTypeformatter.Format(value) : throw new ArgumentNullException(nameof(value));

			FormatSpecificFunc<T?> formatSpecificFunc;
			if (_nullableValueTypesFormatNull == NullFormatHandling.ReturnNull)
				formatSpecificFunc = (value, format) => value != null ? valueTypeformatter.Format(value, format) : null;
			else if (_nullableValueTypesFormatNull == NullFormatHandling.ReturnEmptyString)
				formatSpecificFunc = (value, format) => value != null ? valueTypeformatter.Format(value, format) : "";
			else
				formatSpecificFunc = (value, format) => value != null ? valueTypeformatter.Format(value, format) : throw new ArgumentNullException(nameof(value));

			return new FunctorFormatterObject<T?>(formatFunc, formatSpecificFunc);
		}

		private IFormatter<T> GetValueTypeFormatterObject<T>()
			where T : struct
		{
			//Type type = typeof(T);

			// Enums don't need to be handled differently.
			//if (type.GetTypeInfo().IsEnum)
			//	return GetEnumTypeFormatterObject<T>();

			FormatFunc<T> formatFunc = GetDefaultValueTypeFormatFunc<T>();
			FormatSpecificFunc<T> formatSpecificFunc = GetDefaultValueTypeFormatSpecificFunc<T>();

			return ApplyOptionsAndGetValueTypeFormatterObject(formatFunc, formatSpecificFunc, _missingFormatSpecific);
		}

		private IFormatter<T> GetReferenceTypeFormatterObject<T>()
			where T : class
		{
			if (typeof(T) == typeof(string))
				return (IFormatter<T>)new StringFormatter();

			FormatFunc<T> formatFunc = GetDefaultReferenceTypeFormatFunc<T>();
			FormatSpecificFunc<T> formatSpecificFunc = GetDefaultReferenceTypeFormatSpecificFunc<T>();

			return ApplyOptionsAndGetReferenceTypeFormatterObject(formatFunc, formatSpecificFunc, _missingFormatSpecific);
		}

		private IFormatter<T> AltCreateFormatterObject<T>(FormatFunc<T> formatFunc, FormatSpecificFunc<T> formatSpecificFunc, MissingFormatSpecificHandling missingFormatSpecific)
		{
			Type type = typeof(T);

			string methodName = type.GetTypeInfo().IsValueType ? nameof(ApplyOptionsAndGetValueTypeFormatterObject) : nameof(ApplyOptionsAndGetReferenceTypeFormatterObject);
			MethodInfo factoryMethod = ReflectionHelper.GetPrivateGenericMethod(typeof(FormatterContainer), methodName, type);

			Func<FormatFunc<T>, FormatSpecificFunc<T>, MissingFormatSpecificHandling, IFormatter<T>> factory = factoryMethod.CreateDelegate<Func<FormatFunc<T>, FormatSpecificFunc<T>, MissingFormatSpecificHandling, IFormatter<T>>>(this);
			return factory(formatFunc, formatSpecificFunc, missingFormatSpecific);
		}

		private IFormatter<T> ApplyOptionsAndGetValueTypeFormatterObject<T>(FormatFunc<T> formatFunc, FormatSpecificFunc<T> formatSpecificFunc, MissingFormatSpecificHandling missingFormatSpecific)
		{
			FormatFunc<T> valueTypeFormatFunc = formatFunc;
			FormatSpecificFunc<T> valueTypeFormatSpecificFunc = formatSpecificFunc;

			// formatFunc will never be null because ToString() is always available.

			if (formatSpecificFunc == null)
			{
				if (_missingFormatSpecific == MissingFormatSpecificHandling.ThrowNotSupportedException)
					valueTypeFormatSpecificFunc = FormatSpecificThrowsNotSupportedException;
				else if (_missingFormatSpecific == MissingFormatSpecificHandling.ReturnEmptyString)
					valueTypeFormatSpecificFunc = FormatSpecificReturnsEmptyString;
				else if (_missingFormatSpecific == MissingFormatSpecificHandling.ReturnNull)
					valueTypeFormatSpecificFunc = FormatSpecificReturnsNull;
				else
					valueTypeFormatSpecificFunc = (value, format) => valueTypeFormatFunc(value);
			}

			return new ValueTypeFunctorFormatterObject<T>(valueTypeFormatFunc, valueTypeFormatSpecificFunc);
		}

		private IFormatter<T> ApplyOptionsAndGetReferenceTypeFormatterObject<T>(FormatFunc<T> formatFunc, FormatSpecificFunc<T> formatSpecificFunc, MissingFormatSpecificHandling missingFormatSpecific)
		{
			FormatFunc<T> referenceTypeFormatFunc = formatFunc;
			FormatSpecificFunc<T> referenceTypeFormatSpecificFunc = formatSpecificFunc;

			// formatFunc will never be null because ToString() is always available.
			if (_referenceTypesFormatNullToNull)
				referenceTypeFormatFunc = value => value != null ? formatFunc(value) : null;

			if (formatSpecificFunc == null)
			{
				if (missingFormatSpecific == MissingFormatSpecificHandling.UseToString)
					referenceTypeFormatSpecificFunc = (value, format) => referenceTypeFormatFunc(value);
				else if (missingFormatSpecific == MissingFormatSpecificHandling.ReturnNull)
					referenceTypeFormatSpecificFunc = FormatSpecificReturnsNull;
				else if (missingFormatSpecific == MissingFormatSpecificHandling.ReturnEmptyString)
					referenceTypeFormatSpecificFunc = FormatSpecificReturnsEmptyString;
				else
					referenceTypeFormatSpecificFunc = FormatSpecificThrowsNotSupportedException;
			}

			return new FunctorFormatterObject<T>(referenceTypeFormatFunc, referenceTypeFormatSpecificFunc);
		}

		private FormatFunc<T> GetDefaultValueTypeFormatFunc<T>() where T : struct => value => value.ToString();

		private FormatSpecificFunc<T> GetDefaultValueTypeFormatSpecificFunc<T>()
			where T : struct
		{
			Type type = typeof(T);

			if (type.GetInterfaces().Contains(typeof(IFormattable)))
				return (value, format) => ((IFormattable)value).ToString(format, _defaultCultureInfo);

			MethodInfo method = (
				from m in type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
				where m.Name == Constants.ToStringName
				where m.ReturnType == typeof(string)
				let parameters = m.GetParameters()
				where parameters.Length == 2
				where parameters[0].ParameterType == typeof(string)
				where parameters[1].ParameterType == typeof(IFormatProvider)
				select m
				).SingleOrDefault();

			if (method != null)
			{
				ValueTypeSolitaryToStringFormatWithProviderFunc<T> publicIFormattableToStringFunc = method.CreateDelegate<ValueTypeSolitaryToStringFormatWithProviderFunc<T>>();

				return (value, format) => publicIFormattableToStringFunc(ref value, format, _defaultCultureInfo);
			}

			method = (
				from m in type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
				where m.Name == Constants.ToStringName
				where m.ReturnType == typeof(string)
				let parameters = m.GetParameters()
				where parameters.Length == 1
				where parameters[0].ParameterType == typeof(string)
				select m
				).SingleOrDefault();

			if (method != null)
			{
				ValueTypeSolitaryToStringFormatFunc<T> solitaryToStringFunc = method.CreateDelegate<ValueTypeSolitaryToStringFormatFunc<T>>();

				return (value, format) => solitaryToStringFunc(ref value, format);
			}

			return null;
		}

		private FormatFunc<T> GetDefaultReferenceTypeFormatFunc<T>()
			where T : class
		{
			return value =>
			{
				if (value == null)
					throw new ArgumentNullException(nameof(value));

				return value.ToString();
			};
		}

		private FormatSpecificFunc<T> GetDefaultReferenceTypeFormatSpecificFunc<T>()
			where T : class
		{
			Type type = typeof(T);

			if (typeof(T).GetInterfaces().Contains(typeof(IFormattable)))
			{
				return (value, format) =>
				{
					if (value == null)
						throw new ArgumentNullException(nameof(value));

					return ((IFormattable)value).ToString(format, _defaultCultureInfo);
				};
			}

			MethodInfo method = (
				from m in type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
				where m.Name == nameof(object.ToString)
				where m.ReturnType == typeof(string)
				let parameters = m.GetParameters()
				where parameters.Length == 2
				where parameters[0].ParameterType == typeof(string)
				where parameters[1].ParameterType == typeof(IFormatProvider)
				select m
				).SingleOrDefault();

			if (method != null)
			{
				Func<T, string, IFormatProvider, string> publicIFormattableToStringFunc = method.CreateDelegate<Func<T, string, IFormatProvider, string>>();

				return (value, format) =>
				{
					if (value == null)
						throw new ArgumentNullException(nameof(value));

					return publicIFormattableToStringFunc(value, format, _defaultCultureInfo);
				};
			}

			method = (
				from m in type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
				where m.Name == Constants.ToStringName
				where m.ReturnType == typeof(string)
				let parameters = m.GetParameters()
				where parameters.Length == 1
				where parameters[0].ParameterType == typeof(string)
				select m
				).SingleOrDefault();

			if (method != null)
			{
				Func<T, string, string> solitaryToStringFunc = method.CreateDelegate<Func<T, string, string>>();

				return (value, format) =>
				{
					if (value == null)
						throw new ArgumentNullException(nameof(value));

					return solitaryToStringFunc(value, format);
				};
			}

			return null;
		}

		private static FormatFunc<T> CreateFormatFuncForTypeConverter<T>(TypeConverter converter) => value => (string)converter.ConvertTo(value, typeof(string));

		private static FormatSpecificFunc<T> CreateFormatSpecificFuncForTypeConverter<T>(TypeConverter converter, MissingFormatSpecificHandling missingFormatSpecific) => null;

		private static string FormatSpecificThrowsNotSupportedException<T>(T value, string format) => throw new NotSupportedException();

		private static string FormatSpecificReturnsNull<T>(T value, string format) => null;

		private static string FormatSpecificReturnsEmptyString<T>(T value, string format) => "";
	}
}
