using System;
using System.Collections.Generic;
using System.Linq;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DateTimeSpan;

public class DateTimeSpanRange
{
    [Fact]
    public void Range()
    {
        DateTime actualTimeToTest = DateTime.Now;
        DateTime actualEndTimeToTest = actualTimeToTest.AddMonths(1);
        IronSphere.Extensions.DateTimeSpan span = (actualTimeToTest, actualEndTimeToTest).Range();
        Assert.Equal(span.Start, actualTimeToTest);
        Assert.Equal(span.End, actualTimeToTest.AddMonths(1));

        IEnumerable<DateTime> sundays = span.Where(w => w.Date.DayOfWeek == DayOfWeek.Sunday);

        IEnumerable<IronSphere.Extensions.DateTimeSpan> splitSpans = span.Split(sundays.ToArray()).ToList();

        foreach (IronSphere.Extensions.DateTimeSpan dateTimeSpan in splitSpans)
        {
            if (dateTimeSpan == splitSpans.First())
            {
                Assert.Equal(actualTimeToTest.Date, dateTimeSpan.Start.Date);
                Assert.Equal(actualTimeToTest.Date.DayOfWeek, dateTimeSpan.Start.Date.DayOfWeek);
                Assert.Equal(DayOfWeek.Sunday, dateTimeSpan.End.Date.DayOfWeek);
            }
            else if (dateTimeSpan == splitSpans.Last())
            {
                Assert.Equal(actualEndTimeToTest.Date, dateTimeSpan.End.Date);
                Assert.Equal(actualEndTimeToTest.Date.DayOfWeek, dateTimeSpan.End.Date.DayOfWeek);
                Assert.Equal(DayOfWeek.Sunday, dateTimeSpan.Start.Date.DayOfWeek);
            }
            else
            {
                Assert.Equal(DayOfWeek.Sunday, dateTimeSpan.Start.Date.DayOfWeek);
                Assert.Equal(DayOfWeek.Sunday, dateTimeSpan.End.Date.DayOfWeek);
            }
        }

        IronSphere.Extensions.DateTimeSpan spanToSplit = new(actualTimeToTest, actualEndTimeToTest);
        IronSphere.Extensions.DateTimeSpan[] splitSpan = spanToSplit.Split(actualTimeToTest.AddDays(7));
            
        Assert.Equal(2, splitSpan.Length);
        Assert.Equal(splitSpan[0].Start.AddDays(7), splitSpan[1].Start);

        IronSphere.Extensions.DateTimeSpan[] split = spanToSplit.Split(spanToSplit.Start.AddDays(-1));
        Assert.Single(split);
        Assert.Equal(split.First().Start, actualTimeToTest);
        Assert.Equal(split.Last().End, actualEndTimeToTest);
        Assert.Equal(split.First(), split.Last());
    }
}