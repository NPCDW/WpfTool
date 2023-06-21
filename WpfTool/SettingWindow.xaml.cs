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
    public partial class SettingWindow : Wpf.Ui.Controls.UiWindow
    {
        private bool WindowLoaded = false;

        public SettingWindow()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.autoStartButton.IsChecked = GlobalConfig.Common.autoStart;
            this.WordSelectionIntervalSlider.Value = GlobalConfig.Common.wordSelectionInterval;

            foreach (ComboBoxItem item in this.languageComboBox.Items)
            {
                if (item.DataContext.Equals(GlobalConfig.Common.language.ToString()))
                {
                    languageComboBox.SelectedItem = item;
                    break;
                }
            }

            if (GlobalConfig.USER_DIR_CONFIG_PATH.Equals(GlobalConfig.Common.configPath))
            {
                this.UserConfigRadioButton.IsChecked = true;
            }
            else
            {
                this.AppConfigRadioButton.IsChecked = true;
            }

            String defaultOcrType = GlobalConfig.Ocr.defaultOcrType;
            String defaultOcrLanguage = GlobalConfig.Ocr.defaultOcrLanguage;
            foreach (ComboBoxItem item in this.defaultOcrProvideComboBox.Items)
            {
                if (item.DataContext.Equals(GlobalConfig.Ocr.defaultOcrProvide.ToString()))
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
            foreach (ComboBoxItem item in this.defaultOcrLanguageComboBox.Items)
            {
                if (item.DataContext.ToString().Equals(defaultOcrLanguage))
                {
                    defaultOcrLanguageComboBox.SelectedItem = item;
                    break;
                }
            }

            String defaultTranslateSourceLanguage = GlobalConfig.Translate.defaultTranslateSourceLanguage;
            String defaultTranslateTargetLanguage = GlobalConfig.Translate.defaultTranslateTargetLanguage;
            foreach (ComboBoxItem item in this.defaultTranslateProvideComboBox.Items)
            {
                if (item.DataContext.Equals(GlobalConfig.Translate.defaultTranslateProvide.ToString()))
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

            this.BaiduCloud_AppKeyInput.Text = GlobalConfig.Ocr.BaiduCloud.client_id;
            this.BaiduCloud_SecretKeyInput.Password = GlobalConfig.Ocr.BaiduCloud.client_secret;

            this.TencentCloudOcr_SecretIdInput.Text = GlobalConfig.Ocr.TencentCloud.secret_id;
            this.TencentCloudOcr_SecretKeyInput.Password = GlobalConfig.Ocr.TencentCloud.secret_key;

            this.SpaceOCR_ApiKeyInput.Password = GlobalConfig.Ocr.SpaceOCR.apiKey;

            this.BaiduAI_AppIdInput.Text = GlobalConfig.Translate.BaiduAI.app_id;
            this.BaiduAI_SecretKeyInput.Password = GlobalConfig.Translate.BaiduAI.app_secret;

            this.TencentCloudTranslate_SecretIdInput.Text = GlobalConfig.Translate.TencentCloud.secret_id;
            this.TencentCloudTranslate_SecretKeyInput.Password = GlobalConfig.Translate.TencentCloud.secret_key;

            this.OcrHotKeyTextBox.Text = GlobalConfig.HotKeys.Ocr.Text;
            this.GetWordsTranslateHotKeyTextBox.Text = GlobalConfig.HotKeys.GetWordsTranslate.Text;
            this.ScreenshotTranslateHotKeyTextBox.Text = GlobalConfig.HotKeys.ScreenshotTranslate.Text;
            this.TopMostHotKeyTextBox.Text = GlobalConfig.HotKeys.TopMost.Text;

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
            MessageBox.Show(this.FindResource("Setting_CopyEmailMessage") as String);
        }

        private void defaultOcrProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            defaultOcrProvideComboBox.DataContext = ((ComboBoxItem)defaultOcrProvideComboBox.SelectedItem).DataContext;
            if (this.WindowLoaded)
            {
                GlobalConfig.Ocr.defaultOcrProvide = (GlobalConfig.Ocr.OcrProvideEnum)Enum.Parse(typeof(GlobalConfig.Ocr.OcrProvideEnum), defaultOcrProvideComboBox.DataContext.ToString());
            }
            if (defaultOcrProvideComboBox.DataContext.ToString() == GlobalConfig.Ocr.OcrProvideEnum.BaiduCloud.ToString())
            {
                defaultOcrTypeComboBox.Items.Clear();
                ComboBoxItem item = new ComboBoxItem();
                item.DataContext = GlobalConfig.Ocr.BaiduCloud.OcrTypeEnum.general_basic.ToString();
                item.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_GeneralBasic");
                defaultOcrTypeComboBox.Items.Add(item);
                ComboBoxItem item2 = new ComboBoxItem();
                item2.DataContext = GlobalConfig.Ocr.BaiduCloud.OcrTypeEnum.accurate_basic.ToString();
                item2.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_AccurateBasic");
                defaultOcrTypeComboBox.Items.Add(item2);
                ComboBoxItem item3 = new ComboBoxItem();
                item3.DataContext = GlobalConfig.Ocr.BaiduCloud.OcrTypeEnum.handwriting.ToString();
                item3.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_Handwriting");
                defaultOcrTypeComboBox.Items.Add(item3);

                defaultOcrTypeComboBox.SelectedItem = item;

                defaultOcrLanguageComboBox.Items.Clear();
                ComboBoxItem item4 = new ComboBoxItem();
                item4.DataContext = "auto";
                item4.SetResourceReference(ComboBoxItem.ContentProperty, "Language_auto");
                defaultOcrLanguageComboBox.Items.Add(item4);

                defaultOcrLanguageComboBox.SelectedItem = item4;
            }
            else if (defaultOcrProvideComboBox.DataContext.ToString() == GlobalConfig.Ocr.OcrProvideEnum.TencentCloud.ToString())
            {
                defaultOcrTypeComboBox.Items.Clear();
                ComboBoxItem item = new ComboBoxItem();
                item.DataContext = GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralBasicOCR.ToString();
                item.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_GeneralBasic");
                defaultOcrTypeComboBox.Items.Add(item);
                ComboBoxItem item2 = new ComboBoxItem();
                item2.DataContext = GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralAccurateOCR.ToString();
                item2.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_AccurateBasic");
                defaultOcrTypeComboBox.Items.Add(item2);
                ComboBoxItem item3 = new ComboBoxItem();
                item3.DataContext = GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralHandwritingOCR.ToString();
                item3.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_Handwriting");
                defaultOcrTypeComboBox.Items.Add(item3);

                defaultOcrTypeComboBox.SelectedItem = item;

                defaultOcrLanguageComboBox.Items.Clear();
                ComboBoxItem item4 = new ComboBoxItem();
                item4.DataContext = "auto";
                item4.SetResourceReference(ComboBoxItem.ContentProperty, "Language_auto");
                defaultOcrLanguageComboBox.Items.Add(item4);

                defaultOcrLanguageComboBox.SelectedItem = item4;
            }
            else if (defaultOcrProvideComboBox.DataContext.ToString() == GlobalConfig.Ocr.OcrProvideEnum.SpaceOCR.ToString())
            {
                defaultOcrTypeComboBox.Items.Clear();
                ComboBoxItem item = new ComboBoxItem();
                item.DataContext = GlobalConfig.Ocr.SpaceOCR.OcrTypeEnum.Engine1.ToString();
                item.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_Engine1");
                defaultOcrTypeComboBox.Items.Add(item);
                ComboBoxItem item2 = new ComboBoxItem();
                item2.DataContext = GlobalConfig.Ocr.SpaceOCR.OcrTypeEnum.Engine2.ToString();
                item2.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_Engine2");
                defaultOcrTypeComboBox.Items.Add(item2);
                ComboBoxItem item3 = new ComboBoxItem();
                item3.DataContext = GlobalConfig.Ocr.SpaceOCR.OcrTypeEnum.Engine3.ToString();
                item3.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_Engine3");
                defaultOcrTypeComboBox.Items.Add(item3);
                ComboBoxItem item5 = new ComboBoxItem();
                item5.DataContext = GlobalConfig.Ocr.SpaceOCR.OcrTypeEnum.Engine5.ToString();
                item5.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_Engine5");
                defaultOcrTypeComboBox.Items.Add(item5);

                defaultOcrTypeComboBox.SelectedItem = item;

                defaultOcrLanguageComboBox.Items.Clear();
                foreach (OcrLanguageAttribute item4 in OcrLanguageExtension.TranslateLanguageAttributeList)
                {
                    if (!string.IsNullOrWhiteSpace(item4.getSpaceOcrCode()))
                    {
                        ComboBoxItem comboBoxItem = new ComboBoxItem();
                        comboBoxItem.DataContext = item4.getSpaceOcrCode();
                        comboBoxItem.SetResourceReference(ComboBoxItem.ContentProperty, item4.getName());
                        defaultOcrLanguageComboBox.Items.Add(comboBoxItem);
                    }
                }
                defaultOcrLanguageComboBox.SelectedItem = defaultOcrLanguageComboBox.Items[0];
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
                GlobalConfig.Ocr.defaultOcrType = defaultOcrTypeComboBox.DataContext.ToString();
            }
        }

        private void defaultTranslateProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            defaultTranslateProvideComboBox.DataContext = ((ComboBoxItem)defaultTranslateProvideComboBox.SelectedItem).DataContext;
            if (this.WindowLoaded)
            {
                GlobalConfig.Translate.defaultTranslateProvide = (GlobalConfig.Translate.TranslateProvideEnum)Enum.Parse(typeof(GlobalConfig.Translate.TranslateProvideEnum), defaultTranslateProvideComboBox.DataContext.ToString());
            }
            string translateProvide = defaultTranslateProvideComboBox.DataContext.ToString();
            sourceLanguageComboBox.Items.Clear();
            targetLanguageComboBox.Items.Clear();
            if (translateProvide.Equals(GlobalConfig.Translate.TranslateProvideEnum.BaiduAI.ToString()))
            {
                foreach (TranslateLanguageAttribute item in TranslateLanguageExtension.TranslateLanguageAttributeList)
                {
                    if (!string.IsNullOrWhiteSpace(item.getBaiduAiCode()))
                    {
                        ComboBoxItem comboBoxItem = new ComboBoxItem();
                        comboBoxItem.DataContext = item.getBaiduAiCode();
                        comboBoxItem.SetResourceReference(ComboBoxItem.ContentProperty, item.getName());
                        sourceLanguageComboBox.Items.Add(comboBoxItem);
                        ComboBoxItem comboBoxItem2 = new ComboBoxItem();
                        comboBoxItem2.DataContext = item.getBaiduAiCode();
                        comboBoxItem2.SetResourceReference(ComboBoxItem.ContentProperty, item.getName());
                        targetLanguageComboBox.Items.Add(comboBoxItem2);
                    }
                }
            }
            else if (translateProvide.Equals(GlobalConfig.Translate.TranslateProvideEnum.TencentCloud.ToString()))
            {
                foreach (TranslateLanguageAttribute item in TranslateLanguageExtension.TranslateLanguageAttributeList)
                {
                    if (!string.IsNullOrWhiteSpace(item.getTencentCloudCode()))
                    {
                        ComboBoxItem comboBoxItem = new ComboBoxItem();
                        comboBoxItem.DataContext = item.getTencentCloudCode();
                        comboBoxItem.SetResourceReference(ComboBoxItem.ContentProperty, item.getName());
                        sourceLanguageComboBox.Items.Add(comboBoxItem);
                        ComboBoxItem comboBoxItem2 = new ComboBoxItem();
                        comboBoxItem2.DataContext = item.getTencentCloudCode();
                        comboBoxItem2.SetResourceReference(ComboBoxItem.ContentProperty, item.getName());
                        targetLanguageComboBox.Items.Add(comboBoxItem2);
                    }
                }
            }
            else if (translateProvide.Equals(GlobalConfig.Translate.TranslateProvideEnum.GoogleCloud.ToString()))
            {
                foreach (TranslateLanguageAttribute item in TranslateLanguageExtension.TranslateLanguageAttributeList)
                {
                    if (!string.IsNullOrWhiteSpace(item.getGoogleCloudCode()))
                    {
                        ComboBoxItem comboBoxItem = new ComboBoxItem();
                        comboBoxItem.DataContext = item.getGoogleCloudCode();
                        comboBoxItem.SetResourceReference(ComboBoxItem.ContentProperty, item.getName());
                        sourceLanguageComboBox.Items.Add(comboBoxItem);
                        ComboBoxItem comboBoxItem2 = new ComboBoxItem();
                        comboBoxItem2.DataContext = item.getGoogleCloudCode();
                        comboBoxItem2.SetResourceReference(ComboBoxItem.ContentProperty, item.getName());
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
                GlobalConfig.Translate.defaultTranslateSourceLanguage = sourceLanguageComboBox.DataContext.ToString();
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
                GlobalConfig.Translate.defaultTranslateTargetLanguage = targetLanguageComboBox.DataContext.ToString();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GlobalConfig.SaveConfig();
            Utils.FlushMemory();
        }

        private void TencentCloudOcr_SecretIdInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.Ocr.TencentCloud.secret_id = this.TencentCloudOcr_SecretIdInput.Text;
            }
        }

        private void TencentCloudOcr_SecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.Ocr.TencentCloud.secret_key = this.TencentCloudOcr_SecretKeyInput.Password;
            }
        }

        private void BaiduCloud_AppKeyInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.Ocr.BaiduCloud.client_id = this.BaiduCloud_AppKeyInput.Text;
            }
        }

        private void BaiduCloud_SecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.Ocr.BaiduCloud.client_secret = this.BaiduCloud_SecretKeyInput.Password;
            }
        }

        private void TencentCloudTranslate_SecretIdInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.Translate.TencentCloud.secret_id = this.TencentCloudTranslate_SecretIdInput.Text;
            }
        }

        private void TencentCloudTranslate_SecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.Translate.TencentCloud.secret_key = this.TencentCloudTranslate_SecretKeyInput.Password;
            }
        }

        private void BaiduAI_AppIdInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.Translate.BaiduAI.app_id = this.BaiduAI_AppIdInput.Text;
            }
        }

        private void BaiduAI_SecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.Translate.BaiduAI.app_secret = this.BaiduAI_SecretKeyInput.Password;
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
        string hotkeysText = "";

        private void HotKeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            hotkeysModifiers = 0;
            hotkeysKey = 0;
            hotkeysText = "";
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
            if (hotkeysModifiers == 0 && (key < Key.F1 || key > Key.F12))
            {
                hotkeysKey = 0;
                shortcutText.Clear();
                ((TextBox)sender).Text = hotkeysText = "";
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
            GlobalConfig.HotKeys.Ocr.Modifiers = hotkeysModifiers;
            GlobalConfig.HotKeys.Ocr.Key = hotkeysKey;
            GlobalConfig.HotKeys.Ocr.Text = hotkeysText.ToString();
            HotKeysUtil.ReRegisterHotKey();
            HotKeyConflictCheck();
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
            GlobalConfig.HotKeys.GetWordsTranslate.Modifiers = hotkeysModifiers;
            GlobalConfig.HotKeys.GetWordsTranslate.Key = hotkeysKey;
            GlobalConfig.HotKeys.GetWordsTranslate.Text = hotkeysText.ToString();
            HotKeysUtil.ReRegisterHotKey();
            HotKeyConflictCheck();
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
            GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers = hotkeysModifiers;
            GlobalConfig.HotKeys.ScreenshotTranslate.Key = hotkeysKey;
            GlobalConfig.HotKeys.ScreenshotTranslate.Text = hotkeysText.ToString();
            HotKeysUtil.ReRegisterHotKey();
            HotKeyConflictCheck();
        }

        private void TopMostHotKeyTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
            if (key == Key.LeftShift || key == Key.RightShift
                || key == Key.LeftCtrl || key == Key.RightCtrl
                || key == Key.LeftAlt || key == Key.RightAlt
                || key == Key.LWin || key == Key.RWin)
            {
                return;
            }
            GlobalConfig.HotKeys.TopMost.Modifiers = hotkeysModifiers;
            GlobalConfig.HotKeys.TopMost.Key = hotkeysKey;
            GlobalConfig.HotKeys.TopMost.Text = hotkeysText.ToString();
            HotKeysUtil.ReRegisterHotKey();
            HotKeyConflictCheck();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetWordsTranslateHotKeyTextBox.Text = "F2";
            OcrHotKeyTextBox.Text = "F4";
            ScreenshotTranslateHotKeyTextBox.Text = "Ctrl + F2";
            TopMostHotKeyTextBox.Text = "F6";

            GlobalConfig.HotKeys.GetWordsTranslate.Modifiers = 0;
            GlobalConfig.HotKeys.GetWordsTranslate.Key = 113;
            GlobalConfig.HotKeys.GetWordsTranslate.Text = "F2";
            GlobalConfig.HotKeys.Ocr.Modifiers = 0;
            GlobalConfig.HotKeys.Ocr.Key = 115;
            GlobalConfig.HotKeys.Ocr.Text = "F4";
            GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers = 2;
            GlobalConfig.HotKeys.ScreenshotTranslate.Key = 113;
            GlobalConfig.HotKeys.ScreenshotTranslate.Text = "Ctrl + F2";
            GlobalConfig.HotKeys.TopMost.Modifiers = 0;
            GlobalConfig.HotKeys.TopMost.Key = 117;
            GlobalConfig.HotKeys.TopMost.Text = "F6";

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
            this.TopMostHotKeyConflictLabel.Visibility = GlobalConfig.HotKeys.TopMost.Conflict ? Visibility.Visible : Visibility.Hidden;
        }

        private void ConfigButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(GlobalConfig.Common.configPath);
            System.Diagnostics.Process.Start("Explorer.exe", "/select," + GlobalConfig.Common.configPath);
        }

        private void UserConfigRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.Common.configPath = GlobalConfig.USER_DIR_CONFIG_PATH;
            }
        }

        private void AppConfigRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.Common.configPath = GlobalConfig.APP_DIR_CONFIG_PATH;
            }
        }

        private void languageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                string lang = ((ComboBoxItem)((ComboBox)sender).SelectedItem).DataContext.ToString();
                LanguageUtil.switchLanguage(lang);
                GlobalConfig.Common.language = lang;
                MainWindow.mainWindow.InitialTray();
            }
        }

        private void defaultOcrLanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (defaultOcrLanguageComboBox.SelectedItem == null)
            {
                return;
            }
            defaultOcrLanguageComboBox.DataContext = ((ComboBoxItem)defaultOcrLanguageComboBox.SelectedItem).DataContext;
            if (this.WindowLoaded)
            {
                GlobalConfig.Ocr.defaultOcrLanguage = defaultOcrLanguageComboBox.DataContext.ToString();
            }
        }

        private void SpaceOCR_ApiKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.WindowLoaded)
            {
                GlobalConfig.Ocr.SpaceOCR.apiKey = this.SpaceOCR_ApiKeyInput.Password;
            }
        }
    }
}
