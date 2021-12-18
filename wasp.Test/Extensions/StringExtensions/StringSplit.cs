using System;
using System.Linq;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.StringExtensions;

public class StringSplit
{
    [Theory]
    [InlineData("myProperty is pretty-Good_Andso", "my Property is pretty Good Andso")]
    public void Split(string oldValue, string newValue)
    {
        string[] split = oldValue.Split(
            x => x.In(' ', '-', '_'),
            char.IsUpper
        ).ToArray();

        Assert.Equal(newValue, string.Join(" ", split));
    }

    [Theory]
    [InlineData("myProperty is pretty-Good_Andso", "my Property is pretty- Good_ Andso")]
    public void SplitNoExclude(string oldValue, string newValue)
    {
        string[] split = oldValue.Split(
            onInclude: char.IsUpper
        ).ToArray();

        Assert.Equal(newValue, string.Join(" ", split));
    }

    [Theory]
    [InlineData("myProperty is pretty-Good_Andso", "myProperty is pretty Good Andso")]
    public void SplitNoInclude(string oldValue, string newValue)
    {
        string[] split = oldValue.Split(
            x => x.In(' ', '-', '_')
        ).ToArray();

        Assert.Equal(newValue, string.Join(" ", split));
    }

    [Fact]
    public void SplitNull()
    {
        string? oldValue = null;
        string[] split = oldValue.Split(
            x => x.In(' ', '-', '_')
        ).ToArray();

        Assert.Equal(string.Empty, string.Join(" ", split));
    }
        
    [Fact]
    public void SplitAtPosition()
    {
        const string oldValue = "The quick brown fox jumps over the lazy dog";
        string[] split = oldValue.Split(7).ToArray();

        string[] expectedResult = { "The qui", "ck brow", "n fox j", "umps ov", "er the ", "lazy do", "g" };

        Assert.Equal(expectedResult, split);
    }
        
    [Fact]
    public void SplitNullAtPosition()
    {
        const string? oldValue = null;
        Assert.Throws<ArgumentNullException>(() => oldValue!.Split(7).ToArray());
    }
        
        
    [Theory, InlineData(-1), InlineData(0)]
    public void SplitNullAtTooLowPosition(int data)
    {
        const string oldValue = "The quick brown fox jumps over the lazy dog";
        string[] split = oldValue.Split(data).ToArray();
        Assert.Equal(new[]{oldValue}, split);
    }
}