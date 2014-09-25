using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zlatmet2.Domain.Entities.References
{
    [Table("ReferenceTransports")]
    internal class TransportEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Number { get; set; }

        public double Tara { get; set; }

        public Guid? DriverId { get; set; }

        [ForeignKey("DriverId")]
        public virtual EmployeeEntity Driver { get; set; }
    }
}