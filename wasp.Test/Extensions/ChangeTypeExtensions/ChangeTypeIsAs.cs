using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.ChangeTypeExtensions;

public class ChangeTypeIsAs
{
    private interface ITest
    {
        void DoTest();
    }
    private interface ITestNotImplementing
    {
        void DoTest();
    }
        
    private class Test : ITest
    {
        public void DoTest()
        {
        }
    }
        
    [Fact]
    public void Is()
    {
        Test t = new ();
        Assert.True(t.Is<ITest>());
        Assert.False(t.Is<ITestNotImplementing>());
    }
        
    [Fact]
    public void IsNot()
    {
        Test t = new ();
        Assert.False(t.IsNot<ITest>());
        Assert.True(t.IsNot<ITestNotImplementing>());
    }
        
    [Fact]
    public void As()
    {
        Test t = new ();
        Assert.NotNull(t.As<ITest>());
        Assert.Null(t.As<ITestNotImplementing>());
    }
}