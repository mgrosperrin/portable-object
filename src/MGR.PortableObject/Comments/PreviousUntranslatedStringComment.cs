namespace MGR.PortableObject.Comments
{
    /// <summary>
    /// Represents a previous untranslated string comment.
    /// </summary>
    public class PreviousUntranslatedStringComment : PortableObjectCommentBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="PreviousUntranslatedStringComment"/>.
        /// </summary>
        /// <param name="text">The previous untranslated string.</param>
        public PreviousUntranslatedStringComment(string text) : base(text)
        {
        }
    }
}
