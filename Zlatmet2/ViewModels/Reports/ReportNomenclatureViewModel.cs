using System;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.Views.Reports;

namespace Zlatmet2.ViewModels.Reports
{
    public sealed class ReportNomenclatureViewModel : UniqueLayoutDocumentViewModel
    {
        private DateTime _dateFrom;
        private DateTime _dateTo;

        public ReportNomenclatureViewModel(LayoutDocument layout, Guid id, object optional = null)
            : base(layout, typeof(ReportNomenclatureView), id)
        {
            Title = "Обороты за период";

            Id = Guid.NewGuid();

            DateFrom = DateTime.Today;
            DateTo = DateTime.Today;
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
    }
}
