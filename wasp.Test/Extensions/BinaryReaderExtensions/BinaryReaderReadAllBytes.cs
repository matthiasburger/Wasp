using System.IO;
using System.Text;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.BinaryReaderExtensions
{
    public class BinaryReaderReadAllBytes
    {
        [Fact]
        public void ReadAllBytes()
        {
            const string fakeFileContents = "Hello world";
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
            using MemoryStream fakeMemoryStream = new (fakeFileBytes);
            using BinaryReader binaryReader = new(fakeMemoryStream);
            Assert.Equal(fakeFileBytes, binaryReader.ReadAllBytes());
        }
    }
}