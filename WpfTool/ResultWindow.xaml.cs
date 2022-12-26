using System;
using System.Collections.Generic;
using System.Drawing;
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
                Task.Factory.StartNew(() =>
                {
                    return TencentCloudHelper.translate(ocrText, sourceLanguage, targetLanguage);
                }).ContinueWith(result =>
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
                Task.Factory.StartNew(() =>
                {
                    return BaiduAIHelper.translate(ocrText, sourceLanguage, targetLanguage);
                }).ContinueWith(result =>
                {
                    translateTextBox.Dispatcher.Invoke(new Action(delegate
                    {
                        translateTextBox.Text = result.Result;
                    }));
                });
            }
        }

        public void ocr(Bitmap bmp, String ocrProvideStr = null, String ocrType = null)
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
            if (ocrProvide == GlobalConfig.Ocr.OcrProvideEnum.TencentCloud)
            {
                if (string.IsNullOrEmpty(GlobalConfig.Ocr.TencentCloud.secret_id) || string.IsNullOrEmpty(GlobalConfig.Ocr.TencentCloud.secret_key))
                {
                    MessageBox.Show(this.FindResource("ResultWindows_EmptyKeyMessage") as String);
                    return;
                }
                ocrTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_ocring");
                DispatcherHelper.DoEvents();

                Task.Factory.StartNew(() =>
                {
                    return TencentCloudHelper.ocr(bmp, ocrType);
                }).ContinueWith(result =>
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

                Task.Factory.StartNew(() =>
                {
                    return BaiduCloudHelper.ocr(bmp, ocrType);
                }).ContinueWith(result =>
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

                Task.Factory.StartNew(() =>
                {
                    return TencentCloudHelper.screenshotTranslate(bmp);
                }).ContinueWith(result =>
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

                Task.Factory.StartNew(() =>
                {
                    return BaiduAIHelper.screenshotTranslate(bmp);
                }).ContinueWith(result =>
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
        }

        private void ocrButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.bmp == null)
            {
                MessageBox.Show(this.FindResource("ResultWindows_NotFoundImage") as String);
                return;
            }
            this.ocr(this.bmp, defaultOcrProvideComboBox.DataContext.ToString(), defaultOcrTypeComboBox.DataContext.ToString());
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
            }
        }

        private void defaultOcrTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (defaultOcrTypeComboBox.SelectedItem == null)
            {
                return;
            }
            defaultOcrTypeComboBox.DataContext = ((ComboBoxItem)defaultOcrTypeComboBox.SelectedItem).DataContext;
            if (defaultOcrProvideComboBox.DataContext.ToString().Equals(GlobalConfig.Ocr.defaultOcrProvide.ToString())
                && defaultOcrTypeComboBox.DataContext.ToString().Equals(GlobalConfig.Ocr.defaultOcrType))
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
