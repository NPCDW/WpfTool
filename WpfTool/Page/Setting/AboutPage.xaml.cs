using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfTool.Util;

namespace WpfTool.Page.Setting;

public partial class AboutPage
{
    public AboutPage()
    {
        InitializeComponent();
    }

    private void CopyLabel_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var text = ((Label)sender).DataContext.ToString();
        NativeClipboard.SetText(text!);

        var window = (SettingWindow)Window.GetWindow(this)!;
        var snackbar = window.RootSnackbar;
        snackbar.Title = FindResource("Setting_CopyEmailMessage") as string;
        snackbar.Message = text;
        snackbar.Show();
    }
}