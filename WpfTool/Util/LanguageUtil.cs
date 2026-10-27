using System.Globalization;
using WpfTool.Entity;

namespace WpfTool.Util;

internal static class LanguageUtil
{
    /// <summary>
    ///     切换应用语言。文案来自附属程序集（Strings/zh-CN、Strings/en-US），
    ///     切换后需重建界面才能生效
    /// </summary>
    public static void SwitchLanguage(string lang)
    {
        var culture = string.IsNullOrWhiteSpace(lang) ? "en-US" : lang.Replace('_', '-');
        var cultureInfo = new CultureInfo(culture);
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
        GlobalConfig.Common.Language = lang;
    }
}
