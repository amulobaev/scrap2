using System.Collections.Generic;

namespace Zlatmet2.Domain.Dto.References
{
    [Table("ReferenceOrganizations")]
    public class OrganizationDto : BaseDto
    {
        private List<DivisionDto> _divisions;

        public int Type { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Inn { get; set; }

        public string Bik { get; set; }

        public string Bank { get; set; }

        public string Contract { get; set; }

        public List<DivisionDto> Divisions
        {
            get { return _divisions ?? (_divisions = new List<DivisionDto>()); }
        }
    }
}