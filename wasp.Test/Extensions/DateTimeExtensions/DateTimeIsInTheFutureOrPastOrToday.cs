using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DateTimeExtensions
{
    public class DateTimeIsInTheFutureOrPastOrToday
    {
        [Fact]
        public void IsInTheFuture()
        {
            Assert.False(DateTime.Now.IsInTheFuture());
            Assert.False(DateTime.Now.AddDays(-1).IsInTheFuture());
            Assert.True(DateTime.Now.AddDays(1).IsInTheFuture());
        }
        
        [Fact]
        public void IsInThePast()
        {
            Assert.True(DateTime.Now.IsInThePast());
            Assert.True(DateTime.Now.AddDays(-1).IsInThePast());
            Assert.False(DateTime.Now.AddDays(1).IsInThePast());
        }
        
        [Fact]
        public void IsToday()
        {
            Assert.True(DateTime.Now.IsToday());
            Assert.True(DateTime.Now.EndOfDay().AddHours(-1).IsToday());
            Assert.False(DateTime.Now.AddDays(-1).IsToday());
            Assert.False(DateTime.Now.AddDays(1).IsToday());
        }
    }
}