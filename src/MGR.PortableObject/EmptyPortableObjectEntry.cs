using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MGR.PortableObject.Comments;

namespace MGR.PortableObject;

/// <summary>
/// Represents an empty entry of the PortableObject file.
/// </summary>
public class EmptyPortableObjectEntry : IPortableObjectEntry
{
    private static readonly ConcurrentDictionary<PortableObjectKey, EmptyPortableObjectEntry>  Entries = new();

    private EmptyPortableObjectEntry(PortableObjectKey key)
    {
        Key = key;
    }
    /// <inheritdoc />
    public PortableObjectKey Key { get; }
    /// <inheritdoc />
    public bool HasTranslation { get; } = false;
    /// <inheritdoc />
    public int Count { get; } = 0;
    /// <inheritdoc />
    public IEnumerable<PortableObjectCommentBase> Comments { get; } = Enumerable.Empty<PortableObjectCommentBase>();

    /// <inheritdoc />
    public string GetTranslation(int quantity) => quantity <= 1 ? Key.Id : Key.IdPlural ?? Key.Id;
    /// <summary>
    /// Creates a new instance of <see cref="EmptyPortableObjectEntry"/> for the specified key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static IPortableObjectEntry ForKey(PortableObjectKey key) => Entries.GetOrAdd(key, _ => new EmptyPortableObjectEntry(_));
}