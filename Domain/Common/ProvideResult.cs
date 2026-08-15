namespace Domain.Common;

public sealed record ProvideResult<T>(
    T? Data,
    IReadOnlyList<string> Warnings,
    IReadOnlyList<string> CriticalErrors)
{
    public static ProvideResult<T> Ok(
        T data,
        IReadOnlyList<string>? warnings = null)
    {
        return new(data, warnings ?? [], []);
    }

    public static ProvideResult<T> Fail(
        IReadOnlyList<string> warnings,
        IReadOnlyList<string> criticalErrors)
    {
        return new(default, warnings, criticalErrors);
    }
}