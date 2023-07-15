using System;
using System.Threading;
using System.Windows;
using WpfTool.Util;

namespace WpfTool;

/// <summary>
///     App.xaml 的交互逻辑
/// </summary>
public partial class App
{
    public EventWaitHandle? ProgramStarted;

    protected override void OnStartup(StartupEventArgs e)
    {
        bool createNew;
        ProgramStarted = new EventWaitHandle(false, EventResetMode.AutoReset, "WpfTool", out createNew);

        if (!createNew)
        {
            MessageBox.Show(FindResource("App_Run") as string);
            Environment.Exit(0);
        }

        base.OnStartup(e);
        var _ = new MainWindow();

        Utils.FlushMemory();
    }
}