using System;

namespace Zlatmet2.Domain.Dto.References
{
    [Table("ReferenceNomenclatures")]
    public class NomenclatureDto : BaseDto
    {
        public string Name { get; set; }
    }
}