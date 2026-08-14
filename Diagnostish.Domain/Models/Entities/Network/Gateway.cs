namespace Diagnostish.Domain.Models.Entities.Network;

public sealed record Gateway(
    string Address,
    string Interface
);