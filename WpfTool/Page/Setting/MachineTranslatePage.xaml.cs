using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfTool.Page.Setting;

public partial class MachineTranslatePage : System.Windows.Controls.Page
{
    private bool PageLoaded = false;

    public MachineTranslatePage()
    {
        InitializeComponent();
    }

    private void MachineTranslate_OnLoaded(object sender, RoutedEventArgs e)
    {
        string defaultTranslateSourceLanguage = GlobalConfig.Translate.defaultTranslateSourceLanguage;
        string defaultTranslateTargetLanguage = GlobalConfig.Translate.defaultTranslateTargetLanguage;
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

        this.BaiduAI_AppIdInput.Text = GlobalConfig.Translate.BaiduAI.app_id;
        this.BaiduAI_SecretKeyInput.Password = GlobalConfig.Translate.BaiduAI.app_secret;

        this.TencentCloudTranslate_SecretIdInput.Text = GlobalConfig.Translate.TencentCloud.secret_id;
        this.TencentCloudTranslate_SecretKeyInput.Password = GlobalConfig.Translate.TencentCloud.secret_key;

        this.PageLoaded = true;
    }

    private void defaultTranslateProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        defaultTranslateProvideComboBox.DataContext =
            ((ComboBoxItem)defaultTranslateProvideComboBox.SelectedItem).DataContext;
        if (this.PageLoaded)
        {
            GlobalConfig.Translate.defaultTranslateProvide = (GlobalConfig.Translate.TranslateProvideEnum)Enum.Parse(
                typeof(GlobalConfig.Translate.TranslateProvideEnum),
                defaultTranslateProvideComboBox.DataContext.ToString());
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
        if (this.PageLoaded)
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
        if (this.PageLoaded)
        {
            GlobalConfig.Translate.defaultTranslateTargetLanguage = targetLanguageComboBox.DataContext.ToString();
        }
    }

    private void TencentCloudTranslate_SecretIdInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (this.PageLoaded)
        {
            GlobalConfig.Translate.TencentCloud.secret_id = this.TencentCloudTranslate_SecretIdInput.Text;
        }
    }

    private void TencentCloudTranslate_SecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (this.PageLoaded)
        {
            GlobalConfig.Translate.TencentCloud.secret_key = this.TencentCloudTranslate_SecretKeyInput.Password;
        }
    }

    private void BaiduAI_AppIdInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (this.PageLoaded)
        {
            GlobalConfig.Translate.BaiduAI.app_id = this.BaiduAI_AppIdInput.Text;
        }
    }

    private void BaiduAI_SecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (this.PageLoaded)
        {
            GlobalConfig.Translate.BaiduAI.app_secret = this.BaiduAI_SecretKeyInput.Password;
        }
    }

    private void LinkLabel_MouseDown(object sender, MouseButtonEventArgs e)
    {
        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo.FileName = ((Label)sender).DataContext.ToString();
        proc.Start();
    }
}