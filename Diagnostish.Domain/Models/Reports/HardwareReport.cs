using Diagnostish.Domain.Models.Entities.Hardware;

namespace Diagnostish.Domain.Models.Reports;

public class HardwareReport : IssuesReport
{
    public string CpuName { get; set; } = "Unknown";                        
    public int CpuCores { get; set; }                                       
    public int CpuClockSpeed { get; set; }                                 

    public string RamType { get; set; } = "Unknown";                     
    public double RamCapacity { get; set; }                               
    public int RamSpeed { get; set; }                                   

    public List<GpuInfo> VideoCards { get; set; } = [];             
    public List<StorageDriveInfo> StorageDrives { get; set; } = [];              

    public string BaseBoardModel { get; set; } = "Unknown";                 
    public string BaseBoardManufacturer { get; set; } = "Unknown";          
    public string BaseBoardVersion { get; set; } = "Unknown";              
    public string BaseBoardStatus { get; set; } = "Unknown";                

    public string BiosVersion { get; set; } = "Unknown";                   
    public DateTime BiosReleaseDate { get; set; }                                 
    public string BiosManufacturer { get; set; } = "Unknown";              
}