using System;
using System.Data.Entity;
using Zlatmet2.Core.Tools;
using Zlatmet2.Domain.Entities;

namespace Zlatmet2.Domain
{
    internal class ZlatmetContextInitializer : CreateDatabaseIfNotExists<ZlatmetContext>
    {
        protected override void Seed(ZlatmetContext context)
        {
            UserEntity userEntity = new UserEntity
            {
                Id = Guid.Parse("{C0B709EA-1DC2-41F8-83AF-380D3AA32019}"),
                Login = "Пользователь",
                Password = Helpers.Sha1Pass("123")
            };
            context.Users.Add(userEntity);
            context.SaveChanges();
        }
    }
}