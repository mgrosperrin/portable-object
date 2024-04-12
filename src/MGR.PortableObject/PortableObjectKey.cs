using System;
using System.Diagnostics;

namespace MGR.PortableObject;

/// <summary>
/// Represents a key for the PortableObject translation.
/// </summary>
[DebuggerDisplay("{" + nameof(DebuggerDisplay) + "}")]
public struct PortableObjectKey : IEquatable<PortableObjectKey>
{
    /// <summary>
    /// Gets the Id of the key.
    /// </summary>
    public string Id { get; }
    /// <summary>
    /// Gets the plural Id of the key.
    /// </summary>
    public string? IdPlural { get; }
    /// <summary>
    /// Gets the (optional) context of the key.
    /// </summary>
    public string? Context { get; }
    /// <summary>
    /// Creates a new key.
    /// </summary>
    /// <param name="id">The Id of the key.</param>
    public PortableObjectKey(string id) : this(null, id, null) { }

    /// <summary>
    /// Creates a new key.
    /// </summary>
    /// <param name="context">The Context of the key.</param>
    /// <param name="id">The Id of the key.</param>
    public PortableObjectKey(string context, string id) : this(context, id, null) { }

    /// <summary>
    /// Creates a new key.
    /// </summary>
    /// <param name="context">The Context of the key.</param>
    /// <param name="id">The Id of the key.</param>
    /// <param name="idPlural">The Plural Id of the key.</param>
    public PortableObjectKey(string? context, string id, string? idPlural)
    {
        Id = id;
        Context = context;
        IdPlural = idPlural;
    }

    private readonly string DebuggerDisplay
    {
        get
        {
            var contextDisplay = Context == null ? "" : $"Context={Context}¤";
            var idDisplay = $"Id={Id}";
            var pluralIdDisplay = IdPlural == null ? "" : $"¤IdPlural={IdPlural}";
            return contextDisplay + idDisplay + pluralIdDisplay;
        }
    }

    /// <inheritdoc />
    public readonly bool Equals(PortableObjectKey other) => Id == other.Id && IdPlural == other.IdPlural && Context == other.Context;

    /// <inheritdoc />
    public override readonly bool Equals(object? obj) => obj is PortableObjectKey other && Equals(other);

    /// <inheritdoc />
    public override readonly int GetHashCode()
    {
        unchecked
        {
            var hashCode = Id.GetHashCode();
            hashCode = (hashCode * 397) ^ (IdPlural != null ? IdPlural.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Context != null ? Context.GetHashCode() : 0);
            return hashCode;
        }
    }
}