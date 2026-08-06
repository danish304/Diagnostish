using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Infrastructure.Analyzers.Common;
using Diagnostish.Infrastructure.Analyzers.HardwareInfoAnalyzers.Messages;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;

namespace Diagnostish.Infrastructure.Analyzers.HardwareInfoAnalyzers;

public class BaseBoardInfoAnalyzer(Serilog.ILogger logger) : IAnalyzeDiagnosticInfo<RawBaseBoardInfo, BaseBoardInfo>
{
    public ProvideResult<BaseBoardInfo> AnalyzeInfo(ProvideResult<IReadOnlyList<RawBaseBoardInfo>> providedBaseBoardData)
    {
        var warnings = new List<string>(providedBaseBoardData.Warnings);

        if (providedBaseBoardData.Data is not { Count: > 0 } baseBoardInfo) 
            return ProvideResult<BaseBoardInfo>.Fail(warnings, providedBaseBoardData.CriticalErrors);

        var item = baseBoardInfo[0];

        string model = item.Model.GetValueOrWarning(warnings, logger, BaseBoardAnalyzerMessages.UnknownModel);
        string manufacturer = item.Manufacturer.GetValueOrWarning(warnings, logger, BaseBoardAnalyzerMessages.UnknownManufacturer);
        string version = item.Version.GetValueOrWarning(warnings, logger, BaseBoardAnalyzerMessages.UnknownVersion);
        string status = item.Status.GetValueOrWarning(warnings, logger, BaseBoardAnalyzerMessages.UnknownStatus);

        if (item.Status is not null && status != "OK")
        {
            warnings.Add($"{BaseBoardAnalyzerMessages.BadStatus} Статус: {status}.");
            logger.Warning("{BadStatusMessage} Статус: {Status}.", BaseBoardAnalyzerMessages.BadStatus, status);
        }

        return ProvideResult<BaseBoardInfo>.Ok(new BaseBoardInfo(model, manufacturer, version, status), warnings);
    }
}