using wasp.WebApi.Data.Models;
using Xunit;

namespace wasp.Test.WebApi.Data.Models;

public class TestDataItem
{
    [Fact]
    public void TestEquals()
    {
        DataItem firstDataItem = new();
        DataItem secondDataItem = null;

        Assert.False(firstDataItem == secondDataItem);

        secondDataItem = new();
        Assert.True(firstDataItem.Equals(secondDataItem));

        firstDataItem.Id = "Test-DI";
        firstDataItem.DataTableId = "Test-DT";

        Assert.False(firstDataItem.Equals(secondDataItem));

        secondDataItem.Id = "Test-DI";
        secondDataItem.DataTableId = "Test-DT";
        Assert.True(firstDataItem.Equals(secondDataItem));
        Assert.Equal(firstDataItem, secondDataItem);
        Assert.False(firstDataItem == secondDataItem);
    }
}