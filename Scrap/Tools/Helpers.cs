using System;

namespace Scrap.Tools
{
    public static class Helpers
    {
        public static DateTime GetFirstDateOfWeek(DateTime dayInWeek, DayOfWeek firstDay)
        {
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            return firstDayInWeek;
        }

        public static DateTime GetFirstDateOfQuarter(DateTime date)
        {
            int currQuarter = (date.Month - 1) / 3 + 1;
            return new DateTime(date.Year, 3 * currQuarter - 2, 1);
        }

    }
}