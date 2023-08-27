using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using WpfTool.Entity;

namespace WpfTool.Util;

public static class GetWords
{
    public static string Get()
    {
        SendCtrlC();
        Thread.Sleep(GlobalConfig.Common.WordSelectionInterval);
        return NativeClipboard.GetText();
    }

    private static void SendCtrlC()
    {
        const uint keyEventKeyup = 2;
        const uint keyEventKeydown = 0;
        // 抬起所有键盘控制键
        NativeMethod.keybd_event(Keys.ControlKey, 0, keyEventKeyup, 0);
        NativeMethod.keybd_event(KeyInterop.VirtualKeyFromKey(Key.LeftAlt), 0, keyEventKeyup, 0);
        NativeMethod.keybd_event(KeyInterop.VirtualKeyFromKey(Key.RightAlt), 0, keyEventKeyup, 0);
        NativeMethod.keybd_event(Keys.LWin, 0, keyEventKeyup, 0);
        NativeMethod.keybd_event(Keys.RWin, 0, keyEventKeyup, 0);
        NativeMethod.keybd_event(Keys.ShiftKey, 0, keyEventKeyup, 0);
        // 按下 Ctrl + Insert
        NativeMethod.keybd_event(Keys.ControlKey, 0, keyEventKeydown, 0);
        NativeMethod.keybd_event(Keys.Insert, 0, keyEventKeydown, 0);
        // 抬起 Ctrl + Insert
        NativeMethod.keybd_event(Keys.Insert, 0, keyEventKeyup, 0);
        NativeMethod.keybd_event(Keys.ControlKey, 0, keyEventKeyup, 0);
    }
}