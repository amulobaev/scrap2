using System.Windows;
using Scrap.ViewModels.Service;

namespace Scrap.Views.Service
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
