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
        private System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        public MainWindow()
        {
            GlobalConfig.GetConfig();

            InitHwnd();
            InitialTray();

            if (GlobalConfig.HotKeys.Ocr.Conflict || GlobalConfig.HotKeys.GetWordsTranslate.Conflict || GlobalConfig.HotKeys.ScreenshotTranslate.Conflict)
            {
                MessageBox.Show("全局快捷键有冲突，请您到设置中重新设置");
            }
        }

        public void InitHwnd()
        {
            var helper = new WindowInteropHelper(this);
            helper.EnsureHandle();
        }

        private void InitialTray()
        {
            notifyIcon.BalloonTipText = "程序开始运行";
            notifyIcon.Text = "文字识别/机器翻译";
            notifyIcon.Icon = new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + "Resources\\favicon.ico");
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(1000);

            System.Windows.Forms.MenuItem getWordsTranslationButton = new System.Windows.Forms.MenuItem("划词翻译");
            getWordsTranslationButton.Click += new EventHandler(Translate_Click);

            System.Windows.Forms.MenuItem screenshotTranslationButton = new System.Windows.Forms.MenuItem("截图翻译");
            screenshotTranslationButton.Click += new EventHandler(ScreenshotTranslation_Click);

            System.Windows.Forms.MenuItem ocrButton = new System.Windows.Forms.MenuItem("文字识别");
            ocrButton.Click += new EventHandler(OcrButton_Click);

            System.Windows.Forms.MenuItem settingButton = new System.Windows.Forms.MenuItem("设置");
            settingButton.Click += new EventHandler(Setting_Click);

            System.Windows.Forms.MenuItem exitButton = new System.Windows.Forms.MenuItem("退出");
            exitButton.Click += new EventHandler(Exit_Click);

            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { getWordsTranslationButton, screenshotTranslationButton, ocrButton, settingButton, exitButton };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);
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
            ScreenshotWindow window = new ScreenshotWindow(ScreenshotGoalEnum.ocr);
            window.Show();
            window.Activate();
        }

        private void ScreenshotTranslation_Click(object sender, EventArgs e)
        {
            ScreenshotWindow window = new ScreenshotWindow(ScreenshotGoalEnum.translate);
            window.Show();
            window.Activate();
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
        private void Exit_Click(object sender, EventArgs e)
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
                    Console.WriteLine(wParam.ToString());
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
                    break;
            }
            return IntPtr.Zero;
        }
    }
}
