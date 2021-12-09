using System;
using System.Collections.Generic;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.StringExtensions
{
    public class StringFormat
    {
        [Fact]
        public void FormatDictionary()
        {
            const string helloWorld = "Hello {name}, {date:dd.MM.yyyy HH:mm:ss}";
            const string nullValue = null!;

            IDictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "name", 3 },
                { "date", DateTime.Now }
            };
            IDictionary<string, object> anonymousNullDictionary = null!;
            Assert.Equal(
                $"Hello {dictionary["name"]}, {dictionary["date"]}",
                helloWorld.Format(dictionary)
            );

            Assert.Throws<ArgumentNullException>(() => helloWorld.Format(anonymousNullDictionary));
            Assert.Throws<ArgumentNullException>(() => nullValue!.Format(dictionary));
        }
        
        [Fact]
        public void FormatAnonymous()
        {
            const string helloWorld = "Hello {name}, {date:dd.MM.yyyy HH:mm:ss}";
            var anonymousObject = new { name = "Anna", date = DateTime.Now };
            const object anonymousNullObject = null!;
            const string nullValue = null!;

            Assert.Equal(
                $"Hello {anonymousObject.name}, {anonymousObject.date}",
                helloWorld.Format(anonymousObject)
            );

            Assert.Throws<ArgumentNullException>(() => helloWorld.Format(anonymousNullObject!));
            Assert.Throws<ArgumentNullException>(() => nullValue!.Format(anonymousObject));
        }
    }
}