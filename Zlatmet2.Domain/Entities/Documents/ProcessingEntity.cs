using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zlatmet2.Domain.Entities.Documents
{
    [Table("DocumentProcessing")]
    public class ProcessingEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public int Type { get; set; }

        public int Number { get; set; }

        public DateTime Date { get; set; }

        public Guid? BaseId { get; set; }

        public Guid? ResponsiblePersonId { get; set; }

        public string Comment { get; set; }

        public virtual ICollection<ProcessingItemEntity> Items { get; set; }
    }
}
