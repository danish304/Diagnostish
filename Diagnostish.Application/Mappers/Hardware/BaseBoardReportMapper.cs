using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Domain.Models.Reports.Components;

namespace Diagnostish.Application.Mappers.Hardware;

public class BaseBoardReportMapper 
    : IReportMapper<HardwareReport, BaseBoard>
{
    public void MapInto(
        HardwareReport report, 
        ProvideResult<BaseBoard> result)
    {
        if (report.TryExtractData(result, out var data))
        {
            report.BaseBoardModel = data.Model;
            report.BaseBoardManufacturer = data.Manufacturer;
            report.BaseBoardVersion = data.Version;
            report.BaseBoardStatus = data.Status;
        }
    }
}