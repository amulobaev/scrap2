using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scrap.Domain.Entities
{
    [Table("Users")]
    public class UserEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Login { get; set; }

        public string Password { get; set; }
    }
}
