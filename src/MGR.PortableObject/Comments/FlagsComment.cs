using System.Collections.Generic;

namespace MGR.PortableObject.Comments
{
    /// <summary>
    /// Represents flags.
    /// </summary>
    public class FlagsComment : PortableObjectCommentBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="FlagsComment"/>.
        /// </summary>
        /// <param name="text">The flags.</param>
        public FlagsComment(string text) : base(text)
        {
            Flags = text.Split(',');
        }
        /// <summary>
        /// Gets the flags form the comment.
        /// </summary>
        public IEnumerable<string> Flags { get; }
    }
}
