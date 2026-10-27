using System.Threading;
using WpfTool.Entity;

namespace WpfTool.Util;

public static class GetWords
{
    private const int VkControl = 0x11;
    private const int VkShift = 0x10;
    private const int VkMenu = 0x12;
    private const int VkLWin = 0x5B;
    private const int VkRWin = 0x5C;
    private const int VkInsert = 0x2D;

    /// <summary>
    ///     划词取词：先松开修饰键，再模拟 Ctrl+Insert 把选区送进剪贴板
    /// </summary>
    public static string Get()
    {
        SendCtrlInsert();
        Thread.Sleep(GlobalConfig.Common.WordSelectionInterval);
        return NativeClipboard.GetText();
    }

    private static void SendCtrlInsert()
    {
        NativeMethod.keybd_event(VkControl, 0, NativeMethod.KeyEventKeyUp, 0);
        NativeMethod.keybd_event(VkMenu, 0, NativeMethod.KeyEventKeyUp, 0);
        NativeMethod.keybd_event(VkLWin, 0, NativeMethod.KeyEventKeyUp, 0);
        NativeMethod.keybd_event(VkRWin, 0, NativeMethod.KeyEventKeyUp, 0);
        NativeMethod.keybd_event(VkShift, 0, NativeMethod.KeyEventKeyUp, 0);

        NativeMethod.keybd_event(VkControl, 0, NativeMethod.KeyEventKeyDown, 0);
        NativeMethod.keybd_event(VkInsert, 0, NativeMethod.KeyEventKeyDown, 0);
        NativeMethod.keybd_event(VkInsert, 0, NativeMethod.KeyEventKeyUp, 0);
        NativeMethod.keybd_event(VkControl, 0, NativeMethod.KeyEventKeyUp, 0);
    }
}
