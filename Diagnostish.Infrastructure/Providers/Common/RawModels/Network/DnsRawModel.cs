namespace Diagnostish.Infrastructure.Providers.Common.RawModels.Network;

public sealed record DnsRawModel(
    string? Address,
    string? Interface
);