namespace Diagnostish.Domain.Models.Entities;

public record OperatingSystemInfo(string Caption, 
                                  string Manufacturer, 
                                  string Version, 
                                  DateTime InstallDate, 
                                  string User, 
                                  DateTime LastBoot);