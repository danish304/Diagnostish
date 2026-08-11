using Diagnostish.Domain.Models.Entities.OperatingSystem;
using Diagnostish.Infrastructure.Analyzers.Common;
using Diagnostish.Infrastructure.Providers.Common.RawModels.OperatingSystem;
using Serilog;

using static Diagnostish.Infrastructure.Analyzers.OperatingSystem.Messages.OperatingSystemAnalyzerMessages;

namespace Diagnostish.Infrastructure.Analyzers.OperatingSystem;

public class OperatingSystemAnalyzer(ILogger logger) 
    : IAnalyzer<RawOperatingSystemModel, OperSystem>
{
    public ProvideResult<OperSystem> Analyze(
        ProvideResult<IReadOnlyList<RawOperatingSystemModel>> result)
    {
        var warnings = new List<string>(result.Warnings);

        if (result.Data is not [var rawData, ..])
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
                UNKNOWN_INSTALLDATE, 
                condition: date => date != DateTime.MinValue && date < DateTime.Now);

        DateTime lastBoot = rawData.LastBoot
            .GetValueOrWarning(
                warnings, 
                logger,
                UNKNOWN_LASTBOOTDATE,
                condition: date => date != DateTime.MinValue && date < DateTime.Now);

        return ProvideResult<OperSystem>.Ok(
            new OperSystem(caption, manufacturer, version, installDate, user, lastBoot),
            warnings);
    }
}