using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Core.Enums;

namespace Zlatmet2.Models.References
{
    public class OrganizationWrapper : BaseReferenceWrapper<Organization>
    {
        private string _name;
        private string _fullName;
        private string _address;
        private string _phone;
        private string _inn;
        private string _bik;
        private string _bank;
        private string _contract;

        private readonly ObservableCollection<DivisionWrapper> _divisions = new ObservableCollection<DivisionWrapper>();

        public OrganizationWrapper(OrganizationType organizationType, string name)
        {
            Id = Guid.NewGuid();
            OrganizationType = organizationType;
            Name = name;

            Subscribe();
        }

        public OrganizationWrapper(Organization organization)
            : base(organization)
        {
            if (organization == null)
                throw new ArgumentNullException("organization");

            Id = organization.Id;
            OrganizationType = organization.Type;
            _name = organization.Name;
            _fullName = organization.FullName;
            _address = organization.Address;
            _phone = organization.Phone;
            _inn = organization.Inn;
            _bik = organization.Bik;
            _bank = organization.Bank;
            _contract = organization.Contract;

            Subscribe();

            if (organization.Divisions.Any())
                foreach (Division division in organization.Divisions)
                    Divisions.Add(new DivisionWrapper(division));
        }

        private void Subscribe()
        {
            Divisions.CollectionChanged += DivisionsOnCollectionChanged;
        }

        private void DivisionsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (DivisionWrapper newItem in e.NewItems)
                {
                    newItem.PropertyChanged += DivisionOnPropertyChanged;
                }

            if (e.OldItems != null)
                foreach (DivisionWrapper oldItem in e.OldItems)
                {
                    oldItem.PropertyChanged -= DivisionOnPropertyChanged;
                }
        }

        private void DivisionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsChanged")
                IsChanged = true;
        }

        public OrganizationType OrganizationType { get; private set; }

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

        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (value == _fullName)
                    return;
                _fullName = value;
                RaisePropertyChanged("FullName");
            }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                if (value == _address)
                    return;
                _address = value;
                RaisePropertyChanged("Address");
            }
        }

        public string Phone
        {
            get { return _phone; }
            set
            {
                if (value == _phone)
                    return;
                _phone = value;
                RaisePropertyChanged("Phone");
            }
        }

        public string Inn
        {
            get { return _inn; }
            set
            {
                if (value == _inn)
                    return;
                _inn = value;
                RaisePropertyChanged("Inn");
            }
        }

        public string Bik
        {
            get { return _bik; }
            set
            {
                if (value == _bik)
                    return;
                _bik = value;
                RaisePropertyChanged("Bik");
            }
        }

        public string Bank
        {
            get { return _bank; }
            set
            {
                if (value == _bank)
                    return;
                _bank = value;
                RaisePropertyChanged("Bank");
            }
        }

        public string Contract
        {
            get { return _contract; }
            set
            {
                if (value == _contract)
                    return;
                _contract = value;
                RaisePropertyChanged("Contract");
            }
        }

        /// <summary>
        /// Подразделения
        /// </summary>
        public ObservableCollection<DivisionWrapper> Divisions
        {
            get { return _divisions; }
        }

        public override void UpdateContainer()
        {
            if (Container == null)
                Container = new Organization(Id, OrganizationType);

            Container.Name = Name;
            Container.FullName = FullName;
            Container.Address = Address;
            Container.Phone = Phone;
            Container.Inn = Inn;
            Container.Bik = Bik;
            Container.Bank = Bank;
            Container.Contract = Contract;

            // Подразделения
            if (Container.Divisions.Any())
                Container.Divisions.Clear();
            foreach (DivisionWrapper divisionWrapper in Divisions)
            {
                divisionWrapper.Save();
                Container.Divisions.Add(divisionWrapper.Container);
            }

        }

    }
}
