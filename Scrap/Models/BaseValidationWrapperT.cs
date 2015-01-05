using Scrap.Core;

namespace Scrap.Models
{
    /// <summary>
    /// Базовый класс для обёрток
    /// </summary>
    public abstract class BaseValidationWrapper<T> : BaseValidationWrapper where T : PersistentObject
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dataForContainer"></param>
        protected BaseValidationWrapper(T dataForContainer = default (T))
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