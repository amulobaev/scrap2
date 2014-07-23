using System;
using Zlatmet2.Domain.Dto;

namespace Zlatmet2.ViewModels.Service.Dto
{
    [Table("tableitems")]
    class OldTableItemDto : BaseDto
    {
        public Guid tableitem_id { get; set; }
        public Guid doc_id { get; set; }
        public int number { get; set; }
        public Guid nompog_id { get; set; }
        public Guid nomraz_id { get; set; }
        public double massapog { get; set; }
        public double massaraz { get; set; }
        public double netto { get; set; }
        public double garbage { get; set; }
        public double cost { get; set; }
    }
}