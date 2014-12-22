using System;
using System.Data.Entity.Migrations;
using System.Linq;
using Zlatmet2.Core.Tools;
using Zlatmet2.Domain.Entities;

namespace Zlatmet2.Domain.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ZlatmetContext>
    {
        private readonly bool _pendingMigrations;

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            var migrator = new DbMigrator(this);
            _pendingMigrations = migrator.GetPendingMigrations().Any();
        }

        protected override void Seed(ZlatmetContext context)
        {
            if (!_pendingMigrations)
                return;

            //
            Guid userId = new Guid("{C0B709EA-1DC2-41F8-83AF-380D3AA32019}");
            if (!context.Users.Any(x => x.Id == userId))
            {
                UserEntity userEntity = new UserEntity
                {
                    Id = userId,
                    Login = "Пользователь",
                    Password = Helpers.Sha1Pass("123")
                };
                context.Users.Add(userEntity);

            }
        }
    }
}
