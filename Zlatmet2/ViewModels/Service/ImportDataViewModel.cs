using Dapper;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Zlatmet2.Core.Classes;
using Zlatmet2.Core.Classes.Documents;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Core.Enums;
using Zlatmet2.Domain;
using Zlatmet2.Tools;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.ViewModels.Service.Dto;

namespace Zlatmet2.ViewModels.Service
{
    public class ImportDataViewModel : ValidationViewModelBase
    {
        #region Поля

        private readonly string _password;

        private ObservableCollection<string> _dataSources;

        private string _dataSource;

        private AuthenticationType _authenticationType;
        private ICommand _dropDownOpenedCommand;
        private ObservableCollection<string> _dataBases;
        private string _database;
        private string _userId;
        private ICommand _testCommand;
        private ICommand _importCommand;
        private string _logText = string.Empty;
        private bool _isBusy;
        private string _busyContent = "Импорт данных";

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        public ImportDataViewModel(string password)
        {
            _password = password;
            GetDataSources();
        }

        #region Свойства

        public ObservableCollection<string> DataSources
        {
            get { return _dataSources ?? (_dataSources = new ObservableCollection<string>()); }
        }

        [Required(ErrorMessage = @"Не указано имя сервера")]
        public string DataSource
        {
            get { return _dataSource; }
            set
            {
                if (value == _dataSource)
                    return;
                _dataSource = value;
                RaisePropertyChanged("DataSource");
            }
        }

        public AuthenticationType AuthenticationType
        {
            get { return _authenticationType; }
            set
            {
                if (value == _authenticationType)
                    return;
                _authenticationType = value;
                RaisePropertyChanged("AuthenticationType");
            }
        }

        //[Required(ErrorMessage = @"Не указано имя пользователя")]
        public string UserId
        {
            //get { return AuthenticationType == AuthenticationType.SqlServer ? _userId : " "; }
            get { return _userId; }
            set
            {
                if (value == _userId)
                    return;
                _userId = value;
                RaisePropertyChanged("UserId");
            }
        }

        public ObservableCollection<string> Databases
        {
            get { return _dataBases ?? (_dataBases = new ObservableCollection<string>()); }
        }

        [Required(ErrorMessage = @"Не выбрана база данных")]
        public string Database
        {
            get { return _database; }
            set
            {
                if (value == _database)
                    return;
                _database = value;
                RaisePropertyChanged("Database");
            }
        }

        public string LogText
        {
            get { return _logText; }
            set
            {
                if (value == _logText)
                    return;
                _logText = value;
                RaisePropertyChanged("LogText");
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value.Equals(_isBusy))
                    return;
                _isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public string BusyContent
        {
            get { return _busyContent; }
            set
            {
                if (value == _busyContent)
                    return;
                _busyContent = value;
                RaisePropertyChanged("BusyContent");
            }
        }

        #endregion

        #region Команды

        public ICommand DropDownOpenedCommand
        {
            get
            {
                return _dropDownOpenedCommand ??
                       (_dropDownOpenedCommand =
                           new RelayCommand<PasswordBox>(DropDownOpened, o => !string.IsNullOrEmpty(DataSource)));
            }
        }

        public ICommand TestCommand
        {
            get { return _testCommand ?? (_testCommand = new RelayCommand<PasswordBox>(TestConnection)); }
        }

        public ICommand ImportCommand
        {
            get { return _importCommand ?? (_importCommand = new RelayCommand(Import)); }
        }

        #endregion

        #region Методы

        private void GetDataSources()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (sender, args) =>
            {
                List<string> dataSources = new List<string>();

                DataTable table = SqlDataSourceEnumerator.Instance.GetDataSources();

                foreach (DataRow row in table.Rows)
                {
                    var serverName = row["ServerName"] as string;
                    if (string.IsNullOrEmpty(serverName))
                        continue;

                    string dataSource = serverName;

                    var instanceName = row["InstanceName"] as string;
                    if (!string.IsNullOrEmpty(instanceName))
                        dataSource += "\\" + instanceName;

                    dataSources.Add(dataSource);
                }

                args.Result = dataSources;
            };
            backgroundWorker.RunWorkerCompleted += (sender, args) =>
            {
                var dataSources = args.Result as List<string>;
                if (dataSources == null)
                    return;
                DataSources.AddRange(dataSources);
            };

            backgroundWorker.RunWorkerAsync();
        }

        private string GetConnectionString()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = DataSource
            };

            if (AuthenticationType == AuthenticationType.Windows)
            {
                connectionStringBuilder.IntegratedSecurity = true;
            }
            else if (AuthenticationType == AuthenticationType.SqlServer)
            {
                connectionStringBuilder.UserID = UserId;
                connectionStringBuilder.Password = _password;
            }

            if (!string.IsNullOrEmpty(Database))
                connectionStringBuilder.InitialCatalog = Database;

            return connectionStringBuilder.ConnectionString;
        }

        /// <summary>
        /// Событие при открытии списка баз данных
        /// </summary>
        private void DropDownOpened(PasswordBox passwordBox)
        {
            // Сохраняем выбранную БД
            string database = Database;

            try
            {
                // Формируем строку подключения
                string connectionString = GetConnectionString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    const string query =
                        "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb') ORDER BY name";
                    var databases = connection.Query<string>(query).ToList();

                    Databases.Clear();
                    Databases.AddRange(databases);

                    if (Databases.Any() && !string.IsNullOrEmpty(database))
                        Database = database;
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Ошибка получения списка баз{0}{1}", Environment.NewLine, ex.Message);
                MessageBox.Show(message, MainStorage.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TestConnection(PasswordBox passwordBox)
        {
            try
            {
                // Формируем строку подключения
                string connectionString = GetConnectionString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                    connection.Open();

                MessageBox.Show("Подключение успешно", MainStorage.AppName, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                string message = string.Format("Ошибка подключения{0}{1}", Environment.NewLine, ex.Message);
                MessageBox.Show(message, MainStorage.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Import()
        {
            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += (sender, args) =>
            {
                IsBusy = false;
            };

            IsBusy = true;
            worker.RunWorkerAsync();

        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Формируем строку подключения
            string connectionString = GetConnectionString();

            List<OldUserDto> oldUsers;
            List<OldNomenclatureDto> oldNomenclatures;
            List<OldOrganizationDto> oldOrganizations;
            List<OldEmployeeDto> oldEmployees;
            List<OldTransportDto> oldTransports;
            List<OldDocumentDto> oldDocuments;
            List<OldTableItemDto> oldTableItems;

            //
            // Загрузка данных из старой базы
            //

            Log("Начата загрузка данных");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //
                // Загрузка данных
                //

                // Пользователи
                oldUsers =
                    connection.Query<OldUserDto>(QueryObject.GetAllQuery(typeof(OldUserDto))).ToList();

                // Номенклатура
                oldNomenclatures =
                    connection.Query<OldNomenclatureDto>(QueryObject.GetAllQuery(typeof(OldNomenclatureDto)))
                        .ToList();

                // Организации
                oldOrganizations =
                    connection.Query<OldOrganizationDto>(QueryObject.GetAllQuery(typeof(OldOrganizationDto)))
                        .ToList();

                // Сотрудники
                oldEmployees =
                    connection.Query<OldEmployeeDto>(QueryObject.GetAllQuery(typeof(OldEmployeeDto))).ToList();

                // Транспорт
                oldTransports =
                    connection.Query<OldTransportDto>(QueryObject.GetAllQuery(typeof(OldTransportDto))).ToList();

                // Документы 
                oldDocuments =
                    connection.Query<OldDocumentDto>(QueryObject.GetAllQuery(typeof(OldDocumentDto))).ToList();

                // Табличная часть документов
                oldTableItems =
                    connection.Query<OldTableItemDto>(QueryObject.GetAllQuery(typeof(OldTableItemDto))).ToList();
            }

            Log("Закончена загрузка данных");

            //
            // Преобразование данных
            //

            Log("Начато преобразование данных");

            // Пользователи
            List<User> users = new List<User>();
            foreach (OldUserDto oldUser in oldUsers)
            {
                users.Add(new User(oldUser.user_id)
                {
                    Login = oldUser.login,
                    Password = oldUser.password
                });
            }

            // Номенклатура
            List<Nomenclature> nomenclatures = new List<Nomenclature>();
            foreach (OldNomenclatureDto oldNomenclature in oldNomenclatures)
            {
                nomenclatures.Add(new Nomenclature(oldNomenclature.nomenclature_id)
                {
                    Name = oldNomenclature.name
                });
            }

            // Базы и организации
            List<Organization> organizations = new List<Organization>();

            organizations.Add(new Organization(Guid.Parse("CA761232-ED42-11CE-BACD-00AA0057B223"), OrganizationType.Base)
            {
                Name = "Основная база"
            });

            foreach (OldOrganizationDto oldOrganization in oldOrganizations)
            {
                Organization organization =
                    new Organization(oldOrganization.organization_id, (OrganizationType)oldOrganization.type)
                    {
                        Name = oldOrganization.name,
                        FullName = oldOrganization.name_full,
                        Address = oldOrganization.address,
                        Phone = oldOrganization.phone,
                        Inn = oldOrganization.inn,
                        Bik = oldOrganization.bik,
                        Bank = oldOrganization.bank,
                        Contract = oldOrganization.contract
                    };

                organization.Divisions.Add(new Division(Guid.NewGuid())
                {
                    OrganizationId = organization.Id,
                    Number = 1,
                    Name = "Основное"
                });

                organizations.Add(organization);
            }

            // Сотрудники
            List<Employee> employees = new List<Employee>();
            foreach (OldEmployeeDto oldEmployee in oldEmployees)
            {
                employees.Add(new Employee(oldEmployee.employee_id, (EmployeeType)oldEmployee.type)
                {
                    Name = oldEmployee.name,
                    FullName = oldEmployee.name_full,
                    Phone = oldEmployee.phone
                });
            }

            // Транспорт
            List<Transport> transports = new List<Transport>();
            foreach (OldTransportDto oldTransport in oldTransports)
            {
                transports.Add(new Transport(oldTransport.transport_id)
                {
                    Name = oldTransport.name,
                    Number = oldTransport.transport_number,
                    Tara = oldTransport.tara,
                    DriverId = oldTransport.driver_id
                });
            }

            Log("Закончено преобразование данных");

            // Документы "Перевозка"
            List<Transportation> transportationDocs = new List<Transportation>();
            foreach (OldDocumentDto oldDocument in oldDocuments.Where(x => x.doc_type == 0))
            {
                // Поставщик
                Guid? supplierId = null;
                Guid? supplierDivisionId = null;
                Organization supplier = organizations.FirstOrDefault(x => x.Id == oldDocument.supplier_id);
                if (supplier != null)
                {
                    supplierId = supplier.Id;
                    if (supplier.Type == OrganizationType.Supplier)
                        supplierDivisionId = supplier.Divisions[0].Id;
                }
                else
                    continue;

                // Заказчик
                Guid? customerId = null;
                Guid? customerDivisionId = null;
                Organization customer = organizations.FirstOrDefault(x => x.Id == oldDocument.customer_id);
                if (customer != null)
                {
                    customerId = customer.Id;
                    if (customer.Type == OrganizationType.Customer)
                        customerDivisionId = customer.Divisions[0].Id;
                }
                else
                    continue;

                Transportation transportationDoc = new Transportation(oldDocument.doc_id)
                {
                    UserId = oldDocument.user_id,
                    Type = oldDocument.transport_type == 0
                        ? DocumentType.TransportationAuto
                        : DocumentType.TransportationTrain,
                    Number = oldDocument.number,
                    Date = oldDocument.doc_date,
                    DateOfLoading = oldDocument.datapog,
                    DateOfUnloading = oldDocument.dataraz,
                    SupplierId = supplierId,
                    SupplierDivisionId = supplierDivisionId,
                    CustomerId = customerId,
                    CustomerDivisionId = customerDivisionId,
                    ResponsiblePersonId = oldDocument.responsible_id,
                    TransportId = oldDocument.transport_id,
                    DriverId = oldDocument.driver_id,
                    Wagon = oldDocument.vagon,
                    Psa = oldDocument.psa,
                    Ttn = oldDocument.ttn,
                    Comment = oldDocument.comment
                };

                //

                // Табличная часть
                var tableItems = oldTableItems.Where(x => x.doc_id == oldDocument.doc_id).OrderBy(x => x.number);
                foreach (OldTableItemDto oldTableItemDto in tableItems)
                {
                    transportationDoc.Items.Add(new TransportationItem(oldTableItemDto.tableitem_id)
                    {
                        Number = oldTableItemDto.number,
                        LoadingNomenclatureId = oldTableItemDto.nompog_id,
                        LoadingWeight = oldTableItemDto.massapog,
                        UnloadingNomenclatureId = oldTableItemDto.nomraz_id,
                        UnloadingWeight = oldTableItemDto.massaraz,
                        Netto = oldTableItemDto.netto,
                        Garbage = oldTableItemDto.garbage,
                        Price = (decimal)oldTableItemDto.cost
                    });
                }

                transportationDocs.Add(transportationDoc);
            }

            // Документы "Переработка"
            List<Processing> processingDocs = new List<Processing>();
            foreach (OldDocumentDto oldDocument in oldDocuments.Where(x => x.doc_type == 1))
            {

            }

            // Документы "Корректировка остатков"
            List<Remains> remainsDocs = new List<Remains>();
            foreach (OldDocumentDto oldDocument in oldDocuments.Where(x => x.doc_type == 2))
            {

            }

            //
            // Запись в новую базу
            //

            Log("Начата запись данных");

            using (SqlConnection connection = new SqlConnection(MainStorage.Instance.ConnectionString))
            {
                // Пользователи
                foreach (User user in users)
                {
                    try
                    {
                        MainStorage.Instance.UsersRepository.Create(user);
                    }
                    catch (Exception ex)
                    {
                        Log("Ошибка при добавлении пользователя");
                        Log(ex.Message);
                    }
                }

                // Номенклатура
                foreach (Nomenclature nomenclature in nomenclatures)
                {
                    try
                    {
                        MainStorage.Instance.CreateOrUpdateObject(nomenclature);
                    }
                    catch (Exception ex)
                    {
                        Log("Ошибка при добавлении номенклатуры");
                        Log(ex.Message);
                    }
                }

                // Организации (базы/поставщики/заказчики)
                foreach (Organization organization in organizations)
                {
                    try
                    {
                        MainStorage.Instance.CreateOrUpdateObject(organization);
                    }
                    catch (Exception ex)
                    {
                        Log("Ошибка при добавлении организации");
                        Log(ex.Message);
                    }
                }

                // Сотрудники
                foreach (Employee employee in employees)
                {
                    try
                    {
                        MainStorage.Instance.CreateOrUpdateObject(employee);
                    }
                    catch (Exception ex)
                    {
                        Log("Ошибка при добавлении сотрудника");
                        Log(ex.Message);
                    }
                }

                // Транспорт
                foreach (Transport transport in transports)
                {
                    try
                    {
                        MainStorage.Instance.CreateOrUpdateObject(transport);
                    }
                    catch (Exception ex)
                    {
                        Log("Ошибка при добавлении транспорта");
                        Log(ex.Message);
                    }
                }

                // Документы "Перевозка"
                foreach (Transportation transportation in transportationDocs)
                {
                    try
                    {
                        MainStorage.Instance.TransportationRepository.Create(transportation);
                    }
                    catch (Exception ex)
                    {
                        Log("Ошибка при добавлении документы \"Перевозка\"");
                        Log(ex.Message);
                    }
                }

                // Документы "Переработка"

                // Документы "Корректировка остатков"

            }

            Log("Закончена запись данных");

            try
            {
            }
            catch (Exception ex)
            {
                string message = string.Format("Ошибка подключения{0}{1}", Environment.NewLine, ex.Message);
                MessageBox.Show(message, MainStorage.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Log(string text)
        {
            LogText += string.Format("{0} {1}{2}", DateTime.Now, text, Environment.NewLine);
        }

        #endregion

    }
}
