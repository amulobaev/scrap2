using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes;
using Zlatmet2.Models.Service;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.Views.Service;

namespace Zlatmet2.ViewModels.Service
{
    public sealed class UsersViewModel : BaseEditorViewModel<UserWrapper>
    {
        private ICommand _changePasswordCommand;

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

        public ICommand ChangePasswordCommand
        {
            get
            {
                return _changePasswordCommand ?? (_changePasswordCommand = new RelayCommand(ChangePassword));
            }
        }

        private void ChangePassword()
        {
            EditPasswordWindow editPasswordWindow = new EditPasswordWindow { Owner = MainWindow.Instance };
            if (editPasswordWindow.ShowDialog() == true)
            {

            }
        }

        protected override void AddItem()
        {
            UserWrapper userWrapper = new UserWrapper();
            Items.Add(userWrapper);
            SelectedItem = userWrapper;
        }
    }
}
