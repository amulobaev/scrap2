using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zlatmet2.Domain.Entities.References
{
    [Table("ReferenceEmployees")]
    internal class EmployeeEntity
    {
        [Key]
        public Guid Id { get; set; }

        public int Type { get; set; }

        [Required]
        public string Name { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }
    }
}