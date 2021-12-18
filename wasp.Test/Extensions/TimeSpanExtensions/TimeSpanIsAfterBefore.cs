using System;

using IronSphere.Extensions;

using Xunit;

namespace wasp.Test.Extensions.TimeSpanExtensions;

public class TimeSpanIsAfterBefore
{
    [Fact]
    public void IsAfter()
    {
        TimeSpan timeSpan = DateTime.Now.TimeOfDay;
        TimeSpan future = timeSpan.Add(TimeSpan.FromMinutes(20));
        TimeSpan past = timeSpan.Add(TimeSpan.FromMinutes(-20));
            
        Assert.True(timeSpan.IsAfter(past));
        Assert.True(timeSpan.IsBefore(future));
    }
        
    [Fact]
    public void Round()
    {
        TimeSpan roundedTimeSpanDown = new TimeSpan(13, 22, 0).Round(TimeSpan.FromMinutes(5), MidpointRounding.ToEven);
        TimeSpan roundedTimeSpanUp = new TimeSpan(13, 23, 0).Round(TimeSpan.FromMinutes(5), MidpointRounding.ToEven);
            
        Assert.Equal(new TimeSpan(13,20,0), roundedTimeSpanDown);
        Assert.Equal(new TimeSpan(13,25,0), roundedTimeSpanUp);
    }
}