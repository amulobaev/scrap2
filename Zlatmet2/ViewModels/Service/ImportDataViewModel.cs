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
using Zlatmet2.Tools;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.ViewModels.Service.Dto;

namespace Zlatmet2.ViewModels.Service
{
    public class ImportDataViewModel : BaseValidationViewModel
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
            try
            {
                // Формируем строку подключения
                string connectionString = GetConnectionString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Пользователи
                    List<OldUserDto> oldUsers =
                        connection.Query<OldUserDto>(QueryObject.GetAllQuery(typeof(OldUserDto))).ToList();

                    // Номенклатура
                    List<OldNomenclatureDto> oldNomenclatures =
                        connection.Query<OldNomenclatureDto>(QueryObject.GetAllQuery(typeof(OldNomenclatureDto)))
                            .ToList();

                    // Организации
                    List<OldOrganizationDto> oldOrganizations =
                        connection.Query<OldOrganizationDto>(QueryObject.GetAllQuery(typeof(OldOrganizationDto)))
                            .ToList();

                    // Сотрудники
                    List<OldEmployeeDto> oldEmployees =
                        connection.Query<OldEmployeeDto>(QueryObject.GetAllQuery(typeof(OldEmployeeDto))).ToList();

                    // Транспорт
                    List<OldTransportDto> oldTransports =
                        connection.Query<OldTransportDto>(QueryObject.GetAllQuery(typeof(OldTransportDto))).ToList();

                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Ошибка подключения{0}{1}", Environment.NewLine, ex.Message);
                MessageBox.Show(message, MainStorage.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
