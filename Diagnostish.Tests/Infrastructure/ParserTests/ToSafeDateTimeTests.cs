using Diagnostish.Infrastructure.Shared.Utils;
using FluentAssertions;

namespace Diagnostish.Tests.Infrastructure.ParserTests;

public class ToSafeDateTimeTests
{
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
    [MemberData(nameof(ToSafeDateTime_TestData))]
    public void Tests(string? input, DateTime? expected)
    {
        DateTime? result = Parser.ToSafeDateTime(input);
        result.Should().Be(expected);
    }
}