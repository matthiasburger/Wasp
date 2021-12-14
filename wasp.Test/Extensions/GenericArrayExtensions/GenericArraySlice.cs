using System;
using System.Collections.Generic;
using System.Linq;

using IronSphere.Extensions;

using Xunit;

namespace wasp.Test.Extensions.GenericArrayExtensions
{
    public class GenericArraySlice
    {
        private record Person(int Id, string Name);
        
        [Theory]
        [InlineData(0, 30, 7)]
        [InlineData(40, 130, 12)]
        [InlineData(40, 130, 1200)]
        [InlineData(40, 130, 2)]
        public void Slice(int start, int end, int slice)
        {
            Person[]? persons = null;

            Assert.Empty(persons!.Slice(slice));
            
            persons = start.Range(end).Select(x => new Person(x, $"Name {x}")).ToArray();

            Person[] slicedPersons = persons.Slice(slice);
            Assert.Equal((end-slice).Min(0), slicedPersons.Length);
        }
        
        [Fact]
        public void SliceWrongParams()
        {
            Person[] persons = 0.Range(50).Select(x => new Person(x, $"Name {x}")).ToArray();
            Assert.Empty(persons.Slice(70));
            Assert.Throws<ArgumentException>(() => persons.Slice(-10));
            Assert.Throws<ArgumentException>(() => persons.Slice(10, 2));
        }
        
        
        [Theory]
        [InlineData(0, 30, 7)]
        [InlineData(40, 130, 12)]
        [InlineData(40, 130, 1200)]
        [InlineData(40, 130, 2)]
        [InlineData(40, 130, -4)]
        public void Subdivide(int start, int end, int divideAt)
        {
            Person[]? persons = null;

            Assert.Throws<ArgumentNullException>(() => persons!.Subdivide(divideAt).ToList());
            
            persons = start.Range(end).Select(x => new Person(x, $"Name {x}")).ToArray();
            int amountPersons = persons.Length;

            if (divideAt > 0)
            {
                IEnumerable<Person[]> slicedPersons = persons.Subdivide(divideAt);


                int amountSlices = amountPersons / divideAt;
                if (amountPersons % divideAt > 0)
                    amountSlices++;


                Assert.Equal(amountSlices, slicedPersons.Count());
            }
            else
            {
                Assert.Throws<ArgumentException>(() => persons!.Subdivide(divideAt).ToList());
            }
        }
    }
}