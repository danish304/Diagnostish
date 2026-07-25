namespace Diagnostish.Domain.Common;

public sealed record ProvideResult<TInfo>(TInfo? Data, IReadOnlyList<string> Warnings, IReadOnlyList<string> CriticalErrors)
{
    public static ProvideResult<TInfo> Ok(TInfo data, IReadOnlyList<string>? warnings = null)  
        => new(data, warnings ?? [], []);

    public static ProvideResult<TInfo> Fail(IReadOnlyList<string> warnings, IReadOnlyList<string> criticalErrors)  
        => new(default, warnings, criticalErrors);
}