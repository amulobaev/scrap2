using System;

namespace Scrap.Core.Classes.Service
{
    /// <summary>
    /// Шаблон отчёта
    /// </summary>
    public class Template : PersistentObject
    {
        public Template(Guid id)
            : base(id)
        {
        }

        public string Name { get; set; }

        /// <summary>
        /// Данные
        /// </summary>
        public byte[] Data { get; set; }
    }
}
