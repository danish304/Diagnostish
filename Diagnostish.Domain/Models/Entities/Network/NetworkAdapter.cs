namespace Diagnostish.Domain.Models.Entities.Network;

public sealed record NetworkAdapter(
    string Description,
    string MacAddress,
    string DhcpEnabled
);