using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.StringExtensions;

public class StringUpholster
{
    [Fact]
    public void UpholsterLeft()
    {
        const string newValue = "start";
        Assert.Equal("xxxstart", newValue.UpholsterLeft(3, 'x'));
            
        Assert.Equal("   start", newValue.UpholsterLeft(3));

        const string? nullInput = null;
        Assert.Throws<ArgumentNullException>(() => nullInput!.UpholsterLeft(3));
        Assert.Throws<ArgumentOutOfRangeException>(() => newValue.UpholsterLeft(-3));
    }
        
    [Fact]
    public void UpholsterRight()
    {
        const string newValue = "start";
        Assert.Equal("startxxx", newValue.UpholsterRight(3, 'x'));
            
        Assert.Equal("start   ", newValue.UpholsterRight(3));

        const string? nullInput = null;
        Assert.Throws<ArgumentNullException>(() => nullInput!.UpholsterRight(3));
        Assert.Throws<ArgumentOutOfRangeException>(() => newValue.UpholsterRight(-3));
    }
        
    [Fact]
    public void Upholster()
    {
        const string newValue = "start";
        Assert.Equal("xxxstartxxx", newValue.Upholster(3, 'x'));
            
        Assert.Equal("   start   ", newValue.Upholster(3));

        const string? nullInput = null;
        Assert.Throws<ArgumentNullException>(() => nullInput!.Upholster(3));
        Assert.Throws<ArgumentOutOfRangeException>(() => newValue.Upholster(-3));
    }
}