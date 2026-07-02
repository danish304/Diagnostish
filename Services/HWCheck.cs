using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using Diagnostish.Models;

namespace Diagnostish.Services
{
    public class HWCheck
    {
        // Распознавание названия процессора и количества его ядер
        public HWReport CheckSystemCFG()
        {
            var rep = new HWReport();

            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
            {
                foreach (var item in searcher.Get())
                {
                    rep.ProcessorName = item["Name"]?.ToString() ?? "Unknown";
                    rep.CoresCount = Convert.ToInt32(item["NumberOfCores"]);
                }
            }

            long BytesRAM = 0;      // Общее количество ОЗУ в байтах
            // Распознавание общего количества ОЗУ
            using (var searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory"))
            {
                foreach (var item in searcher.Get())
                {
                    if (item["Capacity"] != null)
                    {
                        BytesRAM += Convert.ToInt64(item["Capacity"]);
                    }
                }
            }
            double GB = (double)BytesRAM / (1024 * 1024 * 1024);        // Перевод общего количества ОЗУ в ГБ
            rep.GBRAM = Math.Round(GB, 2);

            // Распознавание видеокарт
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController"))
            {
                foreach (var item in searcher.Get())
                {
                    rep.VideoCards.Add(item["Name"]?.ToString() ?? "Unknown");
                }
            }

            return rep;
        }
    }
}
