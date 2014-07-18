using System.Data;

namespace Zlatmet2.Core
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