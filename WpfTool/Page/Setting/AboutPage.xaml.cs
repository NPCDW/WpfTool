using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfTool.Page.Setting;

public partial class AboutPage : System.Windows.Controls.Page
{
    public AboutPage()
    {
        InitializeComponent();
    }

    private void CopyLabel_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var text = ((Label)sender).DataContext.ToString();
        NativeClipboard.SetText(text);

        var window = (SettingWindow)Window.GetWindow(this);
        Wpf.Ui.Controls.Snackbar snackbar = window.RootSnackbar;
        snackbar.Title = this.FindResource("Setting_CopyEmailMessage") as String;
        snackbar.Message = text;
        snackbar.Show();
    }
}