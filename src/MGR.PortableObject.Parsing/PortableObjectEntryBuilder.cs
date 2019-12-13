using System;
using System.Collections.Generic;
using System.Linq;

namespace MGR.PortableObject.Parsing
{
    internal class PortableObjectEntryBuilder
    {
        private const string ContextPrefix = "msgctxt";
        private const string IdPrefix = "msgid";
        private const string IdPluralPrefix = "msgid_plural";
        private const string TranslationPrefix = "msgstr";
        private const string CommentPrefix = "#";
        private const char HeaderSeparator = ':';
        private const string HeaderPluralForms = "Plural-Forms";

        private readonly PluralFormParser _pluralFormParser = new PluralFormParser();
        private readonly CatalogBuilder _catalogBuilder;
        private readonly List<List<string>> _currentTranslations = new List<List<string>>();

        private string? _currentContext;
        private string _currentId = string.Empty;
        private string? _currentIdPlural;
        private LineType _lastLineType = LineType.Unknown;

        public PortableObjectEntryBuilder(CatalogBuilder catalogBuilder)
        {
            _catalogBuilder = catalogBuilder;
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
                    _currentContext = content;
                    _lastLineType = LineType.Context;
                    break;
                case IdPluralPrefix:
                    _currentIdPlural = content;
                    _lastLineType = LineType.IdPlural;
                    break;
                case IdPrefix:
                    _currentId = content;
                    _lastLineType = LineType.Id;
                    break;
                case var key when key.StartsWith(TranslationPrefix, StringComparison.Ordinal):
                    var newTranslations = new List<string>();
                    newTranslations.Add(content);
                    _currentTranslations.Add(newTranslations);
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
                case LineType.IdPlural:
                    _currentIdPlural += lineContent;
                    break;
                case LineType.Translation:
                    _currentTranslations[_currentTranslations.Count - 1].Add(lineContent);
                    break;
            }
        }
        public IPortableObjectEntry? BuildEntry()
        {
            IPortableObjectEntry? entry = null;
            if (_currentTranslations.Count > 0)
            {
                if (string.IsNullOrEmpty(_currentId))
                {
                    ParseHeader();
                }
                else
                {
                    entry = new PortableObjectEntry(
                        new PortableObjectKey(_currentContext, _currentId, _currentIdPlural),
                        _catalogBuilder.PluralForm,
                        _currentTranslations.Select(lines => string.Join("", lines)).ToArray());
                }
            }
            _currentId = string.Empty;
            _currentContext = null;
            _currentTranslations.Clear();
            return entry;
        }

        private void ParseHeader()
        {
            var headers = _currentTranslations[0]
                .Where(line => !string.IsNullOrEmpty(line))
                .Select(line => line.Split(HeaderSeparator))
                .ToLookup(header => header[0], header => header[1]);

            if (headers.Contains(HeaderPluralForms))
            {
                var pluralFormsHeader = headers[HeaderPluralForms];
                var pluralForm = _pluralFormParser.Parse(pluralFormsHeader.First());
                _catalogBuilder.SetPluralForm(pluralForm);
            }
        }

        private enum LineType
        {
            Unknown = 0,
            Context = 1,
            Id = 20,
            IdPlural = 21,
            Translation = 30
        }
    }
}
