using System;
using System.Linq;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.GenericExtensions;

public class GenericReplaceIf
{
    [Fact]
    public void ReplaceIfFunc()
    {
        const string originalValue = "bla";
        string newValue = originalValue.ReplaceIf(x => x.All(char.IsLower), x => x.ToUpper());
            
        Assert.Equal(originalValue.ToUpper(), newValue);
    }
        
    [Fact]
    public void ReplaceIf()
    {
        const string originalValue = "bla";
        const string valueToReplace = "BLA";
        string newValue = originalValue.ReplaceIf(x => x.All(char.IsLower), valueToReplace);
            
        Assert.Equal(valueToReplace, newValue);
    }
        
    [Fact]
    public void ReplaceIfExpressionNull()
    {
        const string originalValue = "bla";
        Func<string, string>? nullTransformExpression = null;
        Func<string, bool>? nullCheckExpression = null;
        Assert.Throws<ArgumentNullException>(() => originalValue.ReplaceIf(x => x.All(char.IsLower), nullTransformExpression!));
        Assert.Throws<ArgumentNullException>(() => originalValue.ReplaceIf(nullCheckExpression!, x => x));
        Assert.Throws<ArgumentNullException>(() => originalValue.ReplaceIf(nullCheckExpression!, "result"));
    }
        
    [Fact]
    public void ReplaceIfOriginalNull()
    {
        const string? originalValue = null;
        const string valueToReplace = "BLA";
        string newValue = originalValue.ReplaceIf(x => x == null, valueToReplace)!;
            
        Assert.Equal(valueToReplace, newValue);
    }
        
    [Fact]
    public void ReplaceIfNewNull()
    {
        const string originalValue = "bla";
        const string? valueToReplace = null;
        string? newValue = originalValue.ReplaceIf(x => x == "bla", valueToReplace);
            
        Assert.Equal(valueToReplace, newValue);
    }
}