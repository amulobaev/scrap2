using System;
using Scrap.Core.Enums;

namespace Scrap.Core.Classes.Documents
{
    /// <summary>
    /// Базовый класс для документов
    /// </summary>
    public abstract class BaseDocument : PersistentObject
    {
        //protected BaseDocument(IModelContext context, Guid id)
        protected BaseDocument(Guid id)
            : base(id)
        {
        }

        public Guid? UserId { get; set; }

        public DocumentType Type { get; set; }
        
        public int Number { get; set; }

        public DateTime Date { get; set; }
    }
}
