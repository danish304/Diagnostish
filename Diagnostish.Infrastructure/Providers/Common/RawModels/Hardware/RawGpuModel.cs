namespace Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;

public sealed record RawGpuModel(
    string? Name, 
    double? AdapterRam
);