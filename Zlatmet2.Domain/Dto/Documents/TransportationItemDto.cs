using System;

namespace Zlatmet2.Domain.Dto.Documents
{
    [Table("DocumentTransportationItems")]
    public class TransportationItemDto : BaseDto
    {
        public Guid DocumentId { get; set; }

        public int Number { get; set; }

        public Guid? LoadingNomenclatureId { get; set; }

        public double LoadingWeight { get; set; }

        public Guid? UnloadingNomenclatureId { get; set; }

        public double UnloadingWeight { get; set; }

        public double Netto { get; set; }

        public double Garbage { get; set; }

        public decimal Price { get; set; }
    }
}