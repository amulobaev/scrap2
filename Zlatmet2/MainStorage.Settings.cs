using System;
using Zlatmet2.Properties;

namespace Zlatmet2
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
        public DateTime JournalPeriodFrom { get; set; }

        /// <summary>
        /// До
        /// </summary>
        public DateTime JournalPeriodTo { get; set; }

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
            JournalPeriodFrom = Settings.Default.JournalPeriodFrom;
            JournalPeriodTo = Settings.Default.JournalPeriodTo;
        }

        public void SaveSettings()
        {
            Settings.Default.MainWindowWidth = MainWindowWidth;
            Settings.Default.MainWindowHeight = MainWindowHeight;
            Settings.Default.MainWindowState = MainWindowState;

            Settings.Default.ShowJournal = ShowJournal;
            Settings.Default.JournalPeriodType = JournalPeriodType;
            Settings.Default.JournalPeriodFrom = JournalPeriodFrom;
            Settings.Default.JournalPeriodTo = JournalPeriodTo;

            Settings.Default.Save();
        }
    }
}
