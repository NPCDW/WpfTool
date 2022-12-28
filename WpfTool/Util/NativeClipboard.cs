using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfTool
{
    internal class NativeClipboard
    {
        internal static void SetText(string text)
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

        internal static string GetText()
        {
            string value = string.Empty;
            NativeMethod.OpenClipboard(IntPtr.Zero);
            if (NativeMethod.IsClipboardFormatAvailable(13))
            {
                IntPtr ptr = NativeMethod.GetClipboardData(13);
                if (ptr != IntPtr.Zero)
                {
                    value = Marshal.PtrToStringUni(ptr);
                }
            }
            NativeMethod.CloseClipboard();
            return value;
        }
    }
}
