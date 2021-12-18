using System.Collections.Generic;
using System.Linq;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.IntegerExtensions;

public class IntegerRange
{
    [Fact]
    public void Range()
    {
        IEnumerable<int> range = 3.Range(3).ToList();
            
        Assert.Equal(3, range.Min());
        Assert.Equal(5, range.Max());
        Assert.Equal(3, range.Count());
    }
}