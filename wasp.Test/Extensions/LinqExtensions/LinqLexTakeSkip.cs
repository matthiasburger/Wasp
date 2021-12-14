using System;
using System.Collections.Generic;
using System.Linq;

using IronSphere.Extensions;

using Xunit;

namespace wasp.Test.Extensions.LinqExtensions
{
    public class LinqLexTakeSkip
    {
        [Fact]
        public void Take()
        {
            IEnumerable<int> range = 1.Range(30);

            int first = range.First();
            
            IEnumerable<int>? copyRange = null;
            Assert.Throws<ArgumentNullException>(() => copyRange!.LexTake(5).ToList());

            copyRange = 1.Range(30).LexTake(5).ToList();
            Assert.Equal(5, copyRange.Count());
            Assert.Equal(first, copyRange.First());

            copyRange = 1.Range(30).LexTake(40).ToList();
            Assert.Equal(30, copyRange.Count());
            Assert.Equal(first, copyRange.First());

            copyRange = 1.Range(30).LexTake(null).ToList();
            Assert.Equal(30, copyRange.Count());
            Assert.Equal(first, copyRange.First());
        }

        [Fact]
        public void Skip()
        {
            IEnumerable<int> range = 1.Range(30);

            int first = range.Last();
            
            IEnumerable<int>? copyRange = null;
            Assert.Throws<ArgumentNullException>(() => copyRange!.LexSkip(5).ToList());

            copyRange = 1.Range(30).LexSkip(5).ToList();
            Assert.Equal(25, copyRange.Count());
            Assert.Equal(first, copyRange.Last());

            copyRange = 1.Range(30).LexSkip(40).ToList();
            Assert.Empty(copyRange);

            copyRange = 1.Range(30).LexSkip(null).ToList();
            Assert.Empty(copyRange);
        }
        
        [Fact]
        public void TakeLast()
        {
            IEnumerable<int> range = 1.Range(30);

            int first = range.Last();
            
            IEnumerable<int>? copyRange = null;
            Assert.Throws<ArgumentNullException>(() => copyRange!.LexTakeLast(5).ToList());

            copyRange = 1.Range(30).LexTakeLast(5).ToList();
            Assert.Equal(5, copyRange.Count());
            Assert.Equal(first, copyRange.Last());

            copyRange = 1.Range(30).LexTakeLast(30).ToList();
            Assert.Equal(30, copyRange.Count());
            Assert.Equal(first, copyRange.Last());
        }
        
        [Fact]
        public void SkipLast()
        {
            IEnumerable<int> range = 1.Range(30);

            int first = range.First();
            
            IEnumerable<int>? copyRange = null;
            Assert.Throws<ArgumentNullException>(() => copyRange!.LexSkipLast(5).ToList());

            copyRange = 1.Range(30).LexSkipLast(5).ToList();
            Assert.Equal(25, copyRange.Count());
            Assert.Equal(first, copyRange.First());

            copyRange = 1.Range(30).LexSkipLast(40).ToList();
            Assert.Empty(copyRange);
        }
        
        [Fact]
        public void DistinctBy()
        {
            IEnumerable<int>? range = null;
            Func<int, int>? emptySelector = null;

            Assert.Throws<ArgumentNullException>(() => range!.LexDistinctBy(x => x).ToList());
                
            range = 1.Range(30).ToList();
            Assert.Throws<ArgumentNullException>(() => range.LexDistinctBy(emptySelector!).ToList());
            
            Assert.Equal(30, range.LexDistinctBy(x => x).Count());

            range = range.AddItem(2).ToList();
            Assert.Equal(30, range.LexDistinctBy(x => x).Count());
            
            range = range.AddItem(2000).ToList();
            Assert.Equal(31, range.LexDistinctBy(x => x).Count());
        }
    }
}