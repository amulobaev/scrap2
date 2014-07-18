using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Models.References;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.Views.Reports;

namespace Zlatmet2.ViewModels.Reports
{
    public class ReportSupplierViewModel : UniqueLayoutDocumentViewModel
    {
        private bool _nomenclature;
        private Nomenclature _selectedNomenclature;
        private DateTime? _dateFrom;
        private DateTime? _dateTo;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        public ReportSupplierViewModel(LayoutDocument layout, Guid id)
            : base(layout, typeof(ReportSupplierView), id)
        {
            Title = "Отчёт по поставщику";
            Id = Guid.NewGuid();

            Suppliers = new ObservableCollection<OrganizationWrapper>();
            foreach (Organization supplier in MainStorage.Instance.Suppliers.OrderBy(x => x.Name))
                Suppliers.Add(new OrganizationWrapper(supplier));
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

        public ObservableCollection<OrganizationWrapper> Suppliers { get; private set; }

        public bool Nomenclature
        {
            get { return _nomenclature; }
            set
            {
                if (value.Equals(_nomenclature))
                    return;
                _nomenclature = value;
                RaisePropertyChanged("Nomenclature");
            }
        }

        public Nomenclature SelectedNomenclature
        {
            get { return _selectedNomenclature; }
            set
            {
                if (Equals(value, _selectedNomenclature))
                    return;
                _selectedNomenclature = value;
                RaisePropertyChanged("SelectedNomenclature");
            }
        }

        public ReadOnlyObservableCollection<Nomenclature> Nomenclatures
        {
            get { return MainStorage.Instance.Nomenclatures; }
        }
    }
}
