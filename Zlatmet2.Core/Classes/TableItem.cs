using System;

namespace Zlatmet2.Core.Classes
{
    /// <summary>
    /// Элемент табличной части
    /// </summary>
    public class TableItem
    {
        // Порядковый номер пункта
        public int number { get; set; }

        // Идентификатор номенклатуры погрузки
        public Guid nompog_id { get; set; }
        // Идентификатор номенклатуры разгрузки
        public Guid nomraz_id { get; set; }

        public string nompog { get; set; } //номенклатуры погруз
        public string nomraz { get; set; }  //номенклатуры выгруз
        public double massapog { get; set; } //масса погрузки
        public double massaraz { get; set; } //масса разгруз
        public double cost { get; set; } // Цена (рублей)

        public Guid supplier_id { get; set; } // Для отчета
        public Guid customer_id { get; set; } // Для отчета
        public string date_pog { get; set; }
        public string vagon { get; set; }
        public string supplier { get; set; }
        public string customer { get; set; }
        public int doc_type { get; set; }
        public double netto { get; set; }
        public string comment { get; set; }

        public double tara { get; set; }
        public double brutto { get; set; }
        public double summacost { get; set; }
        public int pp { get; set; }
        public string doc_date { get; set; }
        public DateTime date { get; set; }
    }

}
