using System;
using System.Collections.Generic;
using System.Globalization;

namespace MGR.PortableObject.Parsing
{
    /// <summary>
    /// Represents a builder for a <see cref="ICatalog"/>.
    /// </summary>
    internal class CatalogBuilder
    {
        private const string ContextPrefix = "msgctxt";
        private const string KeyPrefix = "msgid";
        private const string TranslationPrefix = "msgstr";
        private const string CommentPrefix = "#";

        private readonly CultureInfo _culture;

        private readonly Dictionary<PortableObjectKey, IPortableObjectEntry> _entries = new Dictionary<PortableObjectKey, IPortableObjectEntry>();
        private readonly List<string> _currentTranslations = new List<string>();

        private string? _currentContext;
        private string _currentId = string.Empty;
        private LineType _lastLineType = LineType.Unknown;
        private IPluralForm _pluralForm;

        /// <summary>
        /// Creates a new builder.
        /// </summary>
        public CatalogBuilder(CultureInfo culture)
        {
            _culture = culture;
            _pluralForm = PluralForms.For(_culture);
        }

        /// <summary>
        /// Build the catalog with the currently parsed lines.
        /// </summary>
        /// <returns>A catalog</returns>
        public ICatalog BuildCatalog()
        {
            FlushEntry();
            return new Catalog(_entries, _culture);
        }
        public void AppendLine(string line)
        {
            if (line.StartsWith(CommentPrefix))
            {
                return;
            }
            if (line.StartsWithQuote())
            {
                var lineContent = line.Trim().TrimQuote().Unescape();
                AppendLineContent(lineContent);
                return;
            }

            var keyAndValue = line.Split(null, 2);
            if (keyAndValue.Length != 2)
            {
                return;
            }

            var content = keyAndValue[1].Trim().TrimQuote().Unescape();
            switch (keyAndValue[0])
            {
                case ContextPrefix:
                    FlushEntry();
                    _currentContext = content;
                    _lastLineType = LineType.Context;
                    break;
                case KeyPrefix:
                    FlushEntry();
                    _currentId = content;
                    _lastLineType = LineType.Id;
                    break;
                case var key when key.StartsWith(TranslationPrefix, StringComparison.Ordinal):
                    _currentTranslations.Add(content);
                    _lastLineType = LineType.Translation;
                    break;
            }
        }

        private void AppendLineContent(string lineContent)
        {
            switch (_lastLineType)
            {
                case LineType.Context:
                    _currentContext += lineContent;
                    break;
                case LineType.Id:
                    _currentId += lineContent;
                    break;
                case LineType.Translation:
                    _currentTranslations[_currentTranslations.Count - 1] += lineContent;
                    break;
            }
        }

        private void FlushEntry()
        {
            if (_currentTranslations.Count > 0 && !string.IsNullOrEmpty(_currentId))
            {
                var key = new PortableObjectKey(_currentId, _currentContext);
                AddEntry(key, _currentTranslations);

                _currentContext = null;
                _currentId = string.Empty;
            }
            _currentTranslations.Clear();
        }

        private void AddEntry(PortableObjectKey key, List<string> translations)
        {
            var entry = new PortableObjectEntry(key, _pluralForm, translations.ToArray());
            _entries.Add(key, entry);
        }

        private enum LineType
        {
            Unknown = 0,
            Context = 1,
            Id = 2,
            Translation = 3
        }
    }
}