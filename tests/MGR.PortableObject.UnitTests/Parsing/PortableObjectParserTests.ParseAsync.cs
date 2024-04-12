using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MGR.PortableObject.Comments;
using MGR.PortableObject.Parsing;
using Xunit;

namespace MGR.PortableObject.UnitTests.Parsing;

public partial class PortableObjectParserTests
{
    public class ParseAsync
    {
        [Fact]
        public async Task ParseReturnsSimpleEntry()
        {
            var parsingResult = await ParseText("SimpleEntry");
            Assert.True(parsingResult.Success);
            var catalog = parsingResult.Catalog;
            Assert.Equal(1, catalog.Count);

            var entry = catalog.GetEntry(new PortableObjectKey("Unknown system error"));

            Assert.True(entry.HasTranslation);
            Assert.Equal(1, entry.Count);
            Assert.Equal("Erreur système inconnue", entry.GetTranslation());
        }

        [Fact]
        public async Task ParseIgnoresEntryWithoutTranslation()
        {
            var parsingResult = await ParseText("EntryWithoutTranslation");
            Assert.True(parsingResult.Success);
            var catalog = parsingResult.Catalog;

            Assert.Equal(0, catalog.Count);
        }

        [Fact]
        public async Task ParseIgnoresPoeditHeader()
        {
            var parsingResult = await ParseText("PoeditHeader");
            Assert.True(parsingResult.Success);
            var catalog = parsingResult.Catalog;

            Assert.Equal(1, catalog.Count);
            var entry = catalog.GetEntry(new PortableObjectKey("Unknown system error"));
            Assert.True(entry.HasTranslation);
            Assert.Equal(1, entry.Count);
            Assert.Equal("Erreur système inconnue", entry.GetTranslation());
        }

        [Fact]
        public async Task ParseSetsContext()
        {
            var parsingResult = await ParseText("EntryWithContext");
            Assert.True(parsingResult.Success);
            var catalog = parsingResult.Catalog;

            var entry = catalog.GetEntry(new PortableObjectKey("MGR.Localization", "Unknown system error"));
            Assert.True(entry.HasTranslation);
            Assert.Equal("Erreur système inconnue", entry.GetTranslation());
        }

        [Fact]
        public async Task ParseIgnoresComments()
        {
            var parsingResult = await ParseText("EntryWithComments");
            Assert.True(parsingResult.Success);
            var catalog = parsingResult.Catalog;

            var entry = catalog.GetEntry(new PortableObjectKey("MGR.Localization", "Unknown system error"));
            Assert.True(entry.HasTranslation);
            Assert.Equal("Erreur système inconnue", entry.GetTranslation());
            Assert.Equal(2, entry.Comments.Count());
            var firstComment = entry.Comments.First();
            Assert.IsType<PreviousUntranslatedStringComment>(firstComment);
            Assert.Equal("msgctxt previous-context", firstComment.Text);
            var secondComment = entry.Comments.Skip(1).First();
            Assert.IsType<PreviousUntranslatedStringComment>(secondComment);
            Assert.Equal("msgid previous-untranslated-string", secondComment.Text);
        }

        [Fact]
        public async Task ParseOnlyTrimsLeadingAndTrailingQuotes()
        {
            var parsingResult = await ParseText("EntryWithQuotes");
            Assert.True(parsingResult.Success);
            var catalog = parsingResult.Catalog;

            var entry = catalog.GetEntry(new PortableObjectKey("\"{0}\""));
            Assert.True(entry.HasTranslation);
            Assert.Equal("\"{0}\"", entry.GetTranslation());
        }

        [Fact]
        public async Task ParseHandleUnclosedQuote()
        {
            var parsingResult = await ParseText("EntryWithUnclosedQuote");
            Assert.True(parsingResult.Success);
            var catalog = parsingResult.Catalog;

            var entry = catalog.GetEntry(new PortableObjectKey("", "Foo \"{0}\""));
            Assert.True(entry.HasTranslation);
            Assert.Equal("Foo \"{0}\"", entry.GetTranslation());
        }

        [Fact]
        public async Task ParseHandlesMultilineEntry()
        {
            var parsingResult = await ParseText("EntryWithMultilineText");
            Assert.True(parsingResult.Success);
            var catalog = parsingResult.Catalog;

            var entry = catalog.GetEntry(new PortableObjectKey(
                "Here is an example of how one might continue a very long string\nfor the common case the string represents multi-line output."));
            Assert.True(entry.HasTranslation);
            Assert.Equal(
                "Ceci est un exemple de comment une traduction très longue peut continuer\npour le cas commun où le texte serait sur plusieurs lignes.",
                entry.GetTranslation());
        }

        [Fact]
        public async Task ParsePreservesEscapedCharacters()
        {
            var parsingResult = await ParseText("EntryWithEscapedCharacters");
            Assert.True(parsingResult.Success);
            var catalog = parsingResult.Catalog;

            var entry = catalog.GetEntry(new PortableObjectKey("Line:\t\"{0}\"\n"));
            Assert.True(entry.HasTranslation);
            Assert.Equal("Ligne :\t\"{0}\"\n", entry.GetTranslation());
        }

        [Fact]
        public async Task ParseReadsPluralTranslations()
        {
            var parsingResult = await ParseText("EntryWithPlural");
            Assert.True(parsingResult.Success);
            var catalog = parsingResult.Catalog;

            var entry = catalog.GetEntry(new PortableObjectKey(null, "book", "books"));
            Assert.True(entry.HasTranslation);
            Assert.Equal("livres", entry.GetTranslation(0));
            Assert.Equal("livre", entry.GetTranslation(1));
            Assert.Equal("livres", entry.GetTranslation(2));
        }

        [Fact]
        public async Task ParseReadsMultipleEntries()
        {
            var parsingResult = await ParseText("MultipleEntries");
            Assert.True(parsingResult.Success);
            var catalog = parsingResult.Catalog;

            Assert.Equal(2, catalog.Count);

            var entry = catalog.GetEntry(new PortableObjectKey("MGR.File", "File {0} does not exist"));

            Assert.True(entry.HasTranslation);
            Assert.Equal("Le fichier {0} n'existe pas", entry.GetTranslation());

            entry = catalog.GetEntry(new PortableObjectKey("MGR.Directory", "Directory {0} does not exist"));

            Assert.True(entry.HasTranslation);
            Assert.Equal("Le répertoire {0} n'existe pas", entry.GetTranslation());
        }

        [Fact]
        public async Task ParseHandleErrors()
        {
            var parsingResult = await ParseText("EntryWithErrors");

            Assert.False(parsingResult.Success);
            Assert.Single(parsingResult.Errors);
            var error = parsingResult.Errors.First();
            Assert.Equal(3, error.LineNumber);
            Assert.Equal("Unable to find the type of comment.", error.Message);
            Assert.Equal("#! reference:123", error.LineContent);
        }

        private async Task<ParsingResult> ParseText(string resourceName)
        {
            var fullResourceName = $"MGR.PortableObject.UnitTests.Parsing.Resources.{resourceName}.po";
            var parser = new PortableObjectParser();

            var testAssembly = typeof(ParseAsync).Assembly;
            using (var resource =
                testAssembly.GetManifestResourceStream(fullResourceName) ??
                throw new ArgumentException(nameof(resourceName)))
            {
                using (var reader = new StreamReader(resource))
                {
                    return await parser.ParseAsync(reader, new CultureInfo("fr-fr"));
                }
            }
        }
    }
}