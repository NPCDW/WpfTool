using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using WpfTool.CloudService;
using WpfTool.Entity;
using WpfTool.Util;

namespace WpfTool;

/// <summary>
///     Window1.xaml 的交互逻辑
/// </summary>
public partial class ResultWindow
{
    private Bitmap? _bmp;
    private bool _windowLoaded;

    public ResultWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        foreach (ComboBoxItem item in DefaultOcrProvideComboBox.Items)
            if (item.DataContext.ToString()!.Equals(GlobalConfig.Ocr.DefaultOcrProvide.ToString()))
            {
                DefaultOcrProvideComboBox.SelectedItem = item;
                break;
            }

        foreach (ComboBoxItem item in DefaultOcrTypeComboBox.Items)
            if (item.DataContext.ToString()!.Equals(GlobalConfig.Ocr.DefaultOcrType))
            {
                DefaultOcrTypeComboBox.SelectedItem = item;
                break;
            }

        foreach (ComboBoxItem item in DefaultOcrLanguageComboBox.Items)
            if (item.DataContext.ToString()!.Equals(GlobalConfig.Ocr.DefaultOcrLanguage))
            {
                DefaultOcrLanguageComboBox.SelectedItem = item;
                break;
            }

        foreach (ComboBoxItem item in DefaultTranslateProvideComboBox.Items)
            if (item.DataContext.ToString()!.Equals(GlobalConfig.Translate.DefaultTranslateProvide.ToString()))
            {
                DefaultTranslateProvideComboBox.SelectedItem = item;
                break;
            }

        foreach (ComboBoxItem item in SourceLanguageComboBox.Items)
            if (item.DataContext.ToString()!.Equals(GlobalConfig.Translate.DefaultTranslateSourceLanguage))
            {
                SourceLanguageComboBox.SelectedItem = item;
                break;
            }

        foreach (ComboBoxItem item in TargetLanguageComboBox.Items)
            if (item.DataContext.ToString()!.Equals(GlobalConfig.Translate.DefaultTranslateTargetLanguage))
            {
                TargetLanguageComboBox.SelectedItem = item;
                break;
            }

        _windowLoaded = true;
    }

    public void Translate(string? translateProvideStr = null, string? sourceLanguage = null,
        string? targetLanguage = null)
    {
        GlobalConfig.Translate.TranslateProvideEnum translateProvide;
        if (string.IsNullOrWhiteSpace(translateProvideStr))
            translateProvide = GlobalConfig.Translate.DefaultTranslateProvide;
        else
            translateProvide =
                (GlobalConfig.Translate.TranslateProvideEnum)Enum.Parse(
                    typeof(GlobalConfig.Translate.TranslateProvideEnum), translateProvideStr);
        if (string.IsNullOrWhiteSpace(sourceLanguage))
            sourceLanguage = GlobalConfig.Translate.DefaultTranslateSourceLanguage!;
        if (string.IsNullOrWhiteSpace(targetLanguage))
            targetLanguage = GlobalConfig.Translate.DefaultTranslateTargetLanguage!;
        if (translateProvide == GlobalConfig.Translate.TranslateProvideEnum.TencentCloud)
        {
            if (string.IsNullOrEmpty(GlobalConfig.Translate.TencentCloud.SecretId) ||
                string.IsNullOrEmpty(GlobalConfig.Translate.TencentCloud.SecretKey))
            {
                RootDialog.Show("", (FindResource("ResultWindows_EmptyKeyMessage") as string)!);
                return;
            }

            TranslateTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_translating");
            DispatcherHelper.DoEvents();

            var ocrText = OcrTextBox.Text;
            TencentCloudHelper.Translate(ocrText, sourceLanguage, targetLanguage).ContinueWith(result =>
            {
                TranslateTextBox.Dispatcher.Invoke(delegate { TranslateTextBox.Text = result.Result; });
            });
        }
        else if (translateProvide == GlobalConfig.Translate.TranslateProvideEnum.BaiduAi)
        {
            if (string.IsNullOrEmpty(GlobalConfig.Translate.BaiduAi.AppId) ||
                string.IsNullOrEmpty(GlobalConfig.Translate.BaiduAi.AppSecret))
            {
                RootDialog.Show("", (FindResource("ResultWindows_EmptyKeyMessage") as string)!);
                return;
            }

            TranslateTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_translating");
            DispatcherHelper.DoEvents();

            var ocrText = OcrTextBox.Text;
            BaiduAiHelper.Translate(ocrText, sourceLanguage, targetLanguage).ContinueWith(result =>
            {
                TranslateTextBox.Dispatcher.Invoke(delegate { TranslateTextBox.Text = result.Result; });
            });
        }
        else if (translateProvide == GlobalConfig.Translate.TranslateProvideEnum.GoogleCloud)
        {
            TranslateTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_translating");
            DispatcherHelper.DoEvents();

            var ocrText = OcrTextBox.Text;
            GoogleCloudHelper.Translate(ocrText, sourceLanguage, targetLanguage).ContinueWith(result =>
            {
                TranslateTextBox.Dispatcher.Invoke(delegate { TranslateTextBox.Text = result.Result; });
            });
        }
        else if (translateProvide == GlobalConfig.Translate.TranslateProvideEnum.Deeplx)
        {
            if (string.IsNullOrEmpty(GlobalConfig.Translate.Deeplx.Url))
            {
                RootDialog.Show("", (FindResource("ResultWindows_EmptyKeyMessage") as string)!);
                return;
            }

            TranslateTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_translating");
            DispatcherHelper.DoEvents();

            var ocrText = OcrTextBox.Text;
            DeeplxHelper.Translate(ocrText, sourceLanguage, targetLanguage).ContinueWith(result =>
            {
                TranslateTextBox.Dispatcher.Invoke(delegate { TranslateTextBox.Text = result.Result; });
            });
        }
    }

    public void Ocr(Bitmap bmp, string? ocrProvideStr = null, string? ocrType = null, string? ocrLanguage = null)
    {
        _bmp = bmp;
        GlobalConfig.Ocr.OcrProvideEnum ocrProvide;
        if (string.IsNullOrWhiteSpace(ocrProvideStr))
            ocrProvide = GlobalConfig.Ocr.DefaultOcrProvide;
        else
            ocrProvide =
                (GlobalConfig.Ocr.OcrProvideEnum)Enum.Parse(typeof(GlobalConfig.Ocr.OcrProvideEnum), ocrProvideStr);
        if (string.IsNullOrWhiteSpace(ocrType)) ocrType = GlobalConfig.Ocr.DefaultOcrType;
        if (string.IsNullOrWhiteSpace(ocrLanguage)) ocrLanguage = GlobalConfig.Ocr.DefaultOcrLanguage;
        if (ocrProvide == GlobalConfig.Ocr.OcrProvideEnum.TencentCloud)
        {
            if (string.IsNullOrEmpty(GlobalConfig.Ocr.TencentCloud.SecretId) ||
                string.IsNullOrEmpty(GlobalConfig.Ocr.TencentCloud.SecretKey))
            {
                RootDialog.Show("", (FindResource("ResultWindows_EmptyKeyMessage") as string)!);
                return;
            }

            OcrTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_ocring");
            DispatcherHelper.DoEvents();

            TencentCloudHelper.Ocr(bmp, ocrType).ContinueWith(result =>
            {
                OcrTextBox.Dispatcher.Invoke(delegate { OcrTextBox.Text = result.Result; });
            });
        }
        else if (ocrProvide == GlobalConfig.Ocr.OcrProvideEnum.BaiduCloud)
        {
            if (string.IsNullOrEmpty(GlobalConfig.Ocr.BaiduCloud.ClientId) ||
                string.IsNullOrEmpty(GlobalConfig.Ocr.BaiduCloud.ClientSecret))
            {
                RootDialog.Show("", (FindResource("ResultWindows_EmptyKeyMessage") as string)!);
                return;
            }

            OcrTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_ocring");
            DispatcherHelper.DoEvents();

            BaiduCloudHelper.Ocr(bmp, ocrType).ContinueWith(result =>
            {
                OcrTextBox.Dispatcher.Invoke(delegate { OcrTextBox.Text = result.Result; });
            });
        }
        else if (ocrProvide == GlobalConfig.Ocr.OcrProvideEnum.SpaceOcr)
        {
            if (string.IsNullOrEmpty(GlobalConfig.Ocr.SpaceOcr.ApiKey))
            {
                RootDialog.Show("", (FindResource("ResultWindows_EmptyKeyMessage") as string)!);
                return;
            }

            OcrTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_ocring");
            DispatcherHelper.DoEvents();

            SpaceOcrHelper.Ocr(bmp, ocrType, ocrLanguage).ContinueWith(result =>
            {
                OcrTextBox.Dispatcher.Invoke(delegate { OcrTextBox.Text = result.Result; });
            });
        }
    }

    public void ScreenshotTranslate(Bitmap bmp)
    {
        _bmp = bmp;
        if (GlobalConfig.Translate.DefaultTranslateProvide == GlobalConfig.Translate.TranslateProvideEnum.TencentCloud)
        {
            if (string.IsNullOrEmpty(GlobalConfig.Translate.TencentCloud.SecretId) ||
                string.IsNullOrEmpty(GlobalConfig.Translate.TencentCloud.SecretKey))
            {
                RootDialog.Show("", (FindResource("ResultWindows_EmptyKeyMessage") as string)!);
                return;
            }

            OcrTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_ocring");
            TranslateTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_translating");
            DispatcherHelper.DoEvents();

            TencentCloudHelper.ScreenshotTranslate(bmp).ContinueWith(result =>
            {
                var keyValues = result.Result;
                OcrTextBox.Dispatcher.Invoke(delegate { OcrTextBox.Text = keyValues["ocrText"]; });
                TranslateTextBox.Dispatcher.Invoke(delegate { TranslateTextBox.Text = keyValues["translateText"]; });
            });
        }
        else if (GlobalConfig.Translate.DefaultTranslateProvide == GlobalConfig.Translate.TranslateProvideEnum.BaiduAi)
        {
            if (string.IsNullOrEmpty(GlobalConfig.Translate.BaiduAi.AppId) ||
                string.IsNullOrEmpty(GlobalConfig.Translate.BaiduAi.AppSecret))
            {
                RootDialog.Show("", (FindResource("ResultWindows_EmptyKeyMessage") as string)!);
                return;
            }

            OcrTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_ocring");
            TranslateTextBox.SetResourceReference(TextBox.TextProperty, "ResultWindows_translating");
            DispatcherHelper.DoEvents();

            BaiduAiHelper.ScreenshotTranslate(bmp).ContinueWith(result =>
            {
                var keyValues = result.Result;
                OcrTextBox.Dispatcher.Invoke(delegate { OcrTextBox.Text = keyValues["ocrText"]; });
                TranslateTextBox.Dispatcher.Invoke(delegate { TranslateTextBox.Text = keyValues["translateText"]; });
            });
        }
        else if (GlobalConfig.Translate.DefaultTranslateProvide ==
                 GlobalConfig.Translate.TranslateProvideEnum.GoogleCloud)
        {
            RootDialog.Show("", (FindResource("ResultWindows_LimitOcrAndTranslate") as string)!);
        }
    }

    private void ocrButton_Click(object sender, RoutedEventArgs e)
    {
        if (_bmp == null)
        {
            RootDialog.Show("", (FindResource("ResultWindows_NotFoundImage") as string)!);
            return;
        }

        Ocr(_bmp, DefaultOcrProvideComboBox.DataContext.ToString(), DefaultOcrTypeComboBox.DataContext.ToString(),
            DefaultOcrLanguageComboBox.DataContext.ToString());
    }

    private void translateButton_Click(object sender, RoutedEventArgs e)
    {
        Translate(DefaultTranslateProvideComboBox.DataContext.ToString(), SourceLanguageComboBox.DataContext.ToString(),
            TargetLanguageComboBox.DataContext.ToString());
    }

    private void defaultOcrProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        DefaultOcrProvideComboBox.DataContext = ((ComboBoxItem)DefaultOcrProvideComboBox.SelectedItem).DataContext;
        if (DefaultOcrProvideComboBox.DataContext.ToString() == GlobalConfig.Ocr.OcrProvideEnum.BaiduCloud.ToString())
        {
            DefaultOcrTypeComboBox.Items.Clear();
            var item = new ComboBoxItem();
            item.DataContext = GlobalConfig.Ocr.BaiduCloud.OcrTypeEnum.GeneralBasic.ToString();
            item.SetResourceReference(ContentProperty, "OcrType_GeneralBasic");
            DefaultOcrTypeComboBox.Items.Add(item);
            var item2 = new ComboBoxItem();
            item2.DataContext = GlobalConfig.Ocr.BaiduCloud.OcrTypeEnum.AccurateBasic.ToString();
            item2.SetResourceReference(ContentProperty, "OcrType_AccurateBasic");
            DefaultOcrTypeComboBox.Items.Add(item2);
            var item3 = new ComboBoxItem();
            item3.DataContext = GlobalConfig.Ocr.BaiduCloud.OcrTypeEnum.Handwriting.ToString();
            item3.SetResourceReference(ContentProperty, "OcrType_Handwriting");
            DefaultOcrTypeComboBox.Items.Add(item3);

            DefaultOcrTypeComboBox.SelectedItem = item;

            DefaultOcrLanguageComboBox.Items.Clear();
            var item4 = new ComboBoxItem();
            item4.DataContext = "auto";
            item4.SetResourceReference(ContentProperty, "Language_auto");
            DefaultOcrLanguageComboBox.Items.Add(item4);

            DefaultOcrLanguageComboBox.SelectedItem = item4;
        }
        else if (DefaultOcrProvideComboBox.DataContext.ToString() ==
                 GlobalConfig.Ocr.OcrProvideEnum.TencentCloud.ToString())
        {
            DefaultOcrTypeComboBox.Items.Clear();
            var item = new ComboBoxItem();
            item.DataContext = GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralBasicOcr.ToString();
            item.SetResourceReference(ContentProperty, "OcrType_GeneralBasic");
            DefaultOcrTypeComboBox.Items.Add(item);
            var item2 = new ComboBoxItem();
            item2.DataContext = GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralAccurateOcr.ToString();
            item2.SetResourceReference(ContentProperty, "OcrType_AccurateBasic");
            DefaultOcrTypeComboBox.Items.Add(item2);
            var item3 = new ComboBoxItem();
            item3.DataContext = GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralHandwritingOcr.ToString();
            item3.SetResourceReference(ContentProperty, "OcrType_Handwriting");
            DefaultOcrTypeComboBox.Items.Add(item3);

            DefaultOcrTypeComboBox.SelectedItem = item;

            DefaultOcrLanguageComboBox.Items.Clear();
            var item4 = new ComboBoxItem();
            item4.DataContext = "auto";
            item4.SetResourceReference(ContentProperty, "Language_auto");
            DefaultOcrLanguageComboBox.Items.Add(item4);

            DefaultOcrLanguageComboBox.SelectedItem = item4;
        }
        else if (DefaultOcrProvideComboBox.DataContext.ToString() ==
                 GlobalConfig.Ocr.OcrProvideEnum.SpaceOcr.ToString())
        {
            DefaultOcrTypeComboBox.Items.Clear();
            var item = new ComboBoxItem();
            item.DataContext = GlobalConfig.Ocr.SpaceOcr.OcrTypeEnum.Engine1.ToString();
            item.SetResourceReference(ContentProperty, "OcrType_Engine1");
            DefaultOcrTypeComboBox.Items.Add(item);
            var item2 = new ComboBoxItem();
            item2.DataContext = GlobalConfig.Ocr.SpaceOcr.OcrTypeEnum.Engine2.ToString();
            item2.SetResourceReference(ContentProperty, "OcrType_Engine2");
            DefaultOcrTypeComboBox.Items.Add(item2);
            var item3 = new ComboBoxItem();
            item3.DataContext = GlobalConfig.Ocr.SpaceOcr.OcrTypeEnum.Engine3.ToString();
            item3.SetResourceReference(ContentProperty, "OcrType_Engine3");
            DefaultOcrTypeComboBox.Items.Add(item3);
            var item5 = new ComboBoxItem();
            item5.DataContext = GlobalConfig.Ocr.SpaceOcr.OcrTypeEnum.Engine5.ToString();
            item5.SetResourceReference(ContentProperty, "OcrType_Engine5");
            DefaultOcrTypeComboBox.Items.Add(item5);

            DefaultOcrTypeComboBox.SelectedItem = item;

            DefaultOcrLanguageComboBox.Items.Clear();
            foreach (var item4 in OcrLanguageExtension.TranslateLanguageAttributeList)
                if (!string.IsNullOrWhiteSpace(item4.GetSpaceOcrCode()))
                {
                    var comboBoxItem = new ComboBoxItem();
                    comboBoxItem.DataContext = item4.GetSpaceOcrCode();
                    comboBoxItem.SetResourceReference(ContentProperty, item4.GetName());
                    DefaultOcrLanguageComboBox.Items.Add(comboBoxItem);
                }

            DefaultOcrLanguageComboBox.SelectedItem = DefaultOcrLanguageComboBox.Items[0];
        }
    }

    private void OcrTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DefaultOcrTypeComboBox.SelectedItem != null)
            DefaultOcrTypeComboBox.DataContext = ((ComboBoxItem)DefaultOcrTypeComboBox.SelectedItem).DataContext;
        if (DefaultOcrLanguageComboBox.SelectedItem != null)
            DefaultOcrLanguageComboBox.DataContext =
                ((ComboBoxItem)DefaultOcrLanguageComboBox.SelectedItem).DataContext;
        if (DefaultOcrLanguageComboBox.SelectedItem == null || DefaultOcrTypeComboBox.SelectedItem == null) return;
        if (DefaultOcrProvideComboBox.DataContext.ToString()!.Equals(GlobalConfig.Ocr.DefaultOcrProvide.ToString())
            && DefaultOcrTypeComboBox.DataContext.ToString()!.Equals(GlobalConfig.Ocr.DefaultOcrType)
            && DefaultOcrLanguageComboBox.DataContext.ToString()!.Equals(GlobalConfig.Ocr.DefaultOcrLanguage))
        {
            DefaultOcrSettingCheck.IsChecked = true;
            DefaultOcrSettingCheck.IsEnabled = false;
        }
        else
        {
            DefaultOcrSettingCheck.IsChecked = false;
            DefaultOcrSettingCheck.IsEnabled = true;
        }
    }

    private void defaultOcrSettingCheck_Checked(object sender, RoutedEventArgs e)
    {
        if (!DefaultOcrSettingCheck.IsChecked!.Value)
        {
            DefaultOcrSettingCheck.IsEnabled = true;
            return;
        }

        DefaultOcrSettingCheck.IsEnabled = false;
        if (_windowLoaded)
        {
            GlobalConfig.Ocr.DefaultOcrProvide = (GlobalConfig.Ocr.OcrProvideEnum)Enum.Parse(
                typeof(GlobalConfig.Ocr.OcrProvideEnum), DefaultOcrProvideComboBox.DataContext.ToString()!);
            GlobalConfig.Ocr.DefaultOcrType = DefaultOcrTypeComboBox.DataContext.ToString()!;
            GlobalConfig.Ocr.DefaultOcrLanguage = DefaultOcrLanguageComboBox.DataContext.ToString()!;
            GlobalConfig.SaveConfig();
        }
    }

    private void defaultTranslateProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        DefaultTranslateProvideComboBox.DataContext =
            ((ComboBoxItem)DefaultTranslateProvideComboBox.SelectedItem).DataContext;
        var translateProvide = DefaultTranslateProvideComboBox.DataContext.ToString();
        SourceLanguageComboBox.Items.Clear();
        TargetLanguageComboBox.Items.Clear();
        if (translateProvide!.Equals(GlobalConfig.Translate.TranslateProvideEnum.BaiduAi.ToString()))
        {
            foreach (var item in TranslateLanguageExtension.TranslateLanguageAttributeList)
                if (!string.IsNullOrWhiteSpace(item.GetBaiduAiCode()))
                {
                    var comboBoxItem = new ComboBoxItem();
                    comboBoxItem.DataContext = item.GetBaiduAiCode();
                    comboBoxItem.SetResourceReference(ContentProperty, item.GetName());
                    SourceLanguageComboBox.Items.Add(comboBoxItem);
                    var comboBoxItem2 = new ComboBoxItem();
                    comboBoxItem2.DataContext = item.GetBaiduAiCode();
                    comboBoxItem2.SetResourceReference(ContentProperty, item.GetName());
                    TargetLanguageComboBox.Items.Add(comboBoxItem2);
                }
        }
        else if (translateProvide.Equals(GlobalConfig.Translate.TranslateProvideEnum.TencentCloud.ToString()))
        {
            foreach (var item in TranslateLanguageExtension.TranslateLanguageAttributeList)
                if (!string.IsNullOrWhiteSpace(item.GetTencentCloudCode()))
                {
                    var comboBoxItem = new ComboBoxItem();
                    comboBoxItem.DataContext = item.GetTencentCloudCode();
                    comboBoxItem.SetResourceReference(ContentProperty, item.GetName());
                    SourceLanguageComboBox.Items.Add(comboBoxItem);
                    var comboBoxItem2 = new ComboBoxItem();
                    comboBoxItem2.DataContext = item.GetTencentCloudCode();
                    comboBoxItem2.SetResourceReference(ContentProperty, item.GetName());
                    TargetLanguageComboBox.Items.Add(comboBoxItem2);
                }
        }
        else if (translateProvide.Equals(GlobalConfig.Translate.TranslateProvideEnum.GoogleCloud.ToString()))
        {
            foreach (var item in TranslateLanguageExtension.TranslateLanguageAttributeList)
                if (!string.IsNullOrWhiteSpace(item.GetGoogleCloudCode()))
                {
                    var comboBoxItem = new ComboBoxItem();
                    comboBoxItem.DataContext = item.GetGoogleCloudCode();
                    comboBoxItem.SetResourceReference(ContentProperty, item.GetName());
                    SourceLanguageComboBox.Items.Add(comboBoxItem);
                    var comboBoxItem2 = new ComboBoxItem();
                    comboBoxItem2.DataContext = item.GetGoogleCloudCode();
                    comboBoxItem2.SetResourceReference(ContentProperty, item.GetName());
                    TargetLanguageComboBox.Items.Add(comboBoxItem2);
                }
        }
        else if (translateProvide.Equals(GlobalConfig.Translate.TranslateProvideEnum.Deeplx.ToString()))
        {
            foreach (var item in TranslateLanguageExtension.TranslateLanguageAttributeList)
                if (!string.IsNullOrWhiteSpace(item.GetDeeplxCode()))
                {
                    var comboBoxItem = new ComboBoxItem();
                    comboBoxItem.DataContext = item.GetDeeplxCode();
                    comboBoxItem.SetResourceReference(ContentProperty, item.GetName());
                    SourceLanguageComboBox.Items.Add(comboBoxItem);
                    var comboBoxItem2 = new ComboBoxItem();
                    comboBoxItem2.DataContext = item.GetDeeplxCode();
                    comboBoxItem2.SetResourceReference(ContentProperty, item.GetName());
                    TargetLanguageComboBox.Items.Add(comboBoxItem2);
                }
        }

        if (!translateProvide.Equals(GlobalConfig.Translate.TranslateProvideEnum.Deeplx.ToString()))
        {
            TargetLanguageComboBox.Items.RemoveAt(0);
        }
        SourceLanguageComboBox.SelectedItem = SourceLanguageComboBox.Items[0];
        TargetLanguageComboBox.SelectedItem = TargetLanguageComboBox.Items[0];
    }

    private void TranslateLanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SourceLanguageComboBox.SelectedItem != null)
            SourceLanguageComboBox.DataContext = ((ComboBoxItem)SourceLanguageComboBox.SelectedItem).DataContext;
        if (TargetLanguageComboBox.SelectedItem != null)
            TargetLanguageComboBox.DataContext = ((ComboBoxItem)TargetLanguageComboBox.SelectedItem).DataContext;
        if (SourceLanguageComboBox.DataContext == null || TargetLanguageComboBox.DataContext == null) return;
        if (DefaultTranslateProvideComboBox.DataContext.ToString()!
                .Equals(GlobalConfig.Translate.DefaultTranslateProvide.ToString())
            && SourceLanguageComboBox.DataContext.ToString()!
                .Equals(GlobalConfig.Translate.DefaultTranslateSourceLanguage)
            && TargetLanguageComboBox.DataContext.ToString()!
                .Equals(GlobalConfig.Translate.DefaultTranslateTargetLanguage))
        {
            DefaultTranslateSettingCheck.IsChecked = true;
            DefaultTranslateSettingCheck.IsEnabled = false;
        }
        else
        {
            DefaultTranslateSettingCheck.IsChecked = false;
            DefaultTranslateSettingCheck.IsEnabled = true;
        }
    }

    private void defaultTranslateSettingCheck_Checked(object sender, RoutedEventArgs e)
    {
        if (!DefaultTranslateSettingCheck.IsChecked!.Value)
        {
            DefaultTranslateSettingCheck.IsEnabled = true;
            return;
        }

        DefaultTranslateSettingCheck.IsEnabled = false;
        if (_windowLoaded)
        {
            GlobalConfig.Translate.DefaultTranslateProvide = (GlobalConfig.Translate.TranslateProvideEnum)Enum.Parse(
                typeof(GlobalConfig.Translate.TranslateProvideEnum),
                DefaultTranslateProvideComboBox.DataContext.ToString()!);
            GlobalConfig.Translate.DefaultTranslateSourceLanguage = SourceLanguageComboBox.DataContext.ToString();
            GlobalConfig.Translate.DefaultTranslateTargetLanguage = TargetLanguageComboBox.DataContext.ToString();
            GlobalConfig.SaveConfig();
        }
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        Utils.FlushMemory();
    }

    private void RootDialog_OnButtonRightClick(object sender, RoutedEventArgs e)
    {
        RootDialog.Hide();
    }
}