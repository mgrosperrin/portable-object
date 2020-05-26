using System;
using System.Collections.Generic;
using System.Linq;
using MGR.PortableObject.Comments;

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
        private readonly ParsingContext _parsingContext;
        private readonly List<List<string>> _currentTranslations = new List<List<string>>();
        private readonly List<PortableObjectCommentBase> _comments = new List<PortableObjectCommentBase>();

        private bool _headerHasBeenParsed;
        private string? _currentContext;
        private string _currentId = string.Empty;
        private string? _currentIdPlural;
        private LineType _lastLineType = LineType.Header;

        public PortableObjectEntryBuilder(CatalogBuilder catalogBuilder, ParsingContext parsingContext)
        {
            _catalogBuilder = catalogBuilder;
            _parsingContext = parsingContext;
        }

        public void AppendLine(string line, ParsingContext parsingContext)
        {
            if (line.StartsWith(CommentPrefix))
            {
                ParseAndAppendComment(line, parsingContext);
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
                _parsingContext.AddError("The line should contains a key and a content separated by a space.");
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
                default:
                    _parsingContext.AddError("Unable to determine the current line meaning.");
                    break;
            }
        }

        private void ParseAndAppendComment(string line, ParsingContext parsingContext)
        {
            var comment = ParseComment(line, parsingContext);
            if (comment != null)
            {
                _comments.Add(comment);
            }
        }

        private PortableObjectCommentBase? ParseComment(string line, ParsingContext parsingContext)
        {
            var commentPrefix = line.Substring(0, 2);
            var commentContent = line.Substring(2).TrimStart(' ');
            switch (commentPrefix)
            {
                case "#.":
                    return new ProgrammerComment(commentContent);
                case "#:":
                    return new ReferencesComment(commentContent);
                case "#,":
                    return new FlagsComment(commentContent);
                case "#|":
                    return new PreviousUntranslatedStringComment(commentContent);
                case "# ":
                    return new TranslatorComment(commentContent);
                default:
                    parsingContext.AddError("Unable to find the type of comment.");
                    return null;
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
                case LineType.Header:
                    break;
                default:
                    _parsingContext.AddError("The current line has no previous meaning.");
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
                        _currentTranslations.Select(lines => string.Join("", lines)).ToArray(),
                        _comments.ToList());
                }
            }
            _currentId = string.Empty;
            _currentContext = null;
            _currentTranslations.Clear();
            _comments.Clear();
            return entry;
        }

        private void ParseHeader()
        {
            if (!_headerHasBeenParsed)
            {
                _headerHasBeenParsed = true;
                var headers = _currentTranslations[0]
                    .Where(line => !string.IsNullOrEmpty(line))
                    .Select(line => line.Split(HeaderSeparator))
                    .ToLookup(header => header[0], header => header[1]);

                if (headers.Contains(HeaderPluralForms))
                {
                    var pluralFormsHeader = headers[HeaderPluralForms];
                    try
                    {
                        var pluralForm = _pluralFormParser.Parse(pluralFormsHeader.First());
                        _catalogBuilder.SetPluralForm(pluralForm);
                    }
                    catch (InvalidOperationException exception)
                    {
                        _parsingContext.AddError(
                            exception.Message
                            );
                    }
                }
            }
            else
            {
                _parsingContext.AddError("The header has already been parsed. An entry should have a non empty id.");
            }
        }

        private enum LineType
        {
            Header = 5,
            Context = 10,
            Id = 20,
            IdPlural = 21,
            Translation = 30
        }
    }
}
