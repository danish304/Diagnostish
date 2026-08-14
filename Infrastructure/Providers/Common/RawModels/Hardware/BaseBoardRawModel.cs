namespace Infrastructure.Providers.Common.RawModels.Hardware;

public sealed record BaseBoardRawModel(
    string? Model,
    string? Manufacturer,
    string? Version,
    string? Status
);