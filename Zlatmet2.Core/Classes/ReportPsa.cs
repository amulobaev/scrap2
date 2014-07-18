namespace Zlatmet2.Core.Classes
{
    public class ReportPsa
    {
        public int number { get; set; } // Порядковый номер пункта
        public string nomraz { get; set; }  // Номенклатуры выгрузки
        public double brutto { get; set; }
        public double tara { get; set; }
        public double garbage { get; set; }
        public double netto { get; set; }
        public double cost { get; set; } // Цена (рублей)
        public double summacost { get; set; }
    }
}
