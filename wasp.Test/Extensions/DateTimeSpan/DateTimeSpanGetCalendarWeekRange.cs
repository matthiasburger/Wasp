using System;
using System.Collections.Generic;
using System.Linq;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DateTimeSpan
{
    public class DateTimeSpanGetCalendarWeekRange
    {
        [Fact]
        public void GetCalendarWeekRange()
        {
            IronSphere.Extensions.DateTimeSpan dateTimeSpan = DateTime.Now.SpanTo(DateTime.Now.AddDays(28));

            IEnumerable<IronSphere.Extensions.CalendarWeek> range = dateTimeSpan.GetCalendarWeekRange();
            Assert.Equal(5, range.Count());
        }

        [Fact]
        public void GetCalendarWeekRangeOverYear()
        {
            IronSphere.Extensions.DateTimeSpan dateTimeSpan =
                new DateTime(2024, 12, 15)
                    .SpanTo(new DateTime(2025, 1, 14));

            IEnumerable<IronSphere.Extensions.CalendarWeek> range = dateTimeSpan.GetCalendarWeekRange();
            Assert.Equal(6, range.Count());
        }
    }
}