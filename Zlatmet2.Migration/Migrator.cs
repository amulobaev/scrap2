using System;
using System.Diagnostics;
using System.Reflection;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;

namespace Zlatmet2.Migration
{
    /// <summary>
    /// Версионная миграция структуры базы данных
    /// </summary>
    public class Migrator
    {
        readonly string _connectionString;

        public Migrator(string connectionString)
        {
            _connectionString = connectionString;
        }

        private class MigrationOptions : IMigrationProcessorOptions
        {
            public bool PreviewOnly { get; set; }
            public int Timeout { get; set; }
            public string ProviderSwitches { get; set; }
        }

        /// <summary>
        /// Получаем сборку с миграционным файлами, создаем контекст и запускаем сам процесс миграции
        /// </summary>
        /// <param name="runnerAction">Действие запускающее миграцию (Up/Down)</param>
        public void Migrate(Action<IMigrationRunner> runnerAction)
        {
            var options = new MigrationOptions { PreviewOnly = false, Timeout = 0 };
            var factory = new FluentMigrator.Runner.Processors.SqlServer.SqlServer2012ProcessorFactory(); ;
            var assembly = Assembly.GetExecutingAssembly();

            var announcer = new TextWriterAnnouncer(s => Debug.WriteLine(s));
            var migrationContext = new RunnerContext(announcer);
            var processor = factory.Create(_connectionString, announcer, options);
            var runner = new MigrationRunner(assembly, migrationContext, processor);
            runnerAction(runner);
        }
    }
}
