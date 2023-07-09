using System.Windows;
using System.Windows.Controls;
using WpfTool.Entity;
using WpfTool.Util;

namespace WpfTool.Page.Setting;

public partial class CommonPage : System.Windows.Controls.Page
{
    private bool PageLoaded = false;

    public CommonPage()
    {
        InitializeComponent();
    }

    private void Common_OnLoaded(object sender, RoutedEventArgs e)
    {
        this.autoStartButton.IsChecked = GlobalConfig.Common.AutoStart;
        this.WordSelectionIntervalNumberBox.Value = GlobalConfig.Common.WordSelectionInterval;

        foreach (ComboBoxItem item in this.languageComboBox.Items)
        {
            if (item.DataContext.Equals(GlobalConfig.Common.Language.ToString()))
            {
                languageComboBox.SelectedItem = item;
                break;
            }
        }

        if (GlobalConfig.UserDirConfigPath.Equals(GlobalConfig.Common.ConfigPath))
        {
            this.UserConfigRadioButton.IsChecked = true;
        }
        else
        {
            this.AppConfigRadioButton.IsChecked = true;
        }

        this.PageLoaded = true;
    }

    private void autoStartButton_Checked(object sender, RoutedEventArgs e)
    {
        if (this.PageLoaded)
        {
            AutoStart.Enable();
            GlobalConfig.Common.AutoStart = true;
        }
    }

    private void autoStartButton_Unchecked(object sender, RoutedEventArgs e)
    {
        if (this.PageLoaded)
        {
            AutoStart.Disable();
            GlobalConfig.Common.AutoStart = false;
        }
    }

    private void ConfigButton_Click(object sender, RoutedEventArgs e)
    {
        System.Diagnostics.Process.Start("Explorer.exe", "/select," + GlobalConfig.Common.ConfigPath);
    }

    private void UserConfigRadioButton_Checked(object sender, RoutedEventArgs e)
    {
        if (this.PageLoaded)
        {
            GlobalConfig.Common.ConfigPath = GlobalConfig.UserDirConfigPath;
        }
    }

    private void AppConfigRadioButton_Checked(object sender, RoutedEventArgs e)
    {
        if (this.PageLoaded)
        {
            GlobalConfig.Common.ConfigPath = GlobalConfig.AppDirConfigPath;
        }
    }

    private void languageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.PageLoaded)
        {
            string lang = ((ComboBoxItem)((ComboBox)sender).SelectedItem).DataContext.ToString();
            LanguageUtil.switchLanguage(lang);
            GlobalConfig.Common.Language = lang;
            MainWindow.mainWindow.InitialTray();
        }
    }

    private void WordSelectionIntervalNumberBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (this.PageLoaded)
        {
            GlobalConfig.Common.WordSelectionInterval = (int)WordSelectionIntervalNumberBox.Value;
        }
    }
}