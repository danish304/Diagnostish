using static Diagnostish.Infrastructure.Shared.Common.Utils.Parser;

namespace Diagnostish.Tests.Infrastructure.Shared.Common.Utils;

public class ParserTests
{
    public static TheoryData<object?, int?> ToSafeInt_TestData() => new()
    {
        // Невалидные данные
        { DBNull.Value, null },
        { null, null },
        { "", null },
        { "   ", null },
        { "6.0", null},

        // Валидные данные
        { 6, 6 },
        { "6", 6},
        {"   6   ", 6},
        {6.0, 6}
    };

    public static TheoryData<object?, double?> ToSafeDouble_TestData() => new()
    {
        // Невалидные данные
        { DBNull.Value, null },
        { null, null},
        { "", null},
        { "   ", null},

        // Валидные данные
        { 6, 6.0},
        { "6", 6.0},
        { "   6   ", 6.0},
        { "6.0", 6.0},
        { 6.0, 6.0}
    };

    public static TheoryData<object?, string?> ToSafeString_TestData() => new()
    {
        // Невалидные данные
        { DBNull.Value, null },
        { null, null },

        // Валидные данные
        { "", null },
        { "   ", null },
        { "AMD Ryzen 5 5500U", "AMD Ryzen 5 5500U" }
    };

    public static TheoryData<object?, DateTime?> ToSafeDateTime_TestData() => new()
    {
        // Невалидные данные
        { DBNull.Value, null },
        { null, null },
        { "", null },
        { "   ", null },
        { "20261345000000.000000+000", null},
        { "invalid-date", null },
        { "20260710", null},
        { "2026-05-20", null },
        { "2026-07-10 13:43:00", null },

        // Валидные данные
        { "20261212133620.000000+000", new DateTime(2026, 12, 12, 13, 36, 20).ToLocalTime() }
    };

    [Theory]
    [MemberData(nameof(ToSafeInt_TestData))]
    public void ToSafeIntTests(object? input, int? expected)
    {
        // Act
        int? result = ToSafeInt(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(ToSafeDouble_TestData))]
    public void ToSafeDoubleTests(object? input, double? expected)
    {
        // Act
        double? result = ToSafeDouble(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(ToSafeString_TestData))]
    public void ToSafeStringTests(object? input, string? expected)
    {
        // Act
        string? result = ToSafeString(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(ToSafeDateTime_TestData))]
    public void ToSafeDateTimeTests(object? input, DateTime? expected)
    {
        // Act
        DateTime? result = ToSafeDateTime(input);

        // Assert
        result.Should().Be(expected);
    }
}