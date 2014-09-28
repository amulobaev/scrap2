using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zlatmet2.Domain.Entities.Documents
{
    [Table("DocumentRemains")]
    public class RemainsEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public int Type { get; set; }

        public int Number { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<RemainsItemEntity> Items { get; set; }
    }
}