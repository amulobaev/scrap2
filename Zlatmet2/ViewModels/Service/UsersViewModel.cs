using System;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes;
using Zlatmet2.Core.Tools;
using Zlatmet2.Models.Service;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.Views.Service;

namespace Zlatmet2.ViewModels.Service
{
    public sealed class UsersViewModel : BaseEditorViewModel<UserWrapper>
    {
        private ICommand _editItemCommand;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="optional"></param>
        public UsersViewModel(LayoutDocument layout, object optional = null)
            : base(layout, typeof(UsersView))
        {
            Title = "Пользователи";

            IEnumerable<User> users = MainStorage.Instance.UsersRepository.GetAll();
            foreach (User user in users)
                Items.Add(new UserWrapper(user));
        }

        public ICommand EditItemCommand
        {
            get
            {
                return _editItemCommand ?? (_editItemCommand = new RelayCommand(EditUser));
            }
        }

        protected override void AddItem()
        {
            EditUserWindow editUserWindow = new EditUserWindow { Owner = MainWindow.Instance };
            if (editUserWindow.ShowDialog() == true)
            {
                User user = new User(Guid.NewGuid())
                {
                    Login = editUserWindow.Login,
                    Password = Helpers.Sha1Pass(editUserWindow.Password)
                };
                MainStorage.Instance.UsersRepository.Create(user);

                UserWrapper userWrapper = new UserWrapper(user);
                Items.Add(userWrapper);
                SelectedItem = userWrapper;
            }
        }

        private void EditUser()
        {
            if (SelectedItem == null)
                return;

            EditUserWindow editUserWindow = new EditUserWindow(SelectedItem.Name) { Owner = MainWindow.Instance };
            if (editUserWindow.ShowDialog() == true)
            {
                SelectedItem.Name = editUserWindow.Login;
                if (!string.IsNullOrEmpty(editUserWindow.Password))
                    SelectedItem.Password = Helpers.Sha1Pass(editUserWindow.Password);
                SelectedItem.UpdateContainer();

                MainStorage.Instance.UsersRepository.Update(SelectedItem.Container);

                //User user = new User(SelectedItem.Id) { Login = editUserWindow.Login, Password = editUserWindow.Password };
            }
        }

    }
}
