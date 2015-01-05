using Scrap.Core;

namespace Scrap.Models
{
    public abstract class BaseReferenceWrapper<T> : BaseReferenceWrapper where T : PersistentObject
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dataForContainer"></param>
        protected BaseReferenceWrapper(T dataForContainer = null)
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
