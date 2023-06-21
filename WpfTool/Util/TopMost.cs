using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WpfTool
{
    internal class TopMost
    {
        private static Dictionary<IntPtr, bool> topMostDict = new Dictionary<IntPtr, bool>();

        public static void exec()
        {
            int x = Cursor.Position.X;
            int y = Cursor.Position.Y;
            Point p = new Point(x, y);
            IntPtr formHandle = NativeMethod.WindowFromPoint(p);//得到窗口句柄
            StringBuilder title = new StringBuilder(256);
            NativeMethod.GetWindowText(formHandle, title, title.Capacity);//得到窗口的标题
            StringBuilder className = new StringBuilder(256);
            NativeMethod.GetClassName(formHandle, className, className.Capacity);//得到窗口的类名
            if (topMostDict.ContainsKey(formHandle) && topMostDict[formHandle])
            {
                topMostDict[formHandle] = false;
                IntPtr HWND_TOPMOST = new IntPtr(-2);
                NativeMethod.SetWindowPos(formHandle, HWND_TOPMOST, 0, 0, 0, 0, 0x0001 | 0x0002);
            }
            else
            {
                topMostDict[formHandle] = true;
                IntPtr HWND_TOPMOST = new IntPtr(-1);
                NativeMethod.SetWindowPos(formHandle, HWND_TOPMOST, 0, 0, 0, 0, 0x0001 | 0x0002);
            }
        }

    }
}