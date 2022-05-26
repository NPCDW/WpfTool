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
                if (item.DataContext.ToString().Equals(GlobalConfig.Common.defaultOcrProvide.ToString()))
                {
                    defaultOcrProvideComboBox.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in this.defaultOcrTypeComboBox.Items)
            {
                if (item.DataContext.ToString().Equals(GlobalConfig.Common.defaultOcrType))
                {
                    defaultOcrTypeComboBox.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in this.defaultTranslateProvideComboBox.Items)
            {
                if (item.DataContext.ToString().Equals(GlobalConfig.Common.defaultTranslateProvide.ToString()))
                {
                    defaultTranslateProvideComboBox.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in this.sourceLanguageComboBox.Items)
            {
                if (item.DataContext.ToString().Equals(GlobalConfig.Common.defaultTranslateSourceLanguage.ToString()))
                {
                    sourceLanguageComboBox.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in this.targetLanguageComboBox.Items)
            {
                if (item.DataContext.ToString().Equals(GlobalConfig.Common.defaultTranslateTargetLanguage.ToString()))
                {
                    targetLanguageComboBox.SelectedItem = item;
                    break;
                }
            }

            this.WindowLoaded = true;
        }

        public void translate(String translateProvideStr = null, String sourceLanguage = null, String targetLanguage = null)
        {
            GlobalConfig.TranslateProvideEnum translateProvide;
            if (string.IsNullOrWhiteSpace(translateProvideStr))
            {
                translateProvide = GlobalConfig.Common.defaultTranslateProvide;
            }
            else
            {
                translateProvide = (GlobalConfig.TranslateProvideEnum)Enum.Parse(typeof(GlobalConfig.TranslateProvideEnum), translateProvideStr);
            }
            if (string.IsNullOrWhiteSpace(sourceLanguage))
            {
                sourceLanguage = GlobalConfig.Common.defaultTranslateSourceLanguage;
            }
            if (string.IsNullOrWhiteSpace(targetLanguage))
            {
                targetLanguage = GlobalConfig.Common.defaultTranslateTargetLanguage;
            }
            if (translateProvide == GlobalConfig.TranslateProvideEnum.TencentCloud)
            {
                if (string.IsNullOrEmpty(GlobalConfig.TencentCloudTranslate.secret_id) || string.IsNullOrEmpty(GlobalConfig.TencentCloudTranslate.secret_key))
                {
                    MessageBox.Show("请先设置云服务商提供的秘钥信息，可以到设置中点击链接免费领取");
                    return;
                }
                translateTextBox.Text = "翻译中，请稍等。。。";
                DispatcherHelper.DoEvents();

                translateTextBox.Text = TencentCloudHelper.translate(ocrTextBox.Text, sourceLanguage, targetLanguage);
            }
            else if (translateProvide == GlobalConfig.TranslateProvideEnum.BaiduAI)
            {
                if (string.IsNullOrEmpty(GlobalConfig.BaiduAI.app_id) || string.IsNullOrEmpty(GlobalConfig.BaiduAI.app_secret))
                {
                    MessageBox.Show("请先设置云服务商提供的秘钥信息，可以到设置中点击链接免费领取");
                    return;
                }
                translateTextBox.Text = "翻译中，请稍等。。。";
                DispatcherHelper.DoEvents();

                translateTextBox.Text = BaiduAIHelper.translate(ocrTextBox.Text, sourceLanguage, targetLanguage);
            }
        }

        public void ocr(Bitmap bmp, String ocrProvideStr = null, String ocrType = null)
        {
            this.bmp = bmp;
            GlobalConfig.OcrProvideEnum ocrProvide;
            if (string.IsNullOrWhiteSpace(ocrProvideStr))
            {
                ocrProvide = GlobalConfig.Common.defaultOcrProvide;
            }
            else
            {
                ocrProvide = (GlobalConfig.OcrProvideEnum)Enum.Parse(typeof(GlobalConfig.OcrProvideEnum), ocrProvideStr);
            }
            if (string.IsNullOrWhiteSpace(ocrType))
            {
                ocrType = GlobalConfig.Common.defaultOcrType;
            }
            if (ocrProvide == GlobalConfig.OcrProvideEnum.TencentCloud)
            {
                if (string.IsNullOrEmpty(GlobalConfig.TencentCloud.secret_id) || string.IsNullOrEmpty(GlobalConfig.TencentCloud.secret_key))
                {
                    MessageBox.Show("请先设置云服务商提供的秘钥信息，可以到设置中点击链接免费领取");
                    return;
                }
                ocrTextBox.Text = "识别中，请稍等。。。";
                DispatcherHelper.DoEvents();

                ocrTextBox.Text = TencentCloudHelper.ocr(bmp, ocrType);
            }
            else if (ocrProvide == GlobalConfig.OcrProvideEnum.BaiduCloud)
            {
                if (string.IsNullOrEmpty(GlobalConfig.BaiduCloud.client_id) || string.IsNullOrEmpty(GlobalConfig.BaiduCloud.client_secret))
                {
                    MessageBox.Show("请先设置云服务商提供的秘钥信息，可以到设置中点击链接免费领取");
                    return;
                }
                ocrTextBox.Text = "识别中，请稍等。。。";
                DispatcherHelper.DoEvents();

                ocrTextBox.Text = BaiduCloudHelper.ocr(bmp, ocrType);
            }
        }

        public void screenshotTranslate(Bitmap bmp)
        {
            this.bmp = bmp;
            if (GlobalConfig.Common.defaultTranslateProvide == GlobalConfig.TranslateProvideEnum.TencentCloud)
            {
                if (string.IsNullOrEmpty(GlobalConfig.TencentCloudTranslate.secret_id) || string.IsNullOrEmpty(GlobalConfig.TencentCloudTranslate.secret_key))
                {
                    MessageBox.Show("请先设置云服务商提供的秘钥信息，可以到设置中点击链接免费领取");
                    return;
                }
                ocrTextBox.Text = "识别中，请稍等。。。";
                translateTextBox.Text = "翻译中，请稍等。。。";
                DispatcherHelper.DoEvents();

                Dictionary<String, String> keyValues = TencentCloudHelper.screenshotTranslate(bmp);
                ocrTextBox.Text = keyValues["ocrText"];
                translateTextBox.Text = keyValues["translateText"];
            }
            else if (GlobalConfig.Common.defaultTranslateProvide == GlobalConfig.TranslateProvideEnum.BaiduAI)
            {
                if (string.IsNullOrEmpty(GlobalConfig.BaiduAI.app_id) || string.IsNullOrEmpty(GlobalConfig.BaiduAI.app_secret))
                {
                    MessageBox.Show("请先设置云服务商提供的秘钥信息，可以到设置中点击链接免费领取");
                    return;
                }
                ocrTextBox.Text = "识别中，请稍等。。。";
                translateTextBox.Text = "翻译中，请稍等。。。";
                DispatcherHelper.DoEvents();

                Dictionary<String, String> keyValues = BaiduAIHelper.screenshotTranslate(bmp);
                ocrTextBox.Text = keyValues["ocrText"];
                translateTextBox.Text = keyValues["translateText"];
            }
        }

        private void ocrButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.bmp == null)
            {
                MessageBox.Show("未发现截图");
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
            if (defaultOcrProvideComboBox.DataContext.ToString().Equals(GlobalConfig.Common.defaultOcrProvide.ToString())
                && defaultOcrTypeComboBox.DataContext.ToString().Equals(GlobalConfig.Common.defaultOcrType))
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
                    GlobalConfig.Common.defaultOcrProvide = (GlobalConfig.OcrProvideEnum)Enum.Parse(typeof(GlobalConfig.OcrProvideEnum), defaultOcrProvideComboBox.DataContext.ToString());
                    GlobalConfig.Common.defaultOcrType = defaultOcrTypeComboBox.DataContext.ToString();
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
            if (defaultTranslateProvideComboBox.DataContext.ToString().Equals(GlobalConfig.Common.defaultTranslateProvide.ToString())
                && sourceLanguageComboBox.DataContext.ToString().Equals(GlobalConfig.Common.defaultTranslateSourceLanguage)
                && targetLanguageComboBox.DataContext.ToString().Equals(GlobalConfig.Common.defaultTranslateTargetLanguage))
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
                    GlobalConfig.Common.defaultTranslateProvide = (GlobalConfig.TranslateProvideEnum)Enum.Parse(typeof(GlobalConfig.TranslateProvideEnum), defaultTranslateProvideComboBox.DataContext.ToString());
                    GlobalConfig.Common.defaultTranslateSourceLanguage = sourceLanguageComboBox.DataContext.ToString();
                    GlobalConfig.Common.defaultTranslateTargetLanguage = targetLanguageComboBox.DataContext.ToString();
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
