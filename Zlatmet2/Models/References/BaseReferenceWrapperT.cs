using Zlatmet2.Core;

namespace Zlatmet2.Models.References
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
