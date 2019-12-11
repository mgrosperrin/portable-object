using System;
using MGR.PortableObject.Parsing;
using Xunit;

namespace MGR.PortableObject.UnitTests.Parsing
{
    public class PluralFormBuilderTests
    {
        [Fact]
        public void D()
        {
            var p = new PluralFormParser();

            //p.Parse("n%10==1 && n%100!=11 ? 0 : n%10>=2 && n%10<=4 && (n%100<10 || n%100>=20) ? 1 : 2");
            var r = p.Parse("n != 1")??throw new Exception();
            Assert.Equal(0, r.GetPluralFormForNumber(1));
            Assert.Equal(1, r.GetPluralFormForNumber(2));
            Assert.Equal(1, r.GetPluralFormForNumber(3));
            r = p.Parse("n%10==1 && n%100!=11 ? 0 : n%10>=2 && n%10<=4 && (n%100<10 || n%100>=20) ? 1 : 2") ?? throw new Exception();
            Assert.Equal(0, r.GetPluralFormForNumber(1));
            Assert.Equal(2, r.GetPluralFormForNumber(20));
            Assert.Equal(1, r.GetPluralFormForNumber(32));
        }
    }
}
