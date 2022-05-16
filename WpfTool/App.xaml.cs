using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace WpfTool
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public EventWaitHandle ProgramStarted { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            bool createNew;
            ProgramStarted = new EventWaitHandle(false, EventResetMode.AutoReset, "WpfTool", out createNew);

            if (!createNew)
            {
                MessageBox.Show("程序已经在运行了");
                Environment.Exit(0);
            }
            base.OnStartup(e);
            new MainWindow();

            Utils.FlushMemory();
        }
    }
}
