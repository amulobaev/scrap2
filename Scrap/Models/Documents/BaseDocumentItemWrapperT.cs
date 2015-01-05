using Scrap.Core;

namespace Scrap.Models.Documents
{
    public abstract class BaseDocumentItemWrapper<T> : BaseDocumentItemWrapper where T : PersistentObject
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dataForContainer"></param>
        protected BaseDocumentItemWrapper(T dataForContainer = null)
            : base(dataForContainer)
        {
        }

        public new T Container
        {
            get { return (T)base.Container; }
            protected set { base.Container = value; }
        }
    }
}