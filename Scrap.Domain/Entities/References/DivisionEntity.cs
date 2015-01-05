using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scrap.Domain.Entities.References
{
    [Table("ReferenceDivisions")]
    internal class DivisionEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }

        public int Number { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual OrganizationEntity Organization { get; set; }
    }
}