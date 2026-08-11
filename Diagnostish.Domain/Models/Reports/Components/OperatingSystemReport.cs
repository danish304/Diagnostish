using Diagnostish.Domain.Models.Reports.Common;

namespace Diagnostish.Domain.Models.Reports.Components;

public class OperatingSystemReport : BaseIssuesReport
{
    // Информация об операционной системе
    public string OperatingSystemName { get; set; } = "Неизвестно";
    public string OperatingSystemManufacturer { get; set; } = "Неизвестно";
    public string OperatingSystemVersion { get; set; } = "Неизвестно";

    // Данные пользователя
    public string OperatingSystemRegisteredUser { get; set; } = "Неизвестно";

    // Временные метки
    public DateTime OperatingSystemInstallDate { get; set; }
    public DateTime OperatingSystemLastBootUpTime { get; set; }
}