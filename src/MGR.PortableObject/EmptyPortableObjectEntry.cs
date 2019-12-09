using System.Collections.Concurrent;

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
        public string GetTranslation() => Key.Id;

        public string GetPluralTranslation(int pluralForm) => Key.Id;

        public static IPortableObjectEntry ForKey(PortableObjectKey key)
        {
            return Entries.GetOrAdd(key, _ => new EmptyPortableObjectEntry(_));
        }
    }
}