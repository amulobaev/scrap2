using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Zlatmet2.Core.Classes.Documents;
using Zlatmet2.Core.Classes.References;

namespace Zlatmet2.Models.Documents
{
    public sealed class TransportationItemWrapper : BaseDocumentItemWrapper<TransportationItem>
    {
        private Nomenclature _loadingNomenclature;
        private double _loadingWeight;
        private Nomenclature _unloadingNomenclature;
        private double _unloadingWeight;
        private double _netto;
        private double _garbage;
        private decimal _price;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="transportationItem"></param>
        public TransportationItemWrapper(TransportationItem transportationItem = null)
            : base(transportationItem)
        {
            if (transportationItem != null)
            {
                Id = Container.Id;
                LoadingNomenclature =
                    MainStorage.Instance.Nomenclatures.FirstOrDefault(
                        x => x.Id == Container.LoadingNomenclatureId);
                LoadingWeight = Container.LoadingWeight;
                UnloadingNomenclature =
                    MainStorage.Instance.Nomenclatures.FirstOrDefault(
                        x => x.Id == transportationItem.UnloadingNomenclatureId);
                UnloadingWeight = Container.UnloadingWeight;
                Netto = Container.Netto;
                Garbage = Container.Garbage;
                Price = Container.Price;
            }
            else
            {
                Id = Guid.NewGuid();
                LoadingNomenclature = MainStorage.Instance.Nomenclatures.OrderBy(x => x.Name).FirstOrDefault();
                UnloadingNomenclature = MainStorage.Instance.Nomenclatures.OrderBy(x => x.Name).FirstOrDefault();
            }
        }

        /// <summary>
        /// Номенклатура погрузки
        /// </summary>
        [Required]
        public Nomenclature LoadingNomenclature
        {
            get { return _loadingNomenclature; }
            set
            {
                if (Equals(value, _loadingNomenclature))
                    return;
                _loadingNomenclature = value;
                RaisePropertyChanged("LoadingNomenclature");
            }
        }

        public double LoadingWeight
        {
            get { return _loadingWeight; }
            set
            {
                if (value.Equals(_loadingWeight))
                    return;
                _loadingWeight = value;
                RaisePropertyChanged("LoadingWeight");
            }
        }

        /// <summary>
        /// Номенклатура разгрузки
        /// </summary>
        [Required]
        public Nomenclature UnloadingNomenclature
        {
            get { return _unloadingNomenclature; }
            set
            {
                if (Equals(value, _unloadingNomenclature))
                    return;
                _unloadingNomenclature = value;
                RaisePropertyChanged("UnloadingNomenclature");
            }
        }

        public double UnloadingWeight
        {
            get { return _unloadingWeight; }
            set
            {
                if (value.Equals(_unloadingWeight))
                    return;
                _unloadingWeight = value;
                RaisePropertyChanged("UnloadingWeight");
            }
        }

        public double Netto
        {
            get { return _netto; }
            set
            {
                if (value.Equals(_netto))
                    return;
                _netto = value;
                RaisePropertyChanged("Netto");
            }
        }

        public double Garbage
        {
            get { return _garbage; }
            set
            {
                if (value.Equals(_garbage))
                    return;
                _garbage = value;
                RaisePropertyChanged("Garbage");
            }
        }

        /// <summary>
        /// Цена
        /// </summary>
        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value == _price)
                    return;
                _price = value;
                RaisePropertyChanged("Price");
            }
        }

        public override void UpdateContainer()
        {
            if (Container == null)
                Container = new TransportationItem(Id);
            Container.Number = Number;
            Container.LoadingNomenclatureId = LoadingNomenclature.Id;
            Container.LoadingWeight = LoadingWeight;
            Container.UnloadingNomenclatureId = UnloadingNomenclature.Id;
            Container.UnloadingWeight = UnloadingWeight;
            Container.Netto = Netto;
            Container.Garbage = Garbage;
            Container.Price = Price;
        }

    }
}