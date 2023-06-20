using System;
using System.Threading;
using System.Windows;

namespace WpfTool
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public EventWaitHandle? ProgramStarted { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            bool createNew;
            ProgramStarted = new EventWaitHandle(false, EventResetMode.AutoReset, "WpfTool", out createNew);

            if (!createNew)
            {
                MessageBox.Show(this.FindResource("App_Run") as String);
                Environment.Exit(0);
            }
            base.OnStartup(e);
            new MainWindow();

            Utils.FlushMemory();
        }
    }
}
