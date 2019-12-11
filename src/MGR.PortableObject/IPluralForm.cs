namespace MGR.PortableObject
{
    /// <summary>
    /// Defines the computation method of the plural form.
    /// </summary>
    public interface IPluralForm
    {
        /// <summary>
        /// Get the plural form for the specified number of items.
        /// </summary>
        /// <param name="numberOfItems">The number of items.</param>
        /// <returns>The plural form.</returns>
        int GetPluralFormForNumber(int numberOfItems);
    }
}
