using System;
using Moq;
using NUnit.Framework;

namespace MadReflection.Osmotic.Tests
{
	[TestFixture]
	public class FormatterConfiguration_Tests
	{
		[TestCase]
		public void Configuration_Null_Configurator_Throws_NullArgumentException()
		{
			// Arrange

			// Act
			TestDelegate test = () => FormatterContainer.Create(null);

			// Assert
			Assert.That(test, Throws.ArgumentNullException);
		}

		[TestCase]
		public void Configuration_Null_FormatFunc_Throws_NullArgumentException()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseFunc<int>(null);
				});
			};

			// Assert
			Assert.That(test, Throws.ArgumentNullException);
		}

		[TestCase]
		public void Configuration_Null_FormatSpecificFunc_Throws_NullArgumentException()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseFunc<int>(i => "", null);
				});
			};

			// Assert
			Assert.That(test, Throws.ArgumentNullException);
		}

		[TestCase]
		public void Configuration_Null_FormatterObject_Throws_NullArgumentException()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseFormatterObject<int>(null);
				});
			};

			// Assert
			Assert.That(test, Throws.ArgumentNullException);
		}

		[TestCase]
		public void Configuration_Rejects_FormatFunc_For_NullableValueType()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseFunc<int?>(v => "");
				});
			};

			// Assert
		}

		[TestCase]
		public void Configuration_Rejects_FormatFunc_For_Delegate()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseFunc<Func<int>>(v => "");
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_FormatFunc_For_Interface()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseFunc<IFormattable>(v => "");
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_FormatterObject_For_NullableValueType()
		{
			// Arrange
			Mock<IFormatter<int?>> formatterObjectMock = new Mock<IFormatter<int?>>();
			IFormatter<int?> formatterObject = formatterObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseFormatterObject(formatterObject);
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_FormatterObject_For_Delegate()
		{
			// Arrange
			Mock<IFormatter<Func<int>>> formatterObjectMock = new Mock<IFormatter<Func<int>>>();
			IFormatter<Func<int>> formatterObject = formatterObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseFormatterObject(formatterObject);
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_FormatterObject_For_Interface()
		{
			// Arrange
			Mock<IFormatter<IFormattable>> formatterObjectMock = new Mock<IFormatter<IFormattable>>();
			IFormatter<IFormattable> formatterObject = formatterObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseFormatterObject(formatterObject);
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_Default_And_Func()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseDefault<int>();
					config.UseFunc<int>(v => "");
				});
			};

			// Assert
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_Default_And_FormatterObject()
		{
			// Arrange
			Mock<IFormatter<int>> formatterObjectMock = new Mock<IFormatter<int>>();
			IFormatter<int> formatterObject = formatterObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseDefault<int>();
					config.UseFormatterObject(formatterObject);
				});
			};

			// Assert
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_Default_And_TypeConverter()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseDefault<int>();
					config.UseTypeConverter<int>();
				});
			};

			// Assert
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_Func_And_Default()
		{
			// Arrange
			Mock<IFormatter<int>> formatterObjectMock = new Mock<IFormatter<int>>();
			IFormatter<int> formatterObject = formatterObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseFunc<int>(v => "");
					config.UseDefault<int>();
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_Func_And_FormatterObject()
		{
			// Arrange
			Mock<IFormatter<int>> formatterObjectMock = new Mock<IFormatter<int>>();
			IFormatter<int> formatterObject = formatterObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseFunc<int>(v => "");
					config.UseFormatterObject(formatterObject);
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_Func_And_TypeConverter()
		{
			// Arrange
			Mock<IFormatter<int>> formatterObjectMock = new Mock<IFormatter<int>>();
			IFormatter<int> formatterObject = formatterObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseFunc<int>(v => "");
					config.UseTypeConverter<int>();
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_FormatterObject_And_Default()
		{
			// Arrange
			Mock<IFormatter<int>> formatterObjectMock = new Mock<IFormatter<int>>();
			IFormatter<int> formatterObject = formatterObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseFormatterObject(formatterObject);
					config.UseDefault<int>();
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_FormatterObject_And_Func()
		{
			// Arrange
			Mock<IFormatter<int>> formatterObjectMock = new Mock<IFormatter<int>>();
			IFormatter<int> formatterObject = formatterObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseFormatterObject(formatterObject);
					config.UseFunc<int>(c => "");
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_FormatterObject_And_TypeConverter()
		{
			// Arrange
			Mock<IFormatter<int>> formatterObjectMock = new Mock<IFormatter<int>>();
			IFormatter<int> formatterObject = formatterObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseFormatterObject(formatterObject);
					config.UseTypeConverter<int>();
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_TypeConverter_And_Default()
		{
			// Arrange
			Mock<IFormatter<int>> formatterObjectMock = new Mock<IFormatter<int>>();
			IFormatter<int> formatterObject = formatterObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseTypeConverter<int>();
					config.UseDefault<int>();
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_TypeConverter_And_Func()
		{
			// Arrange
			Mock<IFormatter<int>> formatterObjectMock = new Mock<IFormatter<int>>();
			IFormatter<int> formatterObject = formatterObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseTypeConverter<int>();
					config.UseFunc<int>(v => "");
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_TypeConverter_And_FormatterObject()
		{
			// Arrange
			Mock<IFormatter<int>> formatterObjectMock = new Mock<IFormatter<int>>();
			IFormatter<int> formatterObject = formatterObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				FormatterContainer.Create(config =>
				{
					config.UseTypeConverter<int>();
					config.UseFormatterObject(formatterObject);
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_MissingFormatSpecific_Throws_If_Configuration_Locked()
		{
			// Arrange
			FormatterConfiguration outerConfig = null;
			FormatterContainer.Create(config =>
			{
				outerConfig = config;
			});

			// Act
			TestDelegate test = () => outerConfig.MissingFormatSpecific = MissingFormatSpecificHandling.ReturnEmptyString;

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_ReferenceTypesFormatNullToNull_Throws_If_Configuration_Locked()
		{
			// Arrange
			FormatterConfiguration outerConfig = null;
			FormatterContainer.Create(config =>
			{
				outerConfig = config;
			});

			// Act
			TestDelegate test = () => outerConfig.ReferenceTypesFormatNullToNull = true;

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_NullableValueTypesFormatNull_Throws_If_Configuration_Locked()
		{
			// Arrange
			FormatterConfiguration outerConfig = null;
			FormatterContainer.Create(config =>
			{
				outerConfig = config;
			});

			// Act
			TestDelegate test = () => outerConfig.NullableValueTypesFormatNull = NullFormatHandling.ReturnEmptyString;

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_UseDefault_Throws_If_Configuration_Locked()
		{
			// Arrange
			FormatterConfiguration outerConfig = null;
			FormatterContainer.Create(config =>
			{
				outerConfig = config;
			});

			// Act
			TestDelegate test = () => outerConfig.UseDefault<string>();

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_UseFunc_Throws_If_Configuration_Locked()
		{
			// Arrange
			FormatterConfiguration outerConfig = null;
			FormatterContainer.Create(config =>
			{
				outerConfig = config;
			});

			// Act
			TestDelegate test = () => outerConfig.UseFunc<string>(value => "");

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_UseFormatterObject_Throws_If_Configuration_Locked()
		{
			// Arrange
			FormatterConfiguration outerConfig = null;
			FormatterContainer.Create(config =>
			{
				outerConfig = config;
			});

			// Act
			TestDelegate test = () => outerConfig.UseFunc<string>(value => "");

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_UseTypeConverter_Throws_If_Configuration_Locked()
		{
			// Arrange
			FormatterConfiguration outerConfig = null;
			FormatterContainer.Create(config =>
			{
				outerConfig = config;
			});

			// Act
			TestDelegate test = () => outerConfig.UseTypeConverter<string>();

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}
	}
}
