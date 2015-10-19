using System;

namespace Scrap.Core.Classes.Reports
{
    public class ReportAutoTransportData
    {
        /// <summary>
        /// Наименование транспорта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Масса погрузки
        /// </summary>
        public double LoadingWeight { get; set; }

        /// <summary>
        /// Масса разгрузки
        /// </summary>
        public double UnloadingWeight { get; set; }

        /// <summary>
        /// Масса нетто
        /// </summary>
        public double Netto { get; set; }
    }
}
