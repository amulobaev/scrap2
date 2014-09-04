using System;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.Views.Documents;

namespace Zlatmet2.ViewModels.Documents
{
    public class DocumentReportViewModel : UniqueLayoutDocumentViewModel
    {
        private object _optionalContent;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        /// <param name="report"></param>
        public DocumentReportViewModel(LayoutDocument layout, Guid id, StiReport report)
            : base(layout, typeof(DocumentReportView), id)
        {
            Title = "ПСА";
            OptionalContent = report;
        }

        public override object OptionalContent
        {
            get { return _optionalContent; }
            set
            {
                if (Equals(value, _optionalContent))
                    return;
                _optionalContent = value;
                RaisePropertyChanged("OptionalContent");
            }
        }

    }
}