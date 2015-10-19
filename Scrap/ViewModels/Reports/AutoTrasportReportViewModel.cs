using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Scrap.Core.Classes.References;
using Scrap.Core.Classes.Service;
using Scrap.Tools;
using Scrap.Views.Reports;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;

namespace Scrap.ViewModels.Reports
{
    public class AutoTransportReportViewModel : BaseReportViewModel
    {
        private readonly Template _template;
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private ICommand _selectAllCommand;
        private ICommand _unselectAllCommand;
        private readonly ObservableCollection<Transport> _selectedTransport = new ObservableCollection<Transport>();
        private const string _title = "Отчет по транспорту";

        public AutoTransportReportViewModel(LayoutDocument layout, Guid id, object optional = null)
            : base(layout, typeof(AutoTransportReportView), id)
        {
            Title = _title;

            Id = Guid.NewGuid();

            DateFrom = DateTime.Today.AddDays(-7);
            DateTo = DateTime.Today;

            _template = MainStorage.Instance.TemplatesRepository.GetByName(ReportName);

            Report = new StiReport();

            SelectedTransport.AddRange(Transports);
        }

        public override string ReportName
        {
            get { return _title; }
        }

        public ReadOnlyObservableCollection<Transport> Transports
        {
            get { return MainStorage.Instance.Transports; }
        }

        public ObservableCollection<Transport> SelectedTransport
        {
            get { return _selectedTransport; }
        }

        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { Set(() => DateFrom, ref _dateFrom, value); }
        }

        public DateTime DateTo
        {
            get { return _dateTo; }
            set { Set(() => DateTo, ref _dateTo, value); }
        }

        public ICommand SelectAllCommand
        {
            get { return _selectAllCommand ?? (_selectAllCommand = new RelayCommand(SelectAll)); }
        }

        public ICommand UnselectAllCommand
        {
            get { return _unselectAllCommand ?? (_unselectAllCommand = new RelayCommand(UnselectAll)); }
        }

        protected override void PrepareReport()
        {
            throw new NotImplementedException();
        }

        private void SelectAll()
        {
            SelectedTransport.Clear();
            SelectedTransport.AddRange(Transports);
        }

        private void UnselectAll()
        {
            SelectedTransport.Clear();
        }

    }
}