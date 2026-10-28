using System;
using System.Threading.Tasks;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using wotool.Entity;
using wotool.Util;
using WinRT.Interop;

namespace wotool.Views;

/// <summary>
///     隐藏的宿主窗口：只为拿到窗口句柄，用来接收全局热键的 WM_HOTKEY 消息。
///     功能入口都在托盘菜单里，与原 WPF 版本一致
/// </summary>
public sealed partial class MainWindow : Window
{
    private const int WmHotKey = 0x0312;
    private NativeMethod.SubclassProc? _subclassProc;
    private bool _hotKeysInitialized;

    public MainWindow()
    {
        InitializeComponent();
        HideFromTaskbar();
    }

    private void HideFromTaskbar()
    {
        if (AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.SetBorderAndTitleBar(false, false);
            presenter.IsResizable = false;
        }

        try { AppWindow.IsShownInSwitchers = false; } catch { }
        AppWindow.Resize(new Windows.Graphics.SizeInt32(1, 1));
    }

    public void InitializeHotKeys()
    {
        if (_hotKeysInitialized) return;
        _hotKeysInitialized = true;
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
                WindowHelper.Activate<ScreenshotWindow>();
                break;
        }
    }

    /// <summary>
    ///     先取词，再打开并激活结果窗口进行翻译
    /// </summary>
    public void WordTranslation()
    {
        _ = WordTranslationAsync();
    }

    private async Task WordTranslationAsync()
    {
        // Copy from the foreground app before the result window takes focus.
        GetWords.BeginGet();
        await Task.Delay(GlobalConfig.Common.WordSelectionInterval);
        var words = GetWords.ReadClipboardText();
        var window = WindowHelper.Activate<ResultWindow>();
        window.SetOcrText(words.Trim());
        window.StartTranslate();
    }
}
