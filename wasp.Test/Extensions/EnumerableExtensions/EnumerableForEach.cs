using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.EnumerableExtensions;

public class EnumerableForEach
{
    [Fact]
    public void ForEach()
    {
        StringBuilder sb = new(50);
            
        Assert.True(sb.IsEmpty());
            
        IEnumerable<int> values = 0.Range(50);
            
        values.ForEach(x => sb.Append((char)x), false);
        Assert.Empty(sb.ToString());
            
        values.ForEach(x => sb.AppendIf(true, ((char)x).ToString()));
        Assert.Equal(50, sb.ToString().Length);
        Assert.False(sb.IsEmpty());
    }
        
    [Fact]
    public void TryForEach()
    {
        StringBuilder sb = new(50);
            
        IEnumerable<int> values = 0.Range(50);
            
        values.TryForEach(x =>
        {
            int value = 2 / x;
            value = x;
            return sb.Append((char)value);
        });
        Assert.Equal(49, sb.ToString().Length);
    }
}