using System;
using Zlatmet2.Core.Enums;

namespace Zlatmet2.Core.Classes.Documents
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
