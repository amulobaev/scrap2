using System;

namespace Zlatmet2.Domain.Dto.References
{
    [Table("ReferenceDivisions")]
    public class DivisionDto : BaseDto
    {
        public Guid OrganizationId { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }
    }
}