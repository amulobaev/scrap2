using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zlatmet2.Migration
{
    /// <summary>
    /// Класс управляет процессом миграции
    /// </summary>
    public class MigrationManager
    {
        /// <summary>
        /// Запуск миграции
        /// </summary>
        /// <param name="connectionString">Строка соединения с базой</param>
        public static void Start(string connectionString)
        {
            var migrator = new Migrator(connectionString);

            migrator.Migrate(runner => runner.MigrateUp());
        }
    }
}
