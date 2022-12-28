using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfTool
{
    internal class Utils
    {
        public static ImageBrush BitmapToImageBrush(Bitmap bmp)
        {
            ImageBrush brush = new ImageBrush();
            IntPtr hBitmap = bmp.GetHbitmap();
            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            brush.ImageSource = wpfBitmap;
            Utils.FlushMemory();
            return brush;
        }

        public static string BitmapToBase64String(Bitmap bmp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                return Convert.ToBase64String(arr);
            }
        }

        public static byte[] BitmapToByteArray(Bitmap bmp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                return arr;
            }
        }

        /// <summary>
        /// 清理内存
        /// </summary>
        public static void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                NativeMethod.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        public static string Md5(string str)
        {
            return Md5(Encoding.UTF8.GetBytes(str));
        }

        public static string Md5(byte[] byteArray)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytHash = md5.ComputeHash(byteArray);
            md5.Clear();
            string sign = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sign += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sign.ToLower();
        }

    }
}
