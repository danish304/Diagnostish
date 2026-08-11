using Serilog;

namespace Diagnostish.Desktop.Composition;

public static class LoggerConfigurator
{
    public static ILogger Create()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(
                "logs/diagnostish-.txt", 
                rollingInterval: RollingInterval.Day, 
                outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}{NewLine}")
        .CreateLogger();
    }
}