namespace MGR.PortableObject.Comments
{
    /// <summary>
    /// Represents a comment provided by the translator.
    /// </summary>
    public class TranslatorComment : PortableObjectCommentBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="TranslatorComment"/>.
        /// </summary>
        /// <param name="text">The comment provided by the translator.</param>
        public TranslatorComment(string text) : base(text)
        {
        }
    }
}
