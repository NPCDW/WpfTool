using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTool
{
    internal class HotKeysUtil
    {
        public static IntPtr mainFormHandle;

        public static int GetWordsTranslateId = 855;
        public static byte GetWordsTranslateModifiers;
        public static int GetWordsTranslateKey;
        public static int OcrId = 856;
        public static byte OcrModifiers;
        public static int OcrKey;
        public static int ScreenshotTranslateId = 857;
        public static byte ScreenshotTranslateModifiers;
        public static int ScreenshotTranslateKey;

        public static void RegisterHotKey(IntPtr mainFormHandle)
        {
            HotKeysUtil.mainFormHandle = mainFormHandle;

            HotKeysUtil.GetWordsTranslateModifiers = GlobalConfig.HotKeys.GetWordsTranslate.Modifiers;
            HotKeysUtil.GetWordsTranslateKey = GlobalConfig.HotKeys.GetWordsTranslate.Key;
            HotKeysUtil.OcrModifiers = GlobalConfig.HotKeys.Ocr.Modifiers;
            HotKeysUtil.OcrKey = GlobalConfig.HotKeys.Ocr.Key;
            HotKeysUtil.ScreenshotTranslateModifiers = GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers;
            HotKeysUtil.ScreenshotTranslateKey = GlobalConfig.HotKeys.ScreenshotTranslate.Key;

            NativeMethod.RegisterHotKey(mainFormHandle, GetWordsTranslateId, GlobalConfig.HotKeys.GetWordsTranslate.Modifiers, GlobalConfig.HotKeys.GetWordsTranslate.Key);
            NativeMethod.RegisterHotKey(mainFormHandle, OcrId, GlobalConfig.HotKeys.Ocr.Modifiers, GlobalConfig.HotKeys.Ocr.Key);
            NativeMethod.RegisterHotKey(mainFormHandle, ScreenshotTranslateId, GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers, GlobalConfig.HotKeys.ScreenshotTranslate.Key);
        }

        public static void UnRegisterHotKey()
        {
            NativeMethod.UnregisterHotKey(mainFormHandle, GetWordsTranslateId);
            NativeMethod.UnregisterHotKey(mainFormHandle, OcrId);
            NativeMethod.UnregisterHotKey(mainFormHandle, ScreenshotTranslateId);
        }

        public static void ReRegisterHotKey()
        {
            if (GlobalConfig.HotKeys.GetWordsTranslate.Key != 0 && (GetWordsTranslateModifiers != GlobalConfig.HotKeys.GetWordsTranslate.Modifiers || GetWordsTranslateKey != GlobalConfig.HotKeys.GetWordsTranslate.Key))
            {
                NativeMethod.UnregisterHotKey(mainFormHandle, GetWordsTranslateId);
                NativeMethod.RegisterHotKey(mainFormHandle, GetWordsTranslateId, GlobalConfig.HotKeys.GetWordsTranslate.Modifiers, GlobalConfig.HotKeys.GetWordsTranslate.Key);
                HotKeysUtil.GetWordsTranslateModifiers = GlobalConfig.HotKeys.GetWordsTranslate.Modifiers;
                HotKeysUtil.GetWordsTranslateKey = GlobalConfig.HotKeys.GetWordsTranslate.Key;
            }
            if (GlobalConfig.HotKeys.Ocr.Key != 0 && (OcrModifiers != GlobalConfig.HotKeys.Ocr.Modifiers || OcrKey != GlobalConfig.HotKeys.Ocr.Key))
            {
                NativeMethod.UnregisterHotKey(mainFormHandle, OcrId);
                NativeMethod.RegisterHotKey(mainFormHandle, OcrId, GlobalConfig.HotKeys.Ocr.Modifiers, GlobalConfig.HotKeys.Ocr.Key);
                HotKeysUtil.OcrModifiers = GlobalConfig.HotKeys.Ocr.Modifiers;
                HotKeysUtil.OcrKey = GlobalConfig.HotKeys.Ocr.Key;
            }
            if (GlobalConfig.HotKeys.ScreenshotTranslate.Key != 0 && (ScreenshotTranslateModifiers != GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers || ScreenshotTranslateKey != GlobalConfig.HotKeys.ScreenshotTranslate.Key))
            {
                NativeMethod.UnregisterHotKey(mainFormHandle, ScreenshotTranslateId);
                NativeMethod.RegisterHotKey(mainFormHandle, ScreenshotTranslateId, GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers, GlobalConfig.HotKeys.ScreenshotTranslate.Key);
                HotKeysUtil.ScreenshotTranslateModifiers = GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers;
                HotKeysUtil.ScreenshotTranslateKey = GlobalConfig.HotKeys.ScreenshotTranslate.Key;
            }
        }

    }
}
