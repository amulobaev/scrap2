using System.Windows;
using Zlatmet2.ViewModels.Service;

namespace Zlatmet2.Views.Service
{
    /// <summary>
    /// Логика взаимодействия для ImportDataWindow.xaml
    /// </summary>
    public partial class ImportDataWindow : Window
    {
        public ImportDataWindow()
        {
            InitializeComponent();

            this.DataContext = new ImportDataViewModel(PasswordBox.Password);
        }
    }
}
