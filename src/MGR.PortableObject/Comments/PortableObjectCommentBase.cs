namespace MGR.PortableObject.Comments;

/// <summary>
/// Represents the base class for the comments
/// </summary>
public abstract class PortableObjectCommentBase
{
    /// <summary>
    /// Creates an instance of <see cref="PortableObjectCommentBase"/>.
    /// </summary>
    /// <param name="text">The text of the comment.</param>
    protected PortableObjectCommentBase(string text)
    {
        Text = text;
    }
    /// <summary>
    /// Gets the text of the comment.
    /// </summary>
    public string Text { get; }
}
