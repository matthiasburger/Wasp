using System;
using System.IO;
using System.Text;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.BinaryReaderExtensions;

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
    [Fact]
    public void GetBytesFromStream()
    {
        const string fakeFileContents = "Hello world";
        byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
        using MemoryStream fakeMemoryStream = new (fakeFileBytes);
        Assert.Equal(fakeFileBytes, fakeMemoryStream.GetBytes());
    }
        
    [Fact]
    public void ReadAllBytesFromStream()
    {
        const string fakeFileContents = "Hello world";
        byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
        using MemoryStream fakeMemoryStream = new (fakeFileBytes);
        Assert.Equal(fakeFileBytes, fakeMemoryStream.ReadAllBytes());
    }
        
    [Fact]
    public void GetBytesFromNullStream()
    {
        using Stream? fakeMemoryStream = null;
        Assert.Throws<ArgumentNullException>(()=> fakeMemoryStream!.GetBytes());
    }
        
    [Fact]
    public void GetBytesFromMinusLength()
    {
        const string fakeFileContents = "Hello world";
        byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
        using MemoryStream fakeMemoryStream = new (fakeFileBytes);
        Assert.Throws<ArgumentOutOfRangeException>(() => fakeMemoryStream.GetBytes(-200));
    }
}