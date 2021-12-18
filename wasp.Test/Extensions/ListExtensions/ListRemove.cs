using System;
using System.Collections.Generic;
using System.Linq;

using IronSphere.Extensions;

using Xunit;

namespace wasp.Test.Extensions.ListExtensions;

public class ListRemove
{
    private record Person(int Id, string? Name);
        
    [Fact]
    public void Remove()
    {
        List<Person>? listOfValues = null;
        Assert.Throws<ArgumentNullException>(() => listOfValues!.RemoveSingle(x => x.Id == 1));
        Assert.Throws<ArgumentNullException>(() => listOfValues!.RemoveWhere(x => x.Id == 1));
        Assert.Throws<ArgumentNullException>(() => listOfValues!.AddItem(new Person(-3, "")));
        Assert.Throws<ArgumentNullException>(() => listOfValues!.AddItemIf(new Person(-3, ""), x=> x.Name != null));

        listOfValues = 0.Range(50).Select(x=>new Person(x, "Test")).ToList();

        Func<Person, bool>? expression = null;
        Assert.Throws<ArgumentNullException>(() => listOfValues.RemoveSingle(expression!));
        Assert.Throws<ArgumentNullException>(() => listOfValues.RemoveWhere(expression!));

        Assert.Equal(50, listOfValues.Count);

        Person p = listOfValues.First(p => p.Id == 4);
            
        Assert.Contains(p, listOfValues);
        listOfValues.RemoveSingle(x => x.Id == 4);
        Assert.Equal(49, listOfValues.Count);
        Assert.DoesNotContain(p, listOfValues);

        Person s = listOfValues.First(s => s.Id == 2);
        listOfValues.AddItem(s);
        listOfValues.AddItemIf(s, x => listOfValues.All(z => z.Id != s.Id));
        Assert.Equal(2, listOfValues.Count(x=>x.Id == 2));
        Assert.Throws<InvalidOperationException>(() => listOfValues.RemoveSingle(x => x.Id == 2));
        Assert.Equal(50, listOfValues.Count);

        listOfValues.RemoveWhere(x => x.Id == 2);
        Assert.Equal(48, listOfValues.Count);
            
        listOfValues.AddItemIf(s, x => listOfValues.All(z => z.Id != s.Id));
        Assert.Equal(49, listOfValues.Count);
    }
}