using System;

namespace Scrap.Core.Classes
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