using System;
using System.Windows;
using System.Windows.Controls;
using WpfTool.Entity;
using System.Collections.Generic;

namespace WpfTool.Page.Setting;

public partial class MachineTranslatePage
{
    private bool _pageLoaded;

    public MachineTranslatePage()
    {
        InitializeComponent();
    }

    private void MachineTranslate_OnLoaded(object sender, RoutedEventArgs e)
    {
        var defaultTranslateSourceLanguage = GlobalConfig.Translate.DefaultTranslateSourceLanguage;
        var defaultTranslateTargetLanguage = GlobalConfig.Translate.DefaultTranslateTargetLanguage;
        foreach (ComboBoxItem item in DefaultTranslateProvideComboBox.Items)
            if (item.DataContext.Equals(GlobalConfig.Translate.DefaultTranslateProvide.ToString()))
            {
                DefaultTranslateProvideComboBox.SelectedItem = item;
                break;
            }

        foreach (ComboBoxItem item in SourceLanguageComboBox.Items)
            if (item.DataContext.ToString()!.Equals(defaultTranslateSourceLanguage))
            {
                SourceLanguageComboBox.SelectedItem = item;
                break;
            }

        foreach (ComboBoxItem item in TargetLanguageComboBox.Items)
            if (item.DataContext.ToString()!.Equals(defaultTranslateTargetLanguage))
            {
                TargetLanguageComboBox.SelectedItem = item;
                break;
            }

        BaiduAiAppIdInput.Text = GlobalConfig.Translate.BaiduAi.AppId;
        BaiduAiSecretKeyInput.Password = GlobalConfig.Translate.BaiduAi.AppSecret;

        TencentCloudTranslateSecretIdInput.Text = GlobalConfig.Translate.TencentCloud.SecretId;
        TencentCloudTranslateSecretKeyInput.Password = GlobalConfig.Translate.TencentCloud.SecretKey;

        DeeplxUrlInput.Text = GlobalConfig.Translate.Deeplx.Url;
        DeeplxAuthorizationInput.Password = GlobalConfig.Translate.Deeplx.Authorization;

        _pageLoaded = true;
    }

    private void defaultTranslateProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        DefaultTranslateProvideComboBox.DataContext =
            ((ComboBoxItem)DefaultTranslateProvideComboBox.SelectedItem).DataContext;
        if (_pageLoaded)
            GlobalConfig.Translate.DefaultTranslateProvide = (GlobalConfig.Translate.TranslateProvideEnum)Enum.Parse(
                typeof(GlobalConfig.Translate.TranslateProvideEnum),
                DefaultTranslateProvideComboBox.DataContext.ToString()!);

        var translateProvide = DefaultTranslateProvideComboBox.DataContext.ToString()!;
        SourceLanguageComboBox.Items.Clear();
        TargetLanguageComboBox.Items.Clear();
        List<TranslateLanguageAttribute> list;
        if (translateProvide!.Equals(GlobalConfig.Translate.TranslateProvideEnum.BaiduAi.ToString()))
        {
            list = BaiduAiTranslateLanguageExtension.TranslateLanguageAttributeList;
        }
        else if (translateProvide.Equals(GlobalConfig.Translate.TranslateProvideEnum.TencentCloud.ToString()))
        {
            list = TencentTranslateLanguageExtension.TranslateLanguageAttributeList;
        }
        else if (translateProvide.Equals(GlobalConfig.Translate.TranslateProvideEnum.GoogleCloud.ToString()))
        {
            list = GoogleTranslateLanguageExtension.TranslateLanguageAttributeList;
        }
        else if (translateProvide.Equals(GlobalConfig.Translate.TranslateProvideEnum.Deeplx.ToString()))
        {
            list = DeeplxTranslateLanguageExtension.TranslateLanguageAttributeList;
        }
        else
        {
            return;
        }
        foreach (var item in list)
        {
            if (item.GetSource())
            {
                var comboBoxItem = new ComboBoxItem();
                comboBoxItem.DataContext = item.GetCode();
                comboBoxItem.SetResourceReference(ContentControl.ContentProperty, item.GetName());
                SourceLanguageComboBox.Items.Add(comboBoxItem);
            }
            if (item.GetTarget())
            {
                var comboBoxItem2 = new ComboBoxItem();
                comboBoxItem2.DataContext = item.GetCode();
                comboBoxItem2.SetResourceReference(ContentControl.ContentProperty, item.GetName());
                TargetLanguageComboBox.Items.Add(comboBoxItem2);
            }
        }

        SourceLanguageComboBox.SelectedItem = SourceLanguageComboBox.Items[0];
        TargetLanguageComboBox.SelectedItem = TargetLanguageComboBox.Items[0];
    }

    private void sourceLanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SourceLanguageComboBox.SelectedItem == null) return;

        SourceLanguageComboBox.DataContext = ((ComboBoxItem)SourceLanguageComboBox.SelectedItem).DataContext;
        if (_pageLoaded)
            GlobalConfig.Translate.DefaultTranslateSourceLanguage = SourceLanguageComboBox.DataContext.ToString();
    }

    private void targetLanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TargetLanguageComboBox.SelectedItem == null) return;

        TargetLanguageComboBox.DataContext = ((ComboBoxItem)TargetLanguageComboBox.SelectedItem).DataContext;
        if (_pageLoaded)
            GlobalConfig.Translate.DefaultTranslateTargetLanguage = TargetLanguageComboBox.DataContext.ToString();
    }

    private void Deeplx_UrlInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Translate.Deeplx.Url = DeeplxUrlInput.Text;
    }

    private void Deeplx_AuthorizationInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Translate.Deeplx.Authorization = DeeplxAuthorizationInput.Password;
    }

    private void TencentCloudTranslate_SecretIdInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Translate.TencentCloud.SecretId = TencentCloudTranslateSecretIdInput.Text;
    }

    private void TencentCloudTranslate_SecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Translate.TencentCloud.SecretKey = TencentCloudTranslateSecretKeyInput.Password;
    }

    private void BaiduAI_AppIdInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Translate.BaiduAi.AppId = BaiduAiAppIdInput.Text;
    }

    private void BaiduAI_SecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (_pageLoaded) GlobalConfig.Translate.BaiduAi.AppSecret = BaiduAiSecretKeyInput.Password;
    }
}