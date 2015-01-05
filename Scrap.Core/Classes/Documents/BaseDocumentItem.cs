using System;

namespace Scrap.Core.Classes.Documents
{
    public abstract class BaseDocumentItem : PersistentObject
    {
        protected BaseDocumentItem(Guid id)
            : base(id)
        {
        }

        /// <summary>
        /// Порядковый номер пункта
        /// </summary>
        public int Number { get; set; }
    }
}