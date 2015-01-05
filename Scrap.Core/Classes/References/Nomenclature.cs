using System;

namespace Scrap.Core.Classes.References
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

        public string Unit { get; set; }
    }
}