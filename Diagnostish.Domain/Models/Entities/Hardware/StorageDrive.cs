namespace Diagnostish.Domain.Models.Entities.Hardware;

public sealed record StorageDrive(
    string Model,
    double Size,
    string Status
);