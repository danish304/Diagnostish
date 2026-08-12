namespace Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;

public sealed record BiosRawModel(
    string? Version,
    DateTime? ReleaseDate,
    string? Manufacturer
);