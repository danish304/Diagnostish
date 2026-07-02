using System;
using System.Collections.Generic;

namespace Diagnostish.Models
{
    public class HWReport
    {
        public string ProcessorName { get; set; } = "Unknown";                  // Название процессора
        public int CoresCount { get; set; }                                     // Количество ядер процессора
        public double GBRAM { get; set; }                                       // Общее количество ОЗУ в гигабайтах
        public List<string> VideoCards { get; set; } = new List<string>();      // Видеокарты
        public DateTime CheckTime { get; set; } = DateTime.Now;                 // Время проведения сканирования
    }
}
