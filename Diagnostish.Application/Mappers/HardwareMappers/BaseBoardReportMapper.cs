using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Entities.Hardware;

namespace Diagnostish.Application.Mappers.HardwareMappers;

public class BaseBoardReportMapper : IReportMapper<HardwareReport, BaseBoardInfo>
{
    public void MapInto(HardwareReport hardwareReport, ProvideResult<BaseBoardInfo> analysisBaseBoardData)
    {
        if (hardwareReport.TryExtractData(analysisBaseBoardData, out var data))
        {
            hardwareReport.BaseBoardModel = data.Model;
            hardwareReport.BaseBoardManufacturer = data.Manufacturer;
            hardwareReport.BaseBoardVersion = data.Version;
            hardwareReport.BaseBoardStatus = data.Status;
        }
    }
}