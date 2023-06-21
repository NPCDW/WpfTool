using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfTool.CloudService;

namespace WpfTool
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class ResultWindow : Window
    {
        private Bitmap bmp = null;
        private bool WindowLoaded = false;

        public ResultWindow()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (ComboBoxItem item in this.defaultOcrProvideComboBox.Items)
            {
                if (item.DataContext.ToString().Equals(GlobalConfig.Ocr.defaultOcrProvide.ToString()))
                {
                    defaultOcrProvideComboBox.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in this.defaultOcrTypeComboBox.Items)
            {
                if (item.DataContext.ToString().Equals(GlobalConfig.Ocr.defaultOcrType))
                {
                    defaultOcrTypeComboBox.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in this.defaultOcrLanguageComboBox.Items)
            {
                if (item.DataContext.ToString().Equals(GlobalConfig.Ocr.defaultOcrLanguage))
                {
                    defaultOcrLanguageComboBox.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in this.defaultTranslateProvideComboBox.Items)
            {
                if (item.DataContext.ToString().Equals(GlobalConfig.Translate.defaultTranslateProvide.ToString()))
                {
                    defaultTranslateProvideComboBox.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in this.sourceLanguageComboBox.Items)
            {
                if (item.DataContext.ToString().Equals(GlobalConfig.Translate.defaultTranslateSourceLanguage.ToString()))
                {
                    sourceLanguageComboBox.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in this.targetLanguageComboBox.Items)
            {
                if (item.DataContext.ToString().Equals(GlobalConfig.Translate.defaultTranslateTargetLanguage.ToString()))
                {
                    targetLanguageComboBox.SelectedItem = item;
                    break;
                }
            }

            this.WindowLoaded = true;
        }

        public void translate(String translateProvideStr = null, String sourceLanguage = null, String targetLanguage = null)
        {
            GlobalConfig.Translate.TranslateProvideEnum translateProvide;
            if (string.IsNullOrWhiteSpace(translateProvideStr))
            {
                translateProvide = GlobalConfig.Translate.defaultTranslateProvide;
            }
            else
            {
                translateProvide = (GlobalConfig.Translate.TranslateProvideEnum)Enum.Parse(typeof(GlobalConfig.Translate.TranslateProvideEnum), translateProvideStr);
            }
            if (string.IsNullOrWhiteSpace(sourceLanguage))
            {
                sourceLanguage = GlobalConfig.Translate.defaultTranslateSourceLanguage;
            }
            if (string.IsNullOrWhiteSpace(targetLanguage))
            {
                targetLanguage = GlobalConfig.Translate.defaultTranslateTargetLanguage;
            }
            if (translateProvide == GlobalConfig.Translate.TranslateProvideEnum.TencentCloud)
            {
                if (string.IsNullOrEmpty(GlobalConfig.Translate.TencentCloud.secret_id) || string.IsNullOrEmpty(GlobalConfig.Translate.TencentCloud.secret_key))
                {
                    MessageBox.Show(this.FindResource("ResultWindows_EmptyKeyMessage") as String);
                    return;
                }
                translateTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_translating");
                DispatcherHelper.DoEvents();

                String ocrText = ocrTextBox.Text;
                TencentCloudHelper.translate(ocrText, sourceLanguage, targetLanguage).ContinueWith(result =>
                {
                    translateTextBox.Dispatcher.Invoke(new Action(delegate
                    {
                        translateTextBox.Text = result.Result;
                    }));
                });
            }
            else if (translateProvide == GlobalConfig.Translate.TranslateProvideEnum.BaiduAI)
            {
                if (string.IsNullOrEmpty(GlobalConfig.Translate.BaiduAI.app_id) || string.IsNullOrEmpty(GlobalConfig.Translate.BaiduAI.app_secret))
                {
                    MessageBox.Show(this.FindResource("ResultWindows_EmptyKeyMessage") as String);
                    return;
                }
                translateTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_translating");
                DispatcherHelper.DoEvents();

                String ocrText = ocrTextBox.Text;
                BaiduAIHelper.translate(ocrText, sourceLanguage, targetLanguage).ContinueWith(result =>
                {
                    translateTextBox.Dispatcher.Invoke(new Action(delegate
                    {
                        translateTextBox.Text = result.Result;
                    }));
                });
            }
            else if (translateProvide == GlobalConfig.Translate.TranslateProvideEnum.GoogleCloud)
            {
                translateTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_translating");
                DispatcherHelper.DoEvents();

                String ocrText = ocrTextBox.Text;
                GoogleCloudHelper.translate(ocrText, sourceLanguage, targetLanguage).ContinueWith(result =>
                {
                    translateTextBox.Dispatcher.Invoke(new Action(delegate
                    {
                        translateTextBox.Text = result.Result;
                    }));
                });
            }
        }

        public void ocr(Bitmap bmp, String ocrProvideStr = null, String ocrType = null, String ocrLanguage = null)
        {
            this.bmp = bmp;
            GlobalConfig.Ocr.OcrProvideEnum ocrProvide;
            if (string.IsNullOrWhiteSpace(ocrProvideStr))
            {
                ocrProvide = GlobalConfig.Ocr.defaultOcrProvide;
            }
            else
            {
                ocrProvide = (GlobalConfig.Ocr.OcrProvideEnum)Enum.Parse(typeof(GlobalConfig.Ocr.OcrProvideEnum), ocrProvideStr);
            }
            if (string.IsNullOrWhiteSpace(ocrType))
            {
                ocrType = GlobalConfig.Ocr.defaultOcrType;
            }
            if (string.IsNullOrWhiteSpace(ocrLanguage))
            {
                ocrLanguage = GlobalConfig.Ocr.defaultOcrLanguage;
            }
            if (ocrProvide == GlobalConfig.Ocr.OcrProvideEnum.TencentCloud)
            {
                if (string.IsNullOrEmpty(GlobalConfig.Ocr.TencentCloud.secret_id) || string.IsNullOrEmpty(GlobalConfig.Ocr.TencentCloud.secret_key))
                {
                    MessageBox.Show(this.FindResource("ResultWindows_EmptyKeyMessage") as String);
                    return;
                }
                ocrTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_ocring");
                DispatcherHelper.DoEvents();

                TencentCloudHelper.ocr(bmp, ocrType).ContinueWith(result =>
                {
                    ocrTextBox.Dispatcher.Invoke(new Action(delegate
                    {
                        ocrTextBox.Text = result.Result;
                    }));
                });
            }
            else if (ocrProvide == GlobalConfig.Ocr.OcrProvideEnum.BaiduCloud)
            {
                if (string.IsNullOrEmpty(GlobalConfig.Ocr.BaiduCloud.client_id) || string.IsNullOrEmpty(GlobalConfig.Ocr.BaiduCloud.client_secret))
                {
                    MessageBox.Show(this.FindResource("ResultWindows_EmptyKeyMessage") as String);
                    return;
                }
                ocrTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_ocring");
                DispatcherHelper.DoEvents();

                BaiduCloudHelper.ocr(bmp, ocrType).ContinueWith(result =>
                {
                    ocrTextBox.Dispatcher.Invoke(new Action(delegate
                    {
                        ocrTextBox.Text = result.Result;
                    }));
                });
            }
            else if (ocrProvide == GlobalConfig.Ocr.OcrProvideEnum.SpaceOCR)
            {
                if (string.IsNullOrEmpty(GlobalConfig.Ocr.SpaceOCR.apiKey))
                {
                    MessageBox.Show(this.FindResource("ResultWindows_EmptyKeyMessage") as String);
                    return;
                }
                ocrTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_ocring");
                DispatcherHelper.DoEvents();

                SpaceOCRHelper.ocr(bmp, ocrType, ocrLanguage).ContinueWith(result =>
                {
                    ocrTextBox.Dispatcher.Invoke(new Action(delegate
                    {
                        ocrTextBox.Text = result.Result;
                    }));
                });
            }
        }

        public void screenshotTranslate(Bitmap bmp)
        {
            this.bmp = bmp;
            if (GlobalConfig.Translate.defaultTranslateProvide == GlobalConfig.Translate.TranslateProvideEnum.TencentCloud)
            {
                if (string.IsNullOrEmpty(GlobalConfig.Translate.TencentCloud.secret_id) || string.IsNullOrEmpty(GlobalConfig.Translate.TencentCloud.secret_key))
                {
                    MessageBox.Show(this.FindResource("ResultWindows_EmptyKeyMessage") as String);
                    return;
                }
                ocrTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_ocring");
                translateTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_translating");
                DispatcherHelper.DoEvents();

                TencentCloudHelper.screenshotTranslate(bmp).ContinueWith(result =>
                {
                    Dictionary<String, String> keyValues = result.Result;
                    ocrTextBox.Dispatcher.Invoke(new Action(delegate
                    {
                        ocrTextBox.Text = keyValues["ocrText"];
                    }));
                    translateTextBox.Dispatcher.Invoke(new Action(delegate
                    {
                        translateTextBox.Text = keyValues["translateText"];
                    }));
                });
            }
            else if (GlobalConfig.Translate.defaultTranslateProvide == GlobalConfig.Translate.TranslateProvideEnum.BaiduAI)
            {
                if (string.IsNullOrEmpty(GlobalConfig.Translate.BaiduAI.app_id) || string.IsNullOrEmpty(GlobalConfig.Translate.BaiduAI.app_secret))
                {
                    MessageBox.Show(this.FindResource("ResultWindows_EmptyKeyMessage") as String);
                    return;
                }
                ocrTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_ocring");
                translateTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_translating");
                DispatcherHelper.DoEvents();

                BaiduAIHelper.screenshotTranslate(bmp).ContinueWith(result =>
                {
                    Dictionary<String, String> keyValues = result.Result;
                    ocrTextBox.Dispatcher.Invoke(new Action(delegate
                    {
                        ocrTextBox.Text = keyValues["ocrText"];
                    }));
                    translateTextBox.Dispatcher.Invoke(new Action(delegate
                    {
                        translateTextBox.Text = keyValues["translateText"];
                    }));
                });
            }
            else if (GlobalConfig.Translate.defaultTranslateProvide == GlobalConfig.Translate.TranslateProvideEnum.GoogleCloud)
            {
                MessageBox.Show(this.FindResource("ResultWindows_LimitOcrAndTranslate") as String);
            }
        }

        private void ocrButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.bmp == null)
            {
                MessageBox.Show(this.FindResource("ResultWindows_NotFoundImage") as String);
                return;
            }
            this.ocr(this.bmp, defaultOcrProvideComboBox.DataContext.ToString(), defaultOcrTypeComboBox.DataContext.ToString(), defaultOcrLanguageComboBox.DataContext.ToString());
        }

        private void translateButton_Click(object sender, RoutedEventArgs e)
        {
            this.translate(defaultTranslateProvideComboBox.DataContext.ToString(), sourceLanguageComboBox.DataContext.ToString(), targetLanguageComboBox.DataContext.ToString());

        }

        private void defaultOcrProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            defaultOcrProvideComboBox.DataContext = ((ComboBoxItem)defaultOcrProvideComboBox.SelectedItem).DataContext;
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

        private void OcrTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (defaultOcrTypeComboBox.SelectedItem != null)
            {
                defaultOcrTypeComboBox.DataContext = ((ComboBoxItem)defaultOcrTypeComboBox.SelectedItem).DataContext;
            }
            if (defaultOcrLanguageComboBox.SelectedItem != null)
            {
                defaultOcrLanguageComboBox.DataContext = ((ComboBoxItem)defaultOcrLanguageComboBox.SelectedItem).DataContext;
            }
            if (defaultOcrLanguageComboBox.SelectedItem == null || defaultOcrTypeComboBox.SelectedItem == null)
            {
                return;
            }
            if (defaultOcrProvideComboBox.DataContext.ToString().Equals(GlobalConfig.Ocr.defaultOcrProvide.ToString())
                && defaultOcrTypeComboBox.DataContext.ToString().Equals(GlobalConfig.Ocr.defaultOcrType)
                && defaultOcrLanguageComboBox.DataContext.ToString().Equals(GlobalConfig.Ocr.defaultOcrLanguage))
            {
                defaultOcrSettingCheck.IsChecked = true;
                defaultOcrSettingCheck.IsEnabled = false;
            }
            else
            {
                defaultOcrSettingCheck.IsChecked = false;
                defaultOcrSettingCheck.IsEnabled = true;
            }
        }

        private void defaultOcrSettingCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (!defaultOcrSettingCheck.IsChecked.Value)
            {
                defaultOcrSettingCheck.IsEnabled = true;
                return;
            }
            else
            {
                defaultOcrSettingCheck.IsEnabled = false;
                if (this.WindowLoaded)
                {
                    GlobalConfig.Ocr.defaultOcrProvide = (GlobalConfig.Ocr.OcrProvideEnum)Enum.Parse(typeof(GlobalConfig.Ocr.OcrProvideEnum), defaultOcrProvideComboBox.DataContext.ToString());
                    GlobalConfig.Ocr.defaultOcrType = defaultOcrTypeComboBox.DataContext.ToString();
                    GlobalConfig.Ocr.defaultOcrLanguage = defaultOcrLanguageComboBox.DataContext.ToString();
                    GlobalConfig.SaveConfig();
                }
            }
        }

        private void defaultTranslateProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            defaultTranslateProvideComboBox.DataContext = ((ComboBoxItem)defaultTranslateProvideComboBox.SelectedItem).DataContext;
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

        private void TranslateLanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sourceLanguageComboBox.SelectedItem != null)
            {
                sourceLanguageComboBox.DataContext = ((ComboBoxItem)sourceLanguageComboBox.SelectedItem).DataContext;
            }
            if (targetLanguageComboBox.SelectedItem != null)
            {
                targetLanguageComboBox.DataContext = ((ComboBoxItem)targetLanguageComboBox.SelectedItem).DataContext;
            }
            if (sourceLanguageComboBox.DataContext == null || targetLanguageComboBox.DataContext == null)
            {
                return;
            }
            if (defaultTranslateProvideComboBox.DataContext.ToString().Equals(GlobalConfig.Translate.defaultTranslateProvide.ToString())
                && sourceLanguageComboBox.DataContext.ToString().Equals(GlobalConfig.Translate.defaultTranslateSourceLanguage)
                && targetLanguageComboBox.DataContext.ToString().Equals(GlobalConfig.Translate.defaultTranslateTargetLanguage))
            {
                defaultTranslateSettingCheck.IsChecked = true;
                defaultTranslateSettingCheck.IsEnabled = false;
            }
            else
            {
                defaultTranslateSettingCheck.IsChecked = false;
                defaultTranslateSettingCheck.IsEnabled = true;
            }
        }

        private void defaultTranslateSettingCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (!defaultTranslateSettingCheck.IsChecked.Value)
            {
                defaultTranslateSettingCheck.IsEnabled = true;
                return;
            }
            else
            {
                defaultTranslateSettingCheck.IsEnabled = false;
                if (this.WindowLoaded)
                {
                    GlobalConfig.Translate.defaultTranslateProvide = (GlobalConfig.Translate.TranslateProvideEnum)Enum.Parse(typeof(GlobalConfig.Translate.TranslateProvideEnum), defaultTranslateProvideComboBox.DataContext.ToString());
                    GlobalConfig.Translate.defaultTranslateSourceLanguage = sourceLanguageComboBox.DataContext.ToString();
                    GlobalConfig.Translate.defaultTranslateTargetLanguage = targetLanguageComboBox.DataContext.ToString();
                    GlobalConfig.SaveConfig();
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Utils.FlushMemory();
        }
    }
}
