using System;

namespace MGR.PortableObject;

/// <summary>
/// Defines plural form computation based on a <see cref="Func{Int32, Int32}"/>.
/// </summary>
public class FuncBasedPluralForm : IPluralForm
{
    private readonly Func<int, int> _pluralFormCompute;

    /// <summary>
    /// Creates a new instance of <see cref="FuncBasedPluralForm"/>.
    /// </summary>
    /// <param name="numberOfPluralForms">The number of plural forms.</param>
    /// <param name="pluralFormCompute">The <see cref="Func{Int32, Int32}"/> to compute the plural form.</param>
    public FuncBasedPluralForm(int numberOfPluralForms, Func<int, int> pluralFormCompute)
    {
        NumberOfPluralForms = numberOfPluralForms;
        _pluralFormCompute = pluralFormCompute;
    }

    /// <inheritdoc />
    public int NumberOfPluralForms { get; }

    /// <inheritdoc />
    public int GetPluralFormForQuantity(int quantity)
    {
        var pluralForm = _pluralFormCompute(quantity);
        if (pluralForm > NumberOfPluralForms)
        {
            throw new InvalidOperationException("The computed plural form is greater than the number of plural forms.");
        }

        return pluralForm;
    }
}
