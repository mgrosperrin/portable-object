namespace MGR.PortableObject
{
    /// <summary>
    /// Represents an entry of the PortableObject file.
    /// </summary>
    public interface IPortableObjectEntry
    {
        /// <summary>
        /// Gets the key corresponding to the entry.
        /// </summary>
        PortableObjectKey Key { get; }

        /// <summary>
        /// Indicates if the current entry has at least one translation.
        /// </summary>
        bool HasTranslation { get; }

        /// <summary>
        /// Gets the number of translations in this entry.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the primary translation from the current entry.
        /// </summary>
        /// <returns>A string representing the primary translation from the current entry.</returns>
        string GetTranslation();
        /// <summary>
        /// Gets the plural translation for the specified number of items from the current entry.
        /// </summary>
        /// <param name="numberOfItems">The number of items.</param>
        /// <returns>A string representing the requested translation.</returns>
        string GetPluralTranslation(int numberOfItems);
    }
}
