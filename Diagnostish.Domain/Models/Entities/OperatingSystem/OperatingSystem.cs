namespace Diagnostish.Domain.Models.Entities.OperatingSystem;

public sealed record OperatingSystem(
    string Caption, 
    string Manufacturer, 
    string Version, 
    DateTime InstallDate, 
    string User, 
    DateTime LastBoot
);