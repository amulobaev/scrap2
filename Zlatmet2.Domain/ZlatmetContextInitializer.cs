using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
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

            // Найдем все скрипты
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string[] resourceNames =
                executingAssembly.GetManifestResourceNames()
                    .Where(x => x.Contains("Scripts") && x.EndsWith(".sql"))
                    .OrderBy(x => x)
                    .ToArray();
            foreach (string resourceName in resourceNames)
            {
                string query = GetResourceString(resourceName);
                context.Database.ExecuteSqlCommand(query);
            }
        }

        string GetResourceString(string resourceName)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}