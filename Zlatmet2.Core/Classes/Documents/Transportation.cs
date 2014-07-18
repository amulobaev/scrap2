using System;
using System.Collections.Generic;

namespace Zlatmet2.Core.Classes.Documents
{
    /// <summary>
    /// Документ "Перевозка"
    /// </summary>
    public sealed class Transportation : BaseDocument
    {
        private List<TransportationItem> _items;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id"></param>
        public Transportation(Guid id)
            : base(id)
        {
        }

        public DateTime DateOfLoading { get; set; }

        public DateTime DateOfUnloading { get; set; }

        public Guid? SupplierId { get; set; }

        public Guid? SupplierDivisionId { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? CustomerDivisionId { get; set; }

        public Guid? ResponsiblePersonId { get; set; }

        public Guid? TransportId { get; set; }

        public Guid? DriverId { get; set; }

        public string Wagon { get; set; }

        public string Psa { get; set; }

        public string Ttn { get; set; }

        public string Comment { get; set; }

        public List<TransportationItem> Items
        {
            get { return _items ?? (_items = new List<TransportationItem>()); }
        }
    }
}