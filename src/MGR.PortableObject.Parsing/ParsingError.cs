using System.Diagnostics;

namespace MGR.PortableObject.Parsing;

/// <summary>
/// Represents a parsing error.
/// </summary>
[DebuggerDisplay("{" + nameof(DebuggerDisplay) + "}")]
public sealed class ParsingError
{
    /// <summary>
    /// Creates a new instance of <see cref="ParsingError"/>.
    /// </summary>
    /// <param name="message">The message of the error.</param>
    /// <param name="lineContent">The content of the line generating the error.</param>
    /// <param name="lineNumber">The line number where the parsing error occurs.</param>
    internal ParsingError(string message, string lineContent, int lineNumber)
    {
        Message = message;
        LineContent = lineContent;
        LineNumber = lineNumber;
    }
    /// <summary>
    /// Gets the message of the message.
    /// </summary>
    public string Message { get; }
    /// <summary>
    /// Gets the line number where the error occurs.
    /// </summary>
    public int LineNumber { get; }

    /// <summary>
    /// Gets the content of the line generating the error.
    /// </summary>
    public string LineContent { get; }

    private string DebuggerDisplay => $"{Message} at line {LineNumber} ({LineContent})";
}
