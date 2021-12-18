using System;
using System.Globalization;

using IronSphere.Extensions;

using Xunit;

namespace wasp.Test.Extensions.StringExtensions;

public class StringCasting
{
    [Fact]
    public void CastToBool()
    {
        Assert.True("true".ToBool());
        Assert.True("1".ToBool());
        Assert.True("True".ToBool());
        Assert.True("TRUE".ToBool());
        Assert.False("false".ToBool());
        Assert.False("0".ToBool());
        Assert.False("False".ToBool());
        Assert.False("FALSE".ToBool());
    }

    [Fact]
    public void CastToByte()
    {
        Assert.Null("true".ToByte());
        Assert.Equal((byte)1, "1".ToByte());
        Assert.Null("True".ToByte());
        Assert.Null("TRUE".ToByte());
        Assert.Null("false".ToByte());
        Assert.Equal((byte)0, "0".ToByte());
        Assert.Null("-1".ToByte());
        Assert.Null("False".ToByte());
        Assert.Null("FALSE".ToByte());
    }

    [Fact]
    public void CastToDecimal()
    {
        Assert.Equal((decimal)3.5, "3.5".ToDecimal(NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture));
        Assert.Equal((decimal)3.5, "3.5".ToDecimal());
        Assert.Null("3,5".ToDecimal());
    }

    [Fact]
    public void CastToDouble()
    {
        Assert.Equal(3.5, "3.5".ToDouble(NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture));
        Assert.Equal(3.5, "3.5".ToDouble());
        Assert.Null("3,5".ToDouble());
    }

    [Fact]
    public void CastToFloat()
    {
        Assert.Equal((float)3.5, "3.5".ToFloat(NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture));
        Assert.Equal((float)3.5, "3.5".ToFloat());
        Assert.Null("3,5".ToFloat());
    }

    [Fact]
    public void CastToInt()
    {
        Assert.Null("3.5".ToInt());
        Assert.Equal(3, "3".ToInt());
        Assert.Equal(3, "3".ToInt(NumberStyles.Any, CultureInfo.CurrentCulture));
        Assert.Null("3,5".ToInt());
    }

    [Fact]
    public void CastToShort()
    {
        Assert.Null("3.5".ToShort());
        Assert.Equal((short)3, "3".ToShort());
        Assert.Equal((short)3, "3".ToShort(NumberStyles.Any, CultureInfo.CurrentCulture));
        Assert.Null("3,5".ToShort());
    }

    [Fact]
    public void CastToLong()
    {
        Assert.Null("3.5".ToLong());
        Assert.Equal(3, "3".ToLong());
        Assert.Equal(3, "3".ToLong(NumberStyles.Any, CultureInfo.CurrentCulture));
        Assert.Null("3,5".ToLong());
    }

    [Fact]
    public void CastToChar()
    {
        Assert.Null("true".ToChar());
        Assert.Equal('A', "A".ToChar());
        Assert.Equal('1', "1".ToChar());
    }

    [Fact]
    public void CastToDateTime()
    {
        Assert.Null("true".ToDateTime());
        Assert.Equal(new DateTime(1991, 6, 10), "10.06.1991".ToDateTime());
        Assert.Equal(new DateTime(1991, 6, 10), "1991-06-10".ToDateTime());
        Assert.Equal(new DateTime(1991, 6, 10, 20, 5, 30), "1991-06-10T20:05:30".ToDateTime());
        Assert.Equal(new DateTime(1991, 6, 10, 22, 5, 30), "1991-06-10T20:05:30Z".ToDateTime());
        Assert.Equal(new DateTime(1991, 6, 10, 22, 5, 30), "1991-06-10T20:05:30Z".ToDateTime(DateTimeStyles.None, CultureInfo.CurrentCulture));
    }
}