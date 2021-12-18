using System;
using System.Collections.Generic;
using System.Linq;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DateTimeSpan;

public class DateTimeSpanConstruct
{
    [Fact]
    public void Construct()
    {
        DateTime firstOfWeek = DateTime.Today.GetFirstOfWeek();
        DateTime lastOfWeek = DateTime.Today.GetLastOfWeek();
            
        Assert.Equal(firstOfWeek.GetCalendarWeek(), lastOfWeek.GetCalendarWeek());

        IronSphere.Extensions.DateTimeSpan span = new (firstOfWeek, lastOfWeek)
        {
            Step = 1,
            SpanType = DateTimeSpanType.Days
        };
        Assert.Equal(6, span.Count());
        Assert.Equal(6, span.GetDuration(x=>x.Days));
    }
        
    [Fact]
    public void ConstructDifferentTypes()
    {
        IEnumerable<DateTimeSpanType> types = Toolbox.GetEnumValues<DateTimeSpanType>();
        foreach (DateTimeSpanType dateTimeSpanType in types)
        {
            IronSphere.Extensions.DateTimeSpan span = new (DateTime.Today, dateTimeSpanType, 1);
            Assert.True(span.End > span.Start);
            Assert.Equal(1, span.Step);
            Assert.Equal(dateTimeSpanType, span.SpanType);
        }
    }
        
    [Fact]
    public void ConstructEmpty()
    {
        DateTime firstOfWeek = DateTime.Today.GetFirstOfWeek();
        DateTime lastOfWeek = DateTime.Today.GetLastOfWeek();
            
        Assert.Equal(firstOfWeek.GetCalendarWeek(), lastOfWeek.GetCalendarWeek());

        IronSphere.Extensions.DateTimeSpan span = new (firstOfWeek, DateTimeSpanType.Days, 6);
        Assert.Equal(6, span.GetDuration(x => x.Days));

        (DateTime start, DateTime end) = span;
            
        Assert.Equal(firstOfWeek, start);
        Assert.Equal(lastOfWeek, end);
    }

    [Fact]
    public void ToStringTest()
    {
        DateTime firstOfWeek = DateTime.Today.GetFirstOfWeek();
        DateTime lastOfWeek = DateTime.Today.GetLastOfWeek();

        IronSphere.Extensions.DateTimeSpan span = new (firstOfWeek, DateTimeSpanType.Days, 6);

        Assert.Equal($"{firstOfWeek:dd.MM.yyyy HH:mm} - {lastOfWeek:dd.MM.yyyy HH:mm}", span.ToString());
    }

    [Fact]
    public void GetEnumeratorTest()
    {
        DateTime firstOfWeek = DateTime.Today.GetFirstOfWeek();
        DateTime lastOfWeek = DateTime.Today.GetLastOfWeek();

        const int count = 7;

        IronSphere.Extensions.DateTimeSpan span = new (firstOfWeek, DateTimeSpanType.Days, count);

        using IEnumerator<DateTime> enumerator = span.GetEnumerator();
        Assert.NotNull(enumerator);

        int counter = 0;
        DateTime? lastDate = null;
        while (enumerator.MoveNext())
        {
            Assert.True(counter == 0 || lastDate.HasValue);

            if (lastDate.HasValue)
                Assert.True(enumerator.Current > lastDate);

            lastDate = enumerator.Current;
            counter++;
        }
        Assert.Equal(lastOfWeek, lastDate);
        Assert.Equal(count, counter);
    }
}