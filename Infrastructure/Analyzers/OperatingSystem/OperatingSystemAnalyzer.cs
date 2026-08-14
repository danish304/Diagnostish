using Domain.Models.Entities.OperatingSystem;
using Infrastructure.Analyzers.Common;
using Infrastructure.Providers.Common.RawModels.OperatingSystem;
using Serilog;

using static Infrastructure.Analyzers.OperatingSystem.Messages.OperatingSystemAnalyzerMessages;

namespace Infrastructure.Analyzers.OperatingSystem;

public class OperatingSystemAnalyzer(ILogger logger)
    : IAnalyzer<OperatingSystemRawModel, OperSystem>
{
    public ProvideResult<OperSystem> Analyze(
        ProvideResult<IReadOnlyList<OperatingSystemRawModel>> result)
    {
        var warnings = new List<string>(result.Warnings);

        if (result.RawData is not [var rawData, ..])
        {
            return ProvideResult<OperSystem>.Fail(
                warnings,
                result.CriticalErrors);
        }

        string caption = rawData.Caption
            .GetValueOrWarning(warnings, logger, UNKNOWN_CAPTION);

        string manufacturer = rawData.Manufacturer
            .GetValueOrWarning(warnings, logger, UNKNOWN_MANUFACTURER);

        string version = rawData.Version
            .GetValueOrWarning(warnings, logger, UNKNOWN_VERSION);

        string user = rawData.User
            .GetValueOrWarning(warnings, logger, UNKNOWN_USER);

        DateTime installDate = rawData.InstallDate
            .GetValueOrWarning(
                warnings,
                logger,
                UNKNOWN_INSTALL_DATE,
                condition: date => date != DateTime.MinValue && date < DateTime.Now);

        DateTime lastBoot = rawData.LastBoot
            .GetValueOrWarning(
                warnings,
                logger,
                UNKNOWN_LAST_BOOT_DATE,
                condition: date => date != DateTime.MinValue && date < DateTime.Now);

        return ProvideResult<OperSystem>.Ok(
            new OperSystem(caption, manufacturer, version, installDate, user, lastBoot),
            warnings);
    }
}