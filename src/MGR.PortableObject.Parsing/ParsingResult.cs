using System.Collections.Generic;
using System.Linq;

namespace MGR.PortableObject.Parsing
{
    /// <summary>
    /// Represents the result of parsing PortableObject file.
    /// </summary>
    public class ParsingResult
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="catalog"></param>
        /// <param name="errors"></param>
        public ParsingResult(ICatalog catalog, IEnumerable<ParsingError> errors)
        {
            Catalog = catalog;
            Errors = errors;
        }

        /// <summary>
        /// Returns <code>true</code> if the parsing succeeded, <code>false</code> otherwise.
        /// </summary>
        public bool Success => !Errors.Any();

        /// <summary>
        /// Gets the errors (if any) of the parsing.
        /// </summary>
        public IEnumerable<ParsingError> Errors { get; }

        /// <summary>
        /// Gets the <see cref="ICatalog"/> resulting of the parsing.
        /// </summary>
        public ICatalog Catalog { get; }
    }
}