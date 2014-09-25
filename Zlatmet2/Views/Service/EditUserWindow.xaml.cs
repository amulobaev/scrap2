using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Zlatmet2.Views.Service
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
