using System;
using System.Drawing;
using System.IO;
using WpfTool.Util;
using WpfTool.Views;

namespace WpfTool;

/// <summary>
///     托盘常驻逻辑从原来的 MainWindow 里拆出来。WinUI 3 没有 NotifyIcon，
///     这里直接复用 WinForms 的 NotifyIcon（仅托盘图标与右键菜单，不涉及窗口）
/// </summary>
internal sealed class TrayIcon : IDisposable
{
    private readonly System.Windows.Forms.NotifyIcon _notifyIcon = new();
    private System.Windows.Forms.ContextMenuStrip? _menu;

    public void Initialize()
    {
        _notifyIcon.Text = Strings.MainWindows_Title;
        _notifyIcon.Icon = new Icon(Path.Combine(AppContext.BaseDirectory, @"Resources\favicon.ico"));
        _notifyIcon.Visible = true;

        BuildMenu();
        _notifyIcon.ShowBalloonTip(1000, Strings.MainWindows_Title, Strings.MainWindows_Running,
            System.Windows.Forms.ToolTipIcon.Info);
    }

    /// <summary>
    ///     切换语言后需要重建菜单与提示文案
    /// </summary>
    public void BuildMenu()
    {
        _menu?.Dispose();

        _menu = new System.Windows.Forms.ContextMenuStrip();
        // 划词翻译需要先取词，走一次主窗口的逻辑
        _menu.Items.Add(Strings.MainWindows_WordTranslation, null,
            (_, _) => App.Current.MainWindow.WordTranslation());
        _menu.Items.Add(Strings.MainWindows_ScreenshotTranslation, null,
            (_, _) => WindowHelper.Activate<ScreenshotWindow>(ScreenshotGoalEnum.Translate));
        _menu.Items.Add(Strings.MainWindows_OCR, null,
            (_, _) => WindowHelper.Activate<ScreenshotWindow>(ScreenshotGoalEnum.Ocr));
        _menu.Items.Add(Strings.MainWindows_TopMostToggle, null, (_, _) =>
            WindowTopMost.Exec(ScreenshotHelper.GetCursorPosition()));
        _menu.Items.Add(Strings.MainWindows_WordFileExtract, null,
            (_, _) => WindowHelper.Activate<WordFileExtractWindow>());
        _menu.Items.Add(new System.Windows.Forms.ToolStripSeparator());
        _menu.Items.Add(Strings.MainWindows_Setting, null, (_, _) => WindowHelper.Activate<SettingWindow>());
        _menu.Items.Add(Strings.MainWindows_Exit, null, (_, _) => App.Current.Quit());

        _notifyIcon.Text = Strings.MainWindows_Title;
        _notifyIcon.ContextMenuStrip = _menu;
    }

    public void Dispose()
    {
        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
        _menu?.Dispose();
    }
}
