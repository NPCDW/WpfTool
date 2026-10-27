using System;
using System.Globalization;
using System.Resources;

namespace WpfTool;

/// <summary>
///     多语言文案入口，等价于 WPF 版本的 DynamicResource 查表。
///     文案放在 Strings/zh-CN、Strings/en-US 附属程序集里
/// </summary>
internal static class Strings
{
    private static readonly ResourceManager Manager =
        new("WpfTool.Strings.Resources", typeof(Strings).Assembly);

    private static string Get(string key)
    {
        return Manager.GetString(key, CultureInfo.CurrentUICulture) ?? key;
    }

    /// <summary>
    ///     按资源键取文案，用于语言枚举里声明的资源名
    /// </summary>
    public static string ByKey(string key)
    {
        return Get(key);
    }

    public static string App_Run => Get(nameof(App_Run));
    public static string AutoStart_SetFail => Get(nameof(AutoStart_SetFail));

    public static string MainWindows_Title => Get(nameof(MainWindows_Title));
    public static string MainWindows_Running => Get(nameof(MainWindows_Running));
    public static string MainWindows_WordTranslation => Get(nameof(MainWindows_WordTranslation));
    public static string MainWindows_ScreenshotTranslation => Get(nameof(MainWindows_ScreenshotTranslation));
    public static string MainWindows_OCR => Get(nameof(MainWindows_OCR));
    public static string MainWindows_TopMostToggle => Get(nameof(MainWindows_TopMostToggle));
    public static string MainWindows_WordFileExtract => Get(nameof(MainWindows_WordFileExtract));
    public static string MainWindows_Setting => Get(nameof(MainWindows_Setting));
    public static string MainWindows_Exit => Get(nameof(MainWindows_Exit));
    public static string MainWindows_HotkeyConflictMessage => Get(nameof(MainWindows_HotkeyConflictMessage));
    public static string MainWindows_TopMostMessage => Get(nameof(MainWindows_TopMostMessage));

    public static string ResultWindows_Title => Get(nameof(ResultWindows_Title));
    public static string ResultWindows_DialogClose => Get(nameof(ResultWindows_DialogClose));
    public static string ResultWindows_Translate => Get(nameof(ResultWindows_Translate));
    public static string ResultWindows_SetAsDefault => Get(nameof(ResultWindows_SetAsDefault));
    public static string ResultWindows_ReOCR => Get(nameof(ResultWindows_ReOCR));
    public static string ResultWindows_translating => Get(nameof(ResultWindows_translating));
    public static string ResultWindows_ocring => Get(nameof(ResultWindows_ocring));
    public static string ResultWindows_EmptyKeyMessage => Get(nameof(ResultWindows_EmptyKeyMessage));
    public static string ResultWindows_NotFoundImage => Get(nameof(ResultWindows_NotFoundImage));
    public static string ResultWindows_LimitOcrAndTranslate => Get(nameof(ResultWindows_LimitOcrAndTranslate));

    public static string WordFileExtractWindow_Title => Get(nameof(WordFileExtractWindow_Title));
    public static string WordFileExtractWindow_WordFilePath => Get(nameof(WordFileExtractWindow_WordFilePath));
    public static string WordFileExtractWindow_SelectedFile => Get(nameof(WordFileExtractWindow_SelectedFile));
    public static string WordFileExtractWindow_Extract => Get(nameof(WordFileExtractWindow_Extract));
    public static string WordFileExtractWindow_WordNotFoundMessage => Get(nameof(WordFileExtractWindow_WordNotFoundMessage));
    public static string WordFileExtractWindow_WordFileSelectedOutput => Get(nameof(WordFileExtractWindow_WordFileSelectedOutput));
    public static string WordFileExtractWindow_MkdirOutput => Get(nameof(WordFileExtractWindow_MkdirOutput));
    public static string WordFileExtractWindow_ExtractImageOutput => Get(nameof(WordFileExtractWindow_ExtractImageOutput));
    public static string WordFileExtractWindow_ExtractFileOutput => Get(nameof(WordFileExtractWindow_ExtractFileOutput));
    public static string WordFileExtractWindow_ExtractFinishOutput => Get(nameof(WordFileExtractWindow_ExtractFinishOutput));

    public static string Setting_Title => Get(nameof(Setting_Title));
    public static string Setting_Common => Get(nameof(Setting_Common));
    public static string Setting_OCR => Get(nameof(Setting_OCR));
    public static string Setting_MachineTranslate => Get(nameof(Setting_MachineTranslate));
    public static string Setting_GlobalHotkey => Get(nameof(Setting_GlobalHotkey));
    public static string Setting_about => Get(nameof(Setting_about));
    public static string Setting_AutoStart => Get(nameof(Setting_AutoStart));
    public static string Setting_language => Get(nameof(Setting_language));
    public static string Setting_Theme => Get(nameof(Setting_Theme));
    public static string Setting_ThemeAuto => Get(nameof(Setting_ThemeAuto));
    public static string Setting_ThemeLight => Get(nameof(Setting_ThemeLight));
    public static string Setting_ThemeDark => Get(nameof(Setting_ThemeDark));
    public static string Setting_ConfigurationFileStorageLocation => Get(nameof(Setting_ConfigurationFileStorageLocation));
    public static string Setting_UserDirectory => Get(nameof(Setting_UserDirectory));
    public static string Setting_AppDirectory => Get(nameof(Setting_AppDirectory));
    public static string Setting_Open => Get(nameof(Setting_Open));
    public static string Setting_WordSelectionInterval => Get(nameof(Setting_WordSelectionInterval));
    public static string Setting_WordSelectionIntervalToolTip => Get(nameof(Setting_WordSelectionIntervalToolTip));
    public static string Setting_DefaultOCR => Get(nameof(Setting_DefaultOCR));
    public static string Setting_GetFreeOCRQuota => Get(nameof(Setting_GetFreeOCRQuota));
    public static string Setting_CopyEmailMessage => Get(nameof(Setting_CopyEmailMessage));
    public static string Setting_defaultMachineTranslate => Get(nameof(Setting_defaultMachineTranslate));
    public static string Setting_GetFreeTranslateQuota => Get(nameof(Setting_GetFreeTranslateQuota));
    public static string Setting_GetFreeDeeplx => Get(nameof(Setting_GetFreeDeeplx));
    public static string Setting_GoogleFreeTranslate => Get(nameof(Setting_GoogleFreeTranslate));
    public static string Setting_HotkeyConflict => Get(nameof(Setting_HotkeyConflict));
    public static string Setting_WordTranslation => Get(nameof(Setting_WordTranslation));
    public static string Setting_ScreenshotTranslation => Get(nameof(Setting_ScreenshotTranslation));
    public static string Setting_TopMostToggle => Get(nameof(Setting_TopMostToggle));
    public static string Setting_Reset => Get(nameof(Setting_Reset));
    public static string Setting_Version => Get(nameof(Setting_Version));
    public static string Setting_license => Get(nameof(Setting_license));
    public static string Setting_AllRightsReserved => Get(nameof(Setting_AllRightsReserved));
    public static string Setting_SourceCode => Get(nameof(Setting_SourceCode));
    public static string Setting_feedback => Get(nameof(Setting_feedback));
    public static string Setting_Links => Get(nameof(Setting_Links));

    public static string CloudService_BaiduCloud => Get(nameof(CloudService_BaiduCloud));
    public static string CloudService_TencentCloud => Get(nameof(CloudService_TencentCloud));
    public static string CloudService_BaiduAI => Get(nameof(CloudService_BaiduAI));
    public static string CloudService_GoogleCloud => Get(nameof(CloudService_GoogleCloud));
    public static string CloudService_Deeplx => Get(nameof(CloudService_Deeplx));
    public static string CloudService_OpenAI => Get(nameof(CloudService_OpenAI));
    public static string CloudService_SpaceOCR => Get(nameof(CloudService_SpaceOCR));

    public static string OcrType_GeneralBasic => Get(nameof(OcrType_GeneralBasic));
    public static string OcrType_AccurateBasic => Get(nameof(OcrType_AccurateBasic));
    public static string OcrType_Handwriting => Get(nameof(OcrType_Handwriting));
    public static string OcrType_Engine1 => Get(nameof(OcrType_Engine1));
    public static string OcrType_Engine2 => Get(nameof(OcrType_Engine2));
    public static string OcrType_Engine3 => Get(nameof(OcrType_Engine3));
    public static string OcrType_Engine5 => Get(nameof(OcrType_Engine5));

    public static string Language_auto => Get(nameof(Language_auto));
}
