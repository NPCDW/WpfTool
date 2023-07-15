using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WpfTool.Util;

internal static class TopMost
{
    private static readonly Dictionary<IntPtr, bool> TopMostDict = new();

    public static void Exec()
    {
        var x = Cursor.Position.X;
        var y = Cursor.Position.Y;
        var p = new Point(x, y);
        var formHandle = NativeMethod.WindowFromPoint(p); //得到窗口句柄
        var title = new StringBuilder(256);
        NativeMethod.GetWindowText(formHandle, title, title.Capacity); //得到窗口的标题
        var className = new StringBuilder(256);
        NativeMethod.GetClassName(formHandle, className, className.Capacity); //得到窗口的类名
        if (TopMostDict.ContainsKey(formHandle) && TopMostDict[formHandle])
        {
            TopMostDict[formHandle] = false;
            var hwndTopmost = new IntPtr(-2);
            NativeMethod.SetWindowPos(formHandle, hwndTopmost, 0, 0, 0, 0, 0x0001 | 0x0002);
        }
        else
        {
            TopMostDict[formHandle] = true;
            var hwndTopmost = new IntPtr(-1);
            NativeMethod.SetWindowPos(formHandle, hwndTopmost, 0, 0, 0, 0, 0x0001 | 0x0002);
        }
    }
}