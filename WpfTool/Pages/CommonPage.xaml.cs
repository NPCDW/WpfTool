using System;
using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WpfTool.Entity;
using WpfTool.Util;

namespace WpfTool.Pages;

public sealed partial class CommonPage : Page
{
    private bool _pageLoaded;

    public CommonPage()
    {
        InitializeComponent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        PageTitle.Text = Strings.Setting_Common;
        PageDescription.Text = Strings.Setting_CommonDescription;
        AutoStartHeader.Text = Strings.Setting_AutoStart;
        LanguageHeader.Text = Strings.Setting_language;
        ThemeHeader.Text = Strings.Setting_Theme;
        ConfigPathHeader.Text = Strings.Setting_ConfigurationFileStorageLocation;
        UserConfigRadioButton.Content = Strings.Setting_UserDirectory;
        AppConfigRadioButton.Content = Strings.Setting_AppDirectory;
        OpenConfigButton.Content = Strings.Setting_Open;
        WordSelectionIntervalHeader.Text = Strings.Setting_WordSelectionInterval;
        ToolTipService.SetToolTip(WordSelectionIntervalHeader, Strings.Setting_WordSelectionIntervalToolTip);

        ResetLanguageItems();
        ResetThemeItems();

        AutoStartSwitch.IsOn = GlobalConfig.Common.AutoStart;
        WordSelectionIntervalNumberBox.Value = GlobalConfig.Common.WordSelectionInterval;

        if (GlobalConfig.UserDirConfigPath.Equals(GlobalConfig.Common.ConfigPath))
            UserConfigRadioButton.IsChecked = true;
        else
            AppConfigRadioButton.IsChecked = true;

        _pageLoaded = true;
    }

    private void ResetLanguageItems()
    {
        LanguageComboBox.Items.Clear();
        LanguageComboBox.Items.Add(new ComboBoxItem { Tag = "en_US", Content = "English" });
        LanguageComboBox.Items.Add(new ComboBoxItem { Tag = "zh_CN", Content = "简体中文" });
        SelectByTag(LanguageComboBox, GlobalConfig.Common.Language);
    }

    private void ResetThemeItems()
    {
        ThemeComboBox.Items.Clear();
        ThemeComboBox.Items.Add(new ComboBoxItem { Tag = ThemeService.ThemeDefault, Content = Strings.Setting_ThemeAuto });
        ThemeComboBox.Items.Add(new ComboBoxItem { Tag = ThemeService.ThemeLight, Content = Strings.Setting_ThemeLight });
        ThemeComboBox.Items.Add(new ComboBoxItem { Tag = ThemeService.ThemeDark, Content = Strings.Setting_ThemeDark });
        SelectByTag(ThemeComboBox, GlobalConfig.Common.Theme);
    }

    private static void SelectByTag(ComboBox comboBox, string? tag)
    {
        foreach (var item in comboBox.Items)
            if (item is ComboBoxItem comboBoxItem && comboBoxItem.Tag?.ToString() == tag)
            {
                comboBox.SelectedItem = comboBoxItem;
                return;
            }
    }

    private void AutoStartSwitch_Toggled(object sender, RoutedEventArgs e)
    {
        if (!_pageLoaded) return;
        if (AutoStartSwitch.IsOn)
        {
            AutoStart.Enable();
            GlobalConfig.Common.AutoStart = true;
        }
        else
        {
            AutoStart.Disable();
            GlobalConfig.Common.AutoStart = false;
        }
    }

    private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_pageLoaded) return;
        var lang = (LanguageComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
        if (lang == null) return;

        LanguageUtil.SwitchLanguage(lang);
        App.Current.TrayIcon?.BuildMenu();
        WindowHelper.Find<Views.SettingWindow>()?.Reload();
    }

    private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_pageLoaded) return;
        var theme = (ThemeComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
        ThemeService.Apply(theme);
    }

    private void OpenConfigButton_Click(object sender, RoutedEventArgs e)
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

    private void WordSelectionIntervalNumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        if (!_pageLoaded || double.IsNaN(args.NewValue)) return;
        GlobalConfig.Common.WordSelectionInterval = (int)args.NewValue;
    }
}
