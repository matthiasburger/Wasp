using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DateTimeExtensions
{
    public class DateTimeNextPrevious
    {
        [Fact]
        public void Next()
        {
            DateTime today = DateTime.Today;
            DayOfWeek dayOfWeek = today.DayOfWeek;
            
            Assert.Equal(today, today.Next(dayOfWeek));
            Assert.Equal(today.AddDays(6), today.Next(dayOfWeek - 1));
            Assert.Equal(today.AddDays(1), today.Next(dayOfWeek + 1));
        }
        
        [Fact]
        public void Previous()
        {
            DateTime today = DateTime.Today;
            DayOfWeek dayOfWeek = today.DayOfWeek;
            
            Assert.Equal(today.AddDays(-7), today.Previous(dayOfWeek));
            Assert.Equal(today.AddDays(-1), today.Previous(dayOfWeek - 1));
            Assert.Equal(today.AddDays(-6), today.Previous(dayOfWeek + 1));
        }
    }
}