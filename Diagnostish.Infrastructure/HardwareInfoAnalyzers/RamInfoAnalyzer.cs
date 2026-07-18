using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Entities;
using Diagnostish.Domain.Models.Reports;
using Serilog;

namespace Diagnostish.Infrastructure.HardwareInfoAnalyzers
{
    public class RamInfoAnalyzer : IAnalyzeHardwareInfo<RamInfo, HardwareReport>
    {
        private static readonly Dictionary<string, string> RamTypes = new()
        {
            { "20", "DDR" },
            { "21", "DDR2" },
            { "24", "DDR3" },
            { "26", "DDR4" },
            { "34", "DDR5" }
        };

        public void AnalyzeInfo(IEnumerable<RamInfo> ramInfo, HardwareReport hardwareReport)
        {
            double totalRamCapacityInBytes = 0;
            var ramSpeeds = new List<int>();

            foreach (var item in ramInfo)
            {
                if (RamTypes.TryGetValue(item.Type, out var humanReadabletype)) hardwareReport.RamType = humanReadabletype;

                if (item.Capacity > 0) totalRamCapacityInBytes += item.Capacity;
                else hardwareReport.Errors.Add("Не удалось определить корректную емкость одной из планок ОЗУ");
                
                if (item.Speed > 0) ramSpeeds.Add(item.Speed);
                else hardwareReport.Errors.Add("Не удалось определить скорость ОЗУ!");
            }

            if (totalRamCapacityInBytes > 0)
            {
                hardwareReport.RamSize = Math.Round(totalRamCapacityInBytes / (1024 * 1024 * 1024), 2);
            }

            if (ramSpeeds.Count > 0)
            {
                int minSpeed = ramSpeeds.Min();
                hardwareReport.RamSpeed = minSpeed;

                if (ramSpeeds.Any(s => s != minSpeed))
                {
                    hardwareReport.Errors.Add("Установлены модули ОЗУ с разной скоростью. Система ограничена самой медленной планкой.");
                    Log.Warning("Обнаружен конфликт частот RAM: {Speeds}. Выбрана минимальная: {MinSpeed} MHz", string.Join(", ", ramSpeeds), minSpeed);
                }
            }
        }
    }
}
