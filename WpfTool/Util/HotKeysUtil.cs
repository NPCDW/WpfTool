using System;
using WpfTool.Entity;

namespace WpfTool.Util;

/// <summary>
///     全局热键注册。与原来一致：注册在隐藏主窗口的句柄上，按消息 ID 分发
/// </summary>
internal static class HotKeysUtil
{
    public const int GetWordsTranslateId = 855;
    public const int OcrId = 856;

    private static IntPtr _mainWindowHandle;
    private static byte _getWordsTranslateModifiers;
    private static int _getWordsTranslateKey;
    private static byte _ocrModifiers;
    private static int _ocrKey;

    public static void RegisterHotKey(IntPtr mainWindowHandle)
    {
        _mainWindowHandle = mainWindowHandle;

        _getWordsTranslateModifiers = GlobalConfig.HotKeys.GetWordsTranslate.Modifiers;
        _getWordsTranslateKey = GlobalConfig.HotKeys.GetWordsTranslate.Key;
        _ocrModifiers = GlobalConfig.HotKeys.OcrHotKey.Modifiers;
        _ocrKey = GlobalConfig.HotKeys.OcrHotKey.Key;

        if (GlobalConfig.HotKeys.GetWordsTranslate.Key != 0)
            GlobalConfig.HotKeys.GetWordsTranslate.Conflict = !NativeMethod.RegisterHotKey(mainWindowHandle,
                GetWordsTranslateId, GlobalConfig.HotKeys.GetWordsTranslate.Modifiers,
                GlobalConfig.HotKeys.GetWordsTranslate.Key);
        if (GlobalConfig.HotKeys.OcrHotKey.Key != 0)
            GlobalConfig.HotKeys.OcrHotKey.Conflict = !NativeMethod.RegisterHotKey(mainWindowHandle, OcrId,
                GlobalConfig.HotKeys.OcrHotKey.Modifiers, GlobalConfig.HotKeys.OcrHotKey.Key);
    }

    public static void UnRegisterHotKey()
    {
        NativeMethod.UnregisterHotKey(_mainWindowHandle, GetWordsTranslateId);
        NativeMethod.UnregisterHotKey(_mainWindowHandle, OcrId);
    }

    public static void ReRegisterHotKey()
    {
        if (GlobalConfig.HotKeys.GetWordsTranslate.Key == 0)
            NativeMethod.UnregisterHotKey(_mainWindowHandle, GetWordsTranslateId);
        else if (_getWordsTranslateModifiers != GlobalConfig.HotKeys.GetWordsTranslate.Modifiers ||
                 _getWordsTranslateKey != GlobalConfig.HotKeys.GetWordsTranslate.Key)
        {
            NativeMethod.UnregisterHotKey(_mainWindowHandle, GetWordsTranslateId);
            GlobalConfig.HotKeys.GetWordsTranslate.Conflict = !NativeMethod.RegisterHotKey(_mainWindowHandle,
                GetWordsTranslateId, GlobalConfig.HotKeys.GetWordsTranslate.Modifiers,
                GlobalConfig.HotKeys.GetWordsTranslate.Key);
        }

        _getWordsTranslateModifiers = GlobalConfig.HotKeys.GetWordsTranslate.Modifiers;
        _getWordsTranslateKey = GlobalConfig.HotKeys.GetWordsTranslate.Key;

        if (GlobalConfig.HotKeys.OcrHotKey.Key == 0)
            NativeMethod.UnregisterHotKey(_mainWindowHandle, OcrId);
        else if (_ocrModifiers != GlobalConfig.HotKeys.OcrHotKey.Modifiers ||
                 _ocrKey != GlobalConfig.HotKeys.OcrHotKey.Key)
        {
            NativeMethod.UnregisterHotKey(_mainWindowHandle, OcrId);
            GlobalConfig.HotKeys.OcrHotKey.Conflict = !NativeMethod.RegisterHotKey(_mainWindowHandle, OcrId,
                GlobalConfig.HotKeys.OcrHotKey.Modifiers, GlobalConfig.HotKeys.OcrHotKey.Key);
        }

        _ocrModifiers = GlobalConfig.HotKeys.OcrHotKey.Modifiers;
        _ocrKey = GlobalConfig.HotKeys.OcrHotKey.Key;

    }
}
