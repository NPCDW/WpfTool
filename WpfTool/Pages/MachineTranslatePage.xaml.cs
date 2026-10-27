using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WpfTool.Entity;
using WpfTool.Util;

namespace WpfTool.Pages;

public sealed partial class MachineTranslatePage : Page
{
    private bool _pageLoaded;

    public MachineTranslatePage()
    {
        InitializeComponent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        DefaultTranslateHeader.Text = Strings.Setting_defaultMachineTranslate;
        GoogleCloudNote.Text = Strings.CloudService_GoogleCloud + ": " + Strings.Setting_GoogleFreeTranslate;

        DefaultTranslateProvideComboBox.Items.Clear();
        AddItem(DefaultTranslateProvideComboBox, "BaiduAi", Strings.CloudService_BaiduAI);
        AddItem(DefaultTranslateProvideComboBox, "TencentCloud", Strings.CloudService_TencentCloud);
        AddItem(DefaultTranslateProvideComboBox, "GoogleCloud", Strings.CloudService_GoogleCloud);
        AddItem(DefaultTranslateProvideComboBox, "Deeplx", Strings.CloudService_Deeplx);
        AddItem(DefaultTranslateProvideComboBox, "OpenAi", Strings.CloudService_OpenAI);

        SelectByTag(DefaultTranslateProvideComboBox, GlobalConfig.Translate.DefaultTranslateProvide.ToString());
        RefreshTranslateItems();
        SelectByTag(SourceLanguageComboBox, GlobalConfig.Translate.DefaultTranslateSourceLanguage);
        SelectByTag(TargetLanguageComboBox, GlobalConfig.Translate.DefaultTranslateTargetLanguage);

        BaiduAiAppIdInput.Text = GlobalConfig.Translate.BaiduAi.AppId;
        BaiduAiSecretKeyInput.Password = GlobalConfig.Translate.BaiduAi.AppSecret;

        TencentCloudTranslateSecretIdInput.Text = GlobalConfig.Translate.TencentCloud.SecretId;
        TencentCloudTranslateSecretKeyInput.Password = GlobalConfig.Translate.TencentCloud.SecretKey;

        DeeplxUrlInput.Text = GlobalConfig.Translate.Deeplx.Url;
        DeeplxAuthorizationInput.Password = GlobalConfig.Translate.Deeplx.Authorization;

        OpenAiUrlInput.Text = GlobalConfig.Translate.OpenAi.Url;
        OpenAiApiKeyInput.Password = GlobalConfig.Translate.OpenAi.ApiKey;
        OpenAiModelInput.Text = GlobalConfig.Translate.OpenAi.Model;

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

    private void DefaultTranslateProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var provide = TagOf(DefaultTranslateProvideComboBox);
        if (provide == null) return;

        if (_pageLoaded)
            GlobalConfig.Translate.DefaultTranslateProvide = (GlobalConfig.Translate.TranslateProvideEnum)Enum.Parse(
                typeof(GlobalConfig.Translate.TranslateProvideEnum), provide);

        RefreshTranslateItems();
    }

    /// <summary>
    ///     按当前选中的翻译服务商重建源 / 目标语言选项
    /// </summary>
    private void RefreshTranslateItems()
    {
        var provide = TagOf(DefaultTranslateProvideComboBox);
        if (provide == null) return;

        SourceLanguageComboBox.Items.Clear();
        TargetLanguageComboBox.Items.Clear();

        List<TranslateLanguageAttribute> list;
        if (provide.Equals(GlobalConfig.Translate.TranslateProvideEnum.BaiduAi.ToString()))
            list = BaiduAiTranslateLanguageExtension.TranslateLanguageAttributeList;
        else if (provide.Equals(GlobalConfig.Translate.TranslateProvideEnum.TencentCloud.ToString()))
            list = TencentTranslateLanguageExtension.TranslateLanguageAttributeList;
        else if (provide.Equals(GlobalConfig.Translate.TranslateProvideEnum.GoogleCloud.ToString()))
            list = GoogleTranslateLanguageExtension.TranslateLanguageAttributeList;
        else if (provide.Equals(GlobalConfig.Translate.TranslateProvideEnum.Deeplx.ToString()))
            list = DeeplxTranslateLanguageExtension.TranslateLanguageAttributeList;
        else if (provide.Equals(GlobalConfig.Translate.TranslateProvideEnum.OpenAi.ToString()))
            list = OpenAiTranslateLanguageExtension.TranslateLanguageAttributeList;
        else
            return;

        foreach (var item in list)
        {
            if (item.GetSource())
                AddItem(SourceLanguageComboBox, item.GetCode(), Strings.ByKey(item.GetName()));
            if (item.GetTarget())
                AddItem(TargetLanguageComboBox, item.GetCode(), Strings.ByKey(item.GetName()));
        }

        if (SourceLanguageComboBox.Items.Count > 0) SourceLanguageComboBox.SelectedIndex = 0;
        if (TargetLanguageComboBox.Items.Count > 0) TargetLanguageComboBox.SelectedIndex = 0;
    }

    private void SourceLanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_pageLoaded) return;
        var tag = TagOf(SourceLanguageComboBox);
        if (tag != null) GlobalConfig.Translate.DefaultTranslateSourceLanguage = tag;
    }

    private void TargetLanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_pageLoaded) return;
        var tag = TagOf(TargetLanguageComboBox);
        if (tag != null) GlobalConfig.Translate.DefaultTranslateTargetLanguage = tag;
    }

    private void TencentCloudTranslateSecretIdInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Translate.TencentCloud.SecretId = TencentCloudTranslateSecretIdInput.Text;
    }

    private void TencentCloudTranslateSecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Translate.TencentCloud.SecretKey = TencentCloudTranslateSecretKeyInput.Password;
    }

    private void BaiduAiAppIdInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Translate.BaiduAi.AppId = BaiduAiAppIdInput.Text;
    }

    private void BaiduAiSecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Translate.BaiduAi.AppSecret = BaiduAiSecretKeyInput.Password;
    }

    private void DeeplxUrlInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Translate.Deeplx.Url = DeeplxUrlInput.Text;
    }

    private void DeeplxAuthorizationInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Translate.Deeplx.Authorization = DeeplxAuthorizationInput.Password;
    }

    private void OpenAiUrlInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Translate.OpenAi.Url = OpenAiUrlInput.Text;
    }

    private void OpenAiApiKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Translate.OpenAi.ApiKey = OpenAiApiKeyInput.Password;
    }

    private void OpenAiModelInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Translate.OpenAi.Model = OpenAiModelInput.Text;
    }
}
