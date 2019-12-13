using System;
using System.Globalization;
using MGR.PortableObject.Parsing;
using Xunit;

namespace MGR.PortableObject.UnitTests.Parsing
{
    public partial class PluralFormParserTests
    {
        public class Parse
        {
            [Fact]
            public void ParseSuccessfullyForInteger()
            {
                var pluralFormParser = new PluralFormParser();
                var pluralFormFunc = "n != 1 ? 1 : 0";
                var pluralFormHeader = $"Plural-Forms: nplurals=2; plural={pluralFormFunc};\n";

                var pluralForm = pluralFormParser.Parse(pluralFormHeader);
                Assert.Equal(1, pluralForm.GetPluralFormForQuantity(0));
                Assert.Equal(0, pluralForm.GetPluralFormForQuantity(1));
                Assert.Equal(1, pluralForm.GetPluralFormForQuantity(2));
                Assert.Equal(1, pluralForm.GetPluralFormForQuantity(3));
            }

            [Fact]
            public void ParseSuccessfullyForBoolean()
            {
                var pluralFormParser = new PluralFormParser();
                var pluralFormFunc = "n != 1";
                var pluralFormHeader = $"Plural-Forms: nplurals=2; plural={pluralFormFunc};\n";

                var pluralForm = pluralFormParser.Parse(pluralFormHeader);
                Assert.Equal(1, pluralForm.GetPluralFormForQuantity(0));
                Assert.Equal(0, pluralForm.GetPluralFormForQuantity(1));
                Assert.Equal(1, pluralForm.GetPluralFormForQuantity(2));
                Assert.Equal(1, pluralForm.GetPluralFormForQuantity(3));
            }

            [Fact]
            public void ThrowsExceptionWhenPluralFormIsInvalid()
            {
                var pluralFormParser = new PluralFormParser();
                var pluralFormFunc = "n !=";
                var pluralFormHeader = $"Plural-Forms: nplurals=2; plural={pluralFormFunc};\n";
                CultureInfo.CurrentUICulture = new CultureInfo("en-us");
                CultureInfo.CurrentCulture = new CultureInfo("en-us");
                var actualException =
                    Assert.Throws<InvalidOperationException>(() => pluralFormParser.Parse(pluralFormHeader));
                Assert.Equal("Compilation failed, first error is: CS1525: Invalid expression term ';';",
                    actualException.Message);
            }
        }
    }
}
