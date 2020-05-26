using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MGR.PortableObject.Parsing
{
    internal class ParsingContext
    {
        private readonly List<ParsingError> _parsingErrors = new List<ParsingError>();
        private readonly TextReader _textReader;
        private int _currentLineNumber;
        private string? _currentLineContent;

        internal ParsingContext(TextReader textReader)
        {
            _textReader = textReader;
        }

        internal void AddError(string message)
        {
            _parsingErrors.Add(
                new ParsingError(message, _currentLineContent ?? string.Empty, _currentLineNumber)
                );
        }

        internal async Task<string?> ReadLineAsync()
        {
            _currentLineNumber++;
            _currentLineContent = await _textReader.ReadLineAsync();
            return _currentLineContent;
        }

        internal IEnumerable<ParsingError> GetErrors() => _parsingErrors;
    }
}
