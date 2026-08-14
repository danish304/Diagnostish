using Application.Mappers.Common;
using Domain.Models.Entities.Hardware;
using Domain.Models.Reports.Components;

namespace Application.Mappers.Hardware;

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