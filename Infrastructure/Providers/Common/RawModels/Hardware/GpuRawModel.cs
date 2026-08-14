namespace Infrastructure.Providers.Common.RawModels.Hardware;

public sealed record GpuRawModel(
    string? Name,
    double? AdapterRam
);