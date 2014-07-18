using System;

namespace Zlatmet2.Core.Classes.Service
{
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
