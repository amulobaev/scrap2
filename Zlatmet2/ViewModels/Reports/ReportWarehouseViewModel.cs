using System;
using System.Collections.ObjectModel;
using System.Linq;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Core.Classes.Service;
using Zlatmet2.Models.Reports;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.Views.Reports;

namespace Zlatmet2.ViewModels.Reports
{
    /// <summary>
    /// Модель представления "Остатки на базе"
    /// </summary>
    public class ReportWarehouseViewModel : BaseReportViewModel
    {
        private DateTime _date;
        private readonly Template _template;

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

            _template = MainStorage.Instance.TemplatesRepository.GetByName(ReportName);
            
            Report = new StiReport();
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

        public override string ReportName
        {
            get { return "Остатки на базе"; }
        }

        protected override void PrepareReport()
        {
            Report = new StiReport();
            Report.Load(_template.Data);
            Report.Compile();
            Report.Render();
        }

    }
}