using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DateTimeExtensions
{
    public class DateTimeIsEarlierThan
    {
        [Fact]
        public void IsEarlierThan()
        {
            Assert.True(DateTime.Now.AddDays(-3).IsEarlierThan(TimeSpan.FromDays(2)));
            Assert.False(DateTime.Now.AddDays(-1).IsEarlierThan(TimeSpan.FromDays(2)));
        }
    }
}