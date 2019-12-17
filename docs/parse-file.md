# Parse a PortableObject file

## Create a parser

Create a parser:

``` csharp
using MGR.PortableObject.Parsing;
```

``` csharp
TextReader textReader = ...:
var parser = new PortableObjectParser();
```

## Parse a file

You can parse a .po file by passing a `TextReader`
to the parser.

``` csharp
var catalog = await parser.ParseAsync(textReader, culture);
```
