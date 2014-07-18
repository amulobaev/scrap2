using System;

namespace Zlatmet2.Core.Classes
{
    public class User
    {
        public Guid UserId { get; set; }
        public int Number { get; set; } // Номер по порядку в списке
        public string Login { get; set; } // ФИО пользователя
        public string Password  { get; set; } //пароль

        public User()
        {
            UserId = Guid.NewGuid();
        }

        public override string ToString()
        {
            return Login;
        }
    }
}