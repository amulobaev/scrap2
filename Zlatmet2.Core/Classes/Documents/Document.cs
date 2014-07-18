using System;
using Zlatmet2.Core.Enums;

namespace Zlatmet2.Core.Classes.Documents
{
    public class Document
    {
        public Guid Id { get; set; }

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