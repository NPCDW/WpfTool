using System;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using WpfTool.Util;
using WinRT.Interop;

namespace WpfTool.Views;

/// <summary>
///     隐藏的宿主窗口：只为拿到窗口句柄，用来接收全局热键的 WM_HOTKEY 消息。
///     功能入口都在托盘菜单里，与原 WPF 版本一致
/// </summary>
public sealed partial class MainWindow : Window
{
    private const int WmHotKey = 0x0312;
    private NativeMethod.SubclassProc? _subclassProc;

    public MainWindow()
    {
        InitializeComponent();
        HideFromTaskbar();
        RegisterHotKeys();
    }

    private void HideFromTaskbar()
    {
        if (AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.SetBorderAndTitleBar(false, false);
            presenter.IsResizable = false;
        }

        AppWindow.IsShownInSwitchers = false;
        AppWindow.Resize(new Windows.Graphics.SizeInt32(1, 1));
    }

    private void RegisterHotKeys()
    {
        var handle = WindowNative.GetWindowHandle(this);
        HotKeysUtil.RegisterHotKey(handle);

        // WinUI 3 没有 HwndSource，这里用 SetWindowSubclass 接 WndProc
        _subclassProc = WndProc;
        NativeMethod.SetWindowSubclass(handle, _subclassProc, UIntPtr.Zero, UIntPtr.Zero);
    }

    private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam, UIntPtr uIdSubclass,
        UIntPtr dwRefData)
    {
        if (msg == WmHotKey) OnHotKey((int)wParam);
        return NativeMethod.DefSubclassProc(hWnd, msg, wParam, lParam);
    }

    /// <summary>
    ///     热键消息入口
    /// </summary>
    public void OnHotKey(int id)
    {
        switch (id)
        {
            case HotKeysUtil.GetWordsTranslateId:
                WordTranslation();
                break;
            case HotKeysUtil.OcrId:
                WindowHelper.Activate<ScreenshotWindow>(ScreenshotGoalEnum.Ocr);
                break;
            case HotKeysUtil.ScreenshotTranslateId:
                WindowHelper.Activate<ScreenshotWindow>(ScreenshotGoalEnum.Translate);
                break;
            case HotKeysUtil.TopMostId:
                WindowTopMost.Exec(ScreenshotHelper.GetCursorPosition());
                break;
        }
    }

    /// <summary>
    ///     划词翻译：取词 -> 打开结果窗口 -> 自动翻译
    /// </summary>
    public void WordTranslation()
    {
        var words = GetWords.Get();
        var window = WindowHelper.Activate<ResultWindow>();
        if (string.IsNullOrEmpty(words)) return;
        window.OcrTextBox.Text = words.Trim();
        window.StartTranslate();
    }
}
