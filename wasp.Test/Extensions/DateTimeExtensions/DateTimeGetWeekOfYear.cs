using System;
using System.Globalization;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DateTimeExtensions;

public class DateTimeGetWeekOfYear
{
    [Fact]
    public void GetWeekOfYear()
    {
        DateTime current = new(2021, 12, 10);
        int week = current.GetCalendarWeek().Week;
            
        Assert.Equal(week, current.GetWeekOfYear(CultureInfo.CurrentCulture));
        Assert.Equal(week, current.GetWeekOfYear());
        Assert.Equal(week, current.GetWeekOfYear(CultureInfo.CurrentCulture, WeekOfYearStandard.DotNet));
        Assert.Equal(week, current.GetWeekOfYear(null, WeekOfYearStandard.DotNet));
            
        Assert.Throws<ArgumentOutOfRangeException>(()=>current.GetWeekOfYear(null, WeekOfYearStandard.NotImplemented));
    }
}