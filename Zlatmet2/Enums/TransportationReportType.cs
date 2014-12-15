using System.ComponentModel;

namespace Zlatmet2.Enums
{
    public enum TransportationReportType
    {
        [Description("Контрагенты")]
        Contractors = 0,

        [Description("Поставщики и заказчики")]
        SuppliersAndCustomers
    }
}