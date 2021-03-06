﻿using System.ComponentModel;

namespace Scrap.Core.Enums
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