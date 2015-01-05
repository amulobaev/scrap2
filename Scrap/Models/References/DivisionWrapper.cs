using System;
using Scrap.Core.Classes.References;

namespace Scrap.Models.References
{
    /// <summary>
    /// Обёртка для подразделения контрагента
    /// </summary>
    public class DivisionWrapper : BaseReferenceWrapper<Division>
    {
        private int _number;

        private string _name;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="number"></param>
        /// <param name="name"></param>
        public DivisionWrapper(Guid organizationId, int number, string name)
        {
            Id = Guid.NewGuid();
            OrganizationId = organizationId;
            Number = number;
            Name = name;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="division"></param>
        public DivisionWrapper(Division division)
            : base(division)
        {
            Id = division.Id;
            OrganizationId = division.OrganizationId;
            _number = division.Number;
            _name = division.Name;
        }

        public Guid OrganizationId { get; protected set; }

        public int Number
        {
            get { return _number; }
            set
            {
                if (value == _number) return;
                _number = value;
                RaisePropertyChanged("Number");
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

        public override void UpdateContainer()
        {
            if (Container == null)
                Container = new Division(Id) { OrganizationId = OrganizationId };
            Container.Number = Number;
            Container.Name = Name;
        }

        public override void Save()
        {
            if (!IsChanged)
                return;

            UpdateContainer();

            IsChanged = false;
        }

    }
}
