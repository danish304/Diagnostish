namespace Infrastructure.Providers.Common.RawModels.Hardware;

public sealed record CpuRawModel(
    string? Name,
    int? Cores,
    int? ClockSpeed
);