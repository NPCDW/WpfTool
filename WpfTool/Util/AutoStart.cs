using System;
using System.Windows;

namespace WpfTool.Util;

internal static class AutoStart
{
    private const string RegeditAutoStartDir = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    private const string RegeditAutoStartKey = "WpfTool";

    public static bool GetStatus()
    {
        var value = RegeditUtil.GetValue(RegeditAutoStartDir, RegeditAutoStartKey);
        if (value == null) return false;
        if (!value.Equals(Environment.ProcessPath))
            RegeditUtil.SetValue(RegeditAutoStartDir, RegeditAutoStartKey, Environment.ProcessPath!);
        return true;
    }

    public static void Enable()
    {
        var result = RegeditUtil.SetValue(RegeditAutoStartDir, RegeditAutoStartKey, Environment.ProcessPath!);
        if (!result) MessageBox.Show(MainWindow.MainWindowInst!.FindResource("AutoStart_SetFail") as string);
    }

    public static void Disable()
    {
        var result = RegeditUtil.DeleteValue(RegeditAutoStartDir, RegeditAutoStartKey);
        if (!result) MessageBox.Show(MainWindow.MainWindowInst!.FindResource("AutoStart_SetFail") as string);
    }
}