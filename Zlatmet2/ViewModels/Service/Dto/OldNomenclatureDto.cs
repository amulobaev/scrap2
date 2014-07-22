using System;
using Zlatmet2.Domain.Dto;

namespace Zlatmet2.ViewModels.Service.Dto
{
    [Table("nomenclature")]
    class OldNomenclatureDto : BaseDto
    {
        public Guid nomenclature_id { get; set; }

        public string name { get; set; }
    }
}
