using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zlatmet2.Domain.Entities.Documents
{
    [Table("DocumentTransportation")]
    public class TransportationEntity
    {
        [Key]
        public Guid Id { get; set; }

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

        public virtual ICollection<TransportationItemEntity> Items { get; set; }
    }
}