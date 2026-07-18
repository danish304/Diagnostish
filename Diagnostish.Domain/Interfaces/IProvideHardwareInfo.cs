using Diagnostish.Domain.Models.Reports;

namespace Diagnostish.Domain.Interfaces
{
    public interface IProvideHardwareInfo
    {
        HardwareReport ProvideInfo(HardwareReport hardwareReport);
    }
}
