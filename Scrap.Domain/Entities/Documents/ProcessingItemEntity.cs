using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scrap.Domain.Entities.Documents
{
    [Table("DocumentProcessingItems")]
    public class ProcessingItemEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid DocumentId { get; set; }

        public int Number { get; set; }

        public Guid? InputNomenclatureId { get; set; }

        public double InputWeight { get; set; }

        public Guid? OutputNomenclatureId { get; set; }

        public double OutputWeight { get; set; }

        public virtual ProcessingEntity Document { get; set; }
    }
}