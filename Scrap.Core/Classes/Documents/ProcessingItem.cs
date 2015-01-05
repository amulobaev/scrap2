using System;

namespace Scrap.Core.Classes.Documents
{
    /// <summary>
    /// Документ "Переработка", табличная часть
    /// </summary>
    public sealed class ProcessingItem : BaseDocumentItem
    {
        public ProcessingItem(Guid id)
            : base(id)
        {
        }

        /// <summary>
        /// Номенклатура на входе
        /// </summary>
        public Guid InputNomenclatureId { get; set; }

        /// <summary>
        /// Масса на входе
        /// </summary>
        public double InputWeight { get; set; }

        /// <summary>
        /// Номенклатура на выходе
        /// </summary>
        public Guid OutputNomenclatureId { get; set; }

        /// <summary>
        /// Масса на выходе
        /// </summary>
        public double OutputWeight { get; set; }
    }
}