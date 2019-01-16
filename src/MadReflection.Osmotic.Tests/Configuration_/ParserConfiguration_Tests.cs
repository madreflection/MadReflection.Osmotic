using System;
using Moq;
using NUnit.Framework;

namespace MadReflection.Osmotic.Tests
{
	[TestFixture]
	public class ParserConfiguration_Tests
	{
		[TestCase]
		public void Configuration_Null_Configurator_Throws_ArgumentNullException()
		{
			// Arrange

			// Act
			TestDelegate test = () => ParserContainer.Create(null);

			// Assert
			Assert.That(test, Throws.ArgumentNullException);
		}

		[TestCase]
		public void Configuration_Null_ParseFunc_Throws_ArgumentNullException()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseFunc<int>(null);
				});
			};

			// Assert
			Assert.That(test, Throws.ArgumentNullException);
		}

		[TestCase]
		public void Configuration_Null_TryParseFunc_Throws_ArgumentNullException()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseFunc(s => 0, null);
				});
			};

			// Assert
			Assert.That(test, Throws.ArgumentNullException);
		}

		[TestCase]
		public void Configuration_Null_ParserObject_Throws_ArgumentNullException()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseParserObject<int>(null);
				});
			};

			// Assert
			Assert.That(test, Throws.ArgumentNullException);
		}

		[TestCase]
		public void Configuration_Rejects_ParseFunc_For_NullableValueType()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseFunc<int?>(s => null);
				});
			};

			// Assert
			Assert.That(test, Throws.InstanceOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_ParseFunc_For_Delegate()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseFunc<Func<int>>(s => null);
				});
			};

			// Assert
			Assert.That(test, Throws.InstanceOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_ParseFunc_For_Interface()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseFunc<IFormattable>(s => null);
				});
			};

			// Assert
			Assert.That(test, Throws.InstanceOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_ParserObject_For_NullableValueType()
		{
			// Arrange
			Mock<IParser<int?>> parserObjectMock = new Mock<IParser<int?>>();
			IParser<int?> parserObject = parserObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseParserObject(parserObject);
				});
			};

			// Assert
			Assert.That(test, Throws.InstanceOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_ParserObject_For_Delegate()
		{
			// Arrange
			Mock<IParser<Func<int>>> parserObjectMock = new Mock<IParser<Func<int>>>();
			IParser<Func<int>> parserObject = parserObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseParserObject(parserObject);
				});
			};

			// Assert
			Assert.That(test, Throws.InstanceOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_ParserObject_For_Interface()
		{
			// Arrange
			Mock<IParser<IFormattable>> parserObjectMock = new Mock<IParser<IFormattable>>();
			IParser<IFormattable> parserObject = parserObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseParserObject(parserObject);
				});
			};

			// Assert
			Assert.That(test, Throws.InstanceOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_Default_And_Func()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseDefault<int>();
					config.UseFunc(
						s => 0,
						(string s, out int r) =>
						{
							r = 0;
							return true;
						});
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_Default_And_ParserObject()
		{
			// Arrange
			Mock<IParser<int>> parserObjectMock = new Mock<IParser<int>>();
			IParser<int> parserObject = parserObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseDefault<int>();
					config.UseParserObject(parserObject);
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_Default_And_TypeConverter()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseDefault<int>();
					config.UseTypeConverter<int>();
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_Func_And_Default()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseFunc(s => 0);
					config.UseDefault<int>();
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_Func_And_ParserObject()
		{
			// Arrange
			Mock<IParser<int>> parserObjectMock = new Mock<IParser<int>>();
			IParser<int> parserObject = parserObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseFunc(s => 0);
					config.UseParserObject(parserObject);
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_Func_And_TypeConverter()
		{
			// Arrange

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseFunc(s => 0);
					config.UseTypeConverter<int>();
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_ParserObject_And_Default()
		{
			// Arrange
			Mock<IParser<int>> parserObjectMock = new Mock<IParser<int>>();
			IParser<int> parserObject = parserObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseParserObject(parserObject);
					config.UseDefault<int>();
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_ParserObject_And_Func()
		{
			// Arrange
			Mock<IParser<int>> parserObjectMock = new Mock<IParser<int>>();
			IParser<int> parserObject = parserObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseParserObject(parserObject);
					config.UseFunc(s => 0);
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_ParserObject_And_TypeConverter()
		{
			// Arrange
			Mock<IParser<int>> parserObjectMock = new Mock<IParser<int>>();
			IParser<int> parserObject = parserObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseParserObject(parserObject);
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

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
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

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseTypeConverter<int>();
					config.UseFunc(s => 0);
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_Rejects_Mix_Of_TypeConverter_And_ParserObject()
		{
			// Arrange
			Mock<IParser<int>> parserObjectMock = new Mock<IParser<int>>();
			IParser<int> parserObject = parserObjectMock.Object;

			// Act
			TestDelegate test = () =>
			{
				ParserContainer.Create(config =>
				{
					config.UseTypeConverter<int>();
					config.UseDefault<int>();
				});
			};

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_MissingTryParse_Throws_If_Configuration_Locked()
		{
			// Arrange
			ParserConfiguration outerConfig = null;
			ParserContainer.Create(config =>
			{
				outerConfig = config;
			});

			// Act
			TestDelegate test = () => outerConfig.MissingTryParse = MissingTryParseHandling.ReturnFalse;

			// Assert
		}

		[TestCase]
		public void Configuration_ReferenceTypesParseNullToNull_Throws_If_Configuration_Locked()
		{
			// Arrange
			ParserConfiguration outerConfig = null;
			ParserContainer.Create(config =>
			{
				outerConfig = config;
			});

			// Act
			TestDelegate test = () => outerConfig.ReferenceTypesParseNullToNull = true;

			// Assert
		}

		[TestCase]
		public void Configuration_NullableValueTypesParseNullToNull_Throws_If_Configuration_Locked()
		{
			// Arrange
			ParserConfiguration outerConfig = null;
			ParserContainer.Create(config =>
			{
				outerConfig = config;
			});

			// Act
			TestDelegate test = () => outerConfig.NullableValueTypesParseNullToNull = true;

			// Assert
		}

		[TestCase]
		public void Configuration_NullableValueTypesParseEmptyStringToNull_Throws_If_Configuration_Locked()
		{
			// Arrange
			ParserConfiguration outerConfig = null;
			ParserContainer.Create(config =>
			{
				outerConfig = config;
			});

			// Act
			TestDelegate test = () => outerConfig.NullableValueTypesParseEmptyStringToNull = true;

			// Assert
		}

		[TestCase]
		public void Configuration_UseDefault_Throws_If_Configuration_Locked()
		{
			// Arrange
			ParserConfiguration outerConfig = null;
			ParserContainer.Create(config =>
			{
				outerConfig = config;
			});

			// Act
			TestDelegate test = () => outerConfig.UseDefault<string>();

			// Assert
		}

		[TestCase]
		public void Configuration_UseFunc_Throws_If_Configuration_Locked()
		{
			// Arrange
			ParserConfiguration outerConfig = null;
			ParserContainer.Create(config =>
			{
				outerConfig = config;
			});

			// Act
			TestDelegate test = () => outerConfig.UseFunc(s => "");

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_UseParserObject_Throws_If_Configuration_Locked()
		{
			// Arrange
			ParserConfiguration outerConfig = null;
			ParserContainer.Create(config =>
			{
				outerConfig = config;
			});
			Mock<IParser<string>> parserObjectMock = new Mock<IParser<string>>();
			IParser<string> parserObject = parserObjectMock.Object;

			// Act
			TestDelegate test = () => outerConfig.UseParserObject(parserObject);

			// Assert
			Assert.That(test, Throws.TypeOf<OsmoticConfigurationException>());
		}

		[TestCase]
		public void Configuration_UseTypeConverter_Throws_If_Configuration_Locked()
		{
			// Arrange
			ParserConfiguration outerConfig = null;
			ParserContainer.Create(config =>
			{
				outerConfig = config;
			});

			// Act
			TestDelegate test = () => outerConfig.UseTypeConverter<string>();

			// Assert
		}
	}
}
