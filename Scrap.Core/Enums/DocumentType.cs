using System.ComponentModel;

namespace Scrap.Core.Enums
{
    /// <summary>
    /// Тип документа
    /// </summary>
    public enum DocumentType
    {
        [Description("Перевозка")]
        Transportation = 0,

        [Description("Переработка")]
        Processing = 1,

        [Description("Корректировка остатков")]
        Remains = 2,

        [Description("Перевозка (авто)")]
        TransportationAuto = 3,

        [Description("Перевозка (ж/д)")]
        TransportationTrain = 4
    }
}