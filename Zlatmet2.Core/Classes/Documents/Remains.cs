using System;
using System.Collections.Generic;

namespace Zlatmet2.Core.Classes.Documents
{
    /// <summary>
    /// Документ "Корректировка остатков"
    /// </summary>
    public sealed class Remains : BaseDocument
    {
        private List<RemainsItem> _items;

        public Remains(Guid id)
            : base(id)
        {
        }

        public List<RemainsItem> Items
        {
            get { return _items ?? (_items = new List<RemainsItem>()); }
        }

    }
}
