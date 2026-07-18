using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Reports;

namespace Diagnostish.Application.Services
{
    public class ServicesAggregator(IEnumerable<IProvideHardwareInfo> hardwareInfoProviders,
                                    IProvideOperatingSystemInfo operatingSystemInfoProvider)
    {
        public FinalReport GetFinalReport()
        {
            var finalReport = new FinalReport();

            foreach (var provider in hardwareInfoProviders)
            {
                provider.ProvideInfo(finalReport.HardwareReport);
            }

            operatingSystemInfoProvider.ProvideInfo(finalReport.OperatingSystemReport);

            return finalReport;
        }
    }
}
