using System.Windows;

namespace Scrap.Views.Service
{
    /// <summary>
    /// Логика взаимодействия для EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        public EditUserWindow(string login = null)
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(login))
                TextBoxLogin.Text = login;
        }

        internal string Login
        {
            get { return TextBoxLogin.Text; }
        }

        internal string Password
        {
            get { return PasswordBox.Password; }
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text))
            {
                MessageBox.Show("Не указано имя пользователя", MainStorage.AppName, MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            this.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
