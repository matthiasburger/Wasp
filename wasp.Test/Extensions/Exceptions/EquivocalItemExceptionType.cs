using System;

using IronSphere.Extensions.Exceptions;

using Xunit;

namespace wasp.Test.Extensions.Exceptions
{
    public class EquivocalItemExceptionType
    {
        [Fact]
        public void OnCreateInstanceOneArg()
        {
            try
            {
                throw new EquivocalItemException("this item already exists");
            }
            catch (Exception e) when (e is EquivocalItemException)
            {
                Assert.Equal("this item already exists", e.Message);
            }
        }
        [Fact]
        public void OnCreateInstanceTwoArg()
        {
            try
            {
                throw new EquivocalItemException("this item already exists", new Exception("inner one"));
            }
            catch (Exception e) when (e is EquivocalItemException)
            {
                Assert.Equal("this item already exists", e.Message);
                Assert.Equal("inner one", e.InnerException!.Message);
            }
        }
        [Fact]
        public void OnCreateInstanceNoArg()
        {
            try
            {
                throw new EquivocalItemException();
            }
            catch (Exception e) when (e is EquivocalItemException)
            {
                Assert.StartsWith($"Exception of type '{e.GetType().FullName}' was thrown.", e.Message);
            }
        }
    }
}