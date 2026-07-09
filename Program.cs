using Diagnostish.Controllers;
using Diagnostish.Services.Implementations;
using Diagnostish.Services.Interfaces;
using Diagnostish.Views.Implementations;
using Diagnostish.Views.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

static class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()  
            .WriteTo.File("logs/diagnostish-.txt", 
                          rollingInterval: RollingInterval.Day,
                          outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}------------------------------------{NewLine}") 
            .CreateLogger();

        Log.Information("Приложение Diagnostish запущено.");

        var services = new ServiceCollection(); 

        services.AddTransient<IHWCheck, CheckPCConfigurationWMI>();
        services.AddTransient<IOSCheck, CheckOSConfigurationWMI>();

        services.AddSingleton<PrintToConsole>();
        services.AddSingleton<IPrintHW>(sp => sp.GetRequiredService<PrintToConsole>());
        services.AddSingleton<IPrintOS>(sp => sp.GetRequiredService<PrintToConsole>());
        services.AddSingleton<IUserInterface>(sp => sp.GetRequiredService<PrintToConsole>());

        services.AddTransient<DiagnosticController>();

        var serviceProvider = services.BuildServiceProvider();

        var controller = serviceProvider.GetRequiredService<DiagnosticController>();
        controller.StartDiagnostic();

        Log.CloseAndFlush();
    }
}