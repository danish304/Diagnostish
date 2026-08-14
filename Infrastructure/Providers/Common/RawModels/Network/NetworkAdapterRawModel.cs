namespace Infrastructure.Providers.Common.RawModels.Network;

public sealed record NetworkAdapterRawModel(
    string? Description,
    string? MacAddress,
    string? DhcpEnabled
);