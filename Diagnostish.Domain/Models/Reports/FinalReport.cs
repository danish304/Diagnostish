namespace Diagnostish.Domain.Models.Reports
{
    public class FinalReport
    {
        public HardwareReport HardwareReport { get; init; } = new();
        public OperatingSystemReport OperatingSystemReport { get; init; } = new();
    }
}
