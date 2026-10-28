using System;

namespace wotool.Util;

public static class AutoStart
{
    private const string RegeditAutoStartDir = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    private const string RegeditAutoStartKey = "wotool";
    private const string LegacyRegeditAutoStartKey = "wotool";

    /// <summary>
    ///     开机自启设置失败时触发，由 UI 层接管提示
    /// </summary>
    public static event Action? Failed;

    public static bool GetStatus()
    {
        var value = RegeditUtil.GetValue(RegeditAutoStartDir, RegeditAutoStartKey);
        if (value == null)
        {
            value = RegeditUtil.GetValue(RegeditAutoStartDir, LegacyRegeditAutoStartKey);
            if (value == null) return false;
            RegeditUtil.SetValue(RegeditAutoStartDir, RegeditAutoStartKey, value);
            RegeditUtil.DeleteValue(RegeditAutoStartDir, LegacyRegeditAutoStartKey);
        }
        if (!value.Equals(Environment.ProcessPath))
            RegeditUtil.SetValue(RegeditAutoStartDir, RegeditAutoStartKey, Environment.ProcessPath!);
        return true;
    }

    public static void Enable()
    {
        if (!RegeditUtil.SetValue(RegeditAutoStartDir, RegeditAutoStartKey, Environment.ProcessPath!)) Failed?.Invoke();
    }

    public static void Disable()
    {
        var deleted = RegeditUtil.DeleteValue(RegeditAutoStartDir, RegeditAutoStartKey);
        deleted |= RegeditUtil.DeleteValue(RegeditAutoStartDir, LegacyRegeditAutoStartKey);
        if (!deleted) Failed?.Invoke();
    }
}
