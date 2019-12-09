# Parse a Portable Object file

## Installation and usage the package `MGR.PortableObject.Parsing`

You can install the NuGet package from the package manager console:

```
PM> Install-Package MGR.ParsingObject.Parsing
```

or via the .NET Core CLI

```
$ dotnet add package MGR.ParsingObject.Parsing
```

Then create a parser:

``` csharp
using MGR.PortableObject.Parsing;
```

``` csharp
TextReader textReader = ...:
var parser = new PortableObjectParser();

var catalog = await parser.ParseAsync(textReader, culture);
```

You can then try to retrieve entries from the catalog.
The entries are retrievable via a key composed of an id and an optional context:

``` chsarp
var key = new PortableObjectKey(id);
// or
var key = new PortableObjectKey(id, context);
```

``` csharp
var entry = catalog.GetEntry(key);
```

The entry can now be used to retrieve the translation:

```chsarp
// has the entry translation defined?
var hasTranslation = entry.HasTranslation;
// gets the primary translation (or the key if the entry has no translation)
var translation = entry.GetTranslation();
// gets plural form of the translation
var pluralForm = entry.GetPluralForm(1);
```
