using System;

namespace Zlatmet2.Domain.Dto.References
{
    [Table("ReferenceEmployees")]
    public class EmployeeDto : BaseDto
    {
        public int Type { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }
    }
}