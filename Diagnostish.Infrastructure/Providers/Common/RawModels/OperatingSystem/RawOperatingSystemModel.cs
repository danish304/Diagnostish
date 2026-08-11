namespace Diagnostish.Infrastructure.Providers.Common.RawModels.OperatingSystem;

public sealed record RawOperatingSystemModel(
    string? Caption,
    string? Manufacturer,
    string? Version,
    DateTime? InstallDate,
    string? User,
    DateTime? LastBoot
);