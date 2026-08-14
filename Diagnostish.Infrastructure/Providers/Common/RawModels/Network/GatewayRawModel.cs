namespace Diagnostish.Infrastructure.Providers.Common.RawModels.Network;

public sealed record GatewayRawModel(
    string? Address,
    string? Interface
);