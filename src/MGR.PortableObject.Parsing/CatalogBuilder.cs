using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MGR.PortableObject.Parsing;

internal class CatalogBuilder
{
    private readonly CultureInfo _culture;
    private readonly PortableObjectEntryBuilder _entryBuilder;

    private readonly List< IPortableObjectEntry> _entries = [];

    public CatalogBuilder(ParsingContext parsingContext, CultureInfo culture)
    {
        _culture = culture;
        PluralForm = PluralForms.For(culture);
        _entryBuilder = new PortableObjectEntryBuilder(this, parsingContext);
    }

    internal IPluralForm PluralForm { get; private set; }

    public ICatalog BuildCatalog()
    {
        var finalEntry = _entryBuilder.BuildEntry();
        if (finalEntry != null)
        {
            AddEntry(finalEntry);
        }
        return new Catalog(_entries.ToDictionary(_ => _.Key), _culture);
    }

    public void SetPluralForm(IPluralForm pluralForm) => PluralForm = pluralForm;

    public PortableObjectEntryBuilder GetEntryBuilder() => _entryBuilder;

    internal void AddEntry(IPortableObjectEntry portableObjectEntry) => _entries.Add(portableObjectEntry);
}