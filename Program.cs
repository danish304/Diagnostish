using Diagnostish.Controllers;
using Diagnostish.Services;
using Diagnostish.Views;
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
        services.AddSingleton<IPrintHW, PrintToConsole>();
        services.AddSingleton<IPrintOS, PrintToConsole>();
        services.AddSingleton<IUserInterface, PrintToConsole>();

        // 3. Регистрируем Controllers
        services.AddTransient<DiagnosticController>();

        // 4. Строим DI-контейнер
        var serviceProvider = services.BuildServiceProvider();

        // 5. Запрашиваем контроллеры
        var controller = serviceProvider.GetRequiredService<DiagnosticController>();
        controller.StartDiagnostic();
    }
}