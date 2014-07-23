using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Zlatmet2.Core.Classes;
using Zlatmet2.Domain.Repositories.Service;
using Zlatmet2.Tools;
using Zlatmet2.ViewModels.Base;

namespace Zlatmet2.ViewModels
{
    public class LoginViewModel : ValidationViewModelBase
    {
        private readonly Action _closeAction;

        private readonly PasswordBox _passwordBox;

        private readonly ObservableCollection<User> _users = new ObservableCollection<User>();

        private User _user;

        private ICommand _okCommand;
        private ICommand _cancelCommand;

        public LoginViewModel(Action closeAction, PasswordBox passwordBox)
        {
            if (closeAction == null)
                throw new ArgumentNullException("closeAction");
            _closeAction = closeAction;

            if (passwordBox == null)
                throw new ArgumentNullException("passwordBox");
            _passwordBox = passwordBox;

            UsersRepository usersRepository = new UsersRepository(MainStorage.Instance);
            Users.AddRange(usersRepository.GetAll().ToList());
        }

        public ObservableCollection<User> Users
        {
            get { return _users; }
        }

        public User User
        {
            get { return _user; }
            set
            {
                if (Equals(value, _user))
                    return;
                _user = value;
                RaisePropertyChanged("User");

                _passwordBox.Password = string.Empty;
            }
        }

        public ICommand OkCommand
        {
            get { return _okCommand ?? (_okCommand = new RelayCommand(Ok)); }
        }

        public ICommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand(Cancel)); }
        }

        private void Ok()
        {
            string encryptedPassword = Core.Tools.Helpers.Sha1Pass(_passwordBox.Password);

            if (User == null)
                return;

            if (Users.Any(x => x.Login == User.Login && x.Password == encryptedPassword))
            {
                MainStorage.Instance.UserId = User.Id;
                MainStorage.Instance.UserName = User.Login;
                _closeAction();
            }
            else
                MessageBox.Show("Пароль набран неверно", MainStorage.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Cancel()
        {
            _closeAction();
        }

    }
}
