using System;
using System.Globalization;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.CalendarWeek
{
    public class CalendarWeekConstruct
    {
        [Fact]
        public void CreateCalendarWeek()
        {
            IronSphere.Extensions.CalendarWeek current = new(12, 2021);
            Assert.Equal(12, current.Week);
            Assert.Equal(2021, current.Year);

            IronSphere.Extensions.DateTimeSpan dateTimeSpan = current.GetDateTimeSpan();
            dateTimeSpan.SpanType = DateTimeSpanType.Days;
            dateTimeSpan.Step = 1;
            
            Assert.Equal(new DateTime(2021, 3, 22),dateTimeSpan.Start);
            Assert.Equal(new DateTime(2021, 3, 28),dateTimeSpan.End);
            Assert.Equal(dateTimeSpan.Start, current.GetFirstWeekDay());
        }
        
        [Fact]
        public void CreateCalendarWeekCulture()
        {
            IronSphere.Extensions.CalendarWeek current = new(12, 2021, new CultureInfo("de-DE"));
            Assert.Equal(12, current.Week);
            Assert.Equal(2021, current.Year);

            IronSphere.Extensions.DateTimeSpan dateTimeSpan = current.GetDateTimeSpan();
            dateTimeSpan.SpanType = DateTimeSpanType.Days;
            dateTimeSpan.Step = 1;
            
            Assert.Equal(new DateTime(2021, 3, 22),dateTimeSpan.Start);
            Assert.Equal(new DateTime(2021, 3, 28),dateTimeSpan.End);
            Assert.Equal(dateTimeSpan.Start, current.GetFirstWeekDay());
        }
        
        [Fact]
        public void ToStringTest()
        {
            IronSphere.Extensions.CalendarWeek current = new(12, 2021, new CultureInfo("de-DE"));
            Assert.Equal("12/2021", current.ToString());
            Assert.Equal("12-2021", current.ToString("ww-yyyy"));
            Assert.Equal("12-2021", current.ToString("w-yyyy"));
            
            IronSphere.Extensions.CalendarWeek next = new(4, 2021, new CultureInfo("de-DE"));
            Assert.Equal("04-2021", next.ToString("ww-yyyy"));
            Assert.Equal("4-2021", next.ToString("w-yyyy"));
        }
        
        [Fact]
        public void EqualsTest()
        {
            IronSphere.Extensions.CalendarWeek current = new(12, 2021, new CultureInfo("de-DE"));

            IronSphere.Extensions.CalendarWeek toCheck = current.GetFirstWeekDay().GetCalendarWeek();
            
            Assert.True(current.Equals(toCheck));
        }
        
        [Fact]
        public void EqualsTestToday()
        {
            IronSphere.Extensions.CalendarWeek current = DateTime.Today.GetCalendarWeek();
            
            Assert.Equal(current.GetFirstWeekDay(), DateTime.Today.GetFirstOfWeek());
        }
    }
}