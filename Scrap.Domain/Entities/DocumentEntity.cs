using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Scrap.Core.Enums;

namespace Scrap.Domain.Entities
{
    [Table("Documents")]
    public class DocumentEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DocumentType Type { get; set; }

        public int Number { get; set; }

        public DateTime Date { get; set; }

        public string Supplier { get; set; }

        public string Customer { get; set; }

        public string ResponsiblePerson { get; set; }

        public string Psa { get; set; }

        public string Nomenclature { get; set; }

        public double? Netto { get; set; }

        public string Comment { get; set; }
    }
}