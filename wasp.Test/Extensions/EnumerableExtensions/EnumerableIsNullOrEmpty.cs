using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using IronSphere.Extensions;

using Xunit;

namespace wasp.Test.Extensions.EnumerableExtensions;

public class EnumerableIsNullOrEmpty
{
    [Fact]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "unittest-method")]
    public void IsNullOrEmpty()
    {
        IEnumerable<int>? myIntList = null;
        Assert.True(myIntList.IsNullOrEmpty());
        Assert.False(myIntList.IsSingle());

        myIntList = new List<int>(); 
        Assert.True(myIntList.IsNullOrEmpty());
        Assert.False(myIntList.IsSingle());

        myIntList = myIntList.AddItem(3).If((x, _)=>x.Any(s => s == 1));
        Assert.True(myIntList.IsNullOrEmpty());
        Assert.False(myIntList.IsSingle());
            
        myIntList = myIntList.AddItem(3);
        Assert.False(myIntList.IsNullOrEmpty());
        Assert.True(myIntList.IsSingle());
            
        myIntList = myIntList.AddItem(4).If((x, _)=>x.Any(s => s == 3));
        Assert.False(myIntList.IsNullOrEmpty());
        Assert.False(myIntList.IsSingle());
        Assert.Contains(myIntList, i => i == 3);
        Assert.Contains(myIntList, i => i == 4);
        Assert.Equal(2, myIntList.Count());

        myIntList = myIntList.RemoveItem(3).If((i, _) => i.Any(y=>y == 5));
            
        Assert.False(myIntList.IsNullOrEmpty());
        Assert.False(myIntList.IsSingle());
        Assert.True(myIntList.IsSingle(x => x == 3));
        Assert.Contains(myIntList, i => i == 3);
        Assert.Contains(myIntList, i => i == 4);
        Assert.Equal(2, myIntList.Count());
            
        myIntList = myIntList.RemoveItem(3).If((i, _) => i.Any(y=>y == 4));
            
        Assert.False(myIntList.IsNullOrEmpty());
        Assert.Single(myIntList);
        Assert.True(myIntList.IsSingle());
        Assert.DoesNotContain(myIntList, i => i == 3);
        Assert.Contains(myIntList, i => i == 4);
            
        myIntList = myIntList.RemoveItem(4);
            
        Assert.True(myIntList.IsNullOrEmpty());
        Assert.False(myIntList.IsSingle());
    }

    private record Person(string Name);

    [Fact]
    public void IsSingle()
    {
        IEnumerable<Person>? persons = new List<Person>
        {
            new ("test"),
            new ("quick fox"),
            new ("quick fox")
        };
        Assert.False(persons.IsSingle(x => x.Name));

        persons = persons.RemoveItem(persons.First());
        Assert.True(persons.IsSingle(x => x.Name));

        persons = null;
        Assert.False(persons.IsSingle(x => x.Name));
    }
        
    [Fact]
    public void AllPropertiesValuesTheSame()
    {
        IEnumerable<Person>? persons = new List<Person>
        {
            new ("test"),
            new ("quick fox"),
            new ("quick fox")
        };
        Assert.False(persons.AllValuesTheSameFor(x => x.Name));

        persons = persons.RemoveItem(persons.First());
        Assert.True(persons.AllValuesTheSameFor(x => x.Name));

        persons = null;
        Assert.False(persons.AllValuesTheSameFor(x => x.Name));
    }
}