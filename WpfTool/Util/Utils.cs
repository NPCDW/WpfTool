using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfTool.Util;

internal class Utils
{
    public static ImageBrush BitmapToImageBrush(Bitmap bmp)
    {
        var brush = new ImageBrush();
        var hBitmap = bmp.GetHbitmap();
        ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
            hBitmap,
            IntPtr.Zero,
            Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());
        brush.ImageSource = wpfBitmap;
        FlushMemory();
        return brush;
    }

    public static string BitmapToBase64String(Bitmap bmp)
    {
        using (var ms = new MemoryStream())
        {
            bmp.Save(ms, ImageFormat.Jpeg);
            var arr = new byte[ms.Length];
            ms.Position = 0;
            var _ = ms.Read(arr, 0, (int)ms.Length);
            return Convert.ToBase64String(arr);
        }
    }

    public static byte[] BitmapToByteArray(Bitmap bmp)
    {
        using (var ms = new MemoryStream())
        {
            bmp.Save(ms, ImageFormat.Jpeg);
            var arr = new byte[ms.Length];
            ms.Position = 0;
            var _ = ms.Read(arr, 0, (int)ms.Length);
            return arr;
        }
    }

    /// <summary>
    ///     清理内存
    /// </summary>
    public static void FlushMemory()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            NativeMethod.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
    }

    public static string Md5(string str)
    {
        return Md5(Encoding.UTF8.GetBytes(str));
    }

    public static string Md5(byte[] byteArray)
    {
        using (var md5 = MD5.Create())
        {
            var hash = md5.ComputeHash(byteArray);
            var sign = "";
            for (var i = 0; i < hash.Length; i++) sign += hash[i].ToString("X").PadLeft(2, '0');
            return sign.ToLower();
        }
    }
}