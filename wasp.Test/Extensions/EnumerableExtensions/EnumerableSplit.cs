using System;
using System.Collections.Generic;
using System.Linq;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.EnumerableExtensions;

public class EnumerableSplit
{
    [Theory]
    [InlineData(70, 30)]
    [InlineData(50, 5)]
    [InlineData(50, 7)]
    [InlineData(195, 27)]
    public void Split(int lengthOfList, int split)
    {
        IList<int>? items = null;

        Assert.Throws<ArgumentNullException>(() => items!.Split(split).ToList());
            
        items = 0.Range(lengthOfList).ToList();

        int expectedListLength = lengthOfList / split;
        if (lengthOfList % split > 0)
            expectedListLength += 1;

        Assert.Equal(expectedListLength, items.Split(split).Count());
    }
}