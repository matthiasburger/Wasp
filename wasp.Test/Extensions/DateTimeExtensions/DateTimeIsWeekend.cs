using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DateTimeExtensions
{
    public class DateTimeIsWeekend
    {
        [Fact]
        public void IsWeekend()
        {
            IronSphere.Extensions.DateTimeSpan span = DateTime.Today.GetFirstOfMonth()
                .SpanTo(DateTime.Today.GetLastOfMonth());

            Assert.Equal(DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month), span.End.Day);
            
            foreach (DateTime date in span)
                Assert.Equal(date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday, date.IsWeekend());
        }
    }
}