using Diagnostish.Infrastructure.Shared.Utils;
using FluentAssertions;

namespace Diagnostish.Tests.Infrastructure.ParserTests;

public class ToSafeStringTests
{
    public static TheoryData<string?, string?> ToSafeString_TestData() => new()
    {
        { null, null },

        { "", null },
        { "   ", null },
        { "AMD Ryzen 5 5500U", "AMD Ryzen 5 5500U" }
    };

    [Theory]
    [MemberData(nameof(ToSafeString_TestData))]
    public void Tests(string? input, string? expected)
    {
        string? result = Parser.ToSafeString(input);
        result.Should().Be(expected);
    }
}