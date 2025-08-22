using System.Globalization;
using StarWarsInfo.Integrations.Swapi;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace StarWarsInfoTest.Integrations.Swapi
{
    public class SwapiFieldParserTests : IDisposable
    {
        private readonly CultureInfo _originalCulture;

        public SwapiFieldParserTests()
        {
            // SwapiFieldParser.RawTextToTitleCase uses CurrentCulture.
            // Pin culture to a predictable one for these tests.
            _originalCulture = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;
        }

        public void Dispose()
        {
            CultureInfo.CurrentCulture = _originalCulture;
            CultureInfo.CurrentUICulture = _originalCulture;
        }

        // -------- RawTextToIntNullable --------

        [Theory]
        [InlineData("unknown")]
        [InlineData("n/a")]
        [InlineData("")]
        [InlineData("   ")]
        public void RawTextToIntNullable_NullMarkers_ReturnsNull(string input)
        {
            var result = SwapiFieldParser.RawTextToIntNullable(input);
            Assert.Null(result);
        }

        [Theory]
        [InlineData("1,234", 1234)]
        [InlineData("  007  ", 7)]
        [InlineData("speed: 9000", 9000)]
        public void RawTextToIntNullable_RemovesNonNumerics_Parses(string input, int expected)
        {
            var result = SwapiFieldParser.RawTextToIntNullable(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("$5.00")] // becomes "5.00" -> int.Parse throws
        [InlineData("12.3")]
        public void RawTextToIntNullable_DecimalLikeInput_ThrowsFormatException(string input)
        {
            Assert.Throws<FormatException>(() => SwapiFieldParser.RawTextToIntNullable(input));
        }

        // -------- RawTextToDecimalNullable --------

        [Theory]
        [InlineData("unknown")]
        [InlineData("n/a")]
        [InlineData("")]
        [InlineData("     ")]
        public void RawTextToDecimalNullable_NullMarkers_ReturnsNull(string input)
        {
            var result = SwapiFieldParser.RawTextToDecimalNullable(input);
            Assert.Null(result);
        }

        [Theory]
        [InlineData("$1,234.50", 1234.50)]
        [InlineData("mass: 12.0kg", 12.0)]
        [InlineData("  3,000  ", 3000.0)]
        public void RawTextToDecimalNullable_RemovesNonNumerics_Parses(string input, decimal expected)
        {
            var result = SwapiFieldParser.RawTextToDecimalNullable(input);
            Assert.Equal(expected, result);
        }

        // -------- RawTextToDecimal (non-nullable) --------

        [Theory]
        [InlineData("1,234.50", 1234.50)]
        [InlineData("credits: $99,999.99", 99999.99)]
        [InlineData("12kg", 12.0)]
        public void RawTextToDecimal_RemovesNonNumerics_Parses(string input, decimal expected)
        {
            var result = SwapiFieldParser.RawTextToDecimal(input);
            Assert.Equal(expected, result);
        }

        // -------- RawTextToUlongNullable --------

        [Theory]
        [InlineData("unknown")]
        [InlineData("n/a")]
        [InlineData("")]
        [InlineData("   ")]
        public void RawTextToUlongNullable_NullMarkers_ReturnsNull(string input)
        {
            var result = SwapiFieldParser.RawTextToUlongNullable(input);
            Assert.Null(result);
        }

        [Theory]
        [InlineData("42", 42UL)]
        [InlineData("9,007,199,254,740,992", 9007199254740992UL)] // 2^53
        public void RawTextToUlongNullable_RemovesNonNumerics_Parses(string input, ulong expected)
        {
            var result = SwapiFieldParser.RawTextToUlongNullable(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void RawTextToUlongNullable_NegativeNumberString_BecomesPositiveDueToSanitization()
        {
            // "-" is stripped by RemoveNonNumerics, so "-5" becomes "5"
            var result = SwapiFieldParser.RawTextToUlongNullable("-5");
            Assert.Equal<ulong>(5, result!.Value);
        }

        // -------- RawUrlToId --------

        [Theory]
        [InlineData("https://swapi.info/api/starships/3", 3)]
        [InlineData("https://swapi.info/api/people/10/", 10)]
        [InlineData("  https://swapi.dev/api/planets/7/   ", 7)]
        public void RawUrlToId_ExtractsTrailingId(string input, int expected)
        {
            var result = SwapiFieldParser.RawUrlToId(input);
            Assert.Equal(expected, result);
        }

        // -------- RawTextToTitleCase --------

        [Theory]
        [InlineData("unknown")]
        [InlineData("n/a")]
        [InlineData("")]
        [InlineData("   ")]
        public void RawTextToTitleCase_NullMarkersOrWhitespace_ReturnsEmpty(string input)
        {
            var result = SwapiFieldParser.RawTextToTitleCase(input);
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData("luke skywalker", "Luke Skywalker")]
        [InlineData("r2-d2", "R2-D2")] // ToTitleCase leaves digits/hyphens; initial letter capitalized
        [InlineData("obi-wan kenobi", "Obi-Wan Kenobi")]
        public void RawTextToTitleCase_NormalizesToTitleCase(string input, string expected)
        {
            var result = SwapiFieldParser.RawTextToTitleCase(input);
            Assert.Equal(expected, result);
        }
    }
}
