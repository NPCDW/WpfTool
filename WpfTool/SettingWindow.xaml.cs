using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfTool.Page.Setting;

namespace WpfTool
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Wpf.Ui.Controls.UiWindow
    {
        public SettingWindow()
        {
            DataContext = this;

            Wpf.Ui.Appearance.Watcher.Watch(this);

            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level =
                System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
            
            Loaded += (_, _) => RootNavigation.Navigate(typeof(AboutPage));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GlobalConfig.SaveConfig();
            Utils.FlushMemory();
        }
    }
}