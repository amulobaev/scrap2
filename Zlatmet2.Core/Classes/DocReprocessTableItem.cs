using System;
using System.ComponentModel;

namespace Zlatmet2.Core.Classes
{
    /// <summary>
    /// Элемент табличной части документа "Переработка"
    /// </summary>
    public class DocReprocessTableItem : INotifyPropertyChanged
    {
        public Guid ItemId { get; set; } // Идентификатор пункта табличной части

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

        // Идентификатор номенклатуры разгрузки
        private Guid Nomraz_id;
        public Guid nomraz_id
        {
            get { return Nomraz_id; }
            set
            {
                if (Nomraz_id != value)
                {
                    Nomraz_id = value;
                    OnPropertyChanged("nomraz_id");
                }
            }
        }

        // Масса разгрузки
        private double Massaraz;
        public double massaraz
        {
            get { return Massaraz; }
            set
            {
                if (Massaraz != value)
                {
                    Massaraz = value;
                    OnPropertyChanged("massaraz");
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