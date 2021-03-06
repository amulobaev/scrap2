﻿using System;
using System.Data.Entity;
using System.IO;
using InteractivePreGeneratedViews;

namespace Scrap.Domain
{
    /// <summary>
    /// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
    /// </summary>
    public static class ModuleInitializer
    {
        /// <summary>
        /// Initializes the module.
        /// </summary>
        public static void Initialize()
        {
            // Папка для хранения временных файлов
            var appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Zlatmet");

            // Если папка не существует, то её нужно создать
            if (!Directory.Exists(appDataFolder))
                Directory.CreateDirectory(appDataFolder);

            Database.SetInitializer(new ZlatmetContextInitializer());

            using (ZlatmetContext context = new ZlatmetContext())
            {
                InteractiveViews
                    .SetViewCacheFactory(
                        context,
                        new FileViewCacheFactory(Path.Combine(appDataFolder, "MyViews.xml")));
            }
        }
    }
}