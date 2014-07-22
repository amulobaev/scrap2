using System;
using Zlatmet2.Domain.Dto;

namespace Zlatmet2.ViewModels.Service.Dto
{
    [Table("organizations")]
    class OldOrganizationDto : BaseDto
    {
        public Guid organization_id { get; set; }

        public int type { get; set; }

        public string name { get; set; }

        public string name_full { get; set; }

        public string address { get; set; }

        public string phone { get; set; }

        public string inn { get; set; }

        public string bik { get; set; }

        public string bank { get; set; }

        public string contract { get; set; }
    }
}
