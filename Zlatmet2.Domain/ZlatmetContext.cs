using System.Data.Entity;
using Zlatmet2.Domain.Entities;
using Zlatmet2.Domain.Entities.References;

namespace Zlatmet2.Domain
{
    internal class ZlatmetContext : DbContext
    {
        // Контекст настроен для использования строки подключения "ZlatmetContext" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "Zlatmet2.Domain.ZlatmetContext" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "ZlatmetContext" 
        // в файле конфигурации приложения.
        public ZlatmetContext()
            : base("name=ZlatmetContext")
        {
        }

        // Добавьте DbSet для каждого типа сущности, который требуется включить в модель. Дополнительные сведения 
        // о настройке и использовании модели Code First см. в статье http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<UserEntity> Users { get; set; }

        // Справочники

        public virtual DbSet<NomenclatureEntity> Nomenclatures { get; set; }

        public virtual DbSet<OrganizationEntity> Organizations { get; set; }

        public virtual DbSet<TransportEntity> Transports { get; set; }
    }
}