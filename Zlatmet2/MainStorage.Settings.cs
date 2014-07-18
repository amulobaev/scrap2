using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zlatmet2.Properties;

namespace Zlatmet2
{
    public sealed partial class MainStorage
    {
        public double MainWindowWidth { get; set; }

        public double MainWindowHeight { get; set; }

        public int MainWindowState { get; set; }

        public bool ShowJournal { get; set; }

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
        }

        public void SaveSettings()
        {
            Settings.Default.MainWindowWidth = MainWindowWidth;
            Settings.Default.MainWindowHeight = MainWindowHeight;
            Settings.Default.MainWindowState = MainWindowState;

            Settings.Default.ShowJournal = ShowJournal;

            Settings.Default.Save();
        }
    }
}
