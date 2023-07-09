using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfTool.Entity;
using WpfTool.Page.Setting;
using WpfTool.Util;

namespace WpfTool
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Wpf.Ui.Controls.UiWindow
    {
        public SettingWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GlobalConfig.SaveConfig();
            Utils.FlushMemory();
        }

        private void RootNavigation_OnLoaded(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate(0);
        }
    }
}