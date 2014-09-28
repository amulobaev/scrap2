using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zlatmet2.Domain.Entities.References
{
    [Table("ReferenceNomenclatures")]
    internal class NomenclatureEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
