using System.ComponentModel;

namespace Scrap.Enums
{
    public enum TransportationReportType
    {
        [Description("Контрагенты")]
        Contractors = 0,

        [Description("Поставщики и заказчики")]
        SuppliersAndCustomers
    }
}