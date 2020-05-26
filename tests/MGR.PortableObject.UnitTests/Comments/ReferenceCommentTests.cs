using System.Linq;
using MGR.PortableObject.Comments;
using Xunit;

namespace MGR.PortableObject.UnitTests.Comments
{
    public class ReferenceCommentTests
    {
        [Fact]
        public void Correctly_Parse_Reference_Comment()
        {
            var referenceCommentContent = "src/hello.c:123 src/hello.c:456";
            var referenceComment = new ReferencesComment(referenceCommentContent);

            Assert.Equal(2, referenceComment.References.Count());

            var firstSourceCodeReference = referenceComment.References.First();
            Assert.Equal("src/hello.c", firstSourceCodeReference.FilePath);
            Assert.Equal(123, firstSourceCodeReference.LineNumber);

            var secondSourceCodeReference = referenceComment.References.Skip(1).First();
            Assert.Equal("src/hello.c", secondSourceCodeReference.FilePath);
            Assert.Equal(456, secondSourceCodeReference.LineNumber);
        }
    }
}
