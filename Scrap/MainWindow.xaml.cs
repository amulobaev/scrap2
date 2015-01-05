using System;
using System.Windows;
using Scrap.ViewModels;

namespace Scrap
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Instance = this;

            this.Title = string.Format("{0} - {1}", MainStorage.AppName, MainStorage.Instance.UserName);

            // Загрузка размеров окна из настроек
            this.Width = MainStorage.Instance.MainWindowWidth;
            this.Height = MainStorage.Instance.MainWindowHeight;
            this.WindowState = (WindowState)MainStorage.Instance.MainWindowState;

            //
            this.DataContext = new MainViewModel();
        }

        public static MainWindow Instance { get; private set; }

        public MainViewModel ViewModel
        {
            get { return this.DataContext as MainViewModel; }
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ViewModel.Dispose();

            // Сохранение настроек окна
            MainStorage.Instance.MainWindowWidth = this.Width;
            MainStorage.Instance.MainWindowHeight = this.Height;
            MainStorage.Instance.MainWindowState = (int)this.WindowState;

            MainStorage.Instance.Dispose();
        }
    }
}
