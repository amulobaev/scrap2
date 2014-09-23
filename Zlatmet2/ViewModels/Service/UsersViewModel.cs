using System.Collections.Generic;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes;
using Zlatmet2.Models.Service;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.Views.Service;

namespace Zlatmet2.ViewModels.Service
{
    public sealed class UsersViewModel : BaseEditorViewModel<UserWrapper>
    {
        public UsersViewModel(LayoutDocument layout, object optional = null)
            : base(layout, typeof(UsersView))
        {
            Title = "Пользователи";

            IEnumerable<User> users = MainStorage.Instance.UsersRepository.GetAll();
            foreach (User user in users)
                Items.Add(new UserWrapper(user));
        }

        protected override void AddItem()
        {
            UserWrapper userWrapper = new UserWrapper();
            Items.Add(userWrapper);
            SelectedItem = userWrapper;
        }
    }
}
