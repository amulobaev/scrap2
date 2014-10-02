using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Tools;
using Zlatmet2.Views.Reports;

namespace Zlatmet2.ViewModels.Reports
{
    /// <summary>
    /// Модель представления "Отчёт по контрагентам"
    /// </summary>
    public partial class ReportOrganizationViewModel : BaseReportViewModel
    {
        private DateTime? _dateFrom;

        private DateTime? _dateTo;

        private readonly ObservableCollection<ContractorWrapper> _suppliers =
            new ObservableCollection<ContractorWrapper>();

        private readonly ObservableCollection<ContractorWrapper> _customers =
            new ObservableCollection<ContractorWrapper>();

        private readonly ObservableCollection<Nomenclature> _selectedNomenclatures =
            new ObservableCollection<Nomenclature>();

        private ICommand _selectAllNomenclatureCommand;
        private ICommand _unselectAllNomenclatureCommand;
        private ICommand _closeSuppliersCommand;
        private bool _suppliersIsOpen;
        private ICommand _closeCustomersCommand;
        private bool _customersIsOpen;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        /// <param name="optional"></param>
        public ReportOrganizationViewModel(LayoutDocument layout, Guid id, object optional = null)
            : base(layout, typeof(ReportOrganizationView), id)
        {
            Title = "Отчёт по контрагентам";
            Id = Guid.NewGuid();

            DateFrom = DateTime.Today;
            DateTo = DateTime.Today;

            foreach (Organization contractor in MainStorage.Instance.Contractors.OrderBy(x => x.Name))
            {
                Suppliers.Add(new ContractorWrapper(contractor));
                Customers.Add(new ContractorWrapper(contractor));
            }

            SelectAllNomenclature();
        }

        public override string ReportName
        {
            get { return "Отчет по контрагентам"; }
        }

        public DateTime? DateFrom
        {
            get { return _dateFrom; }
            set
            {
                if (value.Equals(_dateFrom))
                    return;
                _dateFrom = value;
                RaisePropertyChanged("DateFrom");
            }
        }

        public DateTime? DateTo
        {
            get { return _dateTo; }
            set
            {
                if (value.Equals(_dateTo))
                    return;
                _dateTo = value;
                RaisePropertyChanged("DateTo");
            }
        }

        public bool SuppliersIsOpen
        {
            get { return _suppliersIsOpen; }
            set
            {
                if (value.Equals(_suppliersIsOpen))
                    return;
                _suppliersIsOpen = value;
                RaisePropertyChanged("SuppliersIsOpen");
            }
        }

        public bool CustomersIsOpen
        {
            get { return _customersIsOpen; }
            set
            {
                if (value.Equals(_customersIsOpen))
                    return;
                _customersIsOpen = value;
                RaisePropertyChanged("CustomersIsOpen");
            }
        }

        /// <summary>
        /// Поставщики
        /// </summary>
        public ObservableCollection<ContractorWrapper> Suppliers
        {
            get { return _suppliers; }
        }

        public ObservableCollection<ContractorWrapper> Customers
        {
            get { return _customers; }
        }

        public ReadOnlyObservableCollection<Nomenclature> Nomenclatures
        {
            get { return MainStorage.Instance.Nomenclatures; }
        }

        public ObservableCollection<Nomenclature> SelectedNomenclatures
        {
            get { return _selectedNomenclatures; }
        }

        public ICommand CloseSuppliersCommand
        {
            get { return _closeSuppliersCommand ?? (_closeSuppliersCommand = new RelayCommand(CloseSuppliers)); }
        }

        public ICommand CloseCustomersCommand
        {
            get { return _closeCustomersCommand ?? (_closeCustomersCommand = new RelayCommand(CloseCustomers)); }
        }

        public ICommand SelectAllNomenclatureCommand
        {
            get
            {
                return _selectAllNomenclatureCommand ??
                       (_selectAllNomenclatureCommand = new RelayCommand(SelectAllNomenclature));
            }
        }

        public ICommand UnselectAllNomenclatureCommand
        {
            get
            {
                return _unselectAllNomenclatureCommand ??
                       (_unselectAllNomenclatureCommand = new RelayCommand(UnselectAllNomenclature));
            }
        }

        private void SelectAllNomenclature()
        {
            SelectedNomenclatures.Clear();
            SelectedNomenclatures.AddRange(Nomenclatures);
        }

        private void UnselectAllNomenclature()
        {
            SelectedNomenclatures.Clear();
        }

        protected override void PrepareReport()
        {
            throw new NotImplementedException();
        }

        private void CloseSuppliers()
        {
            SuppliersIsOpen = false;
        }

        private void CloseCustomers()
        {
            CustomersIsOpen = false;
        }

    }
}