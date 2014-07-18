using System;

namespace Zlatmet2.Core.Classes.References
{
    /// <summary>
    /// Номенклатура
    /// </summary>
    public class Nomenclature : PersistentObject
    {
        public Nomenclature(Guid id)
            : base(id)
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

    }
}