using System;
using Zlatmet2.Core.Classes.References;

namespace Zlatmet2.Models.References
{
    public class NomenclatureWrapper : BaseReferenceWrapper<Nomenclature>
    {
        private string _name;
        private string _unit;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="nomenclature"></param>
        public NomenclatureWrapper(Nomenclature nomenclature = null)
            : base(nomenclature)
        {
            if (nomenclature == null)
            {
                Id = Guid.NewGuid();
                Name = "Новая номенклатура";
                IsChanged = true;
            }
            else
            {
                Id = nomenclature.Id;
                _name = nomenclature.Name;
                _unit = nomenclature.Unit;
                //IsChanged = false;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                    return;
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string Unit
        {
            get { return _unit; }
            set { Set(() => Unit, ref _unit, value); }
        }

        public override void UpdateContainer()
        {
            if (Container == null)
                Container = new Nomenclature(Id);
            Container.Name = Name;
            Container.Unit = Unit;
        }

    }
}