using Domain.Models.Entities.Hardware;
using Infrastructure.Analyzers.Common;
using Infrastructure.Providers.Common.RawModels.Hardware;
using Serilog;

using static Infrastructure.Analyzers.Hardware.Messages.BaseBoardAnalyzerMessages;

namespace Infrastructure.Analyzers.Hardware;

public class BaseBoardAnalyzer(ILogger logger)
    : IAnalyzer<BaseBoardRawModel, BaseBoard>
{
    public ProvideResult<BaseBoard> Analyze(
        ProvideResult<IReadOnlyList<BaseBoardRawModel>> result)
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