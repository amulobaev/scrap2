using System;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.Views.Documents;

namespace Zlatmet2.ViewModels.Documents
{
    public class DocumentReportViewModel : UniqueLayoutDocumentViewModel
    {
        private StiReport _report;

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
