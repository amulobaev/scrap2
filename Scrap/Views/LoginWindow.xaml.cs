using System.Windows;
using Scrap.ViewModels;

namespace Scrap.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            this.DataContext = new LoginViewModel(this.Close, PasswordBox);
        }
    }
}
