namespace MGR.PortableObject.Comments
{
    /// <summary>
    /// Represents a comment provided by the programmer to the translator.
    /// </summary>
    public class ProgrammerComment : PortableObjectCommentBase
    {/// <summary>
     /// Creates a new instance of <see cref="ProgrammerComment"/>.
     /// </summary>
     /// <param name="text">The comment provided by the programmer.</param>
        public ProgrammerComment(string text) : base(text)
        {
        }
    }
}
