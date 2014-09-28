using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zlatmet2.Domain.Entities.References
{
    [Table("ReferenceOrganizations")]
    internal class OrganizationEntity
    {
        [Key]
        public Guid Id { get; set; }

        public int Type { get; set; }

        [Required]
        public string Name { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Inn { get; set; }

        public string Bik { get; set; }

        public string Bank { get; set; }

        public string Contract { get; set; }

        public virtual ICollection<DivisionEntity> Divisions { get; set; }
    }
}