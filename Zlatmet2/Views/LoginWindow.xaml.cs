using System.Windows;
using System.Windows.Input;
using Zlatmet2.ViewModels;

namespace Zlatmet2.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            this.DataContext = new LoginViewModel(this.Close);
        }
    }
}
