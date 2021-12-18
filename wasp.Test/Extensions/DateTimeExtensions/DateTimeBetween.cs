using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DateTimeExtensions;

public class DateTimeBetween
{
    [Fact]
    public void BetweenDateTimeSpan()
    {
        IronSphere.Extensions.DateTimeSpan wholeYear =
            new DateTime(DateTime.Today.Year, 1, 1).SpanTo(12, DateTimeSpanType.Months);
        Assert.Equal(wholeYear.Start, new DateTime(DateTime.Today.Year, 1, 1));
        Assert.Equal(wholeYear.End, new DateTime(DateTime.Today.Year + 1, 1, 1));
        Assert.True(DateTime.Today.Between(wholeYear));
    }
        
    [Fact]
    public void Between()
    {
        DateTime startDate = new (DateTime.Today.Year, 1, 1);
        DateTime endDate = new (DateTime.Today.Year, 12, 31);
        Assert.True(DateTime.Today.Between(startDate, endDate.EndOfDay()));
    }
}