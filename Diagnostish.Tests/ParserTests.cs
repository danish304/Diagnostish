using Diagnostish.Infrastructure.WmiHelpers;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;

namespace Diagnostish.Tests
{
    public class ParserTests
    {
        public static TheoryData<object?, double?> ToSafeDouble_TestData() => new()
        {
            { null, null},
            { "", null},
            { "   ", null},

            { 6, 6.0},
            { "6", 6.0},
            { "   6   ", 6.0},
            { "6.0", 6.0},
            { 6.0, 6.0}
        };

        public static TheoryData<object?, int?> ToSafeInt_TestData() => new()
        {
            { null, null },
            { "", null },
            { "   ", null },
            { "6.0", null},

            { 6, 6 },
            { "6", 6},
            {"   6   ", 6},
            {6.0, 6}
        };

        public static TheoryData<string?, string> ToSafeString_TestData() => new()
        {
            { null, "Unknown" },

            { "", "Unknown" },
            { "   ", "Unknown" },
            { "AMD Ryzen 5 5500U", "AMD Ryzen 5 5500U" }
        };

        public static TheoryData<string?, DateTime?> ToSafeDateTime_TestData() => new()
        {
            { null, null },
            { "", null },
            { "   ", null },
            { "20261345000000.000000+000", null},
            { "invalid-date", null },
            { "20260710", null},
            { "2026-05-20", null },
            { "2026-07-10 13:43:00", null },

            { "20261212133620.000000+000", new DateTime(2026, 12, 12, 13, 36, 20).ToLocalTime() }
        };

        [Theory]
        [MemberData(nameof(ToSafeDouble_TestData))]
        [SuppressMessage("Assertion", "xUnit1045:The type argument object? might not be serializable")]
        public void ToSafeDoubleTests(object? input, double? expected)
        {
            double? result = Parser.ToSafeDouble(input);
            result.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(ToSafeInt_TestData))]
        [SuppressMessage("Assertion", "xUnit1045:The type argument object? might not be serializable")]
        public void ToSafeIntTests(object? input, int? expected)
        {
            int? result = Parser.ToSafeInt(input);
            result.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(ToSafeString_TestData))]
        public void ToSafeStringTests(string? input, string expected)
        {
            string result = Parser.ToSafeString(input);
            result.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(ToSafeDateTime_TestData))]
        public void ToSafeDateTimeTests(string? input, DateTime? expected)
        {
            DateTime? result = Parser.ToSafeDateTime(input);
            result.Should().Be(expected);
        }
    }
}
