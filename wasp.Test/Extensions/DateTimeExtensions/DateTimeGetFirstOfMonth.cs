using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DateTimeExtensions;

public class DateTimeGetFirstOfMonth
{
    [Fact]
    public void GetFirstOfMonth()
    {
        DateTime today = DateTime.Today;
        DateTime firstOfMonth = today.GetFirstOfMonth();

        DateTime current = today;
        while (current.Day > 1)
        {
            Assert.NotEqual(current, firstOfMonth);
            current = current.AddDays(-1);
        }
            
        Assert.Equal(current, firstOfMonth);
    }
        
    [Fact]
    public void GetFirstOfWeekLastWeek()
    {
        DateTime today = DateTime.Today.GetFirstOfWeek().AddDays(-1);
        DateTime firstOfWeek = today.GetFirstOfWeek();

        DateTime current = today;
        while (current.DayOfWeek != DayOfWeek.Monday)
        {
            Assert.NotEqual(current, firstOfWeek);
            current = current.AddDays(-1);
        }
            
        Assert.Equal(current, firstOfWeek);
    }
}