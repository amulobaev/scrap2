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

        private ICommand _showReportWarehouseCommand;
        private ICommand _showReportNomenclatureCommand;
        private ICommand _showReportSupplierCommand;
        private ICommand _showReportCustomerCommand;
        private ICommand _showTemplatesCommand;

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
            ShowReferenceSuppliersCommand = new RelayCommand(ShowReferenceSuppliers);
            ShowReferenceCustomersCommand = new RelayCommand(ShowReferenceCustomers);
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

        public ICommand ShowReferenceBasesCommand { get; private set; }

        public ICommand ShowReferenceSuppliersCommand { get; private set; }

        public ICommand ShowReferenceCustomersCommand { get; private set; }

        public ICommand ShowReferenceResponsiblePersonsCommand { get; private set; }

        public ICommand ShowReferenceTransportsCommand { get; private set; }

        public ICommand ShowReferenceDriversCommand { get; private set; }

        public ICommand ShowJournalCommand { get; private set; }

        public ICommand ShowDocumentTransportationCommand { get; private set; }

        public ICommand ShowDocumentProcessingCommand { get; private set; }

        public ICommand ShowDocumentRestsCommand { get; private set; }

        public ICommand ShowParametersCommand { get; private set; }

        public ICommand ShowReportWarehouseCommand
        {
            get
            {
                return _showReportWarehouseCommand ??
                       (_showReportWarehouseCommand = new RelayCommand(ShowReportWarehouse));
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

        public ICommand ShowReportSupplierCommand
        {
            get
            {
                return _showReportSupplierCommand ??
                       (_showReportSupplierCommand = new RelayCommand(ShowReportSupplier));
            }
        }

        public ICommand ShowReportCustomerCommand
        {
            get
            {
                return _showReportCustomerCommand ??
                       (_showReportCustomerCommand = new RelayCommand(ShowReportCustomer));
            }
        }

        public ICommand ShowTemplatesCommand
        {
            get { return _showTemplatesCommand ?? (_showTemplatesCommand = new RelayCommand(ShowTemplates)); }
        }

        #endregion

        #region Методы

        /// <summary>
        /// Показать вкладку с нужным типом модели представления
        /// </summary>
        /// <param name="viewModelType">Тип модели представления</param>
        /// <param name="id"></param>
        public void ShowLayoutDocument(Type viewModelType, Guid? id = null)
        {
            if (viewModelType == null)
                throw new ArgumentNullException("viewModelType");

            if (typeof(SingletonLayoutDocumentViewModel).IsAssignableFrom(viewModelType))
            {
                LayoutContent layout =
                    _documents.FirstOrDefault(
                        x =>
                            x.Content is FrameworkElement &&
                            (x.Content as FrameworkElement).DataContext is SingletonLayoutDocumentViewModel &&
                            (x.Content as FrameworkElement).DataContext.GetType() == viewModelType);
                if (layout == null)
                {
                    layout = new LayoutDocument();
                    LayoutContentViewModel viewModel =
                        Activator.CreateInstance(viewModelType, layout) as LayoutContentViewModel;
                    if (viewModel == null)
                        throw new InvalidOperationException("viewModel is null");
                    FrameworkElement view = Activator.CreateInstance(viewModel.ViewType) as FrameworkElement;
                    if (view == null)
                        throw new InvalidOperationException("view is null");
                    view.DataContext = viewModel;
                    layout.Content = view;
                    _documents.Add(layout);
                }

                layout.IsSelected = true;
            }
            else if (typeof(UniqueLayoutDocumentViewModel).IsAssignableFrom(viewModelType) && id.HasValue)
            {
                var layout =
                    _documents.FirstOrDefault(
                        x =>
                            x.Content is FrameworkElement &&
                            (x.Content as FrameworkElement).DataContext is UniqueLayoutDocumentViewModel &&
                            (x.Content as FrameworkElement).DataContext.GetType() == viewModelType &&
                            ((x.Content as FrameworkElement).DataContext as UniqueLayoutDocumentViewModel).Id ==
                            id);
                if (layout == null)
                {
                    layout = new LayoutDocument();
                    LayoutContentViewModel viewModel =
                        Activator.CreateInstance(viewModelType, layout, id.Value) as LayoutContentViewModel;
                    if (viewModel == null)
                        throw new InvalidOperationException("viewModel is null");
                    FrameworkElement view = Activator.CreateInstance(viewModel.ViewType) as FrameworkElement;
                    if (view == null)
                        throw new InvalidOperationException("view is null");
                    view.DataContext = viewModel;
                    layout.Content = view;
                    _documents.Add(layout);
                }

                layout.IsSelected = true;
            }
            else
            {
                throw new ArgumentException("Переданы некорректные аргументы");
            }
        }

        private void ShowReferenceNomenclature()
        {
            ShowLayoutDocument(typeof(ReferenceNomenclaturesViewModel));
        }

        private void ShowReferenceBases()
        {
            ShowLayoutDocument(typeof(ReferenceBasesViewModel));
        }

        private void ShowReferenceSuppliers()
        {
            ShowLayoutDocument(typeof(ReferenceSuppliersViewModel));
        }

        private void ShowReferenceCustomers()
        {
            ShowLayoutDocument(typeof(ReferenceCustomersViewModel));
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

        private void ShowReportWarehouse()
        {
            ShowLayoutDocument(typeof(ReportWarehouseViewModel), Guid.Empty);
        }

        private void ShowReportNomenclature()
        {
            ShowLayoutDocument(typeof(ReportNomenclatureViewModel), Guid.Empty);
        }

        private void ShowReportSupplier()
        {
            ShowLayoutDocument(typeof(ReportSupplierViewModel), Guid.Empty);
        }

        private void ShowReportCustomer()
        {
            ShowLayoutDocument(typeof(ReportCustomerViewModel), Guid.Empty);
        }

        private void ShowParameters()
        {
            var parametersWindow = new ParametersWindow { Owner = MainWindow.Instance };
            parametersWindow.ShowDialog();
        }

        private void ShowTemplates()
        {
            ShowLayoutDocument(typeof(TemplatesViewModel));
        }

        #endregion

    }
}