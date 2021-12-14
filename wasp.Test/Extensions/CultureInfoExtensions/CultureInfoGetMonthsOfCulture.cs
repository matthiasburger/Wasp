using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.CultureInfoExtensions
{
    public class CultureInfoGetMonthsOfCulture
    {
        [Fact]
        public void GetMonthsOfCulture()
        {
            IEnumerable<(int, string)> months = new CultureInfo("de-DE")
                .GetMonthsOfCulture().ToArray();
            
            Assert.Equal(12, months.Count());
            Assert.True(months.All(x=>x.Item2.Length > 0));
        }
        
        [Fact]
        public void GetMonthsOfCultureNull()
        {
            CultureInfo? cultureInfo = null;
            Assert.Throws<ArgumentNullException>(() => cultureInfo.GetMonthsOfCulture());
        }
    }
}