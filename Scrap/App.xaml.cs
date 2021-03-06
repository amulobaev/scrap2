﻿using System;
using System.Configuration;
using System.Windows;
using Scrap.Domain.Repositories.Service;
using Scrap.Views;
using Stimulsoft.Report;
using Telerik.Windows.Controls;

namespace Scrap
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            StyleManager.ApplicationTheme = new Office_SilverTheme();

            StiConfig.LoadLocalization("ru.xml");

            // Инициализация хранилища
            MainStorage.Instance.Initialize();

            // Логин и пароль
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            //
#if DEBUG
            string defaultUserId = ConfigurationManager.AppSettings["DefaultUserId"];
            Guid userId;
            if (defaultUserId != null && !string.IsNullOrEmpty(defaultUserId) &&
                Guid.TryParse(defaultUserId, out userId))
            {
                UsersRepository usersRepository = new UsersRepository(MainStorage.Instance);
                var user = usersRepository.GetById(userId);
                if (user != null)
                {
                    MainStorage.Instance.UserId = user.Id;
                    MainStorage.Instance.UserName = user.Login;
                }
            }
            else
            {

#endif
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
#if DEBUG
            }
#endif

            if (MainStorage.Instance.UserId == Guid.Empty)
            {
                this.Shutdown();
                return;
            }

            // Создание главного окна
            this.ShutdownMode = ShutdownMode.OnLastWindowClose;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
