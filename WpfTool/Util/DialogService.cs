using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WpfTool;

/// <summary>
///     WinUI 3 没有 WPF 那样的同步 MessageBox，这里包一层 ContentDialog，
///     调用方可以 fire-and-forget
/// </summary>
internal static class DialogService
{
    public static async void Show(string message, string title = "")
    {
        var dialog = new ContentDialog
        {
            Title = string.IsNullOrEmpty(title) ? Strings.MainWindows_Title : title,
            Content = new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap },
            CloseButtonText = Strings.ResultWindows_DialogClose,
            DefaultButton = ContentDialogButton.Close
        };
        await ShowDialogAsync(dialog);
    }

    public static async Task<ContentDialogResult> ShowDialogAsync(ContentDialog dialog)
    {
        var window = App.Current.MainWindow;
        dialog.XamlRoot = window.Content.XamlRoot;
        try
        {
            return await dialog.ShowAsync();
        }
        catch (Exception)
        {
            // 已有对话框在显示时 WinUI 会抛异常，忽略即可
            return ContentDialogResult.None;
        }
    }
}
