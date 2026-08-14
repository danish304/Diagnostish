namespace Diagnostish.Infrastructure.Providers.Common.RawModels.Network;

public sealed record IpAddressRawModel(
    string? Address,
    string? Subnet,
    string? Interface
);