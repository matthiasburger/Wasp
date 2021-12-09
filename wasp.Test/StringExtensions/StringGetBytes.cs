using System;
using System.Text;

using IronSphere.Extensions;

using Xunit;

namespace wasp.Test.StringExtensions
{
    public class StringGetBytes
    {
        [Theory]
        [InlineData("utf-8")]
        [InlineData("utf-32")]
        [InlineData("us-ascii")]
        public void GetBytes(string encodingStr)
        {
            Encoding encoding = Encoding.GetEncoding(encodingStr);

            byte[] result = EncodingText.GetBytes(encoding);
            Assert.Equal(encoding.GetBytes(EncodingText), result);
        }
        [Fact]
        public void GetBytesDefaultEncoding()
        {
            byte[] result = EncodingText.GetBytes();
            Assert.Equal(Encoding.UTF8.GetBytes(EncodingText), result);
        }
        
        [Fact]
        public void GetBytesNull()
        {
            const string? stringToGetBytes = null;
            Assert.Throws<ArgumentNullException>(() => stringToGetBytes.GetBytes());
        }



        private const string EncodingText = @"Mathematics and Sciences:

  ∮ E⋅da = Q,  n → ∞, ∑ f(i) = ∏ g(i), ∀x∈ℝ: ⌈x⌉ = −⌊−x⌋, α ∧ ¬β = ¬(¬α ∨ β),

  ℕ ⊆ ℕ₀ ⊂ ℤ ⊂ ℚ ⊂ ℝ ⊂ ℂ, ⊥ < a ≠ b ≡ c ≤ d ≪ ⊤ ⇒ (A ⇔ B),

  2H₂ + O₂ ⇌ 2H₂O, R = 4.7 kΩ, ⌀ 200 mm

Linguistics and dictionaries:

  ði ıntəˈnæʃənəl fəˈnɛtık əsoʊsiˈeıʃn
  Y [ˈʏpsilɔn], Yen [jɛn], Yoga [ˈjoːgɑ]

APL:

  ((V⍳V)=⍳⍴V)/V←,V    ⌷←⍳→⍴∆∇⊃‾⍎⍕⌈

Nicer typography in plain text files:

  ╔══════════════════════════════════════════╗
  ║                                          ║
  ║   • ‘single’ and “double” quotes         ║
  ║                                          ║
  ║   • Curly apostrophes: “We’ve been here” ║
  ║                                          ║
  ║   • Latin-1 apostrophe and accents: '´`  ║
  ║                                          ║
  ║   • ‚deutsche‘ „Anführungszeichen“       ║
  ║                                          ║
  ║   • †, ‡, ‰, •, 3–4, —, −5/+5, ™, …      ║
  ║                                          ║
  ║   • ASCII safety test: 1lI|, 0OD, 8B     ║
  ║                      ╭─────────╮         ║
  ║   • the euro symbol: │ 14.95 € │         ║
  ║                      ╰─────────╯         ║
  ╚══════════════════════════════════════════╝

Greek (in Polytonic):";
    }
}