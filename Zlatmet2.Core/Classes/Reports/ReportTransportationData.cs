using System;

namespace Zlatmet2.Core.Classes.Reports
{
    public class ReportTransportationData
    {
        public int Number { get; set; }
        public string TransportType { get; set; }
        public DateTime Date { get; set; }
        public string Nomenclature { get; set; }
        public string Supplier { get; set; }
        public string Customer { get; set; }
        public string Transport { get; set; }
        public double LoadingWeight { get; set; }
        public double UnloadingWeight { get; set; }
        public double Netto { get; set; }
        public string Comment { get; set; }
    }
}
