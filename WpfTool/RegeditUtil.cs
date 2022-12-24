using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTool
{
    internal class RegeditUtil
    {

        public static bool CreateDir(String dir)
        {
            try
            {
                RegistryKey R_local = Registry.CurrentUser;
                RegistryKey R_run = R_local.CreateSubKey(dir);
                R_run.Close();
                R_local.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static String GetValue(String dir, String key)
        {
            try
            {
                RegistryKey R_local = Registry.CurrentUser;
                RegistryKey R_run = R_local.CreateSubKey(dir);
                object value = R_run.GetValue(key);
                R_run.Close();
                R_local.Close();
                if (value == null)
                {
                    return null;
                }
                return value.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static bool SetValue(String dir, String key, String value)
        {
            try
            {
                RegistryKey R_local = Registry.CurrentUser;
                RegistryKey R_run = R_local.CreateSubKey(dir);
                R_run.SetValue(key, value);
                R_run.Close();
                R_local.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteValue(String dir, String key)
        {
            try
            {
                RegistryKey R_local = Registry.CurrentUser;
                RegistryKey R_run = R_local.CreateSubKey(dir);
                R_run.DeleteValue(key, false);
                R_run.Close();
                R_local.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
