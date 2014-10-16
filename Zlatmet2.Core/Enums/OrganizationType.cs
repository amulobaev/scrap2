using System.ComponentModel;

namespace Zlatmet2.Core.Enums
{
    /// <summary>
    /// Тип организации
    /// </summary>
    public enum OrganizationType
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        [Description("Контрагент")]
        Contractor = 0,

        /// <summary>
        /// База
        /// </summary>
        [Description("База")]
        Base
    }
}