using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace WpfTool.Util;

public static class Utils
{
    [DllImport("kernel32.dll")]
    private static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

    public static string BitmapToBase64String(Bitmap bmp)
    {
        using var ms = new MemoryStream();
        bmp.Save(ms, ImageFormat.Jpeg);
        return Convert.ToBase64String(ms.ToArray());
    }

    public static byte[] BitmapToByteArray(Bitmap bmp)
    {
        using var ms = new MemoryStream();
        bmp.Save(ms, ImageFormat.Jpeg);
        return ms.ToArray();
    }

    /// <summary>
    ///     清理内存
    /// </summary>
    public static void FlushMemory()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
    }

    public static string Md5(string str)
    {
        return Md5(Encoding.UTF8.GetBytes(str));
    }

    public static string Md5(byte[] byteArray)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(byteArray);
        var builder = new StringBuilder(hash.Length * 2);
        foreach (var b in hash) builder.Append(b.ToString("X").PadLeft(2, '0'));
        return builder.ToString().ToLower();
    }
}
