using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using IronSphere.Extensions;

using Xunit;

namespace wasp.Test.Extensions.SqlCommandExtensions
{
    public class SqlCommandSetCommand
    {
        [Fact]
        public void SetCommandWithParameters()
        {
            SqlCommand? command = null;
            Assert.Throws<ArgumentNullException>(() => command!.SetCommandWithParameters("select * from account where name = @name", new
            {
                name = "test"
            }));

            command = new SqlCommand();
            Assert.Throws<ArgumentNullException>(() => command.SetCommandWithParameters(null!, new
            {
                name = "test"
            }));
            
            command.SetCommandWithParameters("select * from account where name = @name", new
            {
                name = "test"
            }); 
            Assert.True(command.Parameters.Contains("@name"));
        }
        
        [Fact]
        public void SetCommandWithoutParameters()
        {
            SqlCommand command = new SqlCommand().SetCommandWithParameters("select * from account where name = @name");
            Assert.Equal("select * from account where name = @name", command.CommandText);
            Assert.Equal(0, command.Parameters.Count);
        }
        
        [Fact]
        public void SetCommandWithDictionaryParameters()
        {
            Dictionary<string, object> parameters = new()
            {
                { "name", "test" }
            };
            
            SqlCommand? command = null;
            Assert.Throws<ArgumentNullException>(() => command!.SetCommandWithParameters("select * from account where name = @name", parameters));

            command = new SqlCommand();
            Assert.Throws<ArgumentNullException>(() => command.SetCommandWithParameters(null!, parameters));
            
            command.SetCommandWithParameters("select * from account where name = @name", parameters);
            Assert.True(command.Parameters.Contains("@name"));
        }
        
        [Fact]
        public void SetCommandWithoutDictionaryParameters()
        {
            Dictionary<string, object>? parameters = null;
            
            SqlCommand command = new SqlCommand().SetCommandWithParameters("select * from account where name = @name", parameters);
            Assert.Equal("select * from account where name = @name", command.CommandText);
            Assert.Equal(0, command.Parameters.Count);
        }
    }
}