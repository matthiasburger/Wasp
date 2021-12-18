using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DateTimeExtensions;

public class DateTimeGetLastOfMonth
{
    [Fact]
    public void GetLastOfMonth()
    {
        DateTime today = DateTime.Today;
        int endOfMonth = DateTime.DaysInMonth(today.Year, today.Month);
            
        DateTime lastOfMonth = today.GetLastOfMonth();

        DateTime current = today;
        while (current.Day < endOfMonth)
        {
            Assert.NotEqual(current, lastOfMonth);
            current = current.AddDays(1);
        }
            
        Assert.Equal(current.Date, lastOfMonth.Date);
    }
}