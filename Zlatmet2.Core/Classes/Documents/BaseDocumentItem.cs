using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zlatmet2.Core.Classes.Documents
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