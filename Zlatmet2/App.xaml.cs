using System.Windows;
using Telerik.Windows.Controls;

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

            // Создание главного окна
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
