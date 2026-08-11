namespace Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;

public sealed record RawBiosModel(
    string? Version,
    DateTime? ReleaseDate,
    string? Manufacturer
);