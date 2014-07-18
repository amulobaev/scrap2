using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Zlatmet2.Core.Classes.Documents;
using Zlatmet2.Core.Classes.References;

namespace Zlatmet2.Models.Documents
{
    public sealed class RemainsItemWrapper : BaseDocumentItemWrapper<RemainsItem>
    {
        private Nomenclature _nomenclature;
        private double _weight;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="remainsItem"></param>
        public RemainsItemWrapper(RemainsItem remainsItem = null)
            : base(remainsItem)
        {
            if (remainsItem != null)
            {
                Id = Container.Id;
                Nomenclature = MainStorage.Instance.Nomenclatures.FirstOrDefault(x => x.Id == Container.NomenclatureId);
                Weight = Container.Weight;
            }
            else
            {
                Id = Guid.NewGuid();
                Nomenclature = MainStorage.Instance.Nomenclatures.OrderBy(x => x.Name).FirstOrDefault();
            }
        }

        /// <summary>
        /// Номенклатура
        /// </summary>
        [Required(ErrorMessage = @"Не выбрана номенклатура")]
        public Nomenclature Nomenclature
        {
            get { return _nomenclature; }
            set
            {
                if (Equals(value, _nomenclature))
                    return;
                _nomenclature = value;
                RaisePropertyChanged("Nomenclature");
            }
        }

        /// <summary>
        /// Масса, т (+/-)
        /// </summary>
        public double Weight
        {
            get { return _weight; }
            set
            {
                if (value.Equals(_weight))
                    return;
                _weight = value;
                RaisePropertyChanged("Weight");
            }
        }

        public override void UpdateContainer()
        {
            if (Container == null)
                Container = new RemainsItem(Id);
            Container.Number = Number;
            Container.NomenclatureId = Nomenclature.Id;
            Container.Weight = Weight;
        }

    }
}