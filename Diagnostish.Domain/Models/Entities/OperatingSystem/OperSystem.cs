namespace Diagnostish.Domain.Models.Entities.OperatingSystem;

public sealed record OperSystem(
    string Caption,
    string Manufacturer,
    string Version,
    DateTime InstallDate,
    string User,
    DateTime LastBoot
);