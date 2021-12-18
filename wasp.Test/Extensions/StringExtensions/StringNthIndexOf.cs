using System.Linq;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.StringExtensions;

public class StringNthIndexOf
{
    [Fact]
    public void NthIndexOf()
    {
        const string blindText = "The quick brown fox jumps over the lazy dog";
        int index = blindText.NthIndexOf("e", 2);

        Assert.True(blindText.Count(x=>x == 'e') >= 2);
        char value = blindText[index];
        Assert.Equal('e', value);
    }
        
    [Fact]
    public void NthIndexOfTooHigh()
    {
        const string blindText = "The quick brown fox jumps over the lazy dog";
        int index = blindText.NthIndexOf("e", 20);

        Assert.Equal(-1, index);
    }
}