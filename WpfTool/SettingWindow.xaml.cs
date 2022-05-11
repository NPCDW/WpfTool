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
        public SettingWindow()
        {
            InitializeComponent();
        }

        private void LinkLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = ((Label)sender).Content.ToString();
            proc.Start();
        }

        private async void CopyLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NativeClipboard.SetText(((Label)sender).Content.ToString());
            MessageBox.Show("已复制邮件地址");
        }

        private void defaultOcrProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            defaultOcrProvideComboBox.DataContext = ((ComboBoxItem)defaultOcrProvideComboBox.SelectedItem).DataContext;
            GlobalConfig.Common.defaultOcrProvide = (GlobalConfig.OcrProvideEnum)Enum.Parse(typeof(GlobalConfig.OcrProvideEnum), defaultOcrProvideComboBox.DataContext.ToString());
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
            GlobalConfig.Common.defaultOcrType = defaultOcrTypeComboBox.DataContext.ToString();
        }

        private void defaultTranslateProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            defaultTranslateProvideComboBox.DataContext = ((ComboBoxItem)defaultTranslateProvideComboBox.SelectedItem).DataContext;
            GlobalConfig.Common.defaultTranslateProvide = (GlobalConfig.TranslateProvideEnum)Enum.Parse(typeof(GlobalConfig.TranslateProvideEnum), defaultTranslateProvideComboBox.DataContext.ToString());
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
            if (sourceLanguageComboBox.DataContext == null)
            {
                return;
            }
            sourceLanguageComboBox.DataContext = ((ComboBoxItem)sourceLanguageComboBox.SelectedItem).DataContext;
            GlobalConfig.Common.defaultTranslateSourceLanguage = sourceLanguageComboBox.DataContext.ToString();
        }

        private void targetLanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (targetLanguageComboBox.DataContext == null)
            {
                return;
            }
            targetLanguageComboBox.DataContext = ((ComboBoxItem)targetLanguageComboBox.SelectedItem).DataContext;
            GlobalConfig.Common.defaultTranslateTargetLanguage = targetLanguageComboBox.DataContext.ToString();
        }

    }
}
