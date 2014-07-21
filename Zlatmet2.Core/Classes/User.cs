using System;

namespace Zlatmet2.Core.Classes
{
    public class User : PersistentObject
    {
        public User(Guid id)
            : base(id)
        {
        }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}