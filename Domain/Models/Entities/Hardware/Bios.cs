namespace Domain.Models.Entities.Hardware;

public sealed record Bios(
    string Version,
    DateTime ReleaseDate,
    string Manufacturer
);