using System.Collections.Generic;
using System.Linq;

namespace MGR.PortableObject.Comments
{
    /// <summary>
    /// Represents the references to a program's source code.
    /// </summary>
    public class ReferencesComment : PortableObjectCommentBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="ReferencesComment"/>.
        /// </summary>
        /// <param name="text">The references of the source code.</param>
        public ReferencesComment(string text) : base(text)
        {
            var references = text.Split(null);
            References = references.Select(reference => new SourceCodeReference(reference));
        }
        /// <summary>
        /// Gets the references of the translation entry.
        /// </summary>
        public IEnumerable<SourceCodeReference> References { get; }
    }
}
