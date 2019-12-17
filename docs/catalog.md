# PortableObject catalog

## How to use the catalog

Once you have a catalog 
(for example after [parsing it](./parse-file.md)),
you can get an entry.

### Get an entry for a simple id

``` csharp
ICatalog catalog = GetCatalog();
var key = new PortableObjectKey("book");
var entry = catalog.GetEntry(key);
```

### Get an entry for a simple id and a context

``` csharp
ICatalog catalog = GetCatalog();
var key = new PortableObjectKey("context.view", "book");
var entry = catalog.GetEntry(key);
```

### Get an entry for pluralisation

``` csharp
ICatalog catalog = GetCatalog();
var key = new PortableObjectKey("context.view", "book", "books");
var entry = catalog.GetEntry(key);
```

### Retrieve the translation for the entry

``` csharp
// has the entry translation defined?
var hasTranslation = entry.HasTranslation;

// gets the primary translation (or the key if the entry has no translation)
var translation = entry.GetTranslation();

// gets the plural form of the translation
// for a quantity
var pluralForm = entry.GetTranslation(quantity);
```
