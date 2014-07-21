using System;
using System.Diagnostics;
using System.Windows;
using Telerik.Windows.Controls;
using Zlatmet2.Views;

namespace Zlatmet2
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            StyleManager.ApplicationTheme = new Office_SilverTheme();

            // Инициализация хранилища
            MainStorage.Instance.Initialize();

            // Логин и пароль
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();

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
