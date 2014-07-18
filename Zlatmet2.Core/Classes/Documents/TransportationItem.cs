using System;

namespace Zlatmet2.Core.Classes.Documents
{
    /// <summary>
    /// Документ "Перевозка", табличная часть
    /// </summary>
    public sealed class TransportationItem : BaseDocumentItem
    {
        public TransportationItem(Guid id)
            : base(id)
        {
        }

        /// <summary>
        /// Номенклатура погрузки
        /// </summary>
        public Guid LoadingNomenclatureId { get; set; }

        public double LoadingWeight { get; set; }

        /// <summary>
        /// Номенклатура разгрузки
        /// </summary>
        public Guid UnloadingNomenclatureId { get; set; }

        public double UnloadingWeight { get; set; }

        public double Netto { get; set; }

        public double Garbage { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        public decimal Price { get; set; }
    }
}
