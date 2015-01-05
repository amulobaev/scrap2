using System;
using Scrap.Domain.Dto;

namespace Scrap.ViewModels.Service.Dto
{
    [Table("employees")]
    class OldEmployeeDto : BaseDto
    {
        public Guid employee_id { get; set; }

        public int type { get; set; }
        
        public string name { get; set; }

        public string name_full { get; set; }
        
        public string phone { get; set; }
    }
}
