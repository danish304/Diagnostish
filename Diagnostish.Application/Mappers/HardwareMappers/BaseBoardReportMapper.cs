using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Common;
using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Domain.Models.Reports;

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