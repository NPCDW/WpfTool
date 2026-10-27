using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using WpfTool.Util;

namespace WpfTool;

/// <summary>
///     截图与 DPI 相关工具。原实现依赖 System.Windows.Forms.Screen（WpfScreenHelper），
///     WinUI 3 下改为直接枚举显示器
/// </summary>
internal static class ScreenshotHelper
{
    [StructLayout(LayoutKind.Sequential)]
    private struct MonitorInfo
    {
        public int CbSize;
        public Rect32 Monitor;
        public Rect32 Work;
        public uint Flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Rect32
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [DllImport("user32.dll")]
    private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr rect, MonitorEnumProc callback, IntPtr data);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern bool GetMonitorInfo(IntPtr monitor, ref MonitorInfo info);

    [DllImport("user32.dll")]
    private static extern uint GetDpiForWindow(IntPtr hwnd);

    private delegate bool MonitorEnumProc(IntPtr monitor, IntPtr hdc, IntPtr rect, IntPtr data);

    public static Point GetCursorPosition()
    {
        NativeMethod.GetCursorPos(out var point);
        return point;
    }

    /// <summary>
    ///     所有显示器的物理像素边界
    /// </summary>
    public static List<Rectangle> GetAllMonitorBounds()
    {
        var list = new List<Rectangle>();
        EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (monitor, _, _, _) =>
        {
            var info = new MonitorInfo { CbSize = Marshal.SizeOf<MonitorInfo>() };
            if (!GetMonitorInfo(monitor, ref info)) return true;
            list.Add(new Rectangle(info.Monitor.Left, info.Monitor.Top,
                info.Monitor.Right - info.Monitor.Left, info.Monitor.Bottom - info.Monitor.Top));
            return true;
        }, IntPtr.Zero);
        return list;
    }

    /// <summary>
    ///     鼠标所在显示器的物理像素边界
    /// </summary>
    public static Rectangle GetScreenBoundsOfCursor()
    {
        var cursor = GetCursorPosition();
        var monitors = GetAllMonitorBounds();
        foreach (var bounds in monitors)
            if (bounds.Contains(cursor))
                return bounds;
        return monitors.Count > 0 ? monitors[0] : new Rectangle(0, 0, 1920, 1080);
    }

    public static double GetScaleFactor(Microsoft.UI.Xaml.Window window)
    {
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        return GetScaleFactor(hwnd);
    }

    public static double GetScaleFactor(IntPtr hwnd)
    {
        var dpi = GetDpiForWindow(hwnd);
        return dpi == 0 ? 1 : dpi / 96.0;
    }

    public static Bitmap CaptureScreen(Rectangle bounds)
    {
        var bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
        using var g = Graphics.FromImage(bitmap);
        g.CopyFromScreen(bounds.X, bounds.Y, 0, 0, new Size(bounds.Width, bounds.Height), CopyPixelOperation.SourceCopy);
        return bitmap;
    }

    public static Bitmap Crop(Bitmap source, Rectangle rect)
    {
        var result = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
        using var g = Graphics.FromImage(result);
        g.DrawImage(source, new Rectangle(0, 0, rect.Width, rect.Height), rect, GraphicsUnit.Pixel);
        return result;
    }

    /// <summary>
    ///     Bitmap 转 ImageSource，用于截图窗口的全屏背景
    /// </summary>
    public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
    {
        using var ms = new MemoryStream();
        bitmap.Save(ms, ImageFormat.Png);
        var image = new BitmapImage();
        ms.Position = 0;
        var stream = new InMemoryRandomAccessStream();
        using (var target = stream.AsStreamForWrite())
        {
            ms.CopyTo(target);
            target.Flush();
        }

        stream.Seek(0);
        image.SetSource(stream);
        return image;
    }

    public static Windows.Graphics.RectInt32 ToRectInt32(Rectangle rect)
    {
        return new Windows.Graphics.RectInt32(rect.X, rect.Y, rect.Width, rect.Height);
    }
}
