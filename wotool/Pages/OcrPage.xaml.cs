using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using wotool.Entity;
using wotool.Util;
namespace wotool.Pages;

public sealed partial class OcrPage : Page
{
    private bool _pageLoaded;

    public OcrPage()
    {
        InitializeComponent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        DefaultOcrHeader.Text = Strings.Setting_DefaultOCR;

        DefaultOcrProvideComboBox.Items.Clear();
        AddItem(DefaultOcrProvideComboBox, "BaiduCloud", Strings.CloudService_BaiduCloud);
        AddItem(DefaultOcrProvideComboBox, "TencentCloud", Strings.CloudService_TencentCloud);
        AddItem(DefaultOcrProvideComboBox, "SpaceOcr", Strings.CloudService_SpaceOCR);

        SelectByTag(DefaultOcrProvideComboBox, GlobalConfig.Ocr.DefaultOcrProvide.ToString());
        RefreshOcrItems();
        SelectByTag(DefaultOcrTypeComboBox, GlobalConfig.Ocr.DefaultOcrType);
        SelectByTag(DefaultOcrLanguageComboBox, GlobalConfig.Ocr.DefaultOcrLanguage);

        BaiduCloudAppKeyInput.Text = GlobalConfig.Ocr.BaiduCloud.ClientId;
        BaiduCloudSecretKeyInput.Password = GlobalConfig.Ocr.BaiduCloud.ClientSecret;
        TencentCloudOcrSecretIdInput.Text = GlobalConfig.Ocr.TencentCloud.SecretId;
        TencentCloudOcrSecretKeyInput.Password = GlobalConfig.Ocr.TencentCloud.SecretKey;
        SpaceOcrApiKeyInput.Password = GlobalConfig.Ocr.SpaceOcr.ApiKey;

        _pageLoaded = true;
    }

    private static void AddItem(ComboBox comboBox, string tag, string content)
    {
        comboBox.Items.Add(new ComboBoxItem { Tag = tag, Content = content });
    }

    private static string? TagOf(ComboBox comboBox)
    {
        return (comboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
    }

    private static void SelectByTag(ComboBox comboBox, string? tag)
    {
        if (tag == null) return;
        foreach (var item in comboBox.Items)
            if (item is ComboBoxItem comboBoxItem && comboBoxItem.Tag?.ToString() == tag)
            {
                comboBox.SelectedItem = comboBoxItem;
                return;
            }
    }

    private void DefaultOcrProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var provide = TagOf(DefaultOcrProvideComboBox);
        if (provide != null && _pageLoaded)
            GlobalConfig.Ocr.DefaultOcrProvide = (GlobalConfig.Ocr.OcrProvideEnum)Enum.Parse(
                typeof(GlobalConfig.Ocr.OcrProvideEnum), provide);

        RefreshOcrItems();
    }

    /// <summary>
    ///     按当前选中的 OCR 服务商重建识别类型 / 语言选项
    /// </summary>
    private void RefreshOcrItems()
    {
        var provide = TagOf(DefaultOcrProvideComboBox);

        DefaultOcrTypeComboBox.Items.Clear();
        DefaultOcrLanguageComboBox.Items.Clear();
        if (provide == null) return;

        if (provide == GlobalConfig.Ocr.OcrProvideEnum.BaiduCloud.ToString())
        {
            AddItem(DefaultOcrTypeComboBox, GlobalConfig.Ocr.BaiduCloud.OcrTypeEnum.GeneralBasic.ToString(),
                Strings.OcrType_GeneralBasic);
            AddItem(DefaultOcrTypeComboBox, GlobalConfig.Ocr.BaiduCloud.OcrTypeEnum.AccurateBasic.ToString(),
                Strings.OcrType_AccurateBasic);
            AddItem(DefaultOcrTypeComboBox, GlobalConfig.Ocr.BaiduCloud.OcrTypeEnum.Handwriting.ToString(),
                Strings.OcrType_Handwriting);
            AddItem(DefaultOcrLanguageComboBox, "auto", Strings.Language_auto);
        }
        else if (provide == GlobalConfig.Ocr.OcrProvideEnum.TencentCloud.ToString())
        {
            AddItem(DefaultOcrTypeComboBox, GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralBasicOcr.ToString(),
                Strings.OcrType_GeneralBasic);
            AddItem(DefaultOcrTypeComboBox, GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralAccurateOcr.ToString(),
                Strings.OcrType_AccurateBasic);
            AddItem(DefaultOcrTypeComboBox, GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralHandwritingOcr.ToString(),
                Strings.OcrType_Handwriting);
            AddItem(DefaultOcrLanguageComboBox, "auto", Strings.Language_auto);
        }
        else if (provide == GlobalConfig.Ocr.OcrProvideEnum.SpaceOcr.ToString())
        {
            AddItem(DefaultOcrTypeComboBox, GlobalConfig.Ocr.SpaceOcr.OcrTypeEnum.Engine1.ToString(),
                Strings.OcrType_Engine1);
            AddItem(DefaultOcrTypeComboBox, GlobalConfig.Ocr.SpaceOcr.OcrTypeEnum.Engine2.ToString(),
                Strings.OcrType_Engine2);
            AddItem(DefaultOcrTypeComboBox, GlobalConfig.Ocr.SpaceOcr.OcrTypeEnum.Engine3.ToString(),
                Strings.OcrType_Engine3);
            AddItem(DefaultOcrTypeComboBox, GlobalConfig.Ocr.SpaceOcr.OcrTypeEnum.Engine5.ToString(),
                Strings.OcrType_Engine5);

            foreach (var item in OcrLanguageExtension.OcrLanguageAttributeList)
                if (!string.IsNullOrWhiteSpace(item.GetSpaceOcrCode()))
                    AddItem(DefaultOcrLanguageComboBox, item.GetSpaceOcrCode(), Strings.ByKey(item.GetName()));
        }

        if (DefaultOcrTypeComboBox.Items.Count > 0) DefaultOcrTypeComboBox.SelectedIndex = 0;
        if (DefaultOcrLanguageComboBox.Items.Count > 0) DefaultOcrLanguageComboBox.SelectedIndex = 0;
    }

    private void DefaultOcrTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_pageLoaded) return;
        var tag = TagOf(DefaultOcrTypeComboBox);
        if (tag != null) GlobalConfig.Ocr.DefaultOcrType = tag;
    }

    private void DefaultOcrLanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_pageLoaded) return;
        var tag = TagOf(DefaultOcrLanguageComboBox);
        if (tag != null) GlobalConfig.Ocr.DefaultOcrLanguage = tag;
    }

    private void TencentCloudOcrSecretIdInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Ocr.TencentCloud.SecretId = TencentCloudOcrSecretIdInput.Text;
    }

    private void TencentCloudOcrSecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Ocr.TencentCloud.SecretKey = TencentCloudOcrSecretKeyInput.Password;
    }

    private void BaiduCloudAppKeyInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Ocr.BaiduCloud.ClientId = BaiduCloudAppKeyInput.Text;
    }

    private void BaiduCloudSecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Ocr.BaiduCloud.ClientSecret = BaiduCloudSecretKeyInput.Password;
    }

    private void SpaceOcrApiKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Ocr.SpaceOcr.ApiKey = SpaceOcrApiKeyInput.Password;
    }
}
