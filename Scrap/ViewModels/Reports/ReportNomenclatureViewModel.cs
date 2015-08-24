using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Scrap.Core.Classes.References;
using Scrap.Core.Classes.Reports;
using Scrap.Core.Classes.Service;
using Scrap.Tools;
using Scrap.Views.Reports;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;

namespace Scrap.ViewModels.Reports
{
    /// <summary>
    /// Модель представления отчета "Обороты за период"
    /// </summary>
    public sealed class ReportNomenclatureViewModel : BaseReportViewModel
    {
        private readonly Template _template;

        private DateTime _dateFrom;

        private DateTime _dateTo;

        private bool _isBases = true;

        private bool _isTransit;

        private readonly ObservableCollection<Organization> _selectedBases = new ObservableCollection<Organization>();

        private ICommand _selectAllBasesCommand;

        private ICommand _unselectAllBasesCommand;

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

            SelectedBases.AddRange(Bases);
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

        public ReadOnlyObservableCollection<Organization> Bases
        {
            get { return MainStorage.Instance.Bases; }
        }

        public ObservableCollection<Organization> SelectedBases
        {
            get { return _selectedBases; }
        }

        public bool IsBases
        {
            get { return _isBases; }
            set { Set(() => IsBases, ref _isBases, value); }
        }

        public bool IsTransit
        {
            get { return _isTransit; }
            set { Set(() => IsTransit, ref _isTransit, value); }
        }

        public override string ReportName
        {
            get { return "Обороты за период"; }
        }

        public ICommand SelectAllBasesCommand
        {
            get { return _selectAllBasesCommand ?? (_selectAllBasesCommand = new RelayCommand(SelectAllBases)); }
        }

        public ICommand UnselectAllBasesCommand
        {
            get { return _unselectAllBasesCommand ?? (_unselectAllBasesCommand = new RelayCommand(UnselectAllBases)); }
        }

        public override bool IsValid()
        {
            if (!IsBases && !IsTransit)
            {
                MessageBox.Show("Не выбраны \"Базы\" и/или \"Транзит\"");
                return false;
            }

            if (IsBases && !SelectedBases.Any())
            {
                MessageBox.Show("Не выбрана ни одна база");
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

            // Переменные отчета
            Report.Dictionary.Variables["Bases"].Value = IsBases && SelectedBases.Any()
                ? (String.Format(SelectedBases.Count > 1 ? "складам {0}" : "складу {0}",
                    string.Join(",", SelectedBases.Select(x => x.Name))))
                : "складу";
            Report.Dictionary.Variables["DateFrom"].Value = DateFrom.ToShortDateString();
            Report.Dictionary.Variables["DateTo"].Value = DateTo.ToShortDateString();

            // Данные отчета
            List<ReportNomenclatureData> reportData = MainStorage.Instance.ReportsRepository.ReportNomenclature(
                DateFrom, DateTo, IsBases, SelectedBases.Select(x => x.Id), IsTransit);
            Report.RegBusinessObject("Data", reportData);

            Report.Compile();
            Report.Render(false);
        }

        private void SelectAllBases()
        {
            SelectedBases.Clear();
            SelectedBases.AddRange(Bases);
        }

        private void UnselectAllBases()
        {
            SelectedBases.Clear();
        }

    }
}