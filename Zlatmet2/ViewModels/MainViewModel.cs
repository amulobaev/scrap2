using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.ViewModels.Documents;
using Zlatmet2.ViewModels.References;
using Zlatmet2.ViewModels.Reports;
using Zlatmet2.ViewModels.Service;
using Zlatmet2.Views.Service;

namespace Zlatmet2.ViewModels
{
    public class MainViewModel
    {
        #region Поля

        private readonly ObservableCollection<LayoutContent> _documents;

        private ICommand _showReportRemainsCommand;
        private ICommand _showReportNomenclatureCommand;
        private ICommand _showReportOrganizationCommand;
        private ICommand _showTemplatesCommand;
        private ICommand _importDataCommand;

        private ICommand _showReferenceContractorsCommand;

        private ICommand _showUsersCommand;

        private ICommand _showReportTransportCommand;

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainViewModel()
        {
            Instance = this;

            _documents = MainWindow.Instance.MainDocumentPane.Children;

            // Инициализация команд
            ShowReferenceNomenclatureCommand = new RelayCommand(ShowReferenceNomenclature);

            ShowReferenceBasesCommand = new RelayCommand(ShowReferenceBases);
            ShowReferenceResponsiblePersonsCommand = new RelayCommand(ShowReferenceResponsiblePersons);
            ShowReferenceTransportsCommand = new RelayCommand(ShowReferenceTransports);
            ShowReferenceDriversCommand = new RelayCommand(ShowReferenceDrivers);

            ShowJournalCommand = new RelayCommand(ShowJournal);
            ShowDocumentTransportationCommand = new RelayCommand(ShowDocumentTransportation);
            ShowDocumentProcessingCommand = new RelayCommand(ShowDocumentProcessing);
            ShowDocumentRestsCommand = new RelayCommand(ShowDocumentRests);

            ShowParametersCommand = new RelayCommand(ShowParameters);

            //
            if (MainStorage.Instance.ShowJournal)
                ShowJournal();
        }

        #region Свойства

        public static MainViewModel Instance { get; private set; }

        #endregion

        #region Команды

        public ICommand ShowReferenceNomenclatureCommand { get; private set; }

        public ICommand ShowReferenceContractorsCommand
        {
            get
            {
                return _showReferenceContractorsCommand ??
                       (_showReferenceContractorsCommand = new RelayCommand(ShowReferenceContractors));
            }
        }

        public ICommand ShowReferenceBasesCommand { get; private set; }

        public ICommand ShowReferenceResponsiblePersonsCommand { get; private set; }

        public ICommand ShowReferenceTransportsCommand { get; private set; }

        public ICommand ShowReferenceDriversCommand { get; private set; }

        public ICommand ShowJournalCommand { get; private set; }

        public ICommand ShowDocumentTransportationCommand { get; private set; }

        public ICommand ShowDocumentProcessingCommand { get; private set; }

        public ICommand ShowDocumentRestsCommand { get; private set; }

        public ICommand ShowParametersCommand { get; private set; }

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

        public ICommand ShowReportOrganizationCommand
        {
            get
            {
                return _showReportOrganizationCommand ??
                       (_showReportOrganizationCommand = new RelayCommand(ShowReportOrganization));
            }
        }

        public ICommand ShowReportTransportCommand
        {
            get
            {
                return _showReportTransportCommand ??
                       (_showReportTransportCommand = new RelayCommand(ShowReportTransport));
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

        private void ShowReferenceNomenclature()
        {
            ShowLayoutDocument(typeof(ReferenceNomenclaturesViewModel));
        }

        private void ShowReferenceBases()
        {
            ShowLayoutDocument(typeof(ReferenceBasesViewModel));
        }

        private void ShowReferenceContractors()
        {
            ShowLayoutDocument(typeof(ReferenceContractorsViewModel));
        }

        private void ShowReferenceResponsiblePersons()
        {
            ShowLayoutDocument(typeof(ReferenceResponsiblePersonsViewModel));
        }

        private void ShowReferenceTransports()
        {
            ShowLayoutDocument(typeof(ReferenceTransportsViewModel));
        }

        private void ShowReferenceDrivers()
        {
            ShowLayoutDocument(typeof(ReferenceDriversViewModel));
        }

        private void ShowJournal()
        {
            ShowLayoutDocument(typeof(JournalViewModel));
        }

        private void ShowDocumentTransportation()
        {
            ShowLayoutDocument(typeof(DocumentTransportationViewModel), Guid.Empty);
        }

        private void ShowDocumentProcessing()
        {
            ShowLayoutDocument(typeof(DocumentProcessingViewModel), Guid.Empty);
        }

        private void ShowDocumentRests()
        {
            ShowLayoutDocument(typeof(DocumentRemainsViewModel), Guid.Empty);
        }

        private void ShowReportRemains()
        {
            ShowLayoutDocument(typeof(ReportWarehouseViewModel), Guid.Empty);
        }

        private void ShowReportNomenclature()
        {
            ShowLayoutDocument(typeof(ReportNomenclatureViewModel), Guid.Empty);
        }

        private void ShowReportOrganization()
        {
            ShowLayoutDocument(typeof(ReportOrganizationViewModel), Guid.Empty);
        }

        private void ShowReportTransport()
        {
            ShowLayoutDocument(typeof(ReportTransportViewModel), Guid.Empty);
        }

        private void ShowParameters()
        {
            ParametersWindow parametersWindow = new ParametersWindow { Owner = MainWindow.Instance };
            parametersWindow.ShowDialog();
        }

        private void ShowTemplates()
        {
            ShowLayoutDocument(typeof(TemplatesViewModel));
        }

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

        #endregion

    }
}