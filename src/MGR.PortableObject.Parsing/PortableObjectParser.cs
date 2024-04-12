using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace MGR.PortableObject.Parsing;

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
    /// <param name="culture">The culture of the PortableObject file.</param>
    /// <returns>The translations parsed from the file content.</returns>
    public async Task<ParsingResult> ParseAsync(TextReader textReader, CultureInfo culture)
    {
        var parsingContext = new ParsingContext(textReader);
        var catalogBuilder = new CatalogBuilder(parsingContext, culture);
        var entryBuilder = catalogBuilder.GetEntryBuilder();
        string? line;
        while ((line = await parsingContext.ReadLineAsync()) != null)
        {
            if (string.IsNullOrEmpty(line))
            {
                var entry = entryBuilder.BuildEntry();
                if (entry != null)
                {
                    catalogBuilder.AddEntry(entry);
                }
            }
            else
            {
                entryBuilder.AppendLine(line, parsingContext);
            }
        }
        var catalog = catalogBuilder.BuildCatalog();
        var parsingResult = new ParsingResult(catalog, parsingContext.GetErrors());
        return parsingResult;
    }
}
