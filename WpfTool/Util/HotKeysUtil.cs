using System;
using WpfTool.Entity;

namespace WpfTool.Util;

internal static class HotKeysUtil
{
    public const int GetWordsTranslateId = 855;
    public const int OcrId = 856;
    public const int ScreenshotTranslateId = 857;
    public const int TopMostId = 858;
    private static IntPtr _mainFormHandle;
    private static byte _getWordsTranslateModifiers;
    private static int _getWordsTranslateKey;
    private static byte _ocrModifiers;
    private static int _ocrKey;
    private static byte _screenshotTranslateModifiers;
    private static int _screenshotTranslateKey;
    private static byte _topMostModifiers;
    private static int _topMostKey;

    public static void RegisterHotKey(IntPtr mainFormHandle)
    {
        _mainFormHandle = mainFormHandle;

        _getWordsTranslateModifiers = GlobalConfig.HotKeys.GetWordsTranslate.Modifiers;
        _getWordsTranslateKey = GlobalConfig.HotKeys.GetWordsTranslate.Key;
        _ocrModifiers = GlobalConfig.HotKeys.OcrHotKey.Modifiers;
        _ocrKey = GlobalConfig.HotKeys.OcrHotKey.Key;
        _screenshotTranslateModifiers = GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers;
        _screenshotTranslateKey = GlobalConfig.HotKeys.ScreenshotTranslate.Key;
        _topMostModifiers = GlobalConfig.HotKeys.TopMost.Modifiers;
        _topMostKey = GlobalConfig.HotKeys.TopMost.Key;

        if (GlobalConfig.HotKeys.GetWordsTranslate.Key != 0)
            GlobalConfig.HotKeys.GetWordsTranslate.Conflict = !NativeMethod.RegisterHotKey(mainFormHandle,
                GetWordsTranslateId, GlobalConfig.HotKeys.GetWordsTranslate.Modifiers,
                GlobalConfig.HotKeys.GetWordsTranslate.Key);
        if (GlobalConfig.HotKeys.OcrHotKey.Key != 0)
            GlobalConfig.HotKeys.OcrHotKey.Conflict = !NativeMethod.RegisterHotKey(mainFormHandle, OcrId,
                GlobalConfig.HotKeys.OcrHotKey.Modifiers, GlobalConfig.HotKeys.OcrHotKey.Key);
        if (GlobalConfig.HotKeys.ScreenshotTranslate.Key != 0)
            GlobalConfig.HotKeys.ScreenshotTranslate.Conflict = !NativeMethod.RegisterHotKey(mainFormHandle,
                ScreenshotTranslateId, GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers,
                GlobalConfig.HotKeys.ScreenshotTranslate.Key);
        if (GlobalConfig.HotKeys.TopMost.Key != 0)
            GlobalConfig.HotKeys.TopMost.Conflict = !NativeMethod.RegisterHotKey(mainFormHandle, TopMostId,
                GlobalConfig.HotKeys.TopMost.Modifiers, GlobalConfig.HotKeys.TopMost.Key);
    }

    public static void UnRegisterHotKey()
    {
        NativeMethod.UnregisterHotKey(_mainFormHandle, GetWordsTranslateId);
        NativeMethod.UnregisterHotKey(_mainFormHandle, OcrId);
        NativeMethod.UnregisterHotKey(_mainFormHandle, ScreenshotTranslateId);
        NativeMethod.UnregisterHotKey(_mainFormHandle, TopMostId);
    }

    public static void ReRegisterHotKey()
    {
        if (GlobalConfig.HotKeys.GetWordsTranslate.Key == 0)
        {
            NativeMethod.UnregisterHotKey(_mainFormHandle, GetWordsTranslateId);
        }
        else if (_getWordsTranslateModifiers != GlobalConfig.HotKeys.GetWordsTranslate.Modifiers ||
                 _getWordsTranslateKey != GlobalConfig.HotKeys.GetWordsTranslate.Key)
        {
            NativeMethod.UnregisterHotKey(_mainFormHandle, GetWordsTranslateId);
            GlobalConfig.HotKeys.GetWordsTranslate.Conflict = !NativeMethod.RegisterHotKey(_mainFormHandle,
                GetWordsTranslateId, GlobalConfig.HotKeys.GetWordsTranslate.Modifiers,
                GlobalConfig.HotKeys.GetWordsTranslate.Key);
        }

        _getWordsTranslateModifiers = GlobalConfig.HotKeys.GetWordsTranslate.Modifiers;
        _getWordsTranslateKey = GlobalConfig.HotKeys.GetWordsTranslate.Key;

        if (GlobalConfig.HotKeys.OcrHotKey.Key == 0)
        {
            NativeMethod.UnregisterHotKey(_mainFormHandle, OcrId);
        }
        else if (_ocrModifiers != GlobalConfig.HotKeys.OcrHotKey.Modifiers ||
                 _ocrKey != GlobalConfig.HotKeys.OcrHotKey.Key)
        {
            NativeMethod.UnregisterHotKey(_mainFormHandle, OcrId);
            GlobalConfig.HotKeys.OcrHotKey.Conflict = !NativeMethod.RegisterHotKey(_mainFormHandle, OcrId,
                GlobalConfig.HotKeys.OcrHotKey.Modifiers, GlobalConfig.HotKeys.OcrHotKey.Key);
        }

        _ocrModifiers = GlobalConfig.HotKeys.OcrHotKey.Modifiers;
        _ocrKey = GlobalConfig.HotKeys.OcrHotKey.Key;

        if (GlobalConfig.HotKeys.ScreenshotTranslate.Key == 0)
        {
            NativeMethod.UnregisterHotKey(_mainFormHandle, ScreenshotTranslateId);
        }
        else if (_screenshotTranslateModifiers != GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers ||
                 _screenshotTranslateKey != GlobalConfig.HotKeys.ScreenshotTranslate.Key)
        {
            NativeMethod.UnregisterHotKey(_mainFormHandle, ScreenshotTranslateId);
            GlobalConfig.HotKeys.ScreenshotTranslate.Conflict = !NativeMethod.RegisterHotKey(_mainFormHandle,
                ScreenshotTranslateId, GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers,
                GlobalConfig.HotKeys.ScreenshotTranslate.Key);
        }

        _screenshotTranslateModifiers = GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers;
        _screenshotTranslateKey = GlobalConfig.HotKeys.ScreenshotTranslate.Key;

        if (GlobalConfig.HotKeys.TopMost.Key == 0)
        {
            NativeMethod.UnregisterHotKey(_mainFormHandle, TopMostId);
        }
        else if (_topMostModifiers != GlobalConfig.HotKeys.TopMost.Modifiers ||
                 _topMostKey != GlobalConfig.HotKeys.TopMost.Key)
        {
            NativeMethod.UnregisterHotKey(_mainFormHandle, TopMostId);
            GlobalConfig.HotKeys.TopMost.Conflict = !NativeMethod.RegisterHotKey(_mainFormHandle, TopMostId,
                GlobalConfig.HotKeys.TopMost.Modifiers, GlobalConfig.HotKeys.TopMost.Key);
        }

        _topMostModifiers = GlobalConfig.HotKeys.TopMost.Modifiers;
        _topMostKey = GlobalConfig.HotKeys.TopMost.Key;
    }
}