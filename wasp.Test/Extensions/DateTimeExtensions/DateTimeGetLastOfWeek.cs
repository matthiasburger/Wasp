using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DateTimeExtensions
{
    public class DateTimeGetLastOfWeek
    {
        [Fact]
        public void GetLastOfWeek()
        {
            DateTime today = DateTime.Today;
            DateTime sunday = today;
            
            while (sunday.DayOfWeek != DayOfWeek.Sunday)
            {
                Assert.NotEqual(sunday, today.GetLastOfWeek());
                sunday = sunday.Add(TimeSpan.FromDays(1));
            }
            
            Assert.Equal(sunday, today.GetLastOfWeek());
        }
    }
}