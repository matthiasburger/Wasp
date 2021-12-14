using System;
using System.Collections.Generic;
using System.Linq;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.EnumerableExtensions
{
    public class EnumerableMaxMinIfAny
    {
        [Fact]
        public void MaxIfAny()
        {
            IList<int>? items = null;
            Assert.Throws<ArgumentNullException>(() => items!.MaxIfAny(x => x));

            items = new List<int>();
            Assert.Null(items.MaxIfAny(x => x));
            
            items = 0.Range(50).ToList();
            Assert.Equal(49, items.MaxIfAny(x => x));
        }
        [Fact]
        public void MinIfAny()
        {
            IList<int>? items = null;
            Assert.Throws<ArgumentNullException>(() => items!.MinIfAny(x => x));

            items = new List<int>();
            Assert.Null(items.MinIfAny(x => x));
            Assert.Equal(int.MaxValue, items.MinIfAny(x => x, int.MaxValue));
            
            items = 0.Range(50).ToList();
            Assert.Equal(0, items.MinIfAny(x => x));
        }

        private record Person(int? Id, string Name);
        
        [Fact]
        public void MinObjectIfAny()
        {
            IList<Person?>? items = null;
            Assert.Throws<ArgumentNullException>(() => items!.MinIfAny(x => x?.Id ?? 0));

            items = new List<Person?>();
            Assert.Null(items.MinIfAny(x => x?.Id ?? 0));
            Assert.Equal(int.MaxValue, items.MinIfAny(x => x?.Id ?? 0, int.MaxValue));
            
            items = 0.Range(50).Select(x => (Person?) new Person(x, "Peter")).ToList();
            items.Add(new Person(null, "test"));
            items.Add(null);
            Assert.Equal(0, items.MinIfAny(x => x!.Id ?? 0));
        }
        [Fact]
        public void MaxObjectIfAny()
        {
            IList<Person>? items = null;
            Assert.Throws<ArgumentNullException>(() => items!.MaxIfAny(x => x.Id ?? 0));

            items = new List<Person>();
            Assert.Null(items.MaxIfAny(x => x.Id ?? 0));
            Assert.Equal(int.MaxValue, items.MaxIfAny(x => x.Id ?? 0, int.MaxValue));
            
            items = 0.Range(50).Select(x => new Person(x, "Peter")).ToList();
            items.Add(new Person(null, "test"));
            items.Add(null);
            Assert.Equal(49, items.MaxIfAny(x => x!.Id ?? 0));
        }
    }
}