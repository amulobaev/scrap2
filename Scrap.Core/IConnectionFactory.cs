using System.Data;

namespace Scrap.Core
{
    public interface IConnectionFactory
    {
        /// <summary>
        /// Создание соединения с базой данных
        /// </summary>
        /// <returns></returns>
        IDbConnection Create();
    }
}