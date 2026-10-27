using System;
using Microsoft.Win32;

namespace WpfTool.Util;

internal class RegeditUtil
{
    public static bool CreateDir(string dir)
    {
        try
        {
            var rLocal = Registry.CurrentUser;
            var rRun = rLocal.CreateSubKey(dir);
            rRun.Close();
            rLocal.Close();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static string? GetValue(string dir, string key)
    {
        try
        {
            var rLocal = Registry.CurrentUser;
            var rRun = rLocal.CreateSubKey(dir);
            var value = rRun.GetValue(key);
            rRun.Close();
            rLocal.Close();
            return value?.ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public static bool SetValue(string dir, string key, string value)
    {
        try
        {
            var rLocal = Registry.CurrentUser;
            var rRun = rLocal.CreateSubKey(dir);
            rRun.SetValue(key, value);
            rRun.Close();
            rLocal.Close();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool DeleteValue(string dir, string key)
    {
        try
        {
            var rLocal = Registry.CurrentUser;
            var rRun = rLocal.CreateSubKey(dir);
            rRun.DeleteValue(key, false);
            rRun.Close();
            rLocal.Close();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}