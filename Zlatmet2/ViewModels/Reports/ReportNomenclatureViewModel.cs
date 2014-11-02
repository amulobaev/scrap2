using System;
using System.Collections.Generic;
using System.Windows;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes.Reports;
using Zlatmet2.Core.Classes.Service;
using Zlatmet2.Views.Reports;

namespace Zlatmet2.ViewModels.Reports
{
    /// <summary>
    /// Модель представления отчета "Обороты за период"
    /// </summary>
    public sealed class ReportNomenclatureViewModel : BaseReportViewModel
    {
        private readonly Template _template;

        private DateTime _dateFrom;

        private DateTime _dateTo;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        /// <param name="optional"></param>
        public ReportNomenclatureViewModel(LayoutDocument layout, Guid id, object optional = null)
            : base(layout, typeof(ReportNomenclatureView), id)
        {
            Title = "Обороты за период";

            Id = Guid.NewGuid();

            DateFrom = DateTime.Today;
            DateTo = DateTime.Today;

            _template = MainStorage.Instance.TemplatesRepository.GetByName(ReportName);

            Report = new StiReport();
        }

        public DateTime DateFrom
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

        public DateTime DateTo
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

        public override string ReportName
        {
            get { return "Обороты за период"; }
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

            // Переменные отчета
            Report.Dictionary.Variables["DateFrom"].Value = DateFrom.ToShortDateString();
            Report.Dictionary.Variables["DateTo"].Value = DateTo.ToShortDateString();

            // Данные отчета
            List<ReportNomenclatureData> reportData = MainStorage.Instance.ReportsRepository.ReportNomenclature(
                DateFrom, DateTo);
            Report.RegBusinessObject("Data", reportData);

            Report.Compile();
            Report.Render(false);
        }

    }
}
