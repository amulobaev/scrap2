using System;
using System.Collections.Generic;

namespace Scrap.Core.Classes.Documents
{
    /// <summary>
    /// Документ "Переработка"
    /// </summary>
    public sealed class Processing : BaseDocument
    {
        private List<ProcessingItem> _items;

        public Processing(Guid id)
            : base(id)
        {
        }

        public Guid? BaseId { get; set; }

        public Guid? ResponsiblePersonId { get; set; }

        public string Comment { get; set; }

        public List<ProcessingItem> Items
        {
            get { return _items ?? (_items = new List<ProcessingItem>()); }
        }

    }
}
