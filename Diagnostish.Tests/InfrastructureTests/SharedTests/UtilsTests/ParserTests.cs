using Diagnostish.Infrastructure.Shared.Utils;

namespace Diagnostish.Tests.InfrastructureTests.SharedTests.UtilsTests;

[SuppressMessage("Assertion", "xUnit1045:The type argument object? might not be serializable")]
public class ParserTests
{
    public static TheoryData<object?, int?> ToSafeInt_TestData() => new()
    {
        { DBNull.Value, null },
        { null, null },
        { "", null },
        { "   ", null },
        { "6.0", null},

        { 6, 6 },
        { "6", 6},
        {"   6   ", 6},
        {6.0, 6}
    };

    public static TheoryData<object?, double?> ToSafeDouble_TestData() => new()
    {
        { DBNull.Value, null },
        { null, null},
        { "", null},
        { "   ", null},

        { 6, 6.0},
        { "6", 6.0},
        { "   6   ", 6.0},
        { "6.0", 6.0},
        { 6.0, 6.0}
    };

    public static TheoryData<object?, string?> ToSafeString_TestData() => new()
    {
        { DBNull.Value, null },
        { null, null },

        { "", null },
        { "   ", null },
        { "AMD Ryzen 5 5500U", "AMD Ryzen 5 5500U" }
    };

    public static TheoryData<object?, DateTime?> ToSafeDateTime_TestData() => new()
    {
        { DBNull.Value, null },
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
    [MemberData(nameof(ToSafeInt_TestData))]
    public void ToSafeIntTests(object? input, int? expected)
    {
        // Act
        int? result = Parser.ToSafeInt(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(ToSafeDouble_TestData))]
    public void ToSafeDoubleTests(object? input, double? expected)
    {
        // Act
        double? result = Parser.ToSafeDouble(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(ToSafeString_TestData))]
    public void ToSafeStringTests(object? input, string? expected)
    {
        // Act
        string? result = Parser.ToSafeString(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(ToSafeDateTime_TestData))]
    public void ToSafeDateTimeTests(object? input, DateTime? expected)
    {
        // Act
        DateTime? result = Parser.ToSafeDateTime(input);

        // Assert
        result.Should().Be(expected);
    }
}