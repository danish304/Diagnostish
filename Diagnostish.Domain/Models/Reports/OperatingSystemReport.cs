namespace Diagnostish.Domain.Models.Reports;

public class OperatingSystemReport : IssuesReport
{
    public string OperatingSystemName { get; set; } = "Unknown";                           
    public string OperatingSystemManufacturer { get; set; } = "Unknown";                  
    public string OperatingSystemVersion { get; set; } = "Unknown";                       
    public DateTime OperatingSystemInstallDate { get; set; }                               
    public string OperatingSystemRegisteredUser { get; set; } = "Unknown";                 
    public DateTime OperatingSystemLastBootUpTime { get; set; }                           
}