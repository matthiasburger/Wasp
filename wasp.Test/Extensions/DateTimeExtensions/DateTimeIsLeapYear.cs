using System;
using System.Collections.Generic;
using System.Linq;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DateTimeExtensions;

public class DateTimeIsLeapYear
{
    [Theory]
    [InlineData(2000, 40)]
    public void IsLeapYear(int startYear, int count)
    {
        IEnumerable<int> yearRange = Enumerable.Range(startYear, count).ToList();
        Assert.Equal(count, yearRange.Count());

        foreach (int year in yearRange)
        {
            int daysInFebruary = DateTime.DaysInMonth(year, 2);
            Assert.Equal(daysInFebruary == 29, new DateTime(year, 1, 1).IsLeapYear());
        }
    }
}