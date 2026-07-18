using Diagnostish.Domain.Models.Reports;

namespace Diagnostish.Domain.Interfaces
{
    public interface IProvideOperatingSystemInfo
    {
        OperatingSystemReport ProvideInfo(OperatingSystemReport operatingSystemReport);
    }
}
