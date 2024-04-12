using System.Collections.Generic;
using System.Globalization;

namespace MGR.PortableObject;

/// <summary>
/// Default implementation of a <see cref="ICatalog"/>.
/// </summary>
public class Catalog : ICatalog
{
    private readonly Dictionary<PortableObjectKey, IPortableObjectEntry> _entries;

    /// <summary>
    /// Creates a new instance of <see cref="Catalog"/>.
    /// </summary>
    /// <param name="entries">The different entries composing the catalog.</param>
    /// <param name="culture">The culture of the catalog</param>
    public Catalog(Dictionary<PortableObjectKey, IPortableObjectEntry> entries, CultureInfo culture)
    {
        _entries = entries;
        Culture = culture;
    }

    /// <inheritdoc />
    public IPortableObjectEntry GetEntry(PortableObjectKey key) => _entries.ContainsKey(key) ? _entries[key] : EmptyPortableObjectEntry.ForKey(key);

    /// <inheritdoc />
    public CultureInfo Culture { get; }

    /// <inheritdoc />
    public int Count => _entries.Count;
}
