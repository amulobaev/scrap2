using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
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
            if (_template == null)
            {
                MessageBox.Show(string.Format("Отсутствует шаблон \"{0}\"", ReportName), MainStorage.AppName,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedBases = Bases.Where(x => x.IsChecked).ToList();
            if (!selectedBases.Any())
            {
                MessageBox.Show("Не выбрано ни одной базы", MainStorage.AppName,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedNomenclatures = Nomenclatures.Where(x => x.IsChecked).ToList();
            if (!selectedNomenclatures.Any())
            {
                MessageBox.Show("Не выбрана номенклатура", MainStorage.AppName,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string bases = string.Join(", ", selectedBases.Select(x => x.Text));

            List<ReportData> reportData = PrepareData();

            Report = new StiReport();
            Report.Load(_template.Data);

            Report.Dictionary.Variables["ReportDate"].Value = Date.ToShortDateString();
            Report.Dictionary.Variables["Bases"].Value = bases;

            Report.RegBusinessObject("Data", reportData);

            Report.Compile();
            Report.Render();
        }

        private List<ReportData> PrepareData()
        {
            List<ReportData> reportData = new List<ReportData>();

            return reportData;
        }

        class ReportData
        {
            public int Number { get; set; }
            public string Name { get; set; }
            public double Weight { get; set; }
        }
    }
}