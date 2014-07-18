using System;

namespace Zlatmet2.Core.Classes.Documents
{
    public sealed class RemainsItem : BaseDocumentItem
    {
        /// <summary>
        /// Документ "Корректировка остатков", табличная часть
        /// </summary>
        /// <param name="id"></param>
        public RemainsItem(Guid id)
            : base(id)
        {
        }

        /// <summary>
        /// Номенклатура
        /// </summary>
        public Guid NomenclatureId { get; set; }

        /// <summary>
        /// Масса, т (+/-)
        /// </summary>
        public double Weight { get; set; }

    }
}
