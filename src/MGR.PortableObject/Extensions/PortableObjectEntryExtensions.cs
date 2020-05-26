// ReSharper disable once CheckNamespace
namespace MGR.PortableObject
{
    /// <summary>
    /// Extension's methods for the <see cref="IPortableObjectEntry"/>.
    /// </summary>
    public static class PortableObjectEntryExtensions
    {
        /// <summary>
        /// Gets the translation for one item.
        /// </summary>
        /// <param name="entry">The current entry.</param>
        /// <returns>The translation for one item.</returns>
        public static string GetTranslation(this IPortableObjectEntry entry)
        {
            return entry.GetTranslation(1);
        }
    }
}
