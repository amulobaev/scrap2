﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Scrap.ViewModels.Base;
using Scrap.ViewModels.Documents;
using Scrap.ViewModels.References;
using Scrap.ViewModels.Reports;
using Scrap.ViewModels.Service;
using Scrap.Views.Service;
using Xceed.Wpf.AvalonDock.Layout;

namespace Scrap.ViewModels
{
    public class MainViewModel : ValidationViewModelBase
    {
        #region Поля

        private readonly ObservableCollection<LayoutContent> _documents;

        private ICommand _showReferenceNomenclatureCommand;
        private ICommand _showReferenceContractorsCommand;
        private ICommand _showReferenceResponsiblePersonsCommand;
        private ICommand _showReferenceTransportsCommand;
        private ICommand _showReferenceDriversCommand;

        private ICommand _showReportRemainsCommand;
        private ICommand _showReportNomenclatureCommand;
        private ICommand _showReportTransportCommand;

        private ICommand _showTemplatesCommand;
        private ICommand _importDataCommand;

        private ICommand _showUsersCommand;
        private ICommand _showReferenceBasesCommand;

        private ICommand _showJournalCommand;
        private ICommand _showDocumentTransportationCommand;
        private ICommand _showDocumentProcessingCommand;
        private ICommand _showDocumentRestsCommand;
        private ICommand _showParametersCommand;
        private ICommand _showAutoTransportReportCommand;

        #endregion Поля

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainViewModel()
        {
            Instance = this;

            _documents = MainWindow.Instance.MainDocumentPane.Children;
            Documents = new ReadOnlyObservableCollection<LayoutContent>(_documents);

            if (MainStorage.Instance.ShowJournal)
                ShowJournal();
        }

        #region Свойства

        public static MainViewModel Instance { get; private set; }

        public ReadOnlyObservableCollection<LayoutContent> Documents { get; private set; }

        #endregion

        #region Команды

        public ICommand ShowReferenceNomenclatureCommand
        {
            get
            {
                return _showReferenceNomenclatureCommand ??
                       (_showReferenceNomenclatureCommand = new RelayCommand(ShowReferenceNomenclature));
            }
        }

        public ICommand ShowReferenceContractorsCommand
        {
            get
            {
                return _showReferenceContractorsCommand ??
                       (_showReferenceContractorsCommand = new RelayCommand(ShowReferenceContractors));
            }
        }

        public ICommand ShowReferenceBasesCommand
        {
            get
            {
                return _showReferenceBasesCommand ?? (_showReferenceBasesCommand = new RelayCommand(ShowReferenceBases));
            }
        }

        public ICommand ShowReferenceResponsiblePersonsCommand
        {
            get
            {
                return _showReferenceResponsiblePersonsCommand ??
                       (_showReferenceResponsiblePersonsCommand = new RelayCommand(ShowReferenceResponsiblePersons));
            }
        }

        public ICommand ShowReferenceTransportsCommand
        {
            get
            {
                return _showReferenceTransportsCommand ??
                       (_showReferenceTransportsCommand = new RelayCommand(ShowReferenceTransports));
            }
        }

        public ICommand ShowReferenceDriversCommand
        {
            get
            {
                return _showReferenceDriversCommand ??
                       (_showReferenceDriversCommand = new RelayCommand(ShowReferenceDrivers));
            }
        }

        public ICommand ShowJournalCommand
        {
            get { return _showJournalCommand ?? (_showJournalCommand = new RelayCommand(ShowJournal)); }
        }

        public ICommand ShowDocumentTransportationCommand
        {
            get
            {
                return _showDocumentTransportationCommand ??
                       (_showDocumentTransportationCommand = new RelayCommand(ShowDocumentTransportation));
            }
        }

        public ICommand ShowDocumentProcessingCommand
        {
            get
            {
                return _showDocumentProcessingCommand ??
                       (_showDocumentProcessingCommand = new RelayCommand(ShowDocumentProcessing));
            }
        }

        public ICommand ShowDocumentRestsCommand
        {
            get
            {
                return _showDocumentRestsCommand ?? (_showDocumentRestsCommand = new RelayCommand(ShowDocumentRests));
            }
        }

        public ICommand ShowParametersCommand
        {
            get { return _showParametersCommand ?? (_showParametersCommand = new RelayCommand(ShowParameters)); }
        }

        public ICommand ShowReportRemainsCommand
        {
            get
            {
                return _showReportRemainsCommand ??
                       (_showReportRemainsCommand = new RelayCommand(ShowReportRemains));
            }
        }

        public ICommand ShowReportNomenclatureCommand
        {
            get
            {
                return _showReportNomenclatureCommand ??
                       (_showReportNomenclatureCommand = new RelayCommand(ShowReportNomenclature));
            }
        }

        public ICommand ShowReportTransportationCommand
        {
            get
            {
                return _showReportTransportCommand ??
                       (_showReportTransportCommand = new RelayCommand(ShowReportTransport));
            }
        }

        public ICommand ShowAutoTransportReportCommand
        {
            get
            {
                return _showAutoTransportReportCommand ??
                       (_showAutoTransportReportCommand = new RelayCommand(ShowAutoTransportReport));
            }
        }

        public ICommand ShowTemplatesCommand
        {
            get { return _showTemplatesCommand ?? (_showTemplatesCommand = new RelayCommand(ShowTemplates)); }
        }

        public ICommand ImportDataCommand
        {
            get { return _importDataCommand ?? (_importDataCommand = new RelayCommand(ImportData)); }
        }

        public ICommand ShowUsersCommand
        {
            get { return _showUsersCommand ?? (_showUsersCommand = new RelayCommand(ShowUsers)); }
        }

        private void ShowUsers()
        {
            ShowLayoutDocument(typeof(UsersViewModel));
        }

        #endregion

        #region Методы

        /// <summary>
        /// Показать вкладку с нужным типом модели представления
        /// </summary>
        /// <param name="viewModelType">Тип модели представления</param>
        /// <param name="id">Унакльный идентификатор</param>
        /// <param name="optionalParameter">Опциональные данные</param>
        public void ShowLayoutDocument(Type viewModelType, Guid? id = null, object optionalParameter = null)
        {
            if (viewModelType == null)
                throw new ArgumentNullException("viewModelType");

            LayoutContent layout = null;

            if (viewModelType.IsSubclassOf(typeof(SingletonLayoutDocumentViewModel)))
            {
                layout = _documents.FirstOrDefault(x => x.Content is FrameworkElement &&
                                                        (x.Content as FrameworkElement).DataContext is
                                                            SingletonLayoutDocumentViewModel &&
                                                        (x.Content as FrameworkElement).DataContext.GetType() ==
                                                        viewModelType);
                if (layout == null)
                {
                    layout = new LayoutDocument();

                    LayoutContentViewModel viewModel =
                        Activator.CreateInstance(viewModelType, layout, optionalParameter) as LayoutContentViewModel;
                    if (viewModel == null)
                        throw new InvalidOperationException("viewModel is null");
                    FrameworkElement view = Activator.CreateInstance(viewModel.ViewType) as FrameworkElement;
                    if (view == null)
                        throw new InvalidOperationException("view is null");
                    view.DataContext = viewModel;
                    layout.Content = view;
                    _documents.Add(layout);
                }
            }
            else if (viewModelType.IsSubclassOf(typeof(UniqueLayoutDocumentViewModel)) && id.HasValue)
            {
                layout = _documents.FirstOrDefault(x =>
                    x.Content is FrameworkElement &&
                    (x.Content as FrameworkElement).DataContext is UniqueLayoutDocumentViewModel &&
                    (x.Content as FrameworkElement).DataContext.GetType() == viewModelType &&
                    ((x.Content as FrameworkElement).DataContext as UniqueLayoutDocumentViewModel).Id ==
                    id);
                if (layout == null)
                {
                    layout = new LayoutDocument();

                    LayoutContentViewModel viewModel =
                        Activator.CreateInstance(viewModelType, layout, id, optionalParameter) as LayoutContentViewModel;
                    if (viewModel == null)
                        throw new InvalidOperationException("viewModel is null");
                    FrameworkElement view = Activator.CreateInstance(viewModel.ViewType) as FrameworkElement;
                    if (view == null)
                        throw new InvalidOperationException("view is null");
                    view.DataContext = viewModel;
                    layout.Content = view;
                    _documents.Add(layout);
                }
                else
                {
                    ((layout.Content as FrameworkElement).DataContext as UniqueLayoutDocumentViewModel).OptionalContent
                        = optionalParameter;
                }
            }
            else
                throw new ArgumentException("Переданы некорректные аргументы");

            layout.IsSelected = true;
        }

        /// <summary>
        /// Открыть справочник "Номенклатура"
        /// </summary>
        private void ShowReferenceNomenclature()
        {
            ShowLayoutDocument(typeof(ReferenceNomenclaturesViewModel));
        }

        /// <summary>
        /// Открыть справочник "Базы"
        /// </summary>
        private void ShowReferenceBases()
        {
            ShowLayoutDocument(typeof(ReferenceBasesViewModel));
        }

        /// <summary>
        /// Открыть справочник "Контрагенты"
        /// </summary>
        private void ShowReferenceContractors()
        {
            ShowLayoutDocument(typeof(ReferenceContractorsViewModel));
        }

        /// <summary>
        /// Открыть справочник "Ответственные лица"
        /// </summary>
        private void ShowReferenceResponsiblePersons()
        {
            ShowLayoutDocument(typeof(ReferenceResponsiblePersonsViewModel));
        }

        /// <summary>
        /// Открыть справочник "Транспорт"
        /// </summary>
        private void ShowReferenceTransports()
        {
            ShowLayoutDocument(typeof(ReferenceTransportsViewModel));
        }

        /// <summary>
        /// Открыть справочник "Водители"
        /// </summary>
        private void ShowReferenceDrivers()
        {
            ShowLayoutDocument(typeof(ReferenceDriversViewModel));
        }

        /// <summary>
        /// Открыть журнал документов
        /// </summary>
        private void ShowJournal()
        {
            ShowLayoutDocument(typeof(JournalViewModel));
        }

        /// <summary>
        /// Открыть документ "Перевозка"
        /// </summary>
        private void ShowDocumentTransportation()
        {
            ShowLayoutDocument(typeof(DocumentTransportationViewModel), Guid.Empty);
        }

        /// <summary>
        /// Открыть документ "Переработка"
        /// </summary>
        private void ShowDocumentProcessing()
        {
            ShowLayoutDocument(typeof(DocumentProcessingViewModel), Guid.Empty);
        }

        /// <summary>
        /// Открыть документ "Корректировка остатков"
        /// </summary>
        private void ShowDocumentRests()
        {
            ShowLayoutDocument(typeof(DocumentRemainsViewModel), Guid.Empty);
        }

        /// <summary>
        /// Открыть отчёт "Остатки на базе"
        /// </summary>
        private void ShowReportRemains()
        {
            ShowLayoutDocument(typeof(ReportRemainsViewModel), Guid.Empty);
        }

        /// <summary>
        /// Открыть отчёт "Обороты за период"
        /// </summary>
        private void ShowReportNomenclature()
        {
            ShowLayoutDocument(typeof(ReportNomenclatureViewModel), Guid.Empty);
        }

        /// <summary>
        /// Открыть отчёт "Перевозки"
        /// </summary>
        private void ShowReportTransport()
        {
            ShowLayoutDocument(typeof(ReportTransportationViewModel), Guid.Empty);
        }

        private void ShowAutoTransportReport()
        {
            ShowLayoutDocument(typeof(AutoTransportReportViewModel), Guid.Empty);
        }

        /// <summary>
        /// Открыть диалог параметров
        /// </summary>
        private void ShowParameters()
        {
            ParametersWindow parametersWindow = new ParametersWindow { Owner = MainWindow.Instance };
            parametersWindow.ShowDialog();
        }

        /// <summary>
        /// Открыть диалог "Шаблоны"
        /// </summary>
        private void ShowTemplates()
        {
            ShowLayoutDocument(typeof(TemplatesViewModel));
        }

        /// <summary>
        /// Открыть диалог импорта данных
        /// </summary>
        private void ImportData()
        {
            if (_documents.Any())
            {
                MessageBox.Show("Перез запуском импорта, пожалуйста, закройте все вкладки");
                return;
            }

            ImportDataWindow importDataWindow = new ImportDataWindow { Owner = MainWindow.Instance };
            importDataWindow.ShowDialog();
        }

        /// <summary>
        /// Очистка перед закрытием
        /// </summary>
        public override void Dispose()
        {
            List<IDisposable> viewModelsToDispose =
                _documents.Select(x => x.Content)
                    .OfType<FrameworkElement>()
                    .Select(x => x.DataContext)
                    .OfType<IDisposable>()
                    .ToList();
            foreach (var viewModel in viewModelsToDispose)
                viewModel.Dispose();

            base.Dispose();
        }

        /// <summary>
        /// Обновление журнала
        /// </summary>
        internal void UpdateJournal()
        {
            LayoutContent journalLayout =
                _documents.FirstOrDefault(
                    x => x.Content is FrameworkElement && ((FrameworkElement)x.Content).DataContext is JournalViewModel);
            if (journalLayout == null)
                return;
            JournalViewModel journalViewModel = ((FrameworkElement)journalLayout.Content).DataContext as JournalViewModel;
            if (journalViewModel == null)
                return;
            journalViewModel.Update();
        }

        #endregion

    }
}