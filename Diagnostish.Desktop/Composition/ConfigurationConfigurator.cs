using Microsoft.Extensions.Configuration;
using Serilog;

namespace Diagnostish.Desktop.Composition;

public static class ConfigurationConfigurator
{
    private const string ConfigFileName = "appsettings.json";

    private const string DefaultConfigContent = """
        {
          "Wmi": {
            "WmiQueryTimeoutSeconds": 5
          }
        }
        """;

    public static IConfiguration Create()
    {
        string configPath = Path.Combine(AppContext.BaseDirectory, ConfigFileName);
        EnsureConfigExists(configPath);

        try
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(ConfigFileName, optional: true, reloadOnChange: false)
                .Build();
        }
        catch (Exception ex) 
        {
            Log.Warning(ex, "Файл конфигурации повреждён, используются значения по умолчанию.");
            return new ConfigurationBuilder().Build();
        }
    }

    private static void EnsureConfigExists(string configPath)
    {
        if (File.Exists(configPath))
        {
            return;
        }

        try
        {
            File.WriteAllText(configPath, DefaultConfigContent);
            Log.Information("Файл конфигурации не найден, создан со значениями по умолчанию: {Path}", configPath);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Не удалось создать файл конфигурации, будут использованы встроенные значения по умолчанию.");
        }
    }
}