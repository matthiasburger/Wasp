using System;

using IronSphere.Extensions.Exceptions;

using Xunit;

namespace wasp.Test.Extensions.Exceptions;

public class MissingItemExceptionType
{
    [Fact]
    public void OnCreateInstanceOneArg()
    {
        try
        {
            throw new MissingItemException("this item doesnt exist");
        }
        catch (Exception e) when (e is MissingItemException)
        {
            Assert.Equal("this item doesnt exist", e.Message);
        }
    }
    [Fact]
    public void OnCreateInstanceTwoArg()
    {
        try
        {
            throw new MissingItemException("this item doesnt exist", new Exception("inner one"));
        }
        catch (Exception e) when (e is MissingItemException)
        {
            Assert.Equal("this item doesnt exist", e.Message);
            Assert.Equal("inner one", e.InnerException!.Message);
        }
    }
    [Fact]
    public void OnCreateInstanceNoArg()
    {
        try
        {
            throw new MissingItemException();
        }
        catch (Exception e) when (e is MissingItemException)
        {
            Assert.StartsWith($"Exception of type '{e.GetType().FullName}' was thrown.", e.Message);
        }
    }
}