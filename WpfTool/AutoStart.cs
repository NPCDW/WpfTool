using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfTool
{
    internal class AutoStart
    {

        public static bool GetStatus()
        {
            try
            {
                RegistryKey R_local = Registry.CurrentUser;
                RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                object value = R_run.GetValue("WindowsFormsOCR");
                R_run.Close();
                R_local.Close();
                if (value == null)
                {
                    return false;
                }
                return value.ToString().Equals(Application.ExecutablePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static void Enable()
        {
            try
            {
                RegistryKey R_local = Registry.CurrentUser;
                RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                R_run.SetValue("WindowsFormsOCR", Application.ExecutablePath);
                R_run.Close();
                R_local.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("设置失败，可能需要您使用管理员权限启动应用后再修改");
            }
        }

        public static void Disable()
        {
            try
            {
                RegistryKey R_local = Registry.CurrentUser;
                RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                R_run.DeleteValue("WindowsFormsOCR", false);
                R_run.Close();
                R_local.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("设置失败，可能需要您使用管理员权限启动应用后再修改");
            }
        }

    }
}
