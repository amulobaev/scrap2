using System;
using Scrap.Domain.Dto;

namespace Scrap.ViewModels.Service.Dto
{
    [Table("transport")]
    class OldTransportDto : BaseDto
    {
        public Guid transport_id { get; set; }

        public string name { get; set; }

        public string transport_number { get; set; }

        public Guid? driver_id { get; set; }

        public double tara { get; set; }
    }
}
