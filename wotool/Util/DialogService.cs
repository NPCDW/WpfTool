using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using wotool.Util;
using wotool.Views;

namespace wotool;

/// <summary>
///     对话框服务：优先在当前活跃窗口展示 ContentDialog，
///     若无窗口或未就绪则优雅降级为原生弹窗
/// </summary>
internal static class DialogService
{
    public static void Show(string message, string title = "")
    {
        var caption = string.IsNullOrEmpty(title) ? Strings.MainWindows_Title : title;

        void ShowInternal()
        {
            var targetWindow = WindowHelper.GetOpenWindows()
                .LastOrDefault(w => w is not MainWindow && w.Content?.XamlRoot != null)
                ?? (App.Current?.MainWindow?.Content?.XamlRoot != null ? App.Current.MainWindow : null);

            if (targetWindow?.Content?.XamlRoot != null)
            {
                var dialog = new ContentDialog
                {
                    Title = caption,
                    Content = new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap },
                    CloseButtonText = Strings.ResultWindows_DialogClose,
                    DefaultButton = ContentDialogButton.Close,
                    XamlRoot = targetWindow.Content.XamlRoot
                };

                try
                {
                    _ = dialog.ShowAsync();
                    return;
                }
                catch
                {
                    // 已有弹窗打开或抛出异常时降级
                }
            }

            // 兜底降级方案：无需 XamlRoot 的原生消息框
            System.Windows.Forms.MessageBox.Show(message, caption,
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Information);
        }

        try
        {
            if (WindowHelper.Dispatcher?.HasThreadAccess == true)
            {
                ShowInternal();
            }
            else if (WindowHelper.Dispatcher != null)
            {
                WindowHelper.Dispatcher.TryEnqueue(ShowInternal);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(message, caption,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information);
            }
        }
        catch
        {
            System.Windows.Forms.MessageBox.Show(message, caption,
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Information);
        }
    }
}
