using System;
using IronSphere.Extensions;

using Xunit;

namespace wasp.Test.Extensions.DateTimeExtensions
{
    public class DateTimeGetAge
    {
        [Fact]
        public void GetAge()
        {
            DateTime birthday = new (1991, 6, 10);
            int age = DateTime.Today.Year - birthday.Year;
            Assert.Equal(age, birthday.GetAge());
        }
        
        [Fact]
        public void GetAgeAddDays()
        {
            DateTime birthday = DateTime.Today.AddDays(1);
            int age = (DateTime.Today.Year - birthday.Year) - 1;
            Assert.Equal(age, birthday.GetAge());
        }
    }
}