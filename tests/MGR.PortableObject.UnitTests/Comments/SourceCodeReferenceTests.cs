using MGR.PortableObject.Comments;
using Xunit;

namespace MGR.PortableObject.UnitTests.Comments
{
    public class SourceCodeReferenceTests
    {
        [Fact]
        public void Create_A_SourceCodeReference_Correctly_Parse_The_Reference()
        {
            var reference = "src/hello.c:123";

            var sourceCodeReference = new SourceCodeReference(reference);

            Assert.Equal("src/hello.c", sourceCodeReference.FilePath);
            Assert.Equal(123, sourceCodeReference.LineNumber);
        }
    }
}
