namespace Domain.Models.Entities.Network;

public sealed record Gateway(
    string Address,
    string Interface
);