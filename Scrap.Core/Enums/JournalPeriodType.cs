using System.ComponentModel;

namespace Scrap.Core.Enums
{
    /// <summary>
    /// Тип периода в журнале
    /// </summary>
    public enum JournalPeriodType
    {
        [Description("Без ограничений")]
        Default = 0,

        [Description("Произвольный")]
        Arbitary,

        [Description("Сегодня")]
        Today,

        [Description("Последние 3 дня")]
        Last3Days,

        [Description("Последние 7 дней")]
        Last7Days,

        [Description("Эта неделя")]
        ThisWeek,

        [Description("Этот месяц")]
        ThisMonth,

        [Description("Этот квартал")]
        ThisQuarter,

        [Description("С начала года")]
        ThisYear
    }
}