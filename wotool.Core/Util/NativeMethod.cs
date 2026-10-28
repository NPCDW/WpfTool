using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace wotool.Util;

public static class NativeMethod
{
    /// <summary>
    ///     定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）
    /// </summary>
    [Flags]
    public enum KeyModifiers
    {
        None = 0,
        Alt = 1,
        Ctrl = 2,
        Shift = 4,
        WindowsKey = 8
    }

    public const uint KeyEventKeyDown = 0;
    public const uint KeyEventKeyUp = 2;

    /// <summary>
    ///     模拟触发键盘的按键
    /// </summary>
    /// <param name="vk">按下的键</param>
    /// <param name="bScan"></param>
    /// <param name="dwFlags">触发的方式，0按下，2抬起</param>
    /// <param name="dwExtraInfo"></param>
    [DllImport("user32.dll")]
    public static extern void keybd_event(int vk, byte bScan, uint dwFlags, uint dwExtraInfo);

    /// <summary>
    ///     注册全局热键
    /// </summary>
    /// <param name="hWnd">要定义热键的窗口的句柄</param>
    /// <param name="id">定义热键ID（不能与其它ID重复，全局唯一）</param>
    /// <param name="fsModifiers">标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效</param>
    /// <param name="vk">定义热键的内容</param>
    /// <returns>成功，返回值不为0，失败，返回值为0。要得到扩展错误信息，调用GetLastError</returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool RegisterHotKey(IntPtr hWnd, int id, byte fsModifiers, int vk);

    /// <summary>
    ///     取消注册全局热键
    /// </summary>
    /// <param name="hWnd">要取消热键的窗口的句柄</param>
    /// <param name="id">要取消热键的ID</param>
    /// <returns></returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern bool BringWindowToTop(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy,
        uint flags);

    /// <summary>
    ///     打开剪切板
    /// </summary>
    /// <param name="hWndNewOwner"></param>
    /// <returns></returns>
    [DllImport("User32")]
    public static extern bool OpenClipboard(IntPtr hWndNewOwner);

    /// <summary>
    ///     关闭剪切板
    /// </summary>
    /// <returns></returns>
    [DllImport("User32")]
    public static extern bool CloseClipboard();

    /// <summary>
    ///     清空剪切板
    /// </summary>
    /// <returns></returns>
    [DllImport("User32")]
    public static extern bool EmptyClipboard();

    /// <summary>
    ///     剪切板格式化的数据是否可用
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    [DllImport("User32")]
    public static extern bool IsClipboardFormatAvailable(int format);

    /// <summary>
    ///     获取剪切板数据
    /// </summary>
    /// <param name="uFormat"></param>
    /// <returns></returns>
    [DllImport("User32")]
    public static extern IntPtr GetClipboardData(int uFormat);

    /// <summary>
    ///     设置剪切板数据
    /// </summary>
    /// <param name="uFormat"></param>
    /// <param name="hMem"></param>
    /// <returns></returns>
    [DllImport("User32", CharSet = CharSet.Unicode)]
    public static extern IntPtr SetClipboardData(int uFormat, IntPtr hMem);

    /// <summary>
    ///     获取当前鼠标位置（物理像素）
    /// </summary>
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out Point point);

    public delegate IntPtr SubclassProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam,
        UIntPtr uIdSubclass, UIntPtr dwRefData);

    /// <summary>
    ///     给窗口挂一个子类化回调，用于接管窗口消息（WinUI 3 没有 HwndSource 钩子）
    /// </summary>
    [DllImport("comctl32.dll", SetLastError = true)]
    public static extern bool SetWindowSubclass(IntPtr hWnd, SubclassProc callback, UIntPtr id, UIntPtr refData);

    [DllImport("comctl32.dll", SetLastError = true)]
    public static extern IntPtr DefSubclassProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
}
