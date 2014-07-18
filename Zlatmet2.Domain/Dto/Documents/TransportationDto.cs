using System;
using System.Collections.Generic;

namespace Zlatmet2.Domain.Dto.Documents
{
    [Table("DocumentTransportation")]
    public class TransportationDto : BaseDto
    {
        private List<TransportationItemDto> _items;

        public Guid? UserId { get; set; }

        public int Type { get; set; }

        public int Number { get; set; }

        public DateTime Date { get; set; }

        public DateTime DateOfLoading { get; set; }

        public DateTime DateOfUnloading { get; set; }

        public Guid? SupplierId { get; set; }

        public Guid? SupplierDivisionId { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? CustomerDivisionId { get; set; }

        public Guid? ResponsiblePersonId { get; set; }

        public Guid? TransportId { get; set; }

        public Guid? DriverId { get; set; }

        public string Psa { get; set; }

        public string Ttn { get; set; }

        public string Comment { get; set; }

        public string Wagon { get; set; }

        public List<TransportationItemDto> Items
        {
            get { return _items ?? (_items = new List<TransportationItemDto>()); }
        }
    }
}