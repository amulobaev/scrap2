using System;
using System.ComponentModel;

namespace Zlatmet2.Core.Classes
{
    /// <summary>
    /// Элемент табличной части документа "Корректировка остатков"
    /// </summary>
    public class DocRestsTableItem : INotifyPropertyChanged
    {
        public Guid item_id { get; set; } // Идентификатор пункта табличной части

        // Порядковый номер пункта
        private int Number;
        public int number
        {
            get { return Number; }
            set
            {
                if (Number != value)
                {
                    Number = value;
                    OnPropertyChanged("number");
                }
            }
        }

        // Идентификатор номенклатуры погрузки
        private Guid Nompog_id;
        public Guid nompog_id
        {
            get { return Nompog_id; }
            set
            {
                if (Nompog_id != value)
                {
                    Nompog_id = value;
                    OnPropertyChanged("nompog_id");
                }
            }
        }

        // Масса погрузки
        private double Massapog;
        public double massapog
        {
            get { return Massapog; }
            set
            {
                if (Massapog != value)
                {
                    Massapog = value;
                    OnPropertyChanged("massapog");
                }
            }
        }

        public bool changed { get; set; } // Флаг изменения пункта

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}