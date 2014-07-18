using System;

namespace Zlatmet2.Domain.Dto.References
{
    [Table("ReferenceTransports")]
    public class TransportDto : BaseDto
    {
        public string Name { get; set; }

        public string Number { get; set; }

        public double Tara { get; set; }

        public Guid? DriverId { get; set; }
    }
}