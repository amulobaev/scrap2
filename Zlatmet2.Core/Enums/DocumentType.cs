using System.ComponentModel;

namespace Zlatmet2.Core.Enums
{
    /// <summary>
    /// Тип документа
    /// </summary>
    public enum DocumentType
    {
        [Description("Перевозка")]
        Transportation = 0,

        [Description("Переработка")]
        Processing,

        [Description("Корректировка остатков")]
        Remains,

        [Description("Перевозка (авто)")]
        TransportationAuto,

        [Description("Перевозка (ж/д)")]
        TransportationTrain
    }
}