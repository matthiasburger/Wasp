using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DateTimeExtensions;

public class DateTimeGetCalendarWeek
{
    [Fact]
    public void GetCalendarWeek()
    {
        DateTime dateTime1121 = new (2021, 1,1);
        IronSphere.Extensions.CalendarWeek calendarWeek21 = dateTime1121.GetCalendarWeek();
        Assert.Equal(new IronSphere.Extensions.CalendarWeek(53,2020), calendarWeek21);

        DateTime dateTime1122 = new(2022, 1, 1);
        IronSphere.Extensions.CalendarWeek calendarWeek22 = dateTime1122.GetCalendarWeek();
        Assert.Equal(new IronSphere.Extensions.CalendarWeek(52,2021), calendarWeek22);

        DateTime dateTime1123 = new(2023, 1, 1);
        IronSphere.Extensions.CalendarWeek calendarWeek23 = dateTime1123.GetCalendarWeek();
        Assert.Equal(new IronSphere.Extensions.CalendarWeek(52,2022), calendarWeek23);

        DateTime dateTime1124 = new(2024, 1, 1);
        IronSphere.Extensions.CalendarWeek calendarWeek24 = dateTime1124.GetCalendarWeek();
        Assert.Equal(new IronSphere.Extensions.CalendarWeek(1,2024), calendarWeek24);
    }
}