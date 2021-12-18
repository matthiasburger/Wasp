using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DateTimeExtensions;

public class DateTimeGetFirstOfWeek
{
    [Fact]
    public void GetFirstOfWeek()
    {
        DateTime today = DateTime.Today;
        DateTime firstOfWeek = today.GetFirstOfWeek();

        DateTime current = today;
        while (current.DayOfWeek != DayOfWeek.Monday)
        {
            Assert.NotEqual(current, firstOfWeek);
            current = current.AddDays(-1);
        }
            
        Assert.Equal(current, firstOfWeek);
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