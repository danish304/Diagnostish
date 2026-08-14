using Diagnostish.Domain.Models.Reports.Components;

namespace Diagnostish.Domain.Models.Reports;

public sealed class FinalReport
{
    public HardwareReport HardwareReport { get; init; } = new();
    public OperatingSystemReport OperatingSystemReport { get; init; } = new();
    public NetworkReport NetworkReport { get; init; } = new();
}