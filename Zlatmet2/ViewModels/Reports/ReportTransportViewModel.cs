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
using Zlatmet2.Core.Enums;
using Zlatmet2.Tools;
using Zlatmet2.Views.Reports;

namespace Zlatmet2.ViewModels.Reports
{
    /// <summary>
    /// Модель представления отчёта "Перевозки"
    /// </summary>
    public class ReportTransportViewModel : BaseReportViewModel
    {
        private readonly Template _template;

        private DateTime _dateFrom;
        private DateTime _dateTo;
        private Organization _supplier;
        private Organization _customer;
        private Nomenclature _nomenclature;
        private readonly ObservableCollection<Organization> _suppliers = new ObservableCollection<Organization>();
        private readonly ObservableCollection<Organization> _customers = new ObservableCollection<Organization>();
        private bool _supplierIsEnabled;
        private bool _customerIsEnabled;
        private bool _nomenclatureIsEnabled;
        private readonly ObservableCollection<Nomenclature> _selectedNomenclatures =
            new ObservableCollection<Nomenclature>();
        private TransportType _transportType;
        private ICommand _selectAllNomenclaturesCommand;
        private ICommand _unselectAllNomenclaturesCommand;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        /// <param name="optional"></param>
        public ReportTransportViewModel(LayoutDocument layout, Guid id, object optional = null)
            : base(layout, typeof(ReportTransportView), id)
        {
            Title = "Отчет по перевозкам";

            Id = Guid.NewGuid();

            DateFrom = DateTime.Today;
            DateTo = DateTime.Today;

            _template = MainStorage.Instance.TemplatesRepository.GetByName(ReportName);

            Report = new StiReport();

            FillOrganizations();

            SelectedNomenclatures.AddRange(Nomenclatures);
        }

        private void FillOrganizations()
        {
            _suppliers.AddRange(MainStorage.Instance.Bases.OrderBy(x => x.Name));
            _suppliers.Add(null);
            _suppliers.AddRange(MainStorage.Instance.Contractors.OrderBy(x => x.Name));

            _customers.AddRange(MainStorage.Instance.Bases.OrderBy(x => x.Name));
            _customers.Add(null);
            _customers.AddRange(MainStorage.Instance.Contractors.OrderBy(x => x.Name));
        }

        public override string ReportName
        {
            get { return "Отчет по перевозкам"; }
        }

        public TransportType TransportType
        {
            get { return _transportType; }
            set { Set(() => TransportType, ref _transportType, value); }
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

        public bool SupplierIsEnabled
        {
            get { return _supplierIsEnabled; }
            set { Set(() => SupplierIsEnabled, ref _supplierIsEnabled, value); }
        }

        public ObservableCollection<Organization> Suppliers
        {
            get { return _suppliers; }
        }

        public Organization Supplier
        {
            get { return _supplier; }
            set { Set(() => Supplier, ref _supplier, value); }
        }

        public bool CustomerIsEnabled
        {
            get { return _customerIsEnabled; }
            set { Set(() => CustomerIsEnabled, ref _customerIsEnabled, value); }
        }

        public ObservableCollection<Organization> Customers
        {
            get { return _customers; }
        }

        public Organization Customer
        {
            get { return _customer; }
            set { Set(() => Customer, ref _customer, value); }
        }

        public bool NomenclatureIsEnabled
        {
            get { return _nomenclatureIsEnabled; }
            set { Set(() => NomenclatureIsEnabled, ref _nomenclatureIsEnabled, value); }
        }

        public ReadOnlyObservableCollection<Nomenclature> Nomenclatures
        {
            get { return MainStorage.Instance.Nomenclatures; }
        }

        public ObservableCollection<Nomenclature> SelectedNomenclatures
        {
            get { return _selectedNomenclatures; }
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
            Report.Dictionary.Variables["ТипПеревозок"].Value = TransportType == TransportType.Auto
                ? "автомобильным"
                : "ж/д";
            Report.Dictionary.Variables["НомерТранспорта"].Value = TransportType == TransportType.Auto
                ? "Автомобиль и номер"
                : "Номер вагона";

            Report.Compile();
            Report.Render(false);
        }

    }
}