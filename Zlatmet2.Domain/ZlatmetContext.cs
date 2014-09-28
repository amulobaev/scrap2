using System.Data.Entity;
using Zlatmet2.Domain.Entities;
using Zlatmet2.Domain.Entities.Documents;
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

        public virtual DbSet<DivisionEntity> Divisions { get; set; }

        public virtual DbSet<TransportEntity> Transports { get; set; }

        public virtual DbSet<EmployeeEntity> Employees { get; set; }

        public virtual DbSet<TemplateEntity> Templates { get; set; }

        // Документы

        public virtual DbSet<TransportationEntity> DocumentTransportation { get; set; }

        public virtual DbSet<TransportationItemEntity> DocumentTransportationItems { get; set; }

        public virtual DbSet<ProcessingEntity> DocumentProcessing { get; set; }

        public virtual DbSet<ProcessingItemEntity> DocumentProcessingItems { get; set; }

        public virtual DbSet<RemainsEntity> DocumentRemains { get; set; }

        public virtual DbSet<RemainsItemEntity> DocumentRemainsItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Настройка внешних ключей

            // Организации и подразделения
            modelBuilder.Entity<DivisionEntity>()
                .HasRequired(x => x.Organization)
                .WithMany(x => x.Divisions)
                .HasForeignKey(x => x.OrganizationId)
                .WillCascadeOnDelete();

            // Документ "Перевозка"
            modelBuilder.Entity<TransportationItemEntity>()
                .HasRequired(x => x.Document)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.DocumentId)
                .WillCascadeOnDelete();

            // Документ "Переработка"
            modelBuilder.Entity<ProcessingItemEntity>()
                .HasRequired(x => x.Document)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.DocumentId)
                .WillCascadeOnDelete();

            // Документ "Корректировка остатков"
            modelBuilder.Entity<RemainsItemEntity>()
                .HasRequired(x => x.Document)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.DocumentId)
                .WillCascadeOnDelete();

        }
    }
}