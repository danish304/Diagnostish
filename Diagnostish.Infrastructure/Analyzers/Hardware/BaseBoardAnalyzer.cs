using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Infrastructure.Analyzers.Common;
using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Serilog;

using static Diagnostish.Infrastructure.Analyzers.Hardware.Messages.BaseBoardAnalyzerMessages;

namespace Diagnostish.Infrastructure.Analyzers.Hardware;

public class BaseBoardAnalyzer(ILogger logger)
    : IAnalyzer<RawBaseBoardModel, BaseBoard>
{
    public ProvideResult<BaseBoard> Analyze(
        ProvideResult<IReadOnlyList<RawBaseBoardModel>> result)
    {
        var warnings = new List<string>(result.Warnings);

        if (result.Data is not [var rawData, ..])
        {
            return ProvideResult<BaseBoard>.Fail(
                warnings,
                result.CriticalErrors);
        }

        string model = rawData.Model
            .GetValueOrWarning(warnings, logger, UNKNOWN_MODEL);

        string manufacturer = rawData.Manufacturer
            .GetValueOrWarning(warnings, logger, UNKNOWN_MANUFACTURER);

        string version = rawData.Version
            .GetValueOrWarning(warnings, logger, UNKNOWN_VERSION);

        string status = rawData.Status
            .GetValueOrWarning(warnings, logger, UNKNOWN_STATUS);

        if (rawData.Status is not null && status != "OK")
        {
            warnings.Add($"{BAD_STATUS} Статус: {status}.");
            logger.Warning(BAD_STATUS + " Статус: {Status}.", status);
        }

        return ProvideResult<BaseBoard>.Ok(
            new BaseBoard(model, manufacturer, version, status),
            warnings);
    }
}