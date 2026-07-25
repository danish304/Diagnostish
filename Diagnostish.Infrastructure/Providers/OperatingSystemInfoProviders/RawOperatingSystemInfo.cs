namespace Diagnostish.Infrastructure.Providers.OperatingSystemInfoProviders;

public record RawOperatingSystemInfo(string? Caption, string? Manufacturer, string? Version, DateTime? InstallDate, string? User, DateTime? LastBoot);