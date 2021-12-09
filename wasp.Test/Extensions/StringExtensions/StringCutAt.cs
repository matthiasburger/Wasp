using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.StringExtensions
{
    public class StringCutAt
    {
        [Fact]
        public void CutAt()
        {
            const string data = "The quick brown fox jumps over the lazy dog";

            string cutAtPosition = data.CutAt(7, null);
            Assert.Equal(data[..7], cutAtPosition);


            string cutAtPositionUntil = data.CutAt(7, "...");
            Assert.Equal(data[..7] + "...", cutAtPositionUntil);

            string cutAtPositionUntilWait = data.CutAt(7, "...", true);
            Assert.Equal("The quick...", cutAtPositionUntilWait);

            string cutAtPositionUntilWaitTooLess = data.CutAt(data.Length - 2, ".....", true);
            Assert.Equal(data, cutAtPositionUntilWaitTooLess);

            string cutAtPositionWait = data.CutAt(7, null, true);
            Assert.Equal("The quick", cutAtPositionWait);

            string cutWhiteSpaceAtPositionWait = string.Empty.CutAt(7, null, true);
            Assert.Equal(string.Empty, cutWhiteSpaceAtPositionWait);

            const string? nullData = null;
            Assert.Throws<ArgumentNullException>(() => nullData!.CutAt(7, null, true));

            Assert.Throws<ArgumentOutOfRangeException>(() => data.CutAt(-1, null, true));
        }
    }
}