using System;
using Scrap.Domain.Dto;

namespace Scrap.ViewModels.Service.Dto
{
    [Table("nomenclature")]
    class OldNomenclatureDto : BaseDto
    {
        public Guid nomenclature_id { get; set; }

        public string name { get; set; }
    }
}
