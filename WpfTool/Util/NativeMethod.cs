using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WpfTool.Util
{
    internal class NativeMethod
    {
        /// <summary>
        /// 模拟触发键盘的按键
        /// </summary>
        /// <param name="vk">按下的键</param>
        /// <param name="bScan"></param>
        /// <param name="dwFlags">触发的方式，0按下，2抬起</param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("user32.dll")]
        public static extern void keybd_event(System.Windows.Forms.Keys vk, byte bScan, uint dwFlags, uint dwExtraInfo);

        /// <summary>
        /// 模拟触发键盘的按键
        /// </summary>
        /// <param name="vk">按下的键</param>
        /// <param name="bScan"></param>
        /// <param name="dwFlags">触发的方式，0按下，2抬起</param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("user32.dll")]
        public static extern void keybd_event(int vk, byte bScan, uint dwFlags, uint dwExtraInfo);

        /// <summary>
        /// 注册全局热键
        /// </summary>
        /// <param name="hWnd">要定义热键的窗口的句柄</param>
        /// <param name="id">定义热键ID（不能与其它ID重复，全局唯一）</param>
        /// <param name="fsModifiers">标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效</param>
        /// <param name="vk">定义热键的内容</param>
        /// <returns>成功，返回值不为0，失败，返回值为0。要得到扩展错误信息，调用GetLastError</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, byte fsModifiers, int vk);

        /// <summary>
        /// 取消注册全局热键
        /// </summary>
        /// <param name="hWnd">要取消热键的窗口的句柄</param>
        /// <param name="id">要取消热键的ID</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        /// <summary>
        /// 打开剪切板
        /// </summary>
        /// <param name="hWndNewOwner"></param>
        /// <returns></returns>
        [DllImport("User32")]
        internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

        /// <summary>
        /// 关闭剪切板
        /// </summary>
        /// <returns></returns>
        [DllImport("User32")]
        internal static extern bool CloseClipboard();

        /// <summary>
        /// 清空剪切板
        /// </summary>
        /// <returns></returns>
        [DllImport("User32")]
        internal static extern bool EmptyClipboard();

        /// <summary>
        /// 剪切板格式化的数据是否可用
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [DllImport("User32")]
        internal static extern bool IsClipboardFormatAvailable(int format);

        /// <summary>
        /// 获取剪切板数据
        /// </summary>
        /// <param name="uFormat"></param>
        /// <returns></returns>
        [DllImport("User32")]
        internal static extern IntPtr GetClipboardData(int uFormat);

        /// <summary>
        /// 设置剪切板数据
        /// </summary>
        /// <param name="uFormat"></param>
        /// <param name="hMem"></param>
        /// <returns></returns>
        [DllImport("User32", CharSet = CharSet.Unicode)]
        internal static extern IntPtr SetClipboardData(int uFormat, IntPtr hMem);

        const int HORZRES = 8;
        const int VERTRES = 10;
        const int LOGPIXELSX = 88;
        const int LOGPIXELSY = 90;
        const int DESKTOPVERTRES = 117;
        const int DESKTOPHORZRES = 118;

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr ptr);
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(
        IntPtr hdc, // handle to DC
        int nIndex // index of capability
        );
        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        /// <summary>
        /// 获取窗口标题
        /// </summary>
        /// <returns></returns>
        [DllImport("user32", SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 获取类的名字
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 根据坐标获取窗口句柄
        /// </summary>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern IntPtr WindowFromPoint(System.Drawing.Point Point);

        /// <summary>
        /// 窗口置顶与取消置顶
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hPos, int x, int y, int cx, int cy, uint nflags);

        /// <summary>
        /// 定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）
        /// </summary>
        [Flags()]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WindowsKey = 8
        }
    }
}
