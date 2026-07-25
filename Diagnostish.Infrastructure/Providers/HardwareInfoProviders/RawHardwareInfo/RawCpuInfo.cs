namespace Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;

public record RawCpuInfo(string? Name, int? Cores, int? ClockSpeed);