using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace WpfTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow? mainWindow = null;
        private NotifyIcon notifyIcon = new NotifyIcon();
        public MainWindow()
        {
            mainWindow = this;
            Wpf.Ui.Appearance.Watcher.Watch(this);

            GlobalConfig.GetConfig();
            LanguageUtil.switchLanguage(GlobalConfig.Common.language);

            InitHwnd();
            InitialTray();

            if (GlobalConfig.HotKeys.Ocr.Conflict || GlobalConfig.HotKeys.GetWordsTranslate.Conflict || GlobalConfig.HotKeys.ScreenshotTranslate.Conflict || GlobalConfig.HotKeys.TopMost.Conflict)
            {
                System.Windows.MessageBox.Show(this.FindResource("MainWindows_HotkeyConflictMessage") as String);
            }
        }

        private void InitHwnd()
        {
            var helper = new WindowInteropHelper(this);
            helper.EnsureHandle();
        }

        public void InitialTray()
        {
            System.Windows.Controls.MenuItem TranslateMenuItem = new System.Windows.Controls.MenuItem();
            TranslateMenuItem.Header = this.FindResource("MainWindows_WordTranslation") as String;
            TranslateMenuItem.Click += Translate_Click;

            System.Windows.Controls.MenuItem ScreenshotTranslationMenuItem = new System.Windows.Controls.MenuItem();
            ScreenshotTranslationMenuItem.Header = this.FindResource("MainWindows_ScreenshotTranslation") as String;
            ScreenshotTranslationMenuItem.Click += ScreenshotTranslation_Click;

            System.Windows.Controls.MenuItem OcrButtoMenuItem = new System.Windows.Controls.MenuItem();
            OcrButtoMenuItem.Header = this.FindResource("MainWindows_OCR") as String;
            OcrButtoMenuItem.Click += OcrButton_Click;

            System.Windows.Controls.MenuItem TopMostMenuItem = new System.Windows.Controls.MenuItem();
            TopMostMenuItem.Header = this.FindResource("MainWindows_TopMostToggle") as String;
            TopMostMenuItem.Click += TopMost_Click;

            System.Windows.Controls.MenuItem WordFileExtractMenuItem = new System.Windows.Controls.MenuItem();
            WordFileExtractMenuItem.Header = this.FindResource("MainWindows_WordFileExtract") as String;
            WordFileExtractMenuItem.Click += WordFileExtract_Click;

            System.Windows.Controls.MenuItem SettingMenuItem = new System.Windows.Controls.MenuItem();
            SettingMenuItem.Header = this.FindResource("MainWindows_Setting") as String;
            SettingMenuItem.Click += Setting_Click;

            System.Windows.Controls.MenuItem ExitMenuItem = new System.Windows.Controls.MenuItem();
            ExitMenuItem.Header = this.FindResource("MainWindows_Exit") as String;
            ExitMenuItem.Click += Exit_Click;

            System.Windows.Controls.ContextMenu contextMenu = new System.Windows.Controls.ContextMenu();
            contextMenu.Items.Add(TranslateMenuItem);
            contextMenu.Items.Add(ScreenshotTranslationMenuItem);
            contextMenu.Items.Add(OcrButtoMenuItem);
            contextMenu.Items.Add(TopMostMenuItem);
            contextMenu.Items.Add(WordFileExtractMenuItem);
            contextMenu.Items.Add(SettingMenuItem);
            contextMenu.Items.Add(ExitMenuItem);

            notifyIcon.BeginInit();
            notifyIcon.TooltipText = this.FindResource("MainWindows_Title") as String;
            notifyIcon.Icon = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\favicon.ico"), UriKind.Absolute));
            //notifyIcon.MenuOnRightClick = true;
            //notifyIcon.Menu = contextMenu;
            notifyIcon.Visibility = Visibility.Visible;
            notifyIcon.Register();
            notifyIcon.EndInit();

            Wpf.Ui.Mvvm.Services.NotifyIconService service = new Wpf.Ui.Mvvm.Services.NotifyIconService();
            service.Icon = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\favicon.ico"), UriKind.Absolute));

            notifyIcon.Focus();
            Console.WriteLine(notifyIcon.IsVisible);
            Console.WriteLine(notifyIcon.IsLoaded);
            Console.WriteLine(notifyIcon.IsEnabled);
            Console.WriteLine(notifyIcon.IsInitialized);
            Console.WriteLine(notifyIcon.IsRegistered);
            Console.WriteLine(notifyIcon.Uid);
        }

        private void Translate_Click(object? sender, EventArgs? e)
        {
            String getWordsResult = GetWords.Get();
            ResultWindow? window = null;
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

        private void OcrButton_Click(object? sender, EventArgs? e)
        {
            ScreenshotWindow? window = null;
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

        private void ScreenshotTranslation_Click(object? sender, EventArgs? e)
        {
            ScreenshotWindow? window = null;
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
        private void TopMost_Click(object? sender, EventArgs? e)
        {
            System.Windows.MessageBox.Show(this.FindResource("MainWindows_TopMostMessage") as String);
        }

        /// <summary>
        /// Word图片附件提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordFileExtract_Click(object? sender, EventArgs? e)
        {
            WordFileExtractWindow? window = null;
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
        private void Setting_Click(object? sender, EventArgs? e)
        {
            SettingWindow? window = null;
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
