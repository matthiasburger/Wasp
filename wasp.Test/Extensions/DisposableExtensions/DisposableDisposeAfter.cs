using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DisposableExtensions;

public class DisposableDisposeAfter
{
    private sealed class InheritedDisposableTestClass : DisposableTestClass
    {
        public bool MeDisposed { get; private set; }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            MeDisposed = IsDisposed;
        }
    }
        
    private class DisposableTestClass : IDisposable
    {
        public bool IsDisposed { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    [Fact]
    public void DisposeAfter()
    {
        DisposableTestClass dtc = new ();
        bool isDisposedInner = dtc.DisposeAfter(d => d.IsDisposed);
        Assert.False(isDisposedInner);
        Assert.True(dtc.IsDisposed);
            
        InheritedDisposableTestClass iDtc = new ();
        bool iisDisposedInner = iDtc.DisposeAfter(d => d.IsDisposed);
        Assert.False(iisDisposedInner);
        Assert.True(iDtc.IsDisposed);
        Assert.True(iDtc.MeDisposed);
    }
}