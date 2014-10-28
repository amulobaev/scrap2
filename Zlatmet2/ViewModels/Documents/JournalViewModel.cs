using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes;
using Zlatmet2.Core.Classes.Documents;
using Zlatmet2.Core.Enums;
using Zlatmet2.Tools;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.Views.Documents;

namespace Zlatmet2.ViewModels.Documents
{
    /// <summary>
    /// Модель представления журнала документов
    /// </summary>
    public class JournalViewModel : SingletonLayoutDocumentViewModel
    {
        private readonly ObservableCollectionEx<Document> _items = new ObservableCollectionEx<Document>();

        private JournalPeriodType _periodType;
        private DateTime? _dateFrom;
        private DateTime? _dateTo;

        private Document _selectedItem;

        private ICommand _updateCommand;
        private ICommand _openDocumentCommand;
        private ICommand _deleteDocumentCommand;
        private ICommand _documentDoubleClickCommand;
        private ICommand _duplicateDocumentCommand;
        private ICommand _newDocumentTransportationCommand;
        private ICommand _newDocumentProcessingCommand;
        private ICommand _newDocumentRemainsCommand;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="optional"></param>
        public JournalViewModel(LayoutDocument layout, object optional = null)
            : base(layout, typeof(JournalView))
        {
            Title = "Журнал документов";

            this.PropertyChanged += OnPropertyChanged;

            // Загрузка настроек журнала
            DateFrom = MainStorage.Instance.JournalPeriodFrom != DateTime.MinValue
                ? MainStorage.Instance.JournalPeriodFrom
                : (DateTime?)null;
            DateTo = MainStorage.Instance.JournalPeriodTo != DateTime.MinValue
                ? MainStorage.Instance.JournalPeriodTo
                : (DateTime?)null;
            PeriodType = (JournalPeriodType)MainStorage.Instance.JournalPeriodType;
            RaisePropertyChanged(() => PeriodType);
        }

        public ObservableCollectionEx<Document> Items
        {
            get { return _items; }
        }

        public Document SelectedItem
        {
            get { return _selectedItem; }
            set { Set(() => SelectedItem, ref _selectedItem, value); }
        }

        public ICommand UpdateCommand
        {
            get { return _updateCommand ?? (_updateCommand = new RelayCommand(Update)); }
        }

        public JournalPeriodType PeriodType
        {
            get { return _periodType; }
            set { Set(() => PeriodType, ref _periodType, value); }
        }

        public DateTime? DateFrom
        {
            get { return _dateFrom; }
            set { Set(() => DateFrom, ref _dateFrom, value); }
        }

        public DateTime? DateTo
        {
            get { return _dateTo; }
            set { Set(() => DateTo, ref _dateTo, value); }
        }

        public ICommand NewDocumentTransportationCommand
        {
            get
            {
                return _newDocumentTransportationCommand ??
                       (_newDocumentTransportationCommand = new RelayCommand(NewDocumentTransportation));
            }
        }

        public ICommand NewDocumentProcessingCommand
        {
            get
            {
                return _newDocumentProcessingCommand ??
                       (_newDocumentProcessingCommand = new RelayCommand(NewDocumentProcessing));
            }
        }

        public ICommand NewDocumentRemainsCommand
        {
            get
            {
                return _newDocumentRemainsCommand ?? (_newDocumentRemainsCommand = new RelayCommand(NewDocumentRemains));
            }
        }

        public ICommand OpenDocumentCommand
        {
            get { return _openDocumentCommand ?? (_openDocumentCommand = new RelayCommand(OpenDocument)); }
        }

        public ICommand DuplicateDocumentCommand
        {
            get
            {
                return _duplicateDocumentCommand ?? (_duplicateDocumentCommand = new RelayCommand(DuplicateDocument));
            }
        }

        public ICommand DeleteDocumentCommand
        {
            get { return _deleteDocumentCommand ?? (_deleteDocumentCommand = new RelayCommand(DeleteDocument)); }
        }

        public ICommand DocumentDoubleClickCommand
        {
            get
            {
                return _documentDoubleClickCommand ??
                       (_documentDoubleClickCommand = new RelayCommand(OpenDocument));
            }
        }

        public override void Dispose()
        {
            // Сохранение настроек
            MainStorage.Instance.JournalPeriodType = (int)PeriodType;
            MainStorage.Instance.JournalPeriodFrom = DateFrom.HasValue ? DateFrom.Value : DateTime.MinValue;
            MainStorage.Instance.JournalPeriodTo = DateTo.HasValue ? DateTo.Value : DateTime.MinValue;

            this.PropertyChanged -= OnPropertyChanged;

            base.Dispose();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "PeriodType":
                    CalcPeriod();
                    Update();
                    break;
            }
        }

        private void Update()
        {
            Items.Clear();
            DateTime? dateFrom = DateFrom.HasValue && DateFrom.Value != DateTime.MinValue
                ? DateFrom.Value
                : (DateTime?)null;
            DateTime? dateTo = DateTo.HasValue && DateTo.Value != DateTime.MinValue ? DateTo.Value : (DateTime?)null;
            Items.AddRange(MainStorage.Instance.JournalRepository.GetAll(dateFrom, dateTo));
        }

        private void CalcPeriod()
        {
            switch (PeriodType)
            {
                case JournalPeriodType.Default:
                    DateFrom = null;
                    DateTo = null;
                    break;
                case JournalPeriodType.Arbitary:
                    //DateFrom = null;
                    //DateTo = null;
                    break;
                case JournalPeriodType.Today:
                    DateFrom = DateTime.Today;
                    DateTo = DateTime.Today;
                    break;
                case JournalPeriodType.Last3Days:
                    DateFrom = DateTime.Today.AddDays(-2);
                    DateTo = DateTime.Today;
                    break;
                case JournalPeriodType.Last7Days:
                    DateFrom = DateTime.Today.AddDays(-6);
                    DateTo = DateTime.Today;
                    break;
                case JournalPeriodType.ThisWeek:
                    DateFrom = Helpers.GetFirstDateOfWeek(DateTime.Today, DayOfWeek.Monday);
                    DateTo = DateTime.Today;
                    break;
                case JournalPeriodType.ThisMonth:
                    DateFrom = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    DateTo = DateTime.Today;
                    break;
                case JournalPeriodType.ThisQuarter:
                    DateFrom = Helpers.GetFirstDateOfQuarter(DateTime.Today);
                    DateTo = DateTime.Today;
                    break;
                case JournalPeriodType.ThisYear:
                    DateFrom = new DateTime(DateTime.Today.Year, 1, 1);
                    DateTo = DateTime.Today;
                    break;
            }
        }

        private void NewDocumentTransportation()
        {
            MainViewModel.Instance.ShowLayoutDocument(typeof(DocumentTransportationViewModel), Guid.Empty);
        }

        private void NewDocumentProcessing()
        {
            MainViewModel.Instance.ShowLayoutDocument(typeof(DocumentProcessingViewModel), Guid.Empty);
        }

        private void NewDocumentRemains()
        {
            MainViewModel.Instance.ShowLayoutDocument(typeof(DocumentRemainsViewModel), Guid.Empty);
        }

        private void OpenDocument()
        {
            if (SelectedItem == null)
                return;

            switch (SelectedItem.Type)
            {
                case DocumentType.Transportation:
                case DocumentType.TransportationAuto:
                case DocumentType.TransportationTrain:
                    MainViewModel.Instance.ShowLayoutDocument(typeof(DocumentTransportationViewModel),
                        SelectedItem.Id);
                    break;
                case DocumentType.Processing:
                    MainViewModel.Instance.ShowLayoutDocument(typeof(DocumentProcessingViewModel), SelectedItem.Id);
                    break;
                case DocumentType.Remains:
                    MainViewModel.Instance.ShowLayoutDocument(typeof(DocumentRemainsViewModel), SelectedItem.Id);
                    break;
            }
        }

        /// <summary>
        /// Дублирование документа
        /// </summary>
        private void DuplicateDocument()
        {
            if (SelectedItem == null)
                return;

            switch (SelectedItem.Type)
            {
                case DocumentType.Transportation:
                case DocumentType.TransportationAuto:
                case DocumentType.TransportationTrain:
                    MainViewModel.Instance.ShowLayoutDocument(typeof(DocumentTransportationViewModel), Guid.Empty,
                        SelectedItem.Id);
                    break;
                case DocumentType.Processing:
                    MainViewModel.Instance.ShowLayoutDocument(typeof(DocumentProcessingViewModel), Guid.Empty,
                        SelectedItem.Id);
                    break;
                case DocumentType.Remains:
                    MainViewModel.Instance.ShowLayoutDocument(typeof(DocumentRemainsViewModel), Guid.Empty,
                        SelectedItem.Id);
                    break;
            }
        }

        private void DeleteDocument()
        {
            if (SelectedItem == null)
                return;

            // Выход если не выбран ответ "Да"
            if (MessageBox.Show("Действительно удалить?", MainStorage.AppName, MessageBoxButton.YesNo,
                    MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            bool result = false;

            switch (SelectedItem.Type)
            {
                case DocumentType.Transportation:
                case DocumentType.TransportationAuto:
                case DocumentType.TransportationTrain:
                    result = MainStorage.Instance.TransportationRepository.Delete(SelectedItem.Id);
                    break;
                case DocumentType.Processing:
                    result = MainStorage.Instance.ProcessingRepository.Delete(SelectedItem.Id);
                    break;
                case DocumentType.Remains:
                    result = MainStorage.Instance.RemainsRepository.Delete(SelectedItem.Id);
                    break;
            }

            if (result)
                Items.Remove(SelectedItem);
        }

    }
}