using Diagnostish.Infrastructure.Shared.Utils;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;

namespace Diagnostish.Tests.Infrastructure.ParserTests;

public class ToSafeIntTests
{
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

    [Theory]
    [MemberData(nameof(ToSafeInt_TestData))]
    [SuppressMessage("Assertion", "xUnit1045:The type argument object? might not be serializable")]
    public void Tests(object? input, int? expected)
    {
        int? result = Parser.ToSafeInt(input);
        result.Should().Be(expected);
    }
}