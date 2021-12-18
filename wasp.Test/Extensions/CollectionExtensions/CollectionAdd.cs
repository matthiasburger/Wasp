using System.Collections.Generic;
using System.Linq;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.CollectionExtensions;

public class CollectionAdd
{
    [Fact]
    public void Add()
    {
        IEnumerable<int> newRange = Enumerable.Range(10, 4).ToList();
        ICollection<int> collectionOfInt = new List<int> { 757, 758 }
            .Add(newRange);

        Assert.Contains(collectionOfInt, i => i == 757);
        Assert.Contains(collectionOfInt, i => i == 758);

        foreach (int value in newRange)
            Assert.Contains(collectionOfInt, i => i == value);
    }

    [Fact]
    public void AddMissing()
    {
        IEnumerable<int> newRange = Enumerable.Range(1, 10).ToList();
        ICollection<int> collectionOfInt = new List<int> { 2, 4, 6, 8 }
            .AddMissing(newRange);

        foreach (int value in newRange)
            Assert.Equal(1, collectionOfInt.Count(x => x == value));
    }

    [Fact]
    public void AddMissingFunc()
    {
        IEnumerable<int> newRange = Enumerable.Range(1, 10).ToList();
        ICollection<int> collectionOfInt = new List<int> { 2, 4, 6, 8 }
            .AddMissing(newRange, x => x + 1);

        foreach (int value in newRange)
            Assert.Equal(1, collectionOfInt.Count(x => x == value));
    }
}