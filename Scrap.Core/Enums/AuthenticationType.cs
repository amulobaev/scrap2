using System.ComponentModel;

namespace Scrap.Core.Enums
{
    /// <summary>
    /// Тип проверки пподлинности для SQL Server
    /// </summary>
    public enum AuthenticationType
    {
        [Description("Проверка подлинности Windows")]
        Windows = 0,
        [Description("Проверка подлинности SQL Server")]
        SqlServer
    }
}