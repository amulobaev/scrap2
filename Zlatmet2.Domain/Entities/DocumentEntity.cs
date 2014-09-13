using System;
using System.ComponentModel.DataAnnotations.Schema;
using Zlatmet2.Core.Enums;

namespace Zlatmet2.Domain.Entities
{
    [Table("Documents")]
    public class DocumentEntity
    {
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

        public double Netto { get; set; }

        public string Comment { get; set; }
    }
}