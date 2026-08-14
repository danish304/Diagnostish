namespace Infrastructure.Providers.Common.RawModels.Hardware;

public sealed record RamRawModel(
    string? Type,
    double? Capacity,
    int? Speed
);