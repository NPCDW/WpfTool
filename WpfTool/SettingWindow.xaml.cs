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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfTool
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        private bool WindowLoaded = false;

        public SettingWindow()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            String defaultOcrType = GlobalConfig.Common.defaultOcrType;
            String defaultTranslateSourceLanguage = GlobalConfig.Common.defaultTranslateSourceLanguage;
            String defaultTranslateTargetLanguage = GlobalConfig.Common.defaultTranslateTargetLanguage;
            this.autoStartButton.IsChecked = GlobalConfig.Common.autoStart;
            this.WordSelectionIntervalSlider.Value = GlobalConfig.Common.wordSelectionInterval;

            foreach (ComboBoxItem item in this.defaultOcrProvideComboBox.Items)
            {
                if (item.DataContext.Equals(GlobalConfig.Common.defaultOcrProvide.ToString()))
                {
                    defaultOcrProvideComboBox.SelectedItem = item;
                    break;
                }
            }
            foreach (ComboBoxItem item in this.defaultOcrTypeComboBox.Items)
            {
                if (item.DataContext.ToString().Equals(defaultOcrType))
                {
                    defaultOcrTypeComboBox.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in this.defaultTranslateProvideComboBox.Items)
            {
                if (item.DataContext.Equals(GlobalConfig.Common.defaultTranslateProvide.ToString()))
                {
                    defaultTranslateProvideComboBox.SelectedItem = item;
                    break;
                }
            }
            foreach (ComboBoxItem item in this.sourceLanguageComboBox.Items)
            {
                if (item.DataContext.ToString().Equals(defaultTranslateSourceLanguage))
                {
                    sourceLanguageComboBox.SelectedItem = item;
                    break;
                }
            }
            foreach (ComboBoxItem item in this.targetLanguageComboBox.Items)
            {
                if (item.DataContext.ToString().Equals(defaultTranslateTargetLanguage))
                {
                    targetLanguageComboBox.SelectedItem = item;
                    break;
                }
            }

            this.BaiduCloud_AppKeyInput.Text = GlobalConfig.BaiduCloud.client_id;
            this.BaiduCloud_SecretKeyInput.Password = GlobalConfig.BaiduCloud.client_secret;
            this.BaiduAI_AppIdInput.Text = GlobalConfig.BaiduAI.app_id;
            this.BaiduAI_SecretKeyInput.Password = GlobalConfig.BaiduAI.app_secret;

            this.TencentCloudOcr_SecretIdInput.Text = GlobalConfig.TencentCloud.secret_id;
            this.TencentCloudOcr_SecretKeyInput.Password = GlobalConfig.TencentCloud.secret_key;

            this.TencentCloudTranslate_SecretIdInput.Text = GlobalConfig.TencentCloudTranslate.secret_id;
            this.TencentCloudTranslate_SecretKeyInput.Password = GlobalConfig.TencentCloudTranslate.secret_key;

            this.OcrHotKeyTextBox.Text = GlobalConfig.HotKeys.Ocr.Text;
            this.GetWordsTranslateHotKeyTextBox.Text = GlobalConfig.HotKeys.GetWordsTranslate.Text;
            this.ScreenshotTranslateHotKeyTextBox.Text = GlobalConfig.HotKeys.ScreenshotTranslate.Text;

            HotKeyConflictCheck();

            this.WindowLoaded = true;
        }

        private void LinkLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = ((Label)sender).DataContext.ToString();
            proc.Start();
        }

        private void CopyLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NativeClipboard.SetText(((Label)sender).DataContext.ToString());
            MessageBox.Show("已复制邮件地址");
        }

        private void defaultOcrProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            defaultOcrProvideComboBox.DataContext = ((ComboBoxItem)defaultOcrProvideComboBox.SelectedItem).DataContext;
            if (this.WindowLoaded)
            {
                GlobalConfig.Common.defaultOcrProvide = (GlobalConfig.OcrProvideEnum)Enum.Parse(typeof(GlobalConfig.OcrProvideEnum), defaultOcrProvideComboBox.DataContext.ToString());
            }
            if (defaultOcrProvideComboBox.DataContext.ToString() == GlobalConfig.OcrProvideEnum.BaiduCloud.ToString())
            {
                defaultOcrTypeComboBox.Items.Clear();
                ComboBoxItem item = new ComboBoxItem();
                item.DataContext = GlobalConfig.BaiduCloud.OcrTypeEnum.general_basic.ToString();
                item.Content = "通用";
                defaultOcrTypeComboBox.Items.Add(item);
                ComboBoxItem item2 = new ComboBoxItem();
                item2.DataContext = GlobalConfig.BaiduCloud.OcrTypeEnum.accurate_basic.ToString();
                item2.Content = "高精度";
                defaultOcrTypeComboBox.Items.Add(item2);
                ComboBoxItem item3 = new ComboBoxItem();
                item3.DataContext = GlobalConfig.BaiduCloud.OcrTypeEnum.handwriting.ToString();
                item3.Content = "手写体";
                defaultOcrTypeComboBox.Items.Add(item3);

                defaultOcrTypeComboBox.SelectedItem = item;
            }
            else if (defaultOcrProvideComboBox.DataContext.ToString() == GlobalConfig.OcrProvideEnum.TencentCloud.ToString())
            {
                defaultOcrTypeComboBox.Items.Clear();
                ComboBoxItem item = new ComboBoxItem();
                item.DataContext = GlobalConfig.TencentCloud.OcrTypeEnum.GeneralBasicOCR.ToString();
                item.Content = "通用";
                defaultOcrTypeComboBox.Items.Add(item);
                ComboBoxItem item2 = new ComboBoxItem();
                item2.DataContext = GlobalConfig.TencentCloud.OcrTypeEnum.GeneralAccurateOCR.ToString();
                item2.Content = "高精度";
                defaultOcrTypeComboBox.Items.Add(item2);
                ComboBoxItem item3 = new ComboBoxItem();
                item3.DataContext = GlobalConfig.TencentCloud.OcrTypeEnum.GeneralHandwritingOCR.ToString();
                item3.Content = "手写体";
                defaultOcrTypeComboBox.Items.Add(item3);

                defaultOcrTypeComboBox.SelectedItem = item;
            }
        }

        private void defaultOcrTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (defaultOcrTypeComboBox.SelectedItem == null)
            {
                return;
            }
            defaultOcrTypeComboBox.DataContext = ((ComboBoxItem)defaultOcrTypeComboBox.SelectedItem).DataContext;
            if (this.WindowLoaded)
            {
                GlobalConfig.Common.defaultOcrType = defaultOcrTypeComboBox.DataContext.ToString();
            }
        }

        private void defaultTranslateProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            defaultTranslateProvideComboBox.DataContext = ((ComboBoxItem)defaultTranslateProvideComboBox.SelectedItem).DataContext;
            if (this.WindowLoaded)
            {
                GlobalConfig.Common.defaultTranslateProvide = (GlobalConfig.TranslateProvideEnum)Enum.Parse(typeof(GlobalConfig.TranslateProvideEnum), defaultTranslateProvideComboBox.DataContext.ToString());
            }
            string translateProvide = defaultTranslateProvideComboBox.DataContext.ToString();
            sourceLanguageComboBox.Items.Clear();
            targetLanguageComboBox.Items.Clear();
            if (translateProvide.Equals(GlobalConfig.TranslateProvideEnum.BaiduAI.ToString()))
            {
                foreach (TranslateLanguageAttribute item in TranslateLanguageExtension.TranslateLanguageAttributeList)
                {
                    if (!string.IsNullOrWhiteSpace(item.getBaiduAiCode()))
                    {
                        ComboBoxItem comboBoxItem = new ComboBoxItem();
                        comboBoxItem.DataContext = item.getBaiduAiCode();
                        comboBoxItem.Content = item.getName();
                        sourceLanguageComboBox.Items.Add(comboBoxItem);
                        ComboBoxItem comboBoxItem2 = new ComboBoxItem();
                        comboBoxItem2.DataContext = item.getBaiduAiCode();
                        comboBoxItem2.Content = item.getName();
                        targetLanguageComboBox.Items.Add(comboBoxItem2);
                    }
                }
            }
            else if (translateProvide.Equals(GlobalConfig.TranslateProvideEnum.TencentCloud.ToString()))
            {
                foreach (TranslateLanguageAttribute item in TranslateLanguageExtension.TranslateLanguageAttributeList)
                {
                    if (!string.IsNullOrWhiteSpace(item.getTencentCloudCode()))
                    {
                        ComboBoxItem comboBoxItem = new ComboBoxItem();
                        comboBoxItem.DataContext = item.getBaiduAiCode();
                        comboBoxItem.Content = item.getName();
                        sourceLanguageComboBox.Items.Add(comboBoxItem);
                        ComboBoxItem comboBoxItem2 = new ComboBoxItem();
                        comboBoxItem2.DataContext = item.getBaiduAiCode();
                        comboBoxItem2.Content = item.getName();
                        targetLanguageComboBox.Items.Add(comboBoxItem2);
                    }
                }
            }
            targetLanguageComboBox.Items.RemoveAt(0);
            sourceLanguageComboBox.SelectedItem = sourceLanguageComboBox.Items[0];
            targetLanguageComboBox.SelectedItem = targetLanguageComboBox.Items[0];
        }

        private void sourceLanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sourceLanguageComboBox.SelectedItem == null)
            {
                return;
            }
            sourceLanguageComboBox.DataContext = ((ComboBoxItem)sourceLanguageComboBox.SelectedItem).DataContext;
            if (this.WindowLoaded)
            {
                GlobalConfig.Common.defaultTranslateSourceLanguage = sourceLanguageComboBox.DataContext.ToString();
            }
        }

        private void targetLanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (targetLanguageComboBox.SelectedItem == null)
            {
                return;
            }
            targetLanguageComboBox.DataContext = ((ComboBoxItem)targetLanguageComboBox.SelectedItem).DataContext;
            if (this.WindowLoaded)
            {
                GlobalConfig.Common.defaultTranslateTargetLanguage = targetLanguageComboBox.DataContext.ToString();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GlobalConfig.SaveConfig();
        }

        private void TencentCloudOcr_SecretIdInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.TencentCloud.secret_id = this.TencentCloudOcr_SecretIdInput.Text;
            }
        }

        private void TencentCloudOcr_SecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.TencentCloud.secret_key = this.TencentCloudOcr_SecretKeyInput.Password;
            }
        }

        private void BaiduCloud_AppKeyInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.BaiduCloud.client_id = this.BaiduCloud_AppKeyInput.Text;
            }
        }

        private void BaiduCloud_SecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.BaiduCloud.client_secret = this.BaiduCloud_SecretKeyInput.Password;
            }
        }

        private void TencentCloudTranslate_SecretIdInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.TencentCloudTranslate.secret_id = this.TencentCloudTranslate_SecretIdInput.Text;
            }
        }

        private void TencentCloudTranslate_SecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.TencentCloudTranslate.secret_key = this.TencentCloudTranslate_SecretKeyInput.Password;
            }
        }

        private void BaiduAI_AppIdInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.BaiduAI.app_id = this.BaiduAI_AppIdInput.Text;
            }
        }

        private void BaiduAI_SecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.BaiduAI.app_secret = this.BaiduAI_SecretKeyInput.Password;
            }
        }

        private void autoStartButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                AutoStart.Enable();
                GlobalConfig.Common.autoStart = true;
            }
        }

        private void autoStartButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                AutoStart.Disable();
                GlobalConfig.Common.autoStart = false;
            }
        }

        byte hotkeysModifiers;
        int hotkeysKey;
        String hotkeysText;

        private void HotKeyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeysModifiers = 0;
            hotkeysKey = 0;
            e.Handled = true;
            Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
            if (key == Key.LeftShift || key == Key.RightShift
                || key == Key.LeftCtrl || key == Key.RightCtrl
                || key == Key.LeftAlt || key == Key.RightAlt
                || key == Key.LWin || key == Key.RWin)
            {
                return;
            }
            StringBuilder shortcutText = new StringBuilder();
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                hotkeysModifiers += 2;
                shortcutText.Append("Ctrl + ");
            }
            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                hotkeysModifiers += 4;
                shortcutText.Append("Shift + ");
            }
            if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0)
            {
                hotkeysModifiers += 1;
                shortcutText.Append("Alt + ");
            }
            if (hotkeysModifiers == 0 && (key <= Key.F1 || key >= Key.F12))
            {
                return;
            }
            hotkeysKey = KeyInterop.VirtualKeyFromKey(key);
            shortcutText.Append(key.ToString());
            ((TextBox)sender).Text = hotkeysText = shortcutText.ToString();
        }

        private void OcrHotKeyTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
            if (key == Key.LeftShift || key == Key.RightShift
                || key == Key.LeftCtrl || key == Key.RightCtrl
                || key == Key.LeftAlt || key == Key.RightAlt
                || key == Key.LWin || key == Key.RWin)
            {
                return;
            }
            if (hotkeysKey != 0)
            {
                GlobalConfig.HotKeys.Ocr.Modifiers = hotkeysModifiers;
                GlobalConfig.HotKeys.Ocr.Key = hotkeysKey;
                GlobalConfig.HotKeys.Ocr.Text = hotkeysText.ToString();
                HotKeysUtil.ReRegisterHotKey();
                HotKeyConflictCheck();
            }
        }

        private void GetWordsTranslateHotKeyTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
            if (key == Key.LeftShift || key == Key.RightShift
                || key == Key.LeftCtrl || key == Key.RightCtrl
                || key == Key.LeftAlt || key == Key.RightAlt
                || key == Key.LWin || key == Key.RWin)
            {
                return;
            }
            if (hotkeysKey != 0)
            {
                GlobalConfig.HotKeys.GetWordsTranslate.Modifiers = hotkeysModifiers;
                GlobalConfig.HotKeys.GetWordsTranslate.Key = hotkeysKey;
                GlobalConfig.HotKeys.GetWordsTranslate.Text = hotkeysText.ToString();
                HotKeysUtil.ReRegisterHotKey();
                HotKeyConflictCheck();
            }
        }

        private void ScreenshotTranslateHotKeyTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
            if (key == Key.LeftShift || key == Key.RightShift
                || key == Key.LeftCtrl || key == Key.RightCtrl
                || key == Key.LeftAlt || key == Key.RightAlt
                || key == Key.LWin || key == Key.RWin)
            {
                return;
            }
            if (hotkeysKey != 0)
            {
                GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers = hotkeysModifiers;
                GlobalConfig.HotKeys.ScreenshotTranslate.Key = hotkeysKey;
                GlobalConfig.HotKeys.ScreenshotTranslate.Text = hotkeysText.ToString();
                HotKeysUtil.ReRegisterHotKey();
                HotKeyConflictCheck();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetWordsTranslateHotKeyTextBox.Text = "F2";
            OcrHotKeyTextBox.Text = "F4";
            ScreenshotTranslateHotKeyTextBox.Text = "Ctrl + F2";

            GlobalConfig.HotKeys.GetWordsTranslate.Modifiers = 0;
            GlobalConfig.HotKeys.GetWordsTranslate.Key = 113;
            GlobalConfig.HotKeys.GetWordsTranslate.Text = "F2";
            GlobalConfig.HotKeys.Ocr.Modifiers = 0;
            GlobalConfig.HotKeys.Ocr.Key = 115;
            GlobalConfig.HotKeys.Ocr.Text = "F4";
            GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers = 2;
            GlobalConfig.HotKeys.ScreenshotTranslate.Key = 113;
            GlobalConfig.HotKeys.ScreenshotTranslate.Text = "Ctrl + F2";

            HotKeysUtil.ReRegisterHotKey();
            HotKeyConflictCheck();
        }

        private void WordSelectionIntervalSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.Common.wordSelectionInterval = (int)WordSelectionIntervalSlider.Value;
            }
        }

        private void HotKeyConflictCheck()
        {
            this.OcrHotKeyConflictLabel.Visibility = GlobalConfig.HotKeys.Ocr.Conflict ? Visibility.Visible : Visibility.Hidden;
            this.GetWordsTranslateHotKeyConflictLabel.Visibility = GlobalConfig.HotKeys.GetWordsTranslate.Conflict ? Visibility.Visible : Visibility.Hidden;
            this.ScreenshotTranslateHotKeyConflictLabel.Visibility = GlobalConfig.HotKeys.ScreenshotTranslate.Conflict ? Visibility.Visible : Visibility.Hidden;
        }

    }
}
