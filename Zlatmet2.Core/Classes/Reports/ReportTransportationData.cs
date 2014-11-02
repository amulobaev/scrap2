using System;
using Zlatmet2.Core.Enums;

namespace Zlatmet2.Core.Classes.Reports
{
    public class ReportTransportationData
    {
        public int Number { get; set; }

        public int TransportTypeData { get; set; }

        public string TransportType
        {
            get { return TransportTypeData == 3 ? "Авто" : "Ж/Д"; }
        }

        public DateTime DateData { get; set; }

        public string Date
        {
            get { return DateData.ToShortDateString(); }
        }

        public string Nomenclature { get; set; }

        public string SupplierData { get; set; }

        public string SupplierDivisionData { get; set; }

        public string Supplier
        {
            get
            {
                return string.Format("{0}{1}", SupplierData,
                    !string.IsNullOrEmpty(SupplierDivisionData) ? " - " + SupplierDivisionData : string.Empty);
            }
        }

        public string CustomerData { get; set; }

        public string CustomerDivisionData { get; set; }

        public string Customer
        {
            get
            {
                return string.Format("{0}{1}", CustomerData,
                    !string.IsNullOrEmpty(CustomerDivisionData) ? " - " + CustomerDivisionData : string.Empty);
            }
        }

        public string TransportName { get; set; }

        public string TransportNumber { get; set; }

        public string Wagon { get; set; }

        public string Transport
        {
            get
            {
                switch (TransportTypeData)
                {
                    case (int)DocumentType.TransportationAuto:
                        return string.Format("{0} {1}", TransportName, TransportNumber);
                    case (int)DocumentType.TransportationTrain:
                        return Wagon;
                    default:
                        return null;
                }
            }
        }

        public double LoadingWeight { get; set; }
        public double UnloadingWeight { get; set; }
        public double Netto { get; set; }
        public string Comment { get; set; }
    }
}
