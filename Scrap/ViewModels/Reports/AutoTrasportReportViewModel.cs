using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Scrap.Core.Classes.References;
using Scrap.Core.Classes.Service;
using Scrap.Tools;
using Scrap.Views.Reports;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;
using System.Windows;
using System.Collections.Generic;
using Scrap.Core.Classes.Reports;

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

        public override bool IsValid()
        {
            if (!SelectedTransport.Any())
            {
                MessageBox.Show("Не выбран транспорт");
                return false;
            }

            return base.IsValid();
        }

        protected override void PrepareReport()
        {
            if (_template == null)
            {
                MessageBox.Show(string.Format("Отсутствует шаблон \"{0}\"", ReportName), MainStorage.AppName,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValid())
                return;

            Report = new StiReport();
            Report.Load(_template.Data);

            Report.Dictionary.Variables["DateFrom"].Value = DateFrom.ToShortDateString();
            Report.Dictionary.Variables["DateTo"].Value = DateTo.ToShortDateString();

            // Данные отчета
            List<ReportAutoTransportData> reportData = MainStorage.Instance.ReportsRepository.ReportAutoTransport(
                DateFrom, DateTo, SelectedTransport.Select(x => x.Id));
            Report.RegBusinessObject("Data", reportData);

            Report.Compile();
            Report.Render(false);
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