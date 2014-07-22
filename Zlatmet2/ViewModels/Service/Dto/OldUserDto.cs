using System;
using Zlatmet2.Domain.Dto;

namespace Zlatmet2.ViewModels.Service.Dto
{
    [Table("users")]
    class OldUserDto : BaseDto
    {
        public Guid user_id { get; set; }

        public string login { get; set; }

        public string password { get; set; }
    }
}
