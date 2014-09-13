using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zlatmet2.Domain.Entities.References
{
    [Table("ReferenceBases")]
    class BaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
