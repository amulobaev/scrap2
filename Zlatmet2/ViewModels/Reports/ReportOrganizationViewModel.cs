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
    /// <summary>
    /// Модель представления "Отчёт по контрагентам"
    /// </summary>
    public class ReportOrganizationViewModel : UniqueLayoutDocumentViewModel
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
        /// <param name="optional"></param>
        public ReportOrganizationViewModel(LayoutDocument layout, Guid id, object optional = null)
            : base(layout, typeof(ReportOrganizationView), id)
        {
            Title = "Отчёт по контрагентам";
            Id = Guid.NewGuid();

            Suppliers = new ObservableCollection<OrganizationWrapper>();
            foreach (Organization supplier in MainStorage.Instance.Contractors.OrderBy(x => x.Name))
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
