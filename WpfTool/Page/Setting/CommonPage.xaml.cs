using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WpfTool.Entity;
using WpfTool.Util;

namespace WpfTool.Page.Setting;

public partial class CommonPage
{
    private bool _pageLoaded;

    public CommonPage()
    {
        InitializeComponent();
    }

    private void Common_OnLoaded(object sender, RoutedEventArgs e)
    {
        AutoStartButton.IsChecked = GlobalConfig.Common.AutoStart;
        WordSelectionIntervalNumberBox.Value = GlobalConfig.Common.WordSelectionInterval;

        foreach (ComboBoxItem item in LanguageComboBox.Items)
            if (item.DataContext.Equals(GlobalConfig.Common.Language))
            {
                LanguageComboBox.SelectedItem = item;
                break;
            }

        if (GlobalConfig.UserDirConfigPath.Equals(GlobalConfig.Common.ConfigPath))
            UserConfigRadioButton.IsChecked = true;
        else
            AppConfigRadioButton.IsChecked = true;

        _pageLoaded = true;
    }

    private void autoStartButton_Checked(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded)
        {
            AutoStart.Enable();
            GlobalConfig.Common.AutoStart = true;
        }
    }

    private void autoStartButton_Unchecked(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded)
        {
            AutoStart.Disable();
            GlobalConfig.Common.AutoStart = false;
        }
    }

    private void ConfigButton_Click(object sender, RoutedEventArgs e)
    {
        Process.Start("Explorer.exe", "/select," + GlobalConfig.Common.ConfigPath);
    }

    private void UserConfigRadioButton_Checked(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Common.ConfigPath = GlobalConfig.UserDirConfigPath;
    }

    private void AppConfigRadioButton_Checked(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Common.ConfigPath = GlobalConfig.AppDirConfigPath;
    }

    private void languageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_pageLoaded)
        {
            var lang = ((ComboBoxItem)((ComboBox)sender).SelectedItem).DataContext.ToString()!;
            LanguageUtil.SwitchLanguage(lang);
            GlobalConfig.Common.Language = lang;
            MainWindow.MainWindowInst!.InitialTray();
        }
    }

    private void WordSelectionIntervalNumberBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Common.WordSelectionInterval = (int)WordSelectionIntervalNumberBox.Value;
    }
}