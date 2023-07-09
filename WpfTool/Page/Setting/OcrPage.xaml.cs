using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfTool.Entity;

namespace WpfTool.Page.Setting;

public partial class OcrPage : System.Windows.Controls.Page
{
    private bool PageLoaded = false;

    public OcrPage()
    {
        InitializeComponent();
    }

    private void Ocr_OnLoaded(object sender, RoutedEventArgs e)
    {
        String defaultOcrType = GlobalConfig.Ocr.DefaultOcrType;
        String defaultOcrLanguage = GlobalConfig.Ocr.DefaultOcrLanguage;
        foreach (ComboBoxItem item in this.defaultOcrProvideComboBox.Items)
        {
            if (item.DataContext.Equals(GlobalConfig.Ocr.DefaultOcrProvide.ToString()))
            {
                defaultOcrProvideComboBox.SelectedItem = item;
                break;
            }
        }

        foreach (ComboBoxItem item in this.defaultOcrTypeComboBox.Items)
        {
            if (item.DataContext.ToString().Equals(defaultOcrType))
            {
                defaultOcrTypeComboBox.SelectedItem = item;
                break;
            }
        }

        foreach (ComboBoxItem item in this.defaultOcrLanguageComboBox.Items)
        {
            if (item.DataContext.ToString().Equals(defaultOcrLanguage))
            {
                defaultOcrLanguageComboBox.SelectedItem = item;
                break;
            }
        }

        this.BaiduCloud_AppKeyInput.Text = GlobalConfig.Ocr.BaiduCloud.ClientId;
        this.BaiduCloud_SecretKeyInput.Password = GlobalConfig.Ocr.BaiduCloud.ClientSecret;

        this.TencentCloudOcr_SecretIdInput.Text = GlobalConfig.Ocr.TencentCloud.SecretId;
        this.TencentCloudOcr_SecretKeyInput.Password = GlobalConfig.Ocr.TencentCloud.SecretKey;

        this.SpaceOCR_ApiKeyInput.Password = GlobalConfig.Ocr.SpaceOcr.ApiKey;

        PageLoaded = true;
    }

    private void defaultOcrProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        defaultOcrProvideComboBox.DataContext = ((ComboBoxItem)defaultOcrProvideComboBox.SelectedItem).DataContext;
        if (this.PageLoaded)
        {
            GlobalConfig.Ocr.DefaultOcrProvide = (GlobalConfig.Ocr.OcrProvideEnum)Enum.Parse(
                typeof(GlobalConfig.Ocr.OcrProvideEnum), defaultOcrProvideComboBox.DataContext.ToString());
        }

        if (defaultOcrProvideComboBox.DataContext.ToString() == GlobalConfig.Ocr.OcrProvideEnum.BaiduCloud.ToString())
        {
            defaultOcrTypeComboBox.Items.Clear();
            ComboBoxItem item = new ComboBoxItem();
            item.DataContext = GlobalConfig.Ocr.BaiduCloud.OcrTypeEnum.GeneralBasic.ToString();
            item.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_GeneralBasic");
            defaultOcrTypeComboBox.Items.Add(item);
            ComboBoxItem item2 = new ComboBoxItem();
            item2.DataContext = GlobalConfig.Ocr.BaiduCloud.OcrTypeEnum.AccurateBasic.ToString();
            item2.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_AccurateBasic");
            defaultOcrTypeComboBox.Items.Add(item2);
            ComboBoxItem item3 = new ComboBoxItem();
            item3.DataContext = GlobalConfig.Ocr.BaiduCloud.OcrTypeEnum.Handwriting.ToString();
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
        else if (defaultOcrProvideComboBox.DataContext.ToString() ==
                 GlobalConfig.Ocr.OcrProvideEnum.TencentCloud.ToString())
        {
            defaultOcrTypeComboBox.Items.Clear();
            ComboBoxItem item = new ComboBoxItem();
            item.DataContext = GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralBasicOcr.ToString();
            item.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_GeneralBasic");
            defaultOcrTypeComboBox.Items.Add(item);
            ComboBoxItem item2 = new ComboBoxItem();
            item2.DataContext = GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralAccurateOcr.ToString();
            item2.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_AccurateBasic");
            defaultOcrTypeComboBox.Items.Add(item2);
            ComboBoxItem item3 = new ComboBoxItem();
            item3.DataContext = GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralHandwritingOcr.ToString();
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
        else if (defaultOcrProvideComboBox.DataContext.ToString() ==
                 GlobalConfig.Ocr.OcrProvideEnum.SpaceOcr.ToString())
        {
            defaultOcrTypeComboBox.Items.Clear();
            ComboBoxItem item = new ComboBoxItem();
            item.DataContext = GlobalConfig.Ocr.SpaceOcr.OcrTypeEnum.Engine1.ToString();
            item.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_Engine1");
            defaultOcrTypeComboBox.Items.Add(item);
            ComboBoxItem item2 = new ComboBoxItem();
            item2.DataContext = GlobalConfig.Ocr.SpaceOcr.OcrTypeEnum.Engine2.ToString();
            item2.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_Engine2");
            defaultOcrTypeComboBox.Items.Add(item2);
            ComboBoxItem item3 = new ComboBoxItem();
            item3.DataContext = GlobalConfig.Ocr.SpaceOcr.OcrTypeEnum.Engine3.ToString();
            item3.SetResourceReference(ComboBoxItem.ContentProperty, "OcrType_Engine3");
            defaultOcrTypeComboBox.Items.Add(item3);
            ComboBoxItem item5 = new ComboBoxItem();
            item5.DataContext = GlobalConfig.Ocr.SpaceOcr.OcrTypeEnum.Engine5.ToString();
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

    private void defaultOcrTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (defaultOcrTypeComboBox.SelectedItem == null)
        {
            return;
        }

        defaultOcrTypeComboBox.DataContext = ((ComboBoxItem)defaultOcrTypeComboBox.SelectedItem).DataContext;
        if (this.PageLoaded)
        {
            GlobalConfig.Ocr.DefaultOcrType = defaultOcrTypeComboBox.DataContext.ToString();
        }
    }

    private void TencentCloudOcr_SecretIdInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (this.PageLoaded)
        {
            GlobalConfig.Ocr.TencentCloud.SecretId = this.TencentCloudOcr_SecretIdInput.Text;
        }
    }

    private void TencentCloudOcr_SecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (this.PageLoaded)
        {
            GlobalConfig.Ocr.TencentCloud.SecretKey = this.TencentCloudOcr_SecretKeyInput.Password;
        }
    }

    private void BaiduCloud_AppKeyInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (this.PageLoaded)
        {
            GlobalConfig.Ocr.BaiduCloud.ClientId = this.BaiduCloud_AppKeyInput.Text;
        }
    }

    private void BaiduCloud_SecretKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (this.PageLoaded)
        {
            GlobalConfig.Ocr.BaiduCloud.ClientSecret = this.BaiduCloud_SecretKeyInput.Password;
        }
    }

    private void defaultOcrLanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (defaultOcrLanguageComboBox.SelectedItem == null)
        {
            return;
        }

        defaultOcrLanguageComboBox.DataContext = ((ComboBoxItem)defaultOcrLanguageComboBox.SelectedItem).DataContext;
        if (this.PageLoaded)
        {
            GlobalConfig.Ocr.DefaultOcrLanguage = defaultOcrLanguageComboBox.DataContext.ToString();
        }
    }

    private void SpaceOCR_ApiKeyInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (this.PageLoaded)
        {
            GlobalConfig.Ocr.SpaceOcr.ApiKey = this.SpaceOCR_ApiKeyInput.Password;
        }
    }
}