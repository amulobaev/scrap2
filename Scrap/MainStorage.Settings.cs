using System;
using Scrap.Properties;

namespace Scrap
{
    public sealed partial class MainStorage
    {
        /// <summary>
        /// Ширина главного окна
        /// </summary>
        public double MainWindowWidth { get; set; }

        /// <summary>
        /// Высота главного окна
        /// </summary>
        public double MainWindowHeight { get; set; }

        /// <summary>
        /// Состояние главного окна
        /// </summary>
        public int MainWindowState { get; set; }

        /// <summary>
        /// Показывать журнал при запуске
        /// </summary>
        public bool ShowJournal { get; set; }

        /// <summary>
        /// Период журнала
        /// </summary>
        public int JournalPeriodType { get; set; }

        /// <summary>
        /// От
        /// </summary>
        public DateTime? JournalPeriodFrom { get; set; }

        /// <summary>
        /// До
        /// </summary>
        public DateTime? JournalPeriodTo { get; set; }

        private void LoadSettings()
        {
            if (Settings.Default.UpdateSettings)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpdateSettings = false;
                Settings.Default.Save();
            }

            MainWindowWidth = Settings.Default.MainWindowWidth;
            MainWindowHeight = Settings.Default.MainWindowHeight;
            MainWindowState = Settings.Default.MainWindowState;

            ShowJournal = Settings.Default.ShowJournal;
            JournalPeriodType = Settings.Default.JournalPeriodType;
            DateTime journalPeriod;
            JournalPeriodFrom = DateTime.TryParse(Settings.Default.JournalPeriodFrom, out journalPeriod)
                ? journalPeriod
                : (DateTime?)null;
            JournalPeriodTo = DateTime.TryParse(Settings.Default.JournalPeriodTo, out journalPeriod)
                ? journalPeriod
                : (DateTime?)null;
        }

        public void SaveSettings()
        {
            Settings.Default.MainWindowWidth = MainWindowWidth;
            Settings.Default.MainWindowHeight = MainWindowHeight;
            Settings.Default.MainWindowState = MainWindowState;

            Settings.Default.ShowJournal = ShowJournal;
            Settings.Default.JournalPeriodType = JournalPeriodType;
            Settings.Default.JournalPeriodFrom = JournalPeriodFrom.HasValue
                ? JournalPeriodFrom.Value.Date.ToString()
                : null;
            Settings.Default.JournalPeriodTo = JournalPeriodTo.HasValue ? JournalPeriodTo.Value.Date.ToString() : null;

            Settings.Default.Save();
        }
    }
}
