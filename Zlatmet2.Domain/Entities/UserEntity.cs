using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zlatmet2.Domain.Entities
{
    [Table("Users")]
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
