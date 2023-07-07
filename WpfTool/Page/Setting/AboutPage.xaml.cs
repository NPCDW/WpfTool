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

    private void LinkLabel_MouseDown(object sender, MouseButtonEventArgs e)
    {
        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo.FileName = ((Label)sender).DataContext.ToString();
        proc.Start();
    }

    private void CopyLabel_MouseDown(object sender, MouseButtonEventArgs e)
    {
        NativeClipboard.SetText(((Label)sender).DataContext.ToString());
        MessageBox.Show(this.FindResource("Setting_CopyEmailMessage") as String);
    }
}