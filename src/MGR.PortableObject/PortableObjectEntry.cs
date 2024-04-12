using System;
using System.Collections.Generic;
using MGR.PortableObject.Comments;

namespace MGR.PortableObject;

/// <summary>
/// Default implementation of an entry with translations.
/// </summary>
public class PortableObjectEntry : IPortableObjectEntry
{
    private readonly IPluralForm _pluralForm;
    private readonly string[] _translations;

    /// <inheritdoc />
    public PortableObjectKey Key { get; }

    /// <inheritdoc />
    public bool HasTranslation { get; }

    /// <inheritdoc />
    public int Count => _translations.Length;

    /// <inheritdoc />
    public IEnumerable<PortableObjectCommentBase> Comments { get; }

    /// <summary>
    /// Create a new instance of a <see cref="PortableObjectEntry"/>.
    /// </summary>
    /// <param name="portableObjectKey">The <see cref="PortableObjectKey"/> of the entry.</param>
    /// <param name="pluralForm">The plural form computation.</param>
    /// <param name="translations">The translations of the entry.</param>
    /// <param name="comments">The comments of the entry.</param>
    public PortableObjectEntry(PortableObjectKey portableObjectKey, IPluralForm pluralForm, string[] translations, IEnumerable<PortableObjectCommentBase> comments)
    {
        Key = portableObjectKey;
        Comments = comments;
        _pluralForm = pluralForm;
        _translations = translations;
        if (translations.Length > _pluralForm.NumberOfPluralForms)
        {
            throw new InvalidOperationException("The number of translations is superior to the number of plural forms.");
        }
        HasTranslation = translations.Length > 0;
    }

    private string GetsTheIdForPluralForm(int pluralForm)
    {
        if (pluralForm == 0)
        {
            return Key.Id;
        }

        return Key.IdPlural ?? Key.Id;
    }

    /// <inheritdoc />
    public string GetTranslation(int quantity)
    {
        var pluralForm = _pluralForm.GetPluralFormForQuantity(quantity);
        if (!HasTranslation)
        {
            return GetsTheIdForPluralForm(pluralForm);
        }

        if (_translations.Length < pluralForm)
        {
            return GetsTheIdForPluralForm(pluralForm);
        }

        return _translations[pluralForm];
    }
}