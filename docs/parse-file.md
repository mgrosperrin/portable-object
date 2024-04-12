# Parse a PortableObject file

## Create a parser

Create a parser:

``` csharp
var parser = new MGR.PortableObject.Parsing.PortableObjectParser();
```

## Parse a file

You can parse a .po file by passing a `TextReader`
to the parser.

``` csharp
TextReader textReader = ...;
CultureInfo cultureOfThePOFile = ...;
var catalog = await parser.ParseAsync(textReader, cultureOfThePOFile);
```
