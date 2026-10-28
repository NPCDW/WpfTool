using Microsoft.UI.Xaml;
using wotool.Entity;

namespace wotool;

/// <summary>
///     原 WPF 版本固定使用 Wpf.Ui 的 Dark 主题，WinUI 3 下改为可在设置中选择
/// </summary>
internal static class ThemeService
{
    public const string ThemeDefault = "Default";
    public const string ThemeLight = "Light";
    public const string ThemeDark = "Dark";

    public static void Apply(string? theme)
    {
        GlobalConfig.Common.Theme = theme ?? ThemeDefault;
        ApplyToOpenWindows();
    }

    /// <summary>
    ///     主题作用在根元素上，已打开的窗口一并刷新
    /// </summary>
    public static void ApplyToOpenWindows()
    {
        var theme = ToElementTheme(GlobalConfig.Common.Theme);
        foreach (var window in WindowHelper.GetOpenWindows())
            if (window.Content is FrameworkElement root)
                root.RequestedTheme = theme;
    }

    public static ElementTheme ToElementTheme(string? theme)
    {
        return theme switch
        {
            ThemeLight => ElementTheme.Light,
            ThemeDark => ElementTheme.Dark,
            _ => ElementTheme.Default
        };
    }
}
