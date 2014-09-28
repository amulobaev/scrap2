using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zlatmet2.Domain.Entities.Documents
{
    [Table("DocumentRemainsItems")]
    public class RemainsItemEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid DocumentId { get; set; }

        public int Number { get; set; }

        public Guid? NomenclatureId { get; set; }

        public double Weight { get; set; }

        public virtual RemainsEntity Document { get; set; }
    }
}