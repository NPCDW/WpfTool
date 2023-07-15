using System;
using System.Windows;
using Wpf.Ui.Controls;
using WpfTool.Entity;
using WpfTool.Util;

namespace WpfTool;

/// <summary>
///     SettingWindow.xaml 的交互逻辑
/// </summary>
public partial class SettingWindow
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