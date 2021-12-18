using System;

using IronSphere.Extensions;

using Xunit;

namespace wasp.Test.Extensions.EnumExtensions;

public class EnumFunctionTests
{
    [Flags]
    private enum MyEnum
    {
        One = 1,
        Two = 2,
        Three = One | Two,
        Four = 4,
        Five = Four | One,
        Six = Four | Two,
        Seven = Four | Three,
        Eight = 8
    }
        
    [Fact]
    public void Has()
    {
        MyEnum? value = MyEnum.One | MyEnum.Three;
        Assert.True(value.Has(MyEnum.One));
        Assert.True(value.Has(MyEnum.Three));
        Assert.False(value.Has(MyEnum.Four));
        Assert.False(value.Has((MyEnum?)null));
        Assert.False(value.Has((MyEnum?)27));
    }
        
    [Fact]
    public void Is()
    {
        MyEnum? value = MyEnum.Three;
        Assert.False(value.Is(MyEnum.One));
        Assert.True(value.Is(MyEnum.Three));
        Assert.False(value.Is(MyEnum.Four));
        Assert.False(value.Is((MyEnum?)null));
        Assert.False(value.Is((MyEnum?)27));
    }
        
    [Fact]
    public void AddRemove()
    {
        MyEnum? value = MyEnum.One | MyEnum.Three;
        Assert.True(value.Has(MyEnum.One));
        Assert.True(value.Has(MyEnum.Three));
        Assert.False(value.Has(MyEnum.Four));
        value = value.Add(MyEnum.Four);
        Assert.True(value.Has(MyEnum.Four));
        value = value.Remove(MyEnum.Four);
        Assert.False(value.Has(MyEnum.Four));
        value = value.Remove((MyEnum?)null);
        Assert.Null(value);
            
        value = MyEnum.Eight;
        Assert.Equal(MyEnum.Eight, value);
        value = value.Add((MyEnum?)null);
        Assert.Null(value);
    }
}