using System;
using System.Diagnostics;

namespace MGR.PortableObject
{
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
        /// Gets the (optional) context of the key.
        /// </summary>
        public string? Context { get; }
        /// <summary>
        /// Creates a new key.
        /// </summary>
        /// <param name="id">The Id of the key.</param>
        public PortableObjectKey(string id) : this(id, null) { }
        /// <summary>
        /// Creates a new key.
        /// </summary>
        /// <param name="id">The Id of the key.</param>
        /// <param name="context">The Context of the key.</param>
        public PortableObjectKey(string id, string? context)
        {
            Id = id;
            Context = context;
        }

        /// <inheritdoc />
        public bool Equals(PortableObjectKey other)
        {
            return Id == other.Id && Context == other.Context;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is PortableObjectKey other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode() * 397) ^ (Context != null ? Context.GetHashCode() : 0);
            }
        }

        private string DebuggerDisplay =>
            Context == null ? $"Id={Id}" : $"Context={Context}¤Id={Id}";
    }
}