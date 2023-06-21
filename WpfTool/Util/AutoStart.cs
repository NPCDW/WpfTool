using System;
using System.Windows;

namespace WpfTool
{
    internal class AutoStart
    {
        private static String REGEDIT_AUTO_START_DIR = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private static String REGEDIT_AUTO_START_KEY = "WpfTool";

        public static bool GetStatus()
        {
            String value = RegeditUtil.GetValue(REGEDIT_AUTO_START_DIR, REGEDIT_AUTO_START_KEY);
            if (value == null)
            {
                return false;
            }
            if (!value.Equals(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName))
            {
                RegeditUtil.SetValue(REGEDIT_AUTO_START_DIR, REGEDIT_AUTO_START_KEY, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            }
            return true;
        }

        public static void Enable()
        {
            bool result = RegeditUtil.SetValue(REGEDIT_AUTO_START_DIR, REGEDIT_AUTO_START_KEY, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (!result)
            {
                MessageBox.Show(WpfTool.MainWindow.mainWindow.FindResource("AutoStart_SetFail") as String);
            }
        }

        public static void Disable()
        {
            bool result = RegeditUtil.DeleteValue(REGEDIT_AUTO_START_DIR, REGEDIT_AUTO_START_KEY);
            if (!result)
            {
                MessageBox.Show(WpfTool.MainWindow.mainWindow.FindResource("AutoStart_SetFail") as String);
            }
        }

    }
}
