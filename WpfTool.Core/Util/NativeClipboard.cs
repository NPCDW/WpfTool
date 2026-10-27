using System;
using System.Runtime.InteropServices;

namespace WpfTool.Util;

public static class NativeClipboard
{
    public static void SetText(string text)
    {
        if (!NativeMethod.OpenClipboard(IntPtr.Zero))
        {
            SetText(text);
            return;
        }

        NativeMethod.EmptyClipboard();
        NativeMethod.SetClipboardData(13, Marshal.StringToHGlobalUni(text));
        NativeMethod.CloseClipboard();
    }

    public static string GetText()
    {
        var value = string.Empty;
        NativeMethod.OpenClipboard(IntPtr.Zero);
        if (NativeMethod.IsClipboardFormatAvailable(13))
        {
            var ptr = NativeMethod.GetClipboardData(13);
            if (ptr != IntPtr.Zero) value = Marshal.PtrToStringUni(ptr);
        }

        NativeMethod.CloseClipboard();
        return value?? "";
    }
}