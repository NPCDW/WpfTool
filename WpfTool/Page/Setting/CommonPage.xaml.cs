using System.Windows;
using System.Windows.Controls;

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
        this.autoStartButton.IsChecked = GlobalConfig.Common.autoStart;
        this.WordSelectionIntervalNumberBox.Value = GlobalConfig.Common.wordSelectionInterval;

        foreach (ComboBoxItem item in this.languageComboBox.Items)
        {
            if (item.DataContext.Equals(GlobalConfig.Common.language.ToString()))
            {
                languageComboBox.SelectedItem = item;
                break;
            }
        }

        if (GlobalConfig.USER_DIR_CONFIG_PATH.Equals(GlobalConfig.Common.configPath))
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
            GlobalConfig.Common.autoStart = true;
        }
    }

    private void autoStartButton_Unchecked(object sender, RoutedEventArgs e)
    {
        if (this.PageLoaded)
        {
            AutoStart.Disable();
            GlobalConfig.Common.autoStart = false;
        }
    }

    private void ConfigButton_Click(object sender, RoutedEventArgs e)
    {
        System.Diagnostics.Process.Start("Explorer.exe", "/select," + GlobalConfig.Common.configPath);
    }

    private void UserConfigRadioButton_Checked(object sender, RoutedEventArgs e)
    {
        if (this.PageLoaded)
        {
            GlobalConfig.Common.configPath = GlobalConfig.USER_DIR_CONFIG_PATH;
        }
    }

    private void AppConfigRadioButton_Checked(object sender, RoutedEventArgs e)
    {
        if (this.PageLoaded)
        {
            GlobalConfig.Common.configPath = GlobalConfig.APP_DIR_CONFIG_PATH;
        }
    }

    private void languageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.PageLoaded)
        {
            string lang = ((ComboBoxItem)((ComboBox)sender).SelectedItem).DataContext.ToString();
            LanguageUtil.switchLanguage(lang);
            GlobalConfig.Common.language = lang;
            MainWindow.mainWindow.InitialTray();
        }
    }

    private void WordSelectionIntervalNumberBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (this.PageLoaded)
        {
            GlobalConfig.Common.wordSelectionInterval = (int)WordSelectionIntervalNumberBox.Value;
        }
    }
}