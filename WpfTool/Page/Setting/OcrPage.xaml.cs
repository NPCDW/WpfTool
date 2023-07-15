using System;
using System.Windows;
using System.Windows.Controls;
using WpfTool.Entity;

namespace WpfTool.Page.Setting;

public partial class OcrPage
{
    private bool _pageLoaded;

    public OcrPage()
    {
        InitializeComponent();
    }

    private void Ocr_OnLoaded(object sender, RoutedEventArgs e)
    {
        var defaultOcrType = GlobalConfig.Ocr.DefaultOcrType;
        var defaultOcrLanguage = GlobalConfig.Ocr.DefaultOcrLanguage;
        foreach (ComboBoxItem item in DefaultOcrProvideComboBox.Items)
            if (item.DataContext.Equals(GlobalConfig.Ocr.DefaultOcrProvide.ToString()))
            {
                DefaultOcrProvideComboBox.SelectedItem = item;
                break;
            }

        foreach (ComboBoxItem item in DefaultOcrTypeComboBox.Items)
            if (item.DataContext.ToString()!.Equals(defaultOcrType))
            {
                DefaultOcrTypeComboBox.SelectedItem = item;
                break;
            }

        foreach (ComboBoxItem item in DefaultOcrLanguageComboBox.Items)
            if (item.DataContext.ToString()!.Equals(defaultOcrLanguage))
            {
                DefaultOcrLanguageComboBox.SelectedItem = item;
                break;
            }

        BaiduCloudAppKeyInput.Text = GlobalConfig.Ocr.BaiduCloud.ClientId;
        BaiduCloudSecretKeyInput.Password = GlobalConfig.Ocr.BaiduCloud.ClientSecret;

        TencentCloudOcrSecretIdInput.Text = GlobalConfig.Ocr.TencentCloud.SecretId;
        TencentCloudOcrSecretKeyInput.Password = GlobalConfig.Ocr.TencentCloud.SecretKey;

        SpaceOcrApiKeyInput.Password = GlobalConfig.Ocr.SpaceOcr.ApiKey;

        _pageLoaded = true;
    }

    private void defaultOcrProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        DefaultOcrProvideComboBox.DataContext = ((ComboBoxItem)DefaultOcrProvideComboBox.SelectedItem).DataContext;
        if (_pageLoaded)
            GlobalConfig.Ocr.DefaultOcrProvide = (GlobalConfig.Ocr.OcrProvideEnum)Enum.Parse(
                typeof(GlobalConfig.Ocr.OcrProvideEnum), DefaultOcrProvideComboBox.DataContext.ToString()!);

        if (DefaultOcrProvideComboBox.DataContext.ToString() == GlobalConfig.Ocr.OcrProvideEnum.BaiduCloud.ToString())
        {
            DefaultOcrTypeComboBox.Items.Clear();
            var item = new ComboBoxItem();
            item.DataContext = GlobalConfig.Ocr.BaiduCloud.OcrTypeEnum.GeneralBasic.ToString();
            item.SetResourceReference(ContentControl.ContentProperty, "OcrType_GeneralBasic");
            DefaultOcrTypeComboBox.Items.Add(item);
            var item2 = new ComboBoxItem();
            item2.DataContext = GlobalConfig.Ocr.BaiduCloud.OcrTypeEnum.AccurateBasic.ToString();
            item2.SetResourceReference(ContentControl.ContentProperty, "OcrType_AccurateBasic");
            DefaultOcrTypeComboBox.Items.Add(item2);
            var item3 = new ComboBoxItem();
            item3.DataContext = GlobalConfig.Ocr.BaiduCloud.OcrTypeEnum.Handwriting.ToString();
            item3.SetResourceReference(ContentControl.ContentProperty, "OcrType_Handwriting");
            DefaultOcrTypeComboBox.Items.Add(item3);

            DefaultOcrTypeComboBox.SelectedItem = item;

            DefaultOcrLanguageComboBox.Items.Clear();
            var item4 = new ComboBoxItem();
            item4.DataContext = "auto";
            item4.SetResourceReference(ContentControl.ContentProperty, "Language_auto");
            DefaultOcrLanguageComboBox.Items.Add(item4);

            DefaultOcrLanguageComboBox.SelectedItem = item4;
        }
        else if (DefaultOcrProvideComboBox.DataContext.ToString() ==
                 GlobalConfig.Ocr.OcrProvideEnum.TencentCloud.ToString())
        {
            DefaultOcrTypeComboBox.Items.Clear();
            var item = new ComboBoxItem();
            item.DataContext = GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralBasicOcr.ToString();
            item.SetResourceReference(ContentControl.ContentProperty, "OcrType_GeneralBasic");
            DefaultOcrTypeComboBox.Items.Add(item);
            var item2 = new ComboBoxItem();
            item2.DataContext = GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralAccurateOcr.ToString();
            item2.SetResourceReference(ContentControl.ContentProperty, "OcrType_AccurateBasic");
            DefaultOcrTypeComboBox.Items.Add(item2);
            var item3 = new ComboBoxItem();
            item3.DataContext = GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralHandwritingOcr.ToString();
            item3.SetResourceReference(ContentControl.ContentProperty, "OcrType_Handwriting");
            DefaultOcrTypeComboBox.Items.Add(item3);

            DefaultOcrTypeComboBox.SelectedItem = item;

            DefaultOcrLanguageComboBox.Items.Clear();
            var item4 = new ComboBoxItem();
            item4.DataContext = "auto";
            item4.SetResourceReference(ContentControl.ContentProperty, "Language_auto");
            DefaultOcrLanguageComboBox.Items.Add(item4);

            DefaultOcrLanguageComboBox.SelectedItem = item4;
        }
        else if (DefaultOcrProvideComboBox.DataContext.ToString() ==
                 GlobalConfig.Ocr.OcrProvideEnum.SpaceOcr.ToString())
        {
            DefaultOcrTypeComboBox.Items.Clear();
            var item = new ComboBoxItem();
            item.DataContext = GlobalConfig.Ocr.SpaceOcr.OcrTypeEnum.Engine1.ToString();
            item.SetResourceReference(ContentControl.ContentProperty, "OcrType_Engine1");
            DefaultOcrTypeComboBox.Items.Add(item);
            var item2 = new ComboBoxItem();
            item2.DataContext = GlobalConfig.Ocr.SpaceOcr.OcrTypeEnum.Engine2.ToString();
            item2.SetResourceReference(ContentControl.ContentProperty, "OcrType_Engine2");
            DefaultOcrTypeComboBox.Items.Add(item2);
            var item3 = new ComboBoxItem();
            item3.DataContext = GlobalConfig.Ocr.SpaceOcr.OcrTypeEnum.Engine3.ToString();
            item3.SetResourceReference(ContentControl.ContentProperty, "OcrType_Engine3");
            DefaultOcrTypeComboBox.Items.Add(item3);
            var item5 = new ComboBoxItem();
            item5.DataContext = GlobalConfig.Ocr.SpaceOcr.OcrTypeEnum.Engine5.ToString();
            item5.SetResourceReference(ContentControl.ContentProperty, "OcrType_Engine5");
            DefaultOcrTypeComboBox.Items.Add(item5);

            DefaultOcrTypeComboBox.SelectedItem = item;

            DefaultOcrLanguageComboBox.Items.Clear();
            foreach (var item4 in OcrLanguageExtension.TranslateLanguageAttributeList)
                if (!string.IsNullOrWhiteSpace(item4.GetSpaceOcrCode()))
                {
                    var comboBoxItem = new ComboBoxItem();
                    comboBoxItem.DataContext = item4.GetSpaceOcrCode();
                    comboBoxItem.SetResourceReference(ContentControl.ContentProperty, item4.GetName());
                    DefaultOcrLanguageComboBox.Items.Add(comboBoxItem);
                }

            DefaultOcrLanguageComboBox.SelectedItem = DefaultOcrLanguageComboBox.Items[0];
        }
    }

    private void defaultOcrTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DefaultOcrTypeComboBox.SelectedItem == null) return;

        DefaultOcrTypeComboBox.DataContext = ((ComboBoxItem)DefaultOcrTypeComboBox.SelectedItem).DataContext;
        if (_pageLoaded) GlobalConfig.Ocr.DefaultOcrType = DefaultOcrTypeComboBox.DataContext.ToString()!;
    }

    private void TencentCloudOcr_SecretIdInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Ocr.TencentCloud.SecretId = TencentCloudOcrSecretIdInput.Text;
    }

    private void TencentCloudOcr_SecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Ocr.TencentCloud.SecretKey = TencentCloudOcrSecretKeyInput.Password;
    }

    private void BaiduCloud_AppKeyInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Ocr.BaiduCloud.ClientId = BaiduCloudAppKeyInput.Text;
    }

    private void BaiduCloud_SecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Ocr.BaiduCloud.ClientSecret = BaiduCloudSecretKeyInput.Password;
    }

    private void defaultOcrLanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DefaultOcrLanguageComboBox.SelectedItem == null) return;

        DefaultOcrLanguageComboBox.DataContext = ((ComboBoxItem)DefaultOcrLanguageComboBox.SelectedItem).DataContext;
        if (_pageLoaded) GlobalConfig.Ocr.DefaultOcrLanguage = DefaultOcrLanguageComboBox.DataContext.ToString()!;
    }

    private void SpaceOCR_ApiKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Ocr.SpaceOcr.ApiKey = SpaceOcrApiKeyInput.Password;
    }
}