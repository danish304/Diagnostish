using Diagnostish.Infrastructure.Shared.Utils;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;

namespace Diagnostish.Tests.Infrastructure.ParserTests;

public class ToSafeDoubleTests
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

    [Theory]
    [MemberData(nameof(ToSafeDouble_TestData))]
    [SuppressMessage("Assertion", "xUnit1045:The type argument object? might not be serializable")]
    public void Tests(object? input, double? expected)
    {
        double? result = Parser.ToSafeDouble(input);
        result.Should().Be(expected);
    }
}