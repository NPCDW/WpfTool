using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using WpfTool.Entity;
using WpfTool.Util;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace WpfTool;

/// <summary>
///     MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow
{
    public static MainWindow? MainWindowInst;
    private readonly NotifyIcon _notifyIcon = new();

    public MainWindow()
    {
        MainWindowInst = this;

        GlobalConfig.GetConfig();
        LanguageUtil.SwitchLanguage(GlobalConfig.Common.Language);

        InitHwnd();
        InitialTray();

        if (GlobalConfig.HotKeys.OcrHotKey.Conflict || GlobalConfig.HotKeys.GetWordsTranslate.Conflict ||
            GlobalConfig.HotKeys.ScreenshotTranslate.Conflict ||
            GlobalConfig.HotKeys.TopMost.Conflict)
            MessageBox.Show(FindResource("MainWindows_HotkeyConflictMessage") as string);
    }

    private void InitHwnd()
    {
        var helper = new WindowInteropHelper(this);
        helper.EnsureHandle();
    }

    public void InitialTray()
    {
        _notifyIcon.BalloonTipText = FindResource("MainWindows_Running") as string;
        _notifyIcon.Text = FindResource("MainWindows_Title") as string;
        _notifyIcon.Icon = new Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\favicon.ico"));
        _notifyIcon.Visible = true;
        _notifyIcon.ShowBalloonTip(1000);

        var childen = new ContextMenuStrip();
        childen.Items.Add(FindResource("MainWindows_WordTranslation") as string, null, Translate_Click);
        childen.Items.Add(FindResource("MainWindows_ScreenshotTranslation") as string, null,
            ScreenshotTranslation_Click);
        childen.Items.Add(FindResource("MainWindows_OCR") as string, null, OcrButton_Click);
        childen.Items.Add(FindResource("MainWindows_TopMostToggle") as string, null, TopMost_Click);
        childen.Items.Add(FindResource("MainWindows_WordFileExtract") as string, null, WordFileExtract_Click);
        childen.Items.Add(FindResource("MainWindows_Setting") as string, null, Setting_Click);
        childen.Items.Add(FindResource("MainWindows_Exit") as string, null, Exit_Click);

        _notifyIcon.ContextMenuStrip = childen;
    }

    private void Translate_Click(object? sender, EventArgs? e)
    {
        var getWordsResult = GetWords.Get();
        ResultWindow? window = null;
        foreach (Window item in Application.Current.Windows)
            if (item is ResultWindow)
            {
                window = (ResultWindow)item;
                window.WindowState = WindowState.Normal;
                window.Activate();
                break;
            }

        if (window == null)
        {
            window = new ResultWindow();
            window.Show();
            window.Activate();
        }

        if (string.IsNullOrEmpty(getWordsResult)) return;
        window.OcrTextBox.Text = getWordsResult.Trim();
        DispatcherHelper.DoEvents();
        window.Translate();
    }

    private void OcrButton_Click(object? sender, EventArgs? e)
    {
        ScreenshotWindow? window = null;
        foreach (Window item in Application.Current.Windows)
            if (item is ScreenshotWindow)
            {
                window = (ScreenshotWindow)item;
                window.WindowState = WindowState.Normal;
                window.Activate();
                break;
            }

        if (window == null)
        {
            window = new ScreenshotWindow(ScreenshotGoalEnum.Ocr);
            window.Show();
            window.Activate();
        }
    }

    private void ScreenshotTranslation_Click(object? sender, EventArgs? e)
    {
        ScreenshotWindow? window = null;
        foreach (Window item in Application.Current.Windows)
            if (item is ScreenshotWindow)
            {
                window = (ScreenshotWindow)item;
                window.WindowState = WindowState.Normal;
                window.Activate();
                break;
            }

        if (window == null)
        {
            window = new ScreenshotWindow(ScreenshotGoalEnum.Translate);
            window.Show();
            window.Activate();
        }
    }

    /// <summary>
    ///     置顶/取消置顶
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TopMost_Click(object? sender, EventArgs? e)
    {
        MessageBox.Show(FindResource("MainWindows_TopMostMessage") as string);
    }

    /// <summary>
    ///     Word图片附件提取
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WordFileExtract_Click(object? sender, EventArgs? e)
    {
        WordFileExtractWindow? window = null;
        foreach (Window item in Application.Current.Windows)
            if (item is WordFileExtractWindow)
            {
                window = (WordFileExtractWindow)item;
                window.WindowState = WindowState.Normal;
                window.Activate();
                break;
            }

        if (window == null)
        {
            window = new WordFileExtractWindow();
            window.Show();
            window.Activate();
        }
    }

    /// <summary>
    ///     设置
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Setting_Click(object? sender, EventArgs? e)
    {
        SettingWindow? window = null;
        foreach (Window item in Application.Current.Windows)
            if (item is SettingWindow)
            {
                window = (SettingWindow)item;
                window.WindowState = WindowState.Normal;
                window.Activate();
                break;
            }

        if (window == null)
        {
            window = new SettingWindow();
            window.Show();
            window.Activate();
        }
    }

    /// <summary>
    ///     退出
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Exit_Click(object? sender, EventArgs e)
    {
        _notifyIcon.Dispose();
        Environment.Exit(0);
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        var handle = new WindowInteropHelper(this).Handle;
        HotKeysUtil.RegisterHotKey(handle);

        var source = HwndSource.FromHwnd(handle);
        source!.AddHook(WndProc);
    }

    /// <summary>
    ///     热键的功能
    /// </summary>
    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handle)
    {
        switch (msg)
        {
            case 0x0312: //这个是window消息定义的 注册的热键消息
                if (wParam.ToString().Equals(HotKeysUtil.GetWordsTranslateId + ""))
                    Translate_Click(null, null);
                else if (wParam.ToString().Equals(HotKeysUtil.OcrId + ""))
                    OcrButton_Click(null, null);
                else if (wParam.ToString().Equals(HotKeysUtil.ScreenshotTranslateId + ""))
                    ScreenshotTranslation_Click(null, null);
                else if (wParam.ToString().Equals(HotKeysUtil.TopMostId + "")) TopMost.Exec();
                break;
        }

        return IntPtr.Zero;
    }
}