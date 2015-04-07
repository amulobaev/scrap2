using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Scrap.ViewModels.Base;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;

namespace Scrap.ViewModels.Reports
{
    /// <summary>
    /// Базовая модель представления отчетов
    /// </summary>
    public abstract class BaseReportViewModel : UniqueLayoutDocumentViewModel
    {
        private StiReport _report = new StiReport();
        private ICommand _prepareReportCommand;

        protected BaseReportViewModel(LayoutDocument layout, Type viewType, Guid id)
            : base(layout, viewType, id)
        {
        }

        public StiReport Report
        {
            get { return _report; }
            set { Set(() => Report, ref _report, value); }
        }

        public abstract string ReportName { get; }

        public ICommand PrepareReportCommand
        {
            get { return _prepareReportCommand ?? (_prepareReportCommand = new RelayCommand(PrepareReport)); }
        }

        protected abstract void PrepareReport();
    }
}