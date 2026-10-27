using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WpfTool.Util;

/// <summary>
///     窗口置顶切换：按鼠标位置取窗口句柄，来回切换 TOPMOST
/// </summary>
public static class WindowTopMost
{
    private const uint SwpNoSize = 0x0001;
    private const uint SwpNoMove = 0x0002;
    private const int HwndTopMost = -1;
    private const int HwndNoTopMost = -2;

    private static readonly Dictionary<IntPtr, bool> TopMostDict = new();

    public static void Exec(Point point)
    {
        var formHandle = NativeMethod.WindowFromPoint(point); //得到窗口句柄
        if (formHandle == IntPtr.Zero) return;

        var title = new StringBuilder(256);
        NativeMethod.GetWindowText(formHandle, title, title.Capacity); //得到窗口的标题
        var className = new StringBuilder(256);
        NativeMethod.GetClassName(formHandle, className, className.Capacity); //得到窗口的类名

        var topMost = TopMostDict.TryGetValue(formHandle, out var value) && value;
        TopMostDict[formHandle] = !topMost;
        var insertAfter = new IntPtr(topMost ? HwndNoTopMost : HwndTopMost);
        NativeMethod.SetWindowPos(formHandle, insertAfter, 0, 0, 0, 0, SwpNoSize | SwpNoMove);
    }
}
