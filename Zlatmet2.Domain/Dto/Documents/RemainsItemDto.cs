using System;

namespace Zlatmet2.Domain.Dto.Documents
{
    [Table("DocumentRemainsItems")]
    public class RemainsItemDto : BaseDto
    {
        public Guid DocumentId { get; set; }

        public int Number { get; set; }

        public Guid? NomenclatureId { get; set; }

        public double Weight { get; set; }
    }
}