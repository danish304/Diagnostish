namespace Diagnostish.Domain.Models.Entities.Hardware;

public sealed record Ram(
    string Type,
    double Capacity,
    int Speed
);