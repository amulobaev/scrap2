using System;
using Zlatmet2.Core.Classes;
using Zlatmet2.Models.References;

namespace Zlatmet2.Models.Service
{
    /// <summary>
    /// Обёртка для пользователя
    /// </summary>
    public class UserWrapper : BaseReferenceWrapper<User>
    {
        private string _name;

        public UserWrapper(User user = null)
            : base(user)
        {
            if (user == null)
            {
                Id = Guid.NewGuid();
                Name = "НовыйПользователь";
            }
            else
            {
                Id = user.Id;
                _name = user.Login;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                    return;
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public override void UpdateContainer()
        {
            if (Container == null)
                Container = new User(Id);
            Container.Login = Name;
        }

    }
}