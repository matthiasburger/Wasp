using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DateTimeExtensions
{
    public class DateTimeSetTime
    {
        [Fact]
        public void SetTimeTimeSpan()
        {
            DateTime dateTime = DateTime.Today;
            Assert.Equal(0, dateTime.Hour);
            Assert.Equal(0, dateTime.Minute);
            Assert.Equal(0, dateTime.Second);

            dateTime.SetTime(new TimeSpan(12, 34, 56));
            Assert.Equal(0, dateTime.Hour);
            Assert.Equal(0, dateTime.Minute);
            Assert.Equal(0, dateTime.Second);
            
            dateTime = dateTime.SetTime(new TimeSpan(12, 34, 56));
            Assert.Equal(12, dateTime.Hour);
            Assert.Equal(34, dateTime.Minute);
            Assert.Equal(56, dateTime.Second);
        }
        
        [Fact]
        public void SetTimeValueTuple()
        {
            DateTime dateTime = DateTime.Today;
            Assert.Equal(0, dateTime.Hour);
            Assert.Equal(0, dateTime.Minute);
            Assert.Equal(0, dateTime.Second);

            dateTime.SetTime((12, 34, 56));
            Assert.Equal(0, dateTime.Hour);
            Assert.Equal(0, dateTime.Minute);
            Assert.Equal(0, dateTime.Second);
            
            dateTime = dateTime.SetTime((12, 34, 56));
            Assert.Equal(12, dateTime.Hour);
            Assert.Equal(34, dateTime.Minute);
            Assert.Equal(56, dateTime.Second);
        }
        
        [Fact]
        public void SetTimeHorsMinutes()
        {
            DateTime dateTime = DateTime.Today;
            Assert.Equal(0, dateTime.Hour);
            Assert.Equal(0, dateTime.Minute);
            Assert.Equal(0, dateTime.Second);

            dateTime.SetTime((12, 34));
            Assert.Equal(0, dateTime.Hour);
            Assert.Equal(0, dateTime.Minute);
            Assert.Equal(0, dateTime.Second);
            
            dateTime = dateTime.SetTime((12, 34));
            Assert.Equal(12, dateTime.Hour);
            Assert.Equal(34, dateTime.Minute);
            Assert.Equal(0, dateTime.Second);
        }
    }
}