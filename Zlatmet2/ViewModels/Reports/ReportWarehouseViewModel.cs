using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes.Documents;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Core.Classes.Reports;
using Zlatmet2.Core.Classes.Service;
using Zlatmet2.Tools;
using Zlatmet2.Views.Reports;

namespace Zlatmet2.ViewModels.Reports
{
    /// <summary>
    /// Модель представления "Остатки на базе"
    /// </summary>
    public class ReportWarehouseViewModel : BaseReportViewModel
    {
        private DateTime _date;

        private readonly Template _template;
        private readonly ObservableCollection<Organization> _selectedBases = new ObservableCollection<Organization>();

        private readonly ObservableCollection<Nomenclature> _selectedNomenclatures =
            new ObservableCollection<Nomenclature>();

        private ICommand _selectAllBasesCommand;

        private ICommand _unselectAllBasesCommand;

        private ICommand _selectAllNomenclaturesCommand;

        private ICommand _unselectAllNomenclaturesCommand;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        /// <param name="optional"></param>
        public ReportWarehouseViewModel(LayoutDocument layout, Guid id, object optional = null)
            : base(layout, typeof(ReportWarehouseView), id)
        {
            Title = "Остатки на базе";

            Id = Guid.NewGuid();

            Date = DateTime.Today;

            _template = MainStorage.Instance.TemplatesRepository.GetByName(ReportName);

            Report = new StiReport();

            SelectedBases.AddRange(Bases);
            SelectedNomenclatures.AddRange(Nomenclatures);
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (value.Equals(_date))
                    return;
                _date = value;
                RaisePropertyChanged("Date");
            }
        }

        public ReadOnlyObservableCollection<Organization> Bases
        {
            get { return MainStorage.Instance.Bases; }
        }

        public ObservableCollection<Organization> SelectedBases
        {
            get { return _selectedBases; }
        }

        public ReadOnlyObservableCollection<Nomenclature> Nomenclatures
        {
            get { return MainStorage.Instance.Nomenclatures; }
        }

        public ObservableCollection<Nomenclature> SelectedNomenclatures
        {
            get { return _selectedNomenclatures; }
        }

        public override string ReportName
        {
            get { return "Остатки на базе"; }
        }

        public ICommand SelectAllBasesCommand
        {
            get { return _selectAllBasesCommand ?? (_selectAllBasesCommand = new RelayCommand(SelectAllBases)); }
        }

        public ICommand UnselectAllBasesCommand
        {
            get { return _unselectAllBasesCommand ?? (_unselectAllBasesCommand = new RelayCommand(UnselectAllBases)); }
        }

        public ICommand SelectAllNomenclaturesCommand
        {
            get
            {
                return _selectAllNomenclaturesCommand ??
                       (_selectAllNomenclaturesCommand = new RelayCommand(SelectAllNomenclatures));
            }
        }

        public ICommand UnselectAllNomenclaturesCommand
        {
            get
            {
                return _unselectAllNomenclaturesCommand ??
                       (_unselectAllNomenclaturesCommand = new RelayCommand(UnselectAllNomenclatures));
            }
        }

        protected override void PrepareReport()
        {
            if (_template == null)
            {
                MessageBox.Show(string.Format("Отсутствует шаблон \"{0}\"", ReportName), MainStorage.AppName,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!SelectedBases.Any())
            {
                MessageBox.Show("Не выбрано ни одной базы", MainStorage.AppName,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //var selectedNomenclatures = Nomenclatures.Where(x => x.IsChecked).ToList();
            //if (!selectedNomenclatures.Any())
            //{
            //    MessageBox.Show("Не выбрана номенклатура", MainStorage.AppName,
            //        MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            //string bases = string.Join(", ", selectedBases.Select(x => x.Text));

            List<ReportRemainsBase> reportData = MainStorage.Instance.ReportsRepository.ReportRemains(Date,
                SelectedBases.ToArray(), SelectedNomenclatures.Select(x => x.Id).ToArray());

            Report = new StiReport();
            Report.Load(_template.Data);

            Report.Dictionary.Variables["ReportDate"].Value = Date.ToShortDateString();

            Report.RegBusinessObject("Bases", reportData);

            Report.Compile();
            Report.Render(false);
        }

        private void SelectAllBases()
        {
            SelectedBases.Clear();
            SelectedBases.AddRange(Bases);
        }

        private void SelectAllNomenclatures()
        {
            SelectedNomenclatures.Clear();
            SelectedNomenclatures.AddRange(Nomenclatures);
        }

        private void UnselectAllBases()
        {
            SelectedBases.Clear();
        }

        private void UnselectAllNomenclatures()
        {
            SelectedNomenclatures.Clear();
        }

    }
}