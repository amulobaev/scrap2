using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Core.Classes.Service;
using Zlatmet2.Tools;
using Zlatmet2.Views.Reports;

namespace Zlatmet2.ViewModels.Reports
{
    /// <summary>
    /// Модель представления отчёта "Перевозки"
    /// </summary>
    public partial class ReportTransportationViewModel : BaseReportViewModel
    {
        private readonly Template _template;

        private DateTime _dateFrom;
        private DateTime _dateTo;
        private readonly ObservableCollection<ContractorWrapper> _suppliers = new ObservableCollection<ContractorWrapper>();
        private readonly ObservableCollection<ContractorWrapper> _customers = new ObservableCollection<ContractorWrapper>();
        private readonly ObservableCollection<Nomenclature> _selectedNomenclatures =
            new ObservableCollection<Nomenclature>();
        private ICommand _selectAllNomenclaturesCommand;
        private ICommand _unselectAllNomenclaturesCommand;
        private bool _isAuto = true;
        private bool _isTrain = true;
        private bool _suppliersIsOpen;
        private bool _customersIsOpen;
        private ICommand _selectAllSuppliersCommand;
        private ICommand _unselectAllSuppliersCommand;
        private ICommand _closeSuppliersCommand;
        private ICommand _selectAllCustomersCommand;
        private ICommand _unselectAllCustomersCommand;
        private ICommand _closeCustomersCommand;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        /// <param name="optional"></param>
        public ReportTransportationViewModel(LayoutDocument layout, Guid id, object optional = null)
            : base(layout, typeof(ReportTransportationView), id)
        {
            Title = "Отчет по перевозкам";

            Id = Guid.NewGuid();

            DateFrom = DateTime.Today;
            DateTo = DateTime.Today;

            _template = MainStorage.Instance.TemplatesRepository.GetByName(ReportName);

            Report = new StiReport();

            foreach (Organization @base in MainStorage.Instance.Bases.OrderBy(x => x.Name))
            {
                Suppliers.Add(new ContractorWrapper(@base));
                Customers.Add(new ContractorWrapper(@base));
            }

            foreach (Organization contractor in MainStorage.Instance.Contractors.OrderBy(x => x.Name))
            {
                Suppliers.Add(new ContractorWrapper(contractor));
                Customers.Add(new ContractorWrapper(contractor));
            }

            SelectAllNomenclatures();
        }

        public override string ReportName
        {
            get { return "Отчет по перевозкам"; }
        }

        public bool IsAuto
        {
            get { return _isAuto; }
            set { Set(() => IsAuto, ref _isAuto, value); }
        }

        public bool IsTrain
        {
            get { return _isTrain; }
            set { Set(() => IsTrain, ref _isTrain, value); }
        }

        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { Set(() => DateFrom, ref _dateFrom, value); }
        }

        public DateTime DateTo
        {
            get { return _dateTo; }
            set { Set(() => DateTo, ref _dateTo, value); }
        }

        public bool SuppliersIsOpen
        {
            get { return _suppliersIsOpen; }
            set { Set(() => SuppliersIsOpen, ref _suppliersIsOpen, value); }
        }

        public bool CustomersIsOpen
        {
            get { return _customersIsOpen; }
            set { Set(() => CustomersIsOpen, ref _customersIsOpen, value); }
        }

        public ObservableCollection<ContractorWrapper> Suppliers
        {
            get { return _suppliers; }
        }

        public ObservableCollection<ContractorWrapper> Customers
        {
            get { return _customers; }
        }

        /// <summary>
        /// Вся номенклатура
        /// </summary>
        public ReadOnlyObservableCollection<Nomenclature> Nomenclatures
        {
            get { return MainStorage.Instance.Nomenclatures; }
        }

        /// <summary>
        /// Выбранная номенклатура
        /// </summary>
        public ObservableCollection<Nomenclature> SelectedNomenclatures
        {
            get { return _selectedNomenclatures; }
        }

        public ICommand SelectAllSuppliersCommand
        {
            get
            {
                return _selectAllSuppliersCommand ?? (_selectAllSuppliersCommand = new RelayCommand(SelectAllSuppliers));
            }
        }

        public ICommand UnselectAllSuppliersCommand
        {
            get
            {
                return _unselectAllSuppliersCommand ??
                       (_unselectAllSuppliersCommand = new RelayCommand(UnselectAllSuppliers));
            }
        }

        public ICommand CloseSuppliersCommand
        {
            get { return _closeSuppliersCommand ?? (_closeSuppliersCommand = new RelayCommand(CloseSuppliers)); }
        }

        public ICommand SelectAllCustomersCommand
        {
            get
            {
                return _selectAllCustomersCommand ?? (_selectAllCustomersCommand = new RelayCommand(SelectAllCustomers));
            }
        }

        public ICommand UnselectAllCustomersCommand
        {
            get
            {
                return _unselectAllCustomersCommand ??
                       (_unselectAllCustomersCommand = new RelayCommand(UnselectAllCustomers));
            }
        }

        public ICommand CloseCustomersCommand
        {
            get { return _closeCustomersCommand ?? (_closeCustomersCommand = new RelayCommand(CloseCustomers)); }
        }

        public ICommand SelectAllNomenclaturesCommand
        {
            get
            {
                return _selectAllNomenclaturesCommand ??
                       (_selectAllNomenclaturesCommand = new RelayCommand(SelectAllNomenclatures));
            }
        }

        public ICommand UnselectAllNomenclaturesCommand
        {
            get
            {
                return _unselectAllNomenclaturesCommand ??
                       (_unselectAllNomenclaturesCommand = new RelayCommand(UnselectAllNomenclatures));
            }
        }

        private void SelectAllSuppliers()
        {
            foreach (ContractorWrapper supplier in Suppliers)
                supplier.IsChecked = true;
        }

        private void UnselectAllSuppliers()
        {
            foreach (ContractorWrapper supplier in Suppliers)
                supplier.IsChecked = false;
        }

        private void CloseSuppliers()
        {
            SuppliersIsOpen = false;
        }

        private void SelectAllCustomers()
        {
            foreach (ContractorWrapper customer in Customers)
                customer.IsChecked = true;
        }

        private void UnselectAllCustomers()
        {
            foreach (ContractorWrapper customer in Customers)
                customer.IsChecked = false;
        }

        private void CloseCustomers()
        {
            CustomersIsOpen = false;
        }

        private void SelectAllNomenclatures()
        {
            SelectedNomenclatures.Clear();
            SelectedNomenclatures.AddRange(Nomenclatures);
        }

        private void UnselectAllNomenclatures()
        {
            SelectedNomenclatures.Clear();
        }

        protected override void PrepareReport()
        {
            if (_template == null)
            {
                MessageBox.Show(string.Format("Отсутствует шаблон \"{0}\"", ReportName), MainStorage.AppName,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Report = new StiReport();
            Report.Load(_template.Data);

            Report.Dictionary.Variables["DateFrom"].Value = DateFrom.ToShortDateString();
            Report.Dictionary.Variables["DateTo"].Value = DateTo.ToShortDateString();
            //Report.Dictionary.Variables["ТипПеревозок"].Value = TransportType == TransportType.Auto
            //    ? "автомобильным"
            //    : "ж/д";
            //Report.Dictionary.Variables["НомерТранспорта"].Value = TransportType == TransportType.Auto
            //    ? "Автомобиль и номер"
            //    : "Номер вагона";

            Report.Compile();
            Report.Render(false);
        }

    }
}