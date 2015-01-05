using System;
using Scrap.Core.Classes;

namespace Scrap.Models.Service
{
    /// <summary>
    /// Обёртка для пользователя
    /// </summary>
    public class UserWrapper : BaseReferenceWrapper<User>
    {
        private string _name;
        private string _password;

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

        public string Password
        {
            get { return _password; }
            set
            {
                if (value == _password)
                    return;
                _password = value;
                RaisePropertyChanged("Password");
            }
        }

        public override void UpdateContainer()
        {
            if (Container == null)
                Container = new User(Id);
            Container.Login = Name;
            Container.Password = Password;
        }

    }
}