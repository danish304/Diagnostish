using Diagnostish.Domain.Models.Entities.Hardware;

namespace Diagnostish.Domain.Models.Reports;

public class HardwareReport : IssuesReport
{
    public string CpuName { get; set; } = "Неизвестно";                        
    public int CpuCores { get; set; }                                       
    public int CpuClockSpeed { get; set; }                                 

    public string RamType { get; set; } = "Неизвестно";                     
    public double RamCapacity { get; set; }                               
    public int RamSpeed { get; set; }                                   

    public List<GpuInfo> VideoCards { get; set; } = [];             
    public List<StorageDriveInfo> StorageDrives { get; set; } = [];              

    public string BaseBoardModel { get; set; } = "Неизвестно";                 
    public string BaseBoardManufacturer { get; set; } = "Неизвестно";          
    public string BaseBoardVersion { get; set; } = "Неизвестно";              
    public string BaseBoardStatus { get; set; } = "Неизвестно";                

    public string BiosVersion { get; set; } = "Неизвестно";                   
    public DateTime BiosReleaseDate { get; set; }                                 
    public string BiosManufacturer { get; set; } = "Неизвестно";              
}