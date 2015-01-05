using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scrap.Domain.Entities.Documents
{
    [Table("DocumentTransportationItems")]
    public class TransportationItemEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid DocumentId { get; set; }

        public int Number { get; set; }

        public Guid? LoadingNomenclatureId { get; set; }

        public double LoadingWeight { get; set; }

        public Guid? UnloadingNomenclatureId { get; set; }

        public double UnloadingWeight { get; set; }

        public double Netto { get; set; }

        public double Garbage { get; set; }

        public decimal Price { get; set; }

        public virtual TransportationEntity Document { get; set; }
    }
}