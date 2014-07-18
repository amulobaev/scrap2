using System;
using System.Collections.ObjectModel;
using System.Linq;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Models.Reports;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.Views.Reports;

namespace Zlatmet2.ViewModels.Reports
{
    /// <summary>
    /// Модель представления "Остатки на базе"
    /// </summary>
    public class ReportWarehouseViewModel : UniqueLayoutDocumentViewModel
    {
        private DateTime _date;
        private StiReport _report;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        public ReportWarehouseViewModel(LayoutDocument layout, Guid id)
            : base(layout, typeof(ReportWarehouseView), id)
        {
            Title = "Остатки на базе";

            Id = Guid.NewGuid();

            Date = DateTime.Today;

            Bases = new ObservableCollection<CheckedWrapper>();
            foreach (Organization @base in MainStorage.Instance.Bases.OrderBy(x => x.Name))
                Bases.Add(new CheckedWrapper(@base.Id, true, @base.Name));

            Nomenclatures = new ObservableCollection<CheckedWrapper>();
            foreach (Nomenclature nomenclature in MainStorage.Instance.Nomenclatures.OrderBy(x => x.Name))
                Nomenclatures.Add(new CheckedWrapper(nomenclature.Id, true, nomenclature.Name));
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (value.Equals(_date))
                    return;
                _date = value;
                RaisePropertyChanged("Date");
            }
        }

        public ObservableCollection<CheckedWrapper> Bases { get; private set; }

        public ObservableCollection<CheckedWrapper> Nomenclatures { get; private set; }

        public StiReport Report
        {
            get { return _report; }
            set
            {
                if (Equals(value, _report))
                    return;
                _report = value;
                RaisePropertyChanged("Report");
            }
        }

    }
}
