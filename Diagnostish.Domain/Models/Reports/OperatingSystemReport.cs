namespace Diagnostish.Domain.Models.Reports;

public class OperatingSystemReport : IssuesReport
{
    public string OperatingSystemName { get; set; } = "Неизвестно";                           
    public string OperatingSystemManufacturer { get; set; } = "Неизвестно";                  
    public string OperatingSystemVersion { get; set; } = "Неизвестно";                       
    public DateTime OperatingSystemInstallDate { get; set; }                               
    public string OperatingSystemRegisteredUser { get; set; } = "Неизвестно";                 
    public DateTime OperatingSystemLastBootUpTime { get; set; }                           
}