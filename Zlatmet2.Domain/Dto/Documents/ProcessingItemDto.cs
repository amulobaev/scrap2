using System;

namespace Zlatmet2.Domain.Dto.Documents
{
    [Table("DocumentProcessingItems")]
    public class ProcessingItemDto : BaseDto
    {
        public Guid DocumentId { get; set; }

        public int Number { get; set; }

        public Guid? InputNomenclatureId { get; set; }

        public double InputWeight { get; set; }

        public Guid? OutputNomenclatureId { get; set; }

        public double OutputWeight { get; set; }
    }
}