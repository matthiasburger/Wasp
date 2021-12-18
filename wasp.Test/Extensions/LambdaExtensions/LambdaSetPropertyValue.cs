using System;
using System.Linq.Expressions;

using IronSphere.Extensions;

using Xunit;

namespace wasp.Test.Extensions.LambdaExtensions;

public class LambdaSetPropertyValue
{
    private record Person(int Id, string Name, object DbValue);

    private const string Name = "my const string";
        
    [Fact]
    public void SetPropertyValue()
    {
        Person p = new (2, "Test", 3);
        Expression<Func<Person, int>> expression = x => x.Id;
        p.SetPropertyValue(expression, 3);
        Assert.Equal(3, p.Id);
            
        Assert.Equal("(x) => x.Id", expression.GetReadableExpressionBody());

        p.SetPropertyValue(x => x.Name, Name);
        p.SetPropertyValue(x => x.DbValue, DBNull.Value);
        Assert.Equal(3, p.DbValue);
    }
}