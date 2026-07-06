using Diagnostish.Controllers;
using Diagnostish.Services.Interfaces;
using Diagnostish.Services.Implementations;
using Diagnostish.Views.Interfaces;
using Diagnostish.Views.Implementations;
using Microsoft.Extensions.DependencyInjection;

static class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection(); // Коллекция для регистрации

        // 1. Регистрируем Services
        services.AddTransient<IHWCheck, CheckPCConfigurationWMI>();
        services.AddTransient<IOSCheck, CheckOSConfigurationWMI>();

        // 2. Регистрируем Views
        services.AddSingleton<PrintToConsole>();
        services.AddSingleton<IPrintHW>(sp => sp.GetRequiredService<PrintToConsole>());
        services.AddSingleton<IPrintOS>(sp => sp.GetRequiredService<PrintToConsole>());
        services.AddSingleton<IUserInterface>(sp => sp.GetRequiredService<PrintToConsole>());

        // 3. Регистрируем Controllers
        services.AddTransient<DiagnosticController>();

        // 4. Строим DI-контейнер
        var serviceProvider = services.BuildServiceProvider();

        // 5. Запрашиваем контроллеры
        var controller = serviceProvider.GetRequiredService<DiagnosticController>();
        controller.StartDiagnostic();
    }
}