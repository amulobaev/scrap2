using System;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.Views.Documents;

namespace Zlatmet2.ViewModels.Documents
{
    public class DocumentReportViewModel : UniqueLayoutDocumentViewModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        /// <param name="report"></param>
        public DocumentReportViewModel(LayoutDocument layout, Guid id, StiReport report)
            : base(layout, typeof(DocumentReportView), id, report)
        {
            Title = "ПСА";
        }

        public StiReport Report
        {
            get { return (StiReport)Container; }
            set
            {
                if (Equals(value, Container))
                    return;
                Container = value;
                RaisePropertyChanged("Report");
            }
        }

        public override void SetContainer(object dataForContainer)
        {
            Report = dataForContainer as StiReport;
        }
    }
}
