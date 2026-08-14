namespace Diagnostish.Domain.Models.Entities.Network;

public sealed record Dns(
    string Address,
    string Interface
);