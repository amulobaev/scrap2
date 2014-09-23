using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zlatmet2.Domain.Entities.References
{
    [Table("ReferenceNomenclatures")]
    internal class NomenclatureEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
