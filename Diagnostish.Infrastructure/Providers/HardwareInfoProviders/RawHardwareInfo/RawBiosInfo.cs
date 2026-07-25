namespace Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;

public record RawBiosInfo(string? Version, DateTime? ReleaseDate, string? Manufacturer);