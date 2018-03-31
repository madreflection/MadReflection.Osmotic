# Osmotic

Osmotic is a library for converting to and from strings with a consistent interface.  It was originally designed for storing variants in string format in a database and handling data transformation to and from a user interface.

By convention, the parser uses the static `Parse` and `TryParse` methods provided by each type to convert from strings, and the formatter uses the `ToString` methods provided by each type (including explicit implementations of `IFormattable`) to convert to strings.  Osmotic does this automatically for any type.  Configuration of a type is only necessary if it does not follow this convention or if custom behavior is needed.

## Supported Frameworks

* .NET Framework 3.5 (full and client profile)
* .NET Framework 4.0 (full and client profile)
* .NET Framework 4.5
* Any implementation of .NET Standard 1.3
* Any implementation of .NET Standard 2.0 (with fewer dependencies than targeting 1.3-1.6)

## Getting Started

### Install NuGet Package

```powershell
Install-Package MadReflection.Osmotic
```

*Note: The package is not yet on NuGet.  This command will work once it's been published.*

### Expose Osmotic To Your Application

Osmotic requires an instance in order to be used.  A parser or formatter with the default configuration can be found in the static `Default` property.  To create a configured instance, call the static `Create` method.

The simplest way to expose Osmotic instances within your application is with static properties in a static class.

```csharp
internal static class MyParser
{
    private static ParserContainer _parser = ParserContainer.Create(config =>
    {
        // ...configure...
    });

    public static IParser For(Type type) => _parser.For(type);

    public static IParser<T> For<T>() => _parser.For<T>();
}

internal static class MyFormatter
{
    private static FormatterContainer _formatter = FormatterContainer.Create(config =>
    {
        // ...configure...
    });

    public static IFormatter For(Type type) => _formatter.For(type);

    public static IFormatter<T> For<T>() => _formatter.For<T>();
}
```

The `ParserContainer` and `FormatterContainer` classes should be instantiated only once for each unique configuration.

### Using Osmotic In Your Application

Parsers and formatters are generally used in a chained expression.

```text
[container] -> [interface] -> [method]
```

The container is the `ParserContainer` or `FormatterContainer` instance, which may be exposed by a static class (as shown earlier).  From the container, a `For` method is called to retrieve an interface.  From that interface, one of the parsing or formatting methods is called.

The interface can be retrieved using either a type instance argument or a generic type argument:

```csharp
// Using a type instance argument:
Type type = typeof(int);
int parseResult = MyParser.For(type).Parse("1");
string formatResult = MyFormatter.For(type).Format(1);

// Using a generic type argument:
int parseResult = MyParser.For<int>().Parse("1");
string formatResult = MyFormatter.For<int>().Format(1);
```

While the generic forms are more expressive and avoid boxing of value types, the non-generic forms offer additional flexibility.

### Customization

Osmotic has several customization options.  See the Wiki for details.

### A Note About Dependency Injection

The `Parser` and `Formatter` classes implement the `IParserContainer` and `IFormatterContainer` interfaces, respectively.  However, these interfaces are not very useful for dependency injection purposes.  Multiple instances of either class could be used by an application but only one could be bound to the interface, making just one available and the other not at all.

If you intend to use Osmotic in a dependency-injected manner, consider defining an interface for each purpose-customized instance and encapsulate it in an implementation of that interface that forwards to the Osomtic instance.  For example, a parser configured for storing variant data in a string column might look like this:

```csharp
public interface IDBParser
{
    // These signatures match those in IParserContainer but they are not
    // inherited from it so as to avoid binding issues.
    IParser For(Type type);
    IParser<T> For<T>();
}

public class DBParser : IDBParser
{
    private readonly Parser _parser = Parser.Create(config =>
    {
        // Configure for this purpose.
    });

    public IParser For(Type type) => _parser.For(type);

    public IParser<T> For<T>() =>_parser.For<T>();
}

// Then bind IDBParser to DBParser.
```

## Further Reading

The Wiki eventually will be developed with configuration details and practical examples.

## Bugs

Please report any bugs you find on the Issues tab.

-----

## Osmotic Extensions

Osmotic Extensions are additional functionality implemented as extension methods.

All frameworks supported by Osmotic are supported by Osmotic Extensions.

```powershell
Install-Package MadReflection.Osmotic.Extensions
```

*Note: The package is not yet on NuGet.  This command will work once it's been published.*
