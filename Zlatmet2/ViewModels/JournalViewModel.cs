﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes.Documents;
using Zlatmet2.Core.Enums;
using Zlatmet2.Tools;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.ViewModels.Documents;
using Zlatmet2.Views;

namespace Zlatmet2.ViewModels
{
    /// <summary>
    /// Модель представления журнала документов
    /// </summary>
    public class JournalViewModel : SingletonLayoutDocumentViewModel
    {
        private ObservableCollection<Document> _items;

        private JournalPeriodType _periodType = JournalPeriodType.ThisWeek;
        private DateTime? _dateFrom;
        private DateTime? _dateTo;

        private Document _selectedItem;

        private ICommand _updateCommand;
        private ICommand _openDocumentCommand;
        private ICommand _deleteCommand;
        private ICommand _documentDoubleClickCommand;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        public JournalViewModel(LayoutDocument layout)
            : base(layout, typeof(JournalView))
        {
            Title = "Журнал документов";
        }

        public ObservableCollection<Document> Items
        {
            get { return _items ?? (_items = new ObservableCollection<Document>()); }
        }

        public Document SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (Equals(value, _selectedItem))
                    return;
                _selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }

        public ICommand UpdateCommand
        {
            get { return _updateCommand ?? (_updateCommand = new RelayCommand(Update)); }
        }

        public JournalPeriodType PeriodType
        {
            get { return _periodType; }
            set
            {
                if (value == _periodType)
                    return;
                _periodType = value;
                RaisePropertyChanged("PeriodType");

                CalcPeriod();
                Update();
            }
        }

        public DateTime? DateFrom
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

        public DateTime? DateTo
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

        public ICommand OpenDocumentCommand
        {
            get { return _openDocumentCommand ?? (_openDocumentCommand = new RelayCommand(OpenDocument)); }
        }

        public ICommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new RelayCommand(DeleteDocument)); }
        }

        public ICommand DocumentDoubleClickCommand
        {
            get
            {
                return _documentDoubleClickCommand ??
                       (_documentDoubleClickCommand = new RelayCommand(OpenDocument));
            }
        }

        private void Update()
        {
            Items.Clear();
            DateTime? dateFrom = DateFrom.HasValue && DateFrom.Value != DateTime.MinValue
                ? DateFrom.Value
                : (DateTime?)null;
            DateTime? dateTo = DateTo.HasValue && DateTo.Value != DateTime.MinValue ? DateTo.Value : (DateTime?)null;
            Items.AddRange(MainStorage.Instance.DocumentsRepository.GetAll(dateFrom, dateTo));
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