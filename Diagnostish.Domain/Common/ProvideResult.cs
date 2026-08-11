namespace Diagnostish.Domain.Common;

public sealed record ProvideResult<TData>(
    TData? Data, 
    IReadOnlyList<string> Warnings, 
    IReadOnlyList<string> CriticalErrors)
{
    public static ProvideResult<TData> Ok(
        TData data, 
        IReadOnlyList<string>? warnings = null)
    {
        return new(data, warnings ?? [], []);
    }

    public static ProvideResult<TData> Fail(
        IReadOnlyList<string> warnings, 
        IReadOnlyList<string> criticalErrors)
    {
        return new(default, warnings, criticalErrors);
    }
}