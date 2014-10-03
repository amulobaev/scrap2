using System;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Views.Reports;

namespace Zlatmet2.ViewModels.Reports
{
    /// <summary>
    /// Модель представления отчёта "Перевозки"
    /// </summary>
    public class ReportTransportViewModel : BaseReportViewModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        /// <param name="optional"></param>
        public ReportTransportViewModel(LayoutDocument layout, Guid id, object optional = null)
            : base(layout, typeof(ReportTransportView), id)
        {
            Title = "Перевозки";

            Id = Guid.NewGuid();
        }

        public override string ReportName
        {
            get { return "Перевозки"; }
        }

        protected override void PrepareReport()
        {
            throw new NotImplementedException();
        }
    }
}
