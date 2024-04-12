using System.Diagnostics;

namespace MGR.PortableObject.Comments;

/// <summary>
/// Represent a reference in a source code.
/// </summary>
[DebuggerDisplay("{" + nameof(DebuggerDisplay) + "}")]
public struct SourceCodeReference
{
    /// <summary>
    /// Creates a new instance of <see cref="ReferencesComment"/>.
    /// </summary>
    /// <param name="reference">The references of the source code.</param>
    public SourceCodeReference(string reference)
    {
        var referenceParts = reference.Split(':');
        FilePath = referenceParts[0];
        LineNumber = int.Parse(referenceParts[1]);
    }
    /// <summary>
    /// Gets the file path of the reference.
    /// </summary>
    public string FilePath { get; }
    /// <summary>
    /// Gets the line number of the reference.
    /// </summary>
    public int LineNumber { get; }

    private readonly string DebuggerDisplay => $"{FilePath}:{LineNumber}";
}
