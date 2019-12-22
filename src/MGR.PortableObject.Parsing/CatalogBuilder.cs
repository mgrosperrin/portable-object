using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MGR.PortableObject.Comments;

namespace MGR.PortableObject.Parsing
{
    /// <summary>
    /// Represents a builder for a <see cref="ICatalog"/>.
    /// </summary>
    internal class CatalogBuilder
    {
        private readonly CultureInfo _culture;
        private readonly PortableObjectEntryBuilder _entryBuilder;

        private readonly List< IPortableObjectEntry> _entries = new List<IPortableObjectEntry>();

        /// <summary>
        /// Creates a new builder.
        /// </summary>
        public CatalogBuilder(CultureInfo culture)
        {
            _culture = culture;
            PluralForm = PluralForms.For(culture);
            _entryBuilder = new PortableObjectEntryBuilder(this);
        }

        internal IPluralForm PluralForm { get; private set; }

        /// <summary>
        /// Build the catalog with the currently parsed lines.
        /// </summary>
        /// <returns>A catalog</returns>
        public ICatalog BuildCatalog(List<string> errors)
        {
            var finalEntry = _entryBuilder.BuildEntry(errors);
            if (finalEntry != null)
            {
                AddEntry(finalEntry);
            }
            return new Catalog(_entries.ToDictionary(_ => _.Key), _culture);
        }

        public void SetPluralForm(IPluralForm pluralForm)
        {
            PluralForm = pluralForm;
        }

        public PortableObjectEntryBuilder GetEntryBuilder()
        {
            return _entryBuilder;
        }

        internal void AddEntry(PortableObjectKey key, List<string> translations, IEnumerable<PortableObjectCommentBase> comments)
        {
            var entry = new PortableObjectEntry(key, PluralForm, translations.ToArray(), comments);
            AddEntry(entry);
        }

        internal void AddEntry(IPortableObjectEntry portableObjectEntry)
        {
            _entries.Add(portableObjectEntry);
        }
    }
}