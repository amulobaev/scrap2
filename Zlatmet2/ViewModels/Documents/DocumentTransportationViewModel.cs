﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes.Documents;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Core.Enums;
using Zlatmet2.Models.Documents;
using Zlatmet2.Tools;
using Zlatmet2.Views.Documents;

namespace Zlatmet2.ViewModels.Documents
{
    public class DocumentTransportationViewModel : BaseDocumentViewModel<Transportation, TransportationItemWrapper>
    {
        #region Поля

        private readonly ObservableCollection<Organization> _suppliers = new ObservableCollection<Organization>();

        private readonly ObservableCollection<Organization> _customers = new ObservableCollection<Organization>();

        private Organization _supplier;
        private Organization _customer;
        private Employee _responsiblePerson;
        private DocumentType _transportType;
        private Transport _transport;
        private Employee _driver;
        private string _wagonNumber;
        private string _psa;
        private string _ttn;
        private Division _supplierDivision;
        private Division _customerDivision;
        private DateTime? _dateOfLoading;
        private DateTime? _dateOfUnloading;
        private string _comment;

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        public DocumentTransportationViewModel(LayoutDocument layout, Guid id)
            : base(layout, typeof(DocumentTransportationView), id)
        {
            Title = "Перевозка";

            Suppliers = new ReadOnlyObservableCollection<Organization>(_suppliers);
            FillSuppliers();

            Customers = new ReadOnlyObservableCollection<Organization>(_customers);
            FillCustomers();

            ((INotifyCollectionChanged)MainStorage.Instance.Suppliers).CollectionChanged +=
                (sender, args) => FillSuppliers();
            ((INotifyCollectionChanged)MainStorage.Instance.Customers).CollectionChanged +=
                (sender, args) => FillCustomers();
            ((INotifyCollectionChanged)MainStorage.Instance.Bases).CollectionChanged += (sender, args) =>
            {
                FillSuppliers();
                FillCustomers();
            };

            if (Id != Guid.Empty)
            {
                // Загрузка документа
                LoadDocument(id);
            }
            else
            {
                // Новый документ
                Id = Guid.NewGuid();
                Number = MainStorage.Instance.DocumentsRepository.GetNextDocumentNumber();
                Date = DateTime.Now;
                DateOfLoading = DateTime.Today;
                DateOfUnloading = DateTime.Today;
                TransportType = DocumentType.TransportationAuto;
            }

            this.PropertyChanged += OnPropertyChanged;
        }

        #region Свойства

        [Required]
        public DateTime? DateOfLoading
        {
            get { return _dateOfLoading; }
            set
            {
                if (value.Equals(_dateOfLoading))
                    return;
                _dateOfLoading = value;
                RaisePropertyChanged("DateOfLoading");
            }
        }

        [Required]
        public DateTime? DateOfUnloading
        {
            get { return _dateOfUnloading; }
            set
            {
                if (value.Equals(_dateOfUnloading))
                    return;
                _dateOfUnloading = value;
                RaisePropertyChanged("DateOfUnloading");
            }
        }

        /// <summary>
        /// Поставщики
        /// </summary>
        public ReadOnlyObservableCollection<Organization> Suppliers { get; private set; }

        /// <summary>
        /// Поставщик
        /// </summary>
        [Required(ErrorMessage = @"Не выбран поставщик")]
        public Organization Supplier
        {
            get { return _supplier; }
            set
            {
                if (Equals(value, _supplier))
                    return;
                _supplier = value;
                RaisePropertyChanged("Supplier");
            }
        }

        /// <summary>
        /// Выбранное подразделение поставщика
        /// </summary>
        [Required(ErrorMessage = @"Не выбрано подразделение поставщика")]
        public Division SupplierDivision
        {
            get
            {
                return Supplier != null && Supplier.Type == OrganizationType.Supplier
                    ? _supplierDivision
                    : Division.Empty;
            }
            set
            {
                if (Equals(value, _supplierDivision))
                    return;
                _supplierDivision = value;
                RaisePropertyChanged("SupplierDivision");
            }
        }

        /// <summary>
        /// Заказчики
        /// </summary>
        public ReadOnlyObservableCollection<Organization> Customers { get; private set; }

        /// <summary>
        /// Заказчик
        /// </summary>
        [Required(ErrorMessage = @"Не выбран заказчик")]
        public Organization Customer
        {
            get { return _customer; }
            set
            {
                if (Equals(value, _customer))
                    return;
                _customer = value;
                RaisePropertyChanged("Customer");
            }
        }

        /// <summary>
        /// Выбранное подразделение заказчика
        /// </summary>
        [Required(ErrorMessage = @"Не выбрано подразделение заказчика")]
        public Division CustomerDivision
        {
            get
            {
                return Customer != null && Customer.Type == OrganizationType.Customer
                    ? _customerDivision
                    : Division.Empty;
            }
            set
            {
                if (Equals(value, _customerDivision))
                    return;
                _customerDivision = value;
                RaisePropertyChanged("CustomerDivision");
            }
        }

        /// <summary>
        /// Ответственные лица
        /// </summary>
        public ReadOnlyObservableCollection<Employee> ResponsiblePersons
        {
            get { return MainStorage.Instance.ResponsiblePersons; }
        }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        [Required(ErrorMessage = @"Не выбрано ответственное лицо")]
        public Employee ResponsiblePerson
        {
            get { return _responsiblePerson; }
            set
            {
                if (Equals(value, _responsiblePerson))
                    return;
                _responsiblePerson = value;
                RaisePropertyChanged("ResponsiblePerson");
            }
        }

        public DocumentType TransportType
        {
            get { return _transportType; }
            set
            {
                if (value == _transportType)
                    return;
                _transportType = value;
                RaisePropertyChanged("TransportType");
            }
        }

        /// <summary>
        /// Транспортные средства
        /// </summary>
        public ReadOnlyObservableCollection<Transport> Transports
        {
            get { return MainStorage.Instance.Transports; }
        }

        [Required(ErrorMessage = @"Не выбран автотранспорт")]
        public Transport Transport
        {
            get { return TransportType == DocumentType.TransportationAuto ? _transport : Transport.Empty; }
            set
            {
                if (Equals(value, _transport))
                    return;
                _transport = value;
                RaisePropertyChanged("Transport");
            }
        }

        public ReadOnlyObservableCollection<Employee> Drivers
        {
            get { return MainStorage.Instance.Drivers; }
        }

        [Required(ErrorMessage = @"Не выбран водитель")]
        public Employee Driver
        {
            get { return TransportType == DocumentType.TransportationAuto ? _driver : Employee.Empty; }
            set
            {
                if (Equals(value, _driver))
                    return;
                _driver = value;
                RaisePropertyChanged("Driver");
            }
        }

        /// <summary>
        /// Номер вагона
        /// </summary>
        public string WagonNumber
        {
            get { return _wagonNumber; }
            set
            {
                if (value == _wagonNumber)
                    return;
                _wagonNumber = value;
                RaisePropertyChanged("WagonNumber");
            }
        }

        [Required(ErrorMessage = @"Не заполнено поле ""ПСА""")]
        public string Psa
        {
            get { return _psa; }
            set
            {
                if (value == _psa)
                    return;
                _psa = value;
                RaisePropertyChanged("Psa");
            }
        }

        public string Ttn
        {
            get { return _ttn; }
            set
            {
                if (value == _ttn)
                    return;
                _ttn = value;
                RaisePropertyChanged("Ttn");
            }
        }

        public string Comment
        {
            get { return _comment; }
            set
            {
                if (value == _comment)
                    return;
                _comment = value;
                RaisePropertyChanged("Comment");
            }
        }

        #endregion

        #region Методы

        public override void Dispose()
        {
            this.PropertyChanged -= OnPropertyChanged;

            base.Dispose();
        }

        private void LoadDocument(Guid id)
        {
            Document = MainStorage.Instance.TransportationRepository.GetById(id);
            Number = Document.Number;
            Date = Document.Date;
            DateOfLoading = Document.DateOfLoading;
            DateOfUnloading = Document.DateOfUnloading;
            Supplier = Suppliers.FirstOrDefault(x => x != null && x.Id == Document.SupplierId);
            if (Supplier != null && Supplier.Type == OrganizationType.Supplier)
                SupplierDivision = Supplier.Divisions.FirstOrDefault(x => x.Id == Document.SupplierDivisionId);
            Customer = Customers.FirstOrDefault(x => x != null && x.Id == Document.CustomerId);
            if (Customer != null && Customer.Type == OrganizationType.Customer)
                CustomerDivision = Customer.Divisions.FirstOrDefault(x => x.Id == Document.CustomerDivisionId);
            ResponsiblePerson = ResponsiblePersons.FirstOrDefault(x => x.Id == Document.ResponsiblePersonId);
            TransportType = Document.Type;
            switch (TransportType)
            {
                case DocumentType.TransportationAuto:
                    Transport = Transports.FirstOrDefault(x => x.Id == Document.TransportId);
                    Driver = Drivers.FirstOrDefault(x => x.Id == Document.DriverId);
                    break;
                case DocumentType.TransportationTrain:
                    WagonNumber = Document.Wagon;
                    break;
            }
            Psa = Document.Psa;
            Ttn = Document.Ttn;
            Comment = Document.Comment;

            foreach (TransportationItem item in Document.Items)
                Items.Add(new TransportationItemWrapper(item));
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Supplier":
                    if (Supplier == null || (Supplier != null && Supplier.Type == OrganizationType.Base))
                        SupplierDivision = null;
                    else if (Supplier != null && Supplier.Type == OrganizationType.Supplier)
                        SupplierDivision = Supplier.Divisions.Any() ? Supplier.Divisions.First() : null;
                    break;
                case "Customer":
                    if (Customer == null || (Customer != null && Customer.Type == OrganizationType.Base))
                        CustomerDivision = null;
                    else if (Customer != null && Customer.Type == OrganizationType.Customer)
                        CustomerDivision = Customer.Divisions.Any() ? Customer.Divisions.First() : null;
                    break;
                case "Transport":
                    if (Transport != null && Transport.DriverId.HasValue)
                        Driver = Drivers.FirstOrDefault(x => x.Id == Transport.DriverId.Value);
                    break;
            }
        }

        private void FillSuppliers()
        {
            Guid supplierId = Supplier != null ? Supplier.Id : Guid.Empty;

            _suppliers.Clear();
            _suppliers.AddRange(MainStorage.Instance.Bases.OrderBy(x => x.Name));
            _suppliers.Add(null);
            _suppliers.AddRange(MainStorage.Instance.Suppliers.OrderBy(x => x.Name));

            if (supplierId != Guid.Empty)
                Supplier = _suppliers.FirstOrDefault(x => x != null && x.Id == supplierId);
        }

        private void FillCustomers()
        {
            Guid customerId = Customer != null ? Customer.Id : Guid.Empty;

            _customers.Clear();
            _customers.AddRange(MainStorage.Instance.Bases.OrderBy(x => x.Name));
            _customers.Add(null);
            _customers.AddRange(MainStorage.Instance.Customers);

            if (customerId != Guid.Empty)
                Customer = _customers.FirstOrDefault(x => x != null && x.Id == customerId);
        }

        /// <summary>
        /// Сохранение документа
        /// </summary>
        protected override void SaveDocument()
        {
            if (!this.IsValid())
            {
                MessageBox.Show(this.Error);
                return;
            }

            bool isNew = false;
            if (Document == null)
            {
                isNew = true;
                Document = new Transportation(Id);
            }

            Document.UserId = null;
            Document.Type = TransportType;
            Document.Number = Number;
            Document.Date = Date.Value;
            Document.DateOfLoading = DateOfLoading.Value;
            Document.DateOfUnloading = DateOfUnloading.Value;
            Document.SupplierId = Supplier.Id;
            Document.SupplierDivisionId = SupplierDivision.GetId();
            Document.CustomerId = Customer.Id;
            Document.CustomerDivisionId = CustomerDivision.GetId();
            Document.ResponsiblePersonId = ResponsiblePerson.Id;
            Document.TransportId = TransportType == DocumentType.TransportationAuto ? Transport.Id : (Guid?)null;
            Document.DriverId = TransportType == DocumentType.TransportationAuto ? Driver.Id : (Guid?)null;
            Document.Wagon = WagonNumber;
            Document.Psa = Psa;
            Document.Ttn = Ttn;
            Document.Comment = Comment;

            if (Document.Items.Any())
                Document.Items.Clear();

            foreach (TransportationItemWrapper itemWrapper in Items)
            {
                itemWrapper.UpdateContainer();
                Document.Items.Add(itemWrapper.Container);
            }

            if (isNew)
                MainStorage.Instance.TransportationRepository.Create(Document);
            else
                MainStorage.Instance.TransportationRepository.Update(Document);
        }

        protected override void AddItem()
        {
            var newItem = new TransportationItemWrapper();
            Items.Add(newItem);
        }

        public override bool IsValid()
        {
            return base.IsValid();
        }

        #endregion

    }
}