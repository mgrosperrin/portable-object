namespace MGR.PortableObject;

/// <summary>
/// Defines the computation method of the plural form.
/// </summary>
public interface IPluralForm
{
    /// <summary>
    /// Gets the number of plural forms.
    /// </summary>
    int NumberOfPluralForms { get; }
    /// <summary>
    /// Get the plural form for the specified quantity.
    /// </summary>
    /// <param name="quantity">The quantity.</param>
    /// <returns>The plural form.</returns>
    int GetPluralFormForQuantity(int quantity);
}
