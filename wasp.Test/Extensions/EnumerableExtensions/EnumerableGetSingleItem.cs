using System;
using System.Collections.Generic;
using System.Linq;
using IronSphere.Extensions;
using IronSphere.Extensions.Exceptions;

using Xunit;

namespace wasp.Test.Extensions.EnumerableExtensions;

public class EnumerableGetSingleItem
{
    [Fact]
    public void GetSingleItemOrNull()
    {
        IList<int> items = 0.Range(50).ToList();

        Assert.Equal(1, items.GetSingleItemOrNull(x => x == 1));
            
        items.AddRange(0.Range(50));
            
        Assert.Throws<EquivocalItemException>(() => items.GetSingleItemOrNull(x => x == 1));
            
        Assert.Equal(0, items.GetSingleItemOrNull(x => x == 100));
    }
        
    [Fact]
    public void GetSingleItemOrDefault()
    {
        IList<int> items = 0.Range(50).ToList();

        Assert.Equal(1, items.GetSingleItemOrDefault(x => x == 1));
            
        items.AddRange(0.Range(50));
            
        Assert.Throws<EquivocalItemException>(() => items.GetSingleItemOrDefault(x => x == 1));
            
        Assert.Equal(0, items.GetSingleItemOrDefault(x => x == 100));
    }

    private record Person(int Id, string Name);
        
    [Fact]
    public void GetSingleItem()
    {
        IList<Person> items = 0.Range(50).Select(x => new Person(x, @"Timmy")).ToList();
        Assert.Equal(1, items.GetSingleItem(x => x.Id == 1).Id);
            
        items.AddRange(0.Range(50).Select(x => new Person(x, @"Timmy")).ToList());
            
        Assert.Throws<EquivocalItemException>(() => items.GetSingleItem(x => x.Id == 1));
        Assert.Throws<MissingItemException>(() => items.GetSingleItem(x => x.Id == 100));
            
        IEnumerable<Person>? items2 = null; 
        Assert.Throws<ArgumentNullException>(() => items2!.GetSingleItem(x => x.Id == 1));
    }
        
    [Fact]
    public void GetSingleItemOrDefaultNullOnNullList()
    {
        IEnumerable<int>? items = null; 
        Assert.Throws<ArgumentNullException>(() => items!.GetSingleItemOrNull(x => x == 3));
        Assert.Throws<ArgumentNullException>(() => items!.GetSingleItemOrDefault(x => x == 3));
    }
}