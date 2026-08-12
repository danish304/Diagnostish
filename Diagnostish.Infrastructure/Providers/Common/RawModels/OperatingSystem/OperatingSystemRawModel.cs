namespace Diagnostish.Infrastructure.Providers.Common.RawModels.OperatingSystem;

public sealed record OperatingSystemRawModel(
    string? Caption,
    string? Manufacturer,
    string? Version,
    DateTime? InstallDate,
    string? User,
    DateTime? LastBoot
);