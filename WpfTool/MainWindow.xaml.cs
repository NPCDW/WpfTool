using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow mainWindow = null;
        private System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        public MainWindow()
        {
            mainWindow = this;

            GlobalConfig.GetConfig();
            LanguageUtil.switchLanguage(GlobalConfig.Common.language);

            InitHwnd();
            InitialTray();

            if (GlobalConfig.HotKeys.Ocr.Conflict || GlobalConfig.HotKeys.GetWordsTranslate.Conflict || GlobalConfig.HotKeys.ScreenshotTranslate.Conflict || GlobalConfig.HotKeys.TopMost.Conflict)
            {
                MessageBox.Show(this.FindResource("MainWindows_HotkeyConflictMessage") as String);
            }
        }

        private void InitHwnd()
        {
            var helper = new WindowInteropHelper(this);
            helper.EnsureHandle();
        }

        public void InitialTray()
        {
            notifyIcon.BalloonTipText = this.FindResource("MainWindows_Running") as String;
            notifyIcon.Text = this.FindResource("MainWindows_Title") as String;
            notifyIcon.Icon = new System.Drawing.Icon(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\favicon.ico"));
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(1000);

            System.Windows.Forms.ContextMenuStrip childen = new System.Windows.Forms.ContextMenuStrip();
            childen.Items.Add(this.FindResource("MainWindows_WordTranslation") as String, null, new EventHandler(Translate_Click));
            childen.Items.Add(this.FindResource("MainWindows_ScreenshotTranslation") as String, null, new EventHandler(ScreenshotTranslation_Click));
            childen.Items.Add(this.FindResource("MainWindows_OCR") as String, null, new EventHandler(OcrButton_Click));
            childen.Items.Add(this.FindResource("MainWindows_TopMostToggle") as String, null, new EventHandler(TopMost_Click));
            childen.Items.Add(this.FindResource("MainWindows_WordFileExtract") as String, null, new EventHandler(WordFileExtract_Click));
            childen.Items.Add(this.FindResource("MainWindows_Setting") as String, null, new EventHandler(Setting_Click));
            childen.Items.Add(this.FindResource("MainWindows_Exit") as String, null, Exit_Click);

            notifyIcon.ContextMenuStrip = childen;
        }

        private void Translate_Click(object sender, EventArgs e)
        {
            String getWordsResult = GetWords.Get();
            ResultWindow window = null;
            foreach (Window item in Application.Current.Windows)
            {
                if (item is ResultWindow)
                {
                    window = (ResultWindow)item;
                    window.WindowState = WindowState.Normal;
                    window.Activate();
                    break;
                }
            }
            if (window == null)
            {
                window = new ResultWindow();
                window.Show();
                window.Activate();
            }
            if (string.IsNullOrEmpty(getWordsResult))
            {
                return;
            }
            window.ocrTextBox.Text = getWordsResult.Trim();
            DispatcherHelper.DoEvents();
            window.translate();
        }

        private void OcrButton_Click(object sender, EventArgs e)
        {
            ScreenshotWindow window = null;
            foreach (Window item in Application.Current.Windows)
            {
                if (item is ScreenshotWindow)
                {
                    window = (ScreenshotWindow)item;
                    window.WindowState = WindowState.Normal;
                    window.Activate();
                    break;
                }
            }
            if (window == null)
            {
                window = new ScreenshotWindow(ScreenshotGoalEnum.ocr);
                window.Show();
                window.Activate();
            }
        }

        private void ScreenshotTranslation_Click(object sender, EventArgs e)
        {
            ScreenshotWindow window = null;
            foreach (Window item in Application.Current.Windows)
            {
                if (item is ScreenshotWindow)
                {
                    window = (ScreenshotWindow)item;
                    window.WindowState = WindowState.Normal;
                    window.Activate();
                    break;
                }
            }
            if (window == null)
            {
                window = new ScreenshotWindow(ScreenshotGoalEnum.translate);
                window.Show();
                window.Activate();
            }
        }

        /// <summary>
        /// 置顶/取消置顶
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TopMost_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this.FindResource("MainWindows_TopMostMessage") as String);
        }

        /// <summary>
        /// Word图片附件提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordFileExtract_Click(object sender, EventArgs e)
        {
            WordFileExtractWindow window = null;
            foreach (Window item in Application.Current.Windows)
            {
                if (item is WordFileExtractWindow)
                {
                    window = (WordFileExtractWindow)item;
                    window.WindowState = WindowState.Normal;
                    window.Activate();
                    break;
                }
            }
            if (window == null)
            {
                window = new WordFileExtractWindow();
                window.Show();
                window.Activate();
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Setting_Click(object sender, EventArgs e)
        {
            SettingWindow window = null;
            foreach (Window item in Application.Current.Windows)
            {
                if (item is SettingWindow)
                {
                    window = (SettingWindow)item;
                    window.WindowState = WindowState.Normal;
                    window.Activate();
                    break;
                }
            }
            if (window == null)
            {
                window = new SettingWindow();
                window.Show();
                window.Activate();
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object? sender, EventArgs e)
        {
            notifyIcon.Dispose();
            Environment.Exit(0);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            IntPtr handle = new WindowInteropHelper(this).Handle;
            HotKeysUtil.RegisterHotKey(handle);

            HwndSource source = HwndSource.FromHwnd(handle);
            source.AddHook(WndProc);
        }

        /// <summary>
        /// 热键的功能
        /// </summary>
        /// <param name="m"></param>
        protected IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handle)
        {
            switch (msg)
            {
                case 0x0312: //这个是window消息定义的 注册的热键消息
                    if (wParam.ToString().Equals(HotKeysUtil.GetWordsTranslateId + ""))
                    {
                        this.Translate_Click(null, null);
                    }
                    else if (wParam.ToString().Equals(HotKeysUtil.OcrId + ""))
                    {
                        this.OcrButton_Click(null, null);
                    }
                    else if (wParam.ToString().Equals(HotKeysUtil.ScreenshotTranslateId + ""))
                    {
                        this.ScreenshotTranslation_Click(null, null);
                    }
                    else if (wParam.ToString().Equals(HotKeysUtil.TopMostId + ""))
                    {
                        TopMost.exec();
                    }
                    break;
            }
            return IntPtr.Zero;
        }
    }
}
