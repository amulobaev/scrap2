using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.ViewModels.Base;

namespace Zlatmet2.ViewModels.Reports
{
    /// <summary>
    /// Базовая модель представления отчетов
    /// </summary>
    public abstract class BaseReportViewModel : UniqueLayoutDocumentViewModel
    {
        private StiReport _report;
        private ICommand _prepareReportCommand;

        protected BaseReportViewModel(LayoutDocument layout, Type viewType, Guid id)
            : base(layout, viewType, id)
        {
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

        public abstract string ReportName { get; }

        public ICommand PrepareReportCommand
        {
            get { return _prepareReportCommand ?? (_prepareReportCommand = new RelayCommand(PrepareReport)); }
        }

        protected abstract void PrepareReport();

    }
}