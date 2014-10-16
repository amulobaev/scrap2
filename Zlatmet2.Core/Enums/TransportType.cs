using System.ComponentModel;

namespace Zlatmet2.Core.Enums
{
    /// <summary>
    /// Тип перевозок
    /// </summary>
    public enum TransportType
    {
        [Description("Авто")]
        Auto = 0,
        [Description("Ж/Д")]
        Train
    }
}