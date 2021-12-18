using System;

using IronSphere.Extensions;

using Xunit;

namespace wasp.Test.Extensions.Exceptions;

public class ExceptionThrowIf
{
    [Fact]
    public void ThrowIfArgumentIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _innerMethod(null!));
        Assert.Throws<ArgumentNullException>(() => _innerMethodWithMessage(null!));
        Assert.Throws<ArgumentNullException>(() => _innerMethodWithInnerException(null!));
            
        Assert.Equal(1, _innerMethod("ok"));
        Assert.Equal(1, _innerMethodWithMessage("ok"));
        Assert.Equal(1, _innerMethodWithInnerException("ok"));
    }

    private int _innerMethod(string args)
    {
        args.ThrowIfArgumentIsNull(nameof(args));
        return 1;
    }
        
    private int _innerMethodWithMessage(string args)
    {
        args.ThrowIfArgumentIsNull(nameof(args), "argument may not be null");
        return 1;
    }
        
    private int _innerMethodWithInnerException(string args)
    {
        args.ThrowIfArgumentIsNull(nameof(args), new NullReferenceException("args was null"));
        return 1;
    }
}