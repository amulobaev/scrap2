using System;

namespace Zlatmet2.Domain.Dto.Service
{
    [Table("Users")]
    public class UserDto : BaseDto
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }
}