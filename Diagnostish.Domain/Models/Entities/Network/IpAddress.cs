namespace Diagnostish.Domain.Models.Entities.Network;

public sealed record IpAddress(
    string Address,
    string Subnet,
    string Interface
);