namespace Diagnostish.Domain.Common;

public sealed record ProvideResult<TDataOrRawData>(
    TDataOrRawData? RawData,
    IReadOnlyList<string> Warnings,
    IReadOnlyList<string> CriticalErrors)
{
    public static ProvideResult<TDataOrRawData> Ok(
        TDataOrRawData data,
        IReadOnlyList<string>? warnings = null)
    {
        return new(data, warnings ?? [], []);
    }

    public static ProvideResult<TDataOrRawData> Fail(
        IReadOnlyList<string> warnings,
        IReadOnlyList<string> criticalErrors)
    {
        return new(default, warnings, criticalErrors);
    }
}