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
using Dapper;
using GalaSoft.MvvmLight.Command;
using Zlatmet2.Core.Enums;
using Zlatmet2.Domain;
using Zlatmet2.Domain.Dto.References;
using Zlatmet2.Domain.Dto.Service;
using Zlatmet2.Domain.Tools;
using Zlatmet2.Tools;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.ViewModels.Service.Dto;

namespace Zlatmet2.ViewModels.Service
{
    public class ImportDataViewModel : ValidationViewModelBase
    {
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

        /// <summary>
        /// Конструктор
        /// </summary>
        public ImportDataViewModel(string password)
        {
            _password = password;
            GetDataSources();
        }

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
            List<UserDto> users = new List<UserDto>();
            foreach (var oldUser in oldUsers)
            {
                users.Add(new UserDto
                {
                    Id = oldUser.user_id,
                    Login = oldUser.login,
                    Password = oldUser.password
                });
            }

            // Номенклатура
            List<NomenclatureDto> nomenclatures = new List<NomenclatureDto>();
            foreach (var oldNomenclature in oldNomenclatures)
            {
                nomenclatures.Add(new NomenclatureDto
                {
                    Id = oldNomenclature.nomenclature_id,
                    Name = oldNomenclature.name
                });
            }

            // Базы и организации
            List<OrganizationDto> organizations = new List<OrganizationDto>();

            organizations.Add(new OrganizationDto
            {
                Id = Guid.Parse("CA761232-ED42-11CE-BACD-00AA0057B223"),
                Type = 2,
                Name = "Основная база"
            });

            foreach (var oldOrganization in oldOrganizations)
            {
                OrganizationDto organization = new OrganizationDto
                {
                    Id = oldOrganization.organization_id,
                    Type = oldOrganization.type,
                    Name = oldOrganization.name,
                    FullName = oldOrganization.name_full,
                    Address = oldOrganization.address,
                    Phone = oldOrganization.phone,
                    Inn = oldOrganization.inn,
                    Bik = oldOrganization.bik,
                    Bank = oldOrganization.bank,
                    Contract = oldOrganization.contract
                };

                organization.Divisions.Add(new DivisionDto
                {
                    Id = Guid.NewGuid(),
                    OrganizationId = organization.Id,
                    Number = 1,
                    Name = "Основное"
                });

                organizations.Add(organization);
            }

            // Сотрудники
            List<EmployeeDto> employees = new List<EmployeeDto>();
            foreach (var oldEmployee in oldEmployees)
            {
                employees.Add(new EmployeeDto
                {
                    Id = oldEmployee.employee_id,
                    Type = oldEmployee.type,
                    Name = oldEmployee.name,
                    FullName = oldEmployee.name_full,
                    Phone = oldEmployee.phone
                });
            }

            // Транспорт
            List<TransportDto> transports = new List<TransportDto>();
            foreach (OldTransportDto oldTransport in oldTransports)
            {
                transports.Add(new TransportDto
                {
                    Id = oldTransport.transport_id,
                    Name = oldTransport.name,
                    Number = oldTransport.transport_number,
                    Tara = oldTransport.tara,
                    DriverId = oldTransport.driver_id
                });
            }

            Log("Закончено преобразование данных");

            //
            // Запись в новую базу
            //

            Log("Начата запись данных");

            using (SqlConnection connection = new SqlConnection(MainStorage.Instance.ConnectionString))
            {
                // Пользователи
                foreach (var userDto in users)
                {
                    try
                    {
                        connection.Execute(userDto.InsertQuery(), userDto);
                    }
                    catch (Exception ex)
                    {
                        Log("Ошибка при добавлении пользователя");
                        Log(ex.Message);
                    }
                }

                // Номенклатура
                foreach (NomenclatureDto nomenclatureDto in nomenclatures)
                {
                    try
                    {
                        connection.Execute(nomenclatureDto.InsertQuery(), nomenclatureDto);
                    }
                    catch (Exception ex)
                    {
                        Log("Ошибка при добавлении номенклатуры");
                        Log(ex.Message);
                    }
                }

                // Базы


                // Организации
                foreach (OrganizationDto organizationDto in organizations)
                {
                    try
                    {
                        connection.Execute(organizationDto.InsertQuery(), organizationDto);

                        if (organizationDto.Divisions.Any())
                            foreach (DivisionDto division in organizationDto.Divisions)
                                connection.Execute(division.InsertQuery(), division);
                    }
                    catch (Exception ex)
                    {
                        Log("Ошибка при добавлении организации");
                        Log(ex.Message);
                    }
                }

                // Сотрудники
                foreach (EmployeeDto employeeDto in employees)
                {
                    try
                    {
                        connection.Execute(employeeDto.InsertQuery(), employeeDto);
                    }
                    catch (Exception ex)
                    {
                        Log("Ошибка при добавлении сотрудника");
                        Log(ex.Message);
                    }
                }

                // Транспорт
                foreach (TransportDto transportDto in transports)
                {
                    try
                    {
                        connection.Execute(transportDto.InsertQuery(), transportDto);
                    }
                    catch (Exception ex)
                    {
                        Log("Ошибка при добавлении транспорта");
                        Log(ex.Message);
                    }
                }

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

    }
}
