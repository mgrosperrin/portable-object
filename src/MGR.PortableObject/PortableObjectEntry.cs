namespace MGR.PortableObject
{
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

        /// <summary>
        /// Create a new instance of a <see cref="PortableObjectEntry"/>.
        /// </summary>
        /// <param name="portableObjectKey">The <see cref="PortableObjectKey"/> of the entry.</param>
        /// <param name="pluralForm">The plural form computation.</param>
        /// <param name="translations">The translations of the entry.</param>
        public PortableObjectEntry(PortableObjectKey portableObjectKey, IPluralForm pluralForm, string[] translations)
        {
            Key = portableObjectKey;
            _pluralForm = pluralForm;
            _translations = translations;
            HasTranslation = translations.Length > 0;
        }

        /// <inheritdoc />
        public string GetTranslation()
        {
            return HasTranslation ? _translations[0] : Key.Id;
        }

        /// <inheritdoc />
        public string GetPluralTranslation(int numberOfItems)
        {
            if (!HasTranslation)
            {
                return Key.Id;
            }

            var pluralForm = _pluralForm.GetPluralFormForNumber(numberOfItems);
            if (_translations.Length < pluralForm)
            {
                return Key.Id;
            }

            return _translations[pluralForm];
        }
    }
}