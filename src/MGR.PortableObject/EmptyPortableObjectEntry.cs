using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MGR.PortableObject.Comments;

namespace MGR.PortableObject
{
    internal class EmptyPortableObjectEntry : IPortableObjectEntry
    {
        private static readonly ConcurrentDictionary<PortableObjectKey, EmptyPortableObjectEntry>  Entries = new ConcurrentDictionary<PortableObjectKey, EmptyPortableObjectEntry>();

        private EmptyPortableObjectEntry(PortableObjectKey key)
        {
            Key = key;
        }

        public PortableObjectKey Key { get; }
        public bool HasTranslation { get; } = false;
        public int Count { get; } = 0;
        public IEnumerable<PortableObjectCommentBase> Comments { get; } = Enumerable.Empty<PortableObjectCommentBase>();

        public string GetTranslation(int quantity)
        {
            return quantity <= 1 ? Key.Id : Key.IdPlural ?? Key.Id;
        }

        public static IPortableObjectEntry ForKey(PortableObjectKey key)
        {
            return Entries.GetOrAdd(key, _ => new EmptyPortableObjectEntry(_));
        }
    }
}