using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Scrap.Core.Classes.Documents;
using Scrap.Core.Classes.References;
using Scrap.Core.Classes.Service;
using Scrap.Core.Enums;
using Scrap.Models.Documents;
using Scrap.Tools;
using Scrap.Views.Documents;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;

namespace Scrap.ViewModels.Documents
{
    /// <summary>
    /// Модель представления документа "Перевозка"
    /// </summary>
    public class DocumentTransportationViewModel : BaseDocumentViewModel<Transportation, TransportationItemWrapper>
    {
        #region Поля

        private readonly ObservableCollection<Organization> _suppliers = new ObservableCollection<Organization>();

        private readonly ObservableCollection<Organization> _customers = new ObservableCollection<Organization>();

        private DateTime? _date;

        private Organization _supplier;
        private Organization _customer;
        private Employee _responsiblePerson;
        private DocumentType _transportType = DocumentType.TransportationAuto;
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

        private ICommand _printCommand;

        /// <summary>
        /// Шаблон отчёта "ПСА"
        /// </summary>
        private Template _psaTemplate;

        #endregion Поля

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        /// <param name="optional"></param>
        public DocumentTransportationViewModel(LayoutDocument layout, Guid id, object optional = null)
            : base(layout, typeof(DocumentTransportationView), id)
        {
            Suppliers = new ReadOnlyObservableCollection<Organization>(_suppliers);
            Customers = new ReadOnlyObservableCollection<Organization>(_customers);

            FillOrganizations();

            ((INotifyCollectionChanged)MainStorage.Instance.Bases).CollectionChanged +=
                (sender, args) => FillOrganizations();
            ((INotifyCollectionChanged)MainStorage.Instance.Contractors).CollectionChanged +=
                (sender, args) => FillOrganizations();

            if (Id != Guid.Empty)
            {
                // Загрузка документа
                LoadDocument(id);
            }
            else
            {
                // Новый документ
                Id = Guid.NewGuid();

                Guid idToLoad;
                if (optional != null && Guid.TryParse(optional.ToString(), out idToLoad))
                {
                    LoadDocument(idToLoad);
                    Container = null;
                }

                Number = MainStorage.Instance.JournalRepository.GetNextDocumentNumber();
                Date = DateTime.Now;

                DateOfLoading = DateTime.Today;
                DateOfUnloading = DateTime.Today;
            }

            UpdateTitle();

            this.PropertyChanged += OnPropertyChanged;
        }

        #region Свойства

        /// <summary>
        /// Дата документа
        /// </summary>
        [Required]
        public DateTime? Date
        {
            get { return _date; }
            set
            {
                if (value == _date)
                    return;

                // Сохраним старое значение
                DateTime? oldValue = _date.HasValue ? _date.Value.Date : _date;

                _date = value;
                RaisePropertyChanged(() => Date);

                if (DateOfLoading == oldValue)
                    DateOfLoading = value;
                if (DateOfUnloading == oldValue)
                    DateOfUnloading = value;
            }
        }

        [Required]
        public DateTime? DateOfLoading
        {
            get { return _dateOfLoading; }
            set { Set(() => DateOfLoading, ref _dateOfLoading, value); }
        }

        [Required]
        public DateTime? DateOfUnloading
        {
            get { return _dateOfUnloading; }
            set { Set(() => DateOfUnloading, ref _dateOfUnloading, value); }
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
            set { Set(() => Supplier, ref _supplier, value); }
        }

        /// <summary>
        /// Выбранное подразделение поставщика
        /// </summary>
        [Required(ErrorMessage = @"Не выбрано подразделение поставщика")]
        public Division SupplierDivision
        {
            get
            {
                return Supplier != null && Supplier.Type == OrganizationType.Contractor
                    ? _supplierDivision
                    : Division.Empty;
            }
            set { Set(() => SupplierDivision, ref _supplierDivision, value); }
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
            set { Set(() => Customer, ref _customer, value); }
        }

        /// <summary>
        /// Выбранное подразделение заказчика
        /// </summary>
        [Required(ErrorMessage = @"Не выбрано подразделение заказчика")]
        public Division CustomerDivision
        {
            get
            {
                return Customer != null && Customer.Type == OrganizationType.Contractor
                    ? _customerDivision
                    : Division.Empty;
            }
            set { Set(() => CustomerDivision, ref _customerDivision, value); }
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
            set { Set(() => ResponsiblePerson, ref _responsiblePerson, value); }
        }

        public DocumentType TransportType
        {
            get { return _transportType; }
            set { Set(() => TransportType, ref _transportType, value); }
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
            set { Set(() => Transport, ref _transport, value); }
        }

        public ReadOnlyObservableCollection<Employee> Drivers
        {
            get { return MainStorage.Instance.Drivers; }
        }

        [Required(ErrorMessage = @"Не выбран водитель")]
        public Employee Driver
        {
            get { return TransportType == DocumentType.TransportationAuto ? _driver : Employee.Empty; }
            set { Set(() => Driver, ref _driver, value); }
        }

        /// <summary>
        /// Номер вагона
        /// </summary>
        public string WagonNumber
        {
            get { return _wagonNumber; }
            set { Set(() => WagonNumber, ref _wagonNumber, value); }
        }

        public string Psa
        {
            get { return _psa; }
            set { Set(() => Psa, ref _psa, value); }
        }

        public string Ttn
        {
            get { return _ttn; }
            set { Set(() => Ttn, ref _ttn, value); }
        }

        public string Comment
        {
            get { return _comment; }
            set { Set(() => Comment, ref _comment, value); }
        }

        public ICommand PrintCommand
        {
            get { return _printCommand ?? (_printCommand = new RelayCommand(PrintDocument)); }
        }

        #endregion Свойства

        #region Методы

        public override void Dispose()
        {
            this.PropertyChanged -= OnPropertyChanged;

            base.Dispose();
        }

        private void LoadDocument(Guid id)
        {
            Container = MainStorage.Instance.TransportationRepository.GetById(id);
            Number = Container.Number;
            Date = Container.Date;
            DateOfLoading = Container.DateOfLoading;
            DateOfUnloading = Container.DateOfUnloading;
            Supplier = Suppliers.FirstOrDefault(x => x != null && x.Id == Container.SupplierId);
            if (Supplier != null && Supplier.Type == OrganizationType.Contractor)
                SupplierDivision = Supplier.Divisions.FirstOrDefault(x => x.Id == Container.SupplierDivisionId);
            Customer = Customers.FirstOrDefault(x => x != null && x.Id == Container.CustomerId);
            if (Customer != null && Customer.Type == OrganizationType.Contractor)
                CustomerDivision = Customer.Divisions.FirstOrDefault(x => x.Id == Container.CustomerDivisionId);
            ResponsiblePerson = ResponsiblePersons.FirstOrDefault(x => x.Id == Container.ResponsiblePersonId);
            TransportType = Container.Type;
            switch (TransportType)
            {
                case DocumentType.TransportationAuto:
                    Transport = Transports.FirstOrDefault(x => x.Id == Container.TransportId);
                    Driver = Drivers.FirstOrDefault(x => x.Id == Container.DriverId);
                    break;
                case DocumentType.TransportationTrain:
                    WagonNumber = Container.Wagon;
                    break;
            }
            Psa = Container.Psa;
            Ttn = Container.Ttn;
            Comment = Container.Comment;

            foreach (TransportationItem item in Container.Items)
                Items.Add(new TransportationItemWrapper(item));
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Supplier":
                    if (Supplier == null || (Supplier != null && Supplier.Type == OrganizationType.Base))
                        SupplierDivision = null;
                    else if (Supplier != null && Supplier.Type == OrganizationType.Contractor)
                        SupplierDivision = Supplier.Divisions.Any() ? Supplier.Divisions.First() : null;
                    break;
                case "Customer":
                    if (Customer == null || (Customer != null && Customer.Type == OrganizationType.Base))
                        CustomerDivision = null;
                    else if (Customer != null && Customer.Type == OrganizationType.Contractor)
                        CustomerDivision = Customer.Divisions.Any() ? Customer.Divisions.First() : null;
                    break;
                case "Transport":
                    if (Transport != null && Transport.DriverId.HasValue)
                        Driver = Drivers.FirstOrDefault(x => x.Id == Transport.DriverId.Value);
                    break;
            }
        }

        /// <summary>
        /// Заполнение списка организаций
        /// </summary>
        private void FillOrganizations()
        {
            // Поставщики
            Guid supplierId = Supplier != null ? Supplier.Id : Guid.Empty;

            _suppliers.Clear();
            _suppliers.AddRange(MainStorage.Instance.Bases.OrderBy(x => x.Name));
            _suppliers.Add(null);
            _suppliers.AddRange(MainStorage.Instance.Contractors.OrderBy(x => x.Name));

            if (supplierId != Guid.Empty)
                Supplier = _suppliers.FirstOrDefault(x => x != null && x.Id == supplierId);

            // Заказчики
            Guid customerId = Customer != null ? Customer.Id : Guid.Empty;

            _customers.Clear();
            _customers.AddRange(MainStorage.Instance.Bases.OrderBy(x => x.Name));
            _customers.Add(null);
            _customers.AddRange(MainStorage.Instance.Contractors.OrderBy(x => x.Name));

            if (customerId != Guid.Empty)
                Customer = _customers.FirstOrDefault(x => x != null && x.Id == customerId);
        }

        protected override string DocumentTitle
        {
            get { return "Перевозка"; }
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
            if (Container == null)
            {
                isNew = true;
                Container = new Transportation(Id);
            }

            Container.UserId = null;
            Container.Type = TransportType;
            Container.Number = Number;
            Container.Date = Date.Value;
            Container.DateOfLoading = DateOfLoading.Value;
            Container.DateOfUnloading = DateOfUnloading.Value;
            Container.SupplierId = Supplier.Id;
            Container.SupplierDivisionId = SupplierDivision.GetId();
            Container.CustomerId = Customer.Id;
            Container.CustomerDivisionId = CustomerDivision.GetId();
            Container.ResponsiblePersonId = ResponsiblePerson.Id;
            Container.TransportId = TransportType == DocumentType.TransportationAuto ? Transport.Id : (Guid?)null;
            Container.DriverId = TransportType == DocumentType.TransportationAuto ? Driver.Id : (Guid?)null;
            Container.Wagon = WagonNumber;
            Container.Psa = Psa;
            Container.Ttn = Ttn;
            Container.Comment = Comment;

            if (Container.Items.Any())
                Container.Items.Clear();

            foreach (TransportationItemWrapper itemWrapper in Items)
            {
                itemWrapper.UpdateContainer();
                Container.Items.Add(itemWrapper.Container);
            }

            if (isNew)
                MainStorage.Instance.TransportationRepository.Create(Container);
            else
                MainStorage.Instance.TransportationRepository.Update(Container);

            UpdateJournal();
        }

        protected override void AddItem()
        {
            var newItem = new TransportationItemWrapper();
            Items.Add(newItem);
        }

        protected override void UpdateTitle()
        {
            Title = string.Format("{0} №{1} от {2}", DocumentTitle, Number,
                Date.HasValue ? Date.Value.ToShortDateString() : string.Empty);
        }

        private void PrintDocument()
        {
            if (!Items.Any())
            {
                MessageBox.Show("Не заполнена табличная часть");
                return;
            }

            // Подготовка отчета
            StiReport report = new StiReport();
            try
            {
                // Загрузка формы отчета
                const string templateName = "ПСА";
                if (_psaTemplate == null)
                    _psaTemplate = MainStorage.Instance.TemplatesRepository.GetByName(templateName);
                if (_psaTemplate == null)
                {
                    MessageBox.Show(string.Format("Ошибка загрузки шаблона \"{0}\"", templateName), MainStorage.AppName,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                report.Load(_psaTemplate.Data);

                // Тара
                double tara = Transport != null ? Transport.Tara : 0;

                // Переменные
                report.Dictionary.Variables["Номер"].Value = Number.ToString();
                report.Dictionary.Variables["Дата"].Value = Date.HasValue ? Date.Value.ToShortDateString() : string.Empty;
                report.Dictionary.Variables["Поставщик"].Value = Supplier != null ? Supplier.FullName : string.Empty;
                report.Dictionary.Variables["ИНН"].Value = Supplier != null ? Supplier.Inn : string.Empty;
                report.Dictionary.Variables["БИК"].Value = Supplier != null ? Supplier.Bik : string.Empty;
                report.Dictionary.Variables["Банк"].Value = Supplier != null ? Supplier.Bank : string.Empty;
                report.Dictionary.Variables["Транспорт"].Value = Transport != null
                    ? string.Format("{0} {1}", Transport.Name, Transport.Number)
                    : string.Empty;
                report.Dictionary.Variables["Договор"].Value = Supplier != null ? Supplier.Contract : string.Empty;
                report.Dictionary.Variables["Примечание"].Value = Comment;
                report.Dictionary.Variables["Тара"].ValueObject = tara;
                report.Dictionary.Variables["ОтвЛицо"].Value = ResponsiblePerson != null
                    ? ResponsiblePerson.Name
                    : string.Empty;

                // Заполнение табличной части
                List<PsaItem> reportData = Items.Select(item => new PsaItem
                {
                    Nomenclature = item.UnloadingNomenclature.Name,
                    Brutto = tara + item.UnloadingWeight,
                    Garbage = item.Garbage,
                    Netto = item.Netto,
                    Price = (double)item.Price,
                    Sum = item.Netto * (double)item.Price
                }).ToList();

                report.Dictionary.Variables["МассаПрописью"].Value =
                    RusCurrency.Str2(Math.Round(reportData.Sum(x => x.Netto) * 1000), false, "кг", "кг", "кг", "", "", "");
                report.Dictionary.Variables["СуммаПрописью"].Value = RusCurrency.Str(reportData.Sum(x => x.Sum));

                double sumNetto = reportData.Sum(x => x.Netto);

                report.RegBusinessObject("Data", reportData);

                report.Compile();
                report.Render(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при подготовке отчета" + Environment.NewLine + ex.Message);
                return;
            }

            MainViewModel.Instance.ShowLayoutDocument(typeof(DocumentReportViewModel), Id, report);
        }

        #endregion

        private class PsaItem
        {
            public string Nomenclature { get; set; }
            public double Brutto { get; set; }
            public double Garbage { get; set; }
            public double Netto { get; set; }
            public double Price { get; set; }
            public double Sum { get; set; }
        }
    }
}