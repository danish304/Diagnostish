namespace Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;

public sealed record RawCpuModel(
    string? Name,
    int? Cores,
    int? ClockSpeed
);