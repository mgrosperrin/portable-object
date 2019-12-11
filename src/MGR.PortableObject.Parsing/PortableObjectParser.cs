using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace MGR.PortableObject.Parsing
{
    /// <summary>
    /// A parser of PortableObject file.
    /// </summary>
    public class PortableObjectParser
    {
        /*
         * https://www.gnu.org/software/gettext/manual/html_node/PO-Files.html
         * https://www.gnu.org/software/gettext/manual/html_node/Plural-forms.html#Plural-forms
         * https://www.gnu.org/software/gettext/manual/html_node/Translating-plural-forms.html#Translating-plural-forms
         */
        /// <summary>
        /// Parses a PortableObject file.
        /// </summary>
        /// <param name="textReader">The <see cref="TextReader"/> representing the content of the file.</param>
        /// <param name="culture">The culture of the Portable Object file.</param>
        /// <returns>The translations parsed from the file content.</returns>
        public async Task<ICatalog> ParseAsync(TextReader textReader, CultureInfo culture)
        {
            var translationsBuilder = new CatalogBuilder(culture);
            string? line;
            while ((line = await textReader.ReadLineAsync()) != null)
            {
                translationsBuilder.AppendLine(line);
            }
            var translations = translationsBuilder.BuildCatalog();
            return translations;
        }
    }
}
