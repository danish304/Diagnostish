namespace Diagnostish.Domain.Models.Entities.Hardware;

public sealed record BaseBoard(
    string Model,
    string Manufacturer,
    string Version,
    string Status
);