using Scrap.Classes;

namespace Scrap.Models
{
    /// <summary>
    /// Базовый класс для обёрток
    /// </summary>
    public abstract class BaseWrapper : BaseModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dataForContainer"></param>
        protected BaseWrapper(object dataForContainer = null)
        {
            Container = dataForContainer;
        }

        public object Container { get; protected set; }

        public abstract void UpdateContainer();
    }
}
