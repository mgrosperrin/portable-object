using System.Globalization;

namespace MGR.PortableObject;

/// <summary>
/// Represents a catalog contained in a PortableObject file.
/// </summary>
public interface ICatalog
{
    /// <summary>
    /// Gets an entry from the current catalog for the specified key.
    /// </summary>
    /// <param name="key">A key.</param>
    /// <returns>The entry corresponding to the key.</returns>
    IPortableObjectEntry GetEntry(PortableObjectKey key);
    /// <summary>
    /// Gets the culture for the current catalog.
    /// </summary>
    CultureInfo Culture { get; }
    /// <summary>
    /// Gets the number of translation in the current catalog.
    /// </summary>
    int Count { get; }
}
