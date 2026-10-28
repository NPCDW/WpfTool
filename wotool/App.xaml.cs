using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.UI.Xaml;
using wotool.Entity;
using wotool.Util;
using wotool.Views;

namespace wotool;

/// <summary>
///     应用入口，等价于原来 WPF 版本的 App.xaml.cs：
///     单实例检测、读取配置、常驻托盘，主窗口只是持有消息循环的隐藏窗口
/// </summary>
public partial class App : Application
{
    private EventWaitHandle? _programStarted;
    private MainWindow? _mainWindow;
    private TrayIcon? _trayIcon;

    public App()
    {
        InitializeComponent();
    }

    public static new App Current => (App)Application.Current;

    public MainWindow MainWindow => _mainWindow!;

    internal TrayIcon? TrayIcon => _trayIcon;

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        UnhandledException += (s, e) =>
        {
            Debug.WriteLine($"[UnhandledException] {e.Message}: {e.Exception}");
        };

        _programStarted = new EventWaitHandle(false, EventResetMode.AutoReset, "wotool", out var createNew);
        if (!createNew)
        {
            DialogService.Show(Strings.App_Run, Strings.MainWindows_Title);
            Environment.Exit(0);
            return;
        }

        GlobalConfig.GetConfig();
        LanguageUtil.SwitchLanguage(GlobalConfig.Common.Language);

        _mainWindow = new MainWindow();
        WindowHelper.Track(_mainWindow);
        _mainWindow.Activate();
        _mainWindow.AppWindow.Hide();
        _mainWindow.InitializeHotKeys();

        // 等主窗口创建后再应用主题，否则没有可刷新的根元素
        ThemeService.Apply(GlobalConfig.Common.Theme);

        _trayIcon = new TrayIcon();
        _trayIcon.Initialize();

        if (GlobalConfig.HotKeys.OcrHotKey.Conflict || GlobalConfig.HotKeys.GetWordsTranslate.Conflict)
            DialogService.Show(Strings.MainWindows_HotkeyConflictMessage, Strings.MainWindows_Title);

        AutoStart.Failed += () => DialogService.Show(Strings.AutoStart_SetFail, Strings.MainWindows_Title);

        Utils.FlushMemory();
    }

    /// <summary>
    ///     退出应用（托盘菜单「退出」）
    /// </summary>
    public void Quit()
    {
        _trayIcon?.Dispose();
        Environment.Exit(0);
    }
}
