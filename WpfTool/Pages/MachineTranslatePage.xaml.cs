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
        AiProviderHeader.Text = Strings.Setting_AiProviderHeader;
        AddAiProviderButton.Content = Strings.Setting_AddAiProvider;
        AiProviderNote.Content = Strings.CloudService_AI;

        DefaultTranslateProvideComboBox.Items.Clear();
        AddItem(DefaultTranslateProvideComboBox, "BaiduAi", Strings.CloudService_BaiduAI);
        AddItem(DefaultTranslateProvideComboBox, "GoogleCloud", Strings.CloudService_GoogleCloud);
        AddItem(DefaultTranslateProvideComboBox, "Deeplx", Strings.CloudService_Deeplx);
        AddItem(DefaultTranslateProvideComboBox, "Ai", Strings.CloudService_AI);

        SelectByTag(DefaultTranslateProvideComboBox, GlobalConfig.Translate.DefaultTranslateProvide.ToString());
        RefreshAiModels();
        SelectByTag(AiModelComboBox, GlobalConfig.Translate.DefaultAiModel);
        RefreshTranslateItems();
        SelectByTag(SourceLanguageComboBox, GlobalConfig.Translate.DefaultTranslateSourceLanguage);
        SelectByTag(TargetLanguageComboBox, GlobalConfig.Translate.DefaultTranslateTargetLanguage);

        BaiduAiAppIdInput.Text = GlobalConfig.Translate.BaiduAi.AppId;
        BaiduAiSecretKeyInput.Password = GlobalConfig.Translate.BaiduAi.AppSecret;

        DeeplxUrlInput.Text = GlobalConfig.Translate.Deeplx.Url;
        DeeplxAuthorizationInput.Password = GlobalConfig.Translate.Deeplx.Authorization;

        RefreshAiProviderBoxes();
        BuildAiProviderCards();

        _pageLoaded = true;
    }

    /// <summary>
    ///     刷新默认服务商下拉，并按当前服务商重建模型下拉
    /// </summary>
    private void RefreshAiProviderBoxes()
    {
        AiProviderComboBox.Items.Clear();
        foreach (var provider in GlobalConfig.Translate.AiProviders)
            AddItem(AiProviderComboBox, provider.Id, provider.ToString());

        SelectByTag(AiProviderComboBox, GlobalConfig.Translate.DefaultAiProvider);
        if (AiProviderComboBox.SelectedItem == null && AiProviderComboBox.Items.Count > 0)
            AiProviderComboBox.SelectedIndex = 0;
        RefreshAiModels();
    }

    private void RefreshAiModels()
    {
        AiModelComboBox.Items.Clear();
        var provider = GlobalConfig.Translate.GetAiProvider(TagOf(AiProviderComboBox));
        foreach (var model in provider.Models) AddItem(AiModelComboBox, model.Id, model.ToString());
        if (AiModelComboBox.Items.Count > 0) AiModelComboBox.SelectedIndex = 0;
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

        var isAi = provide == GlobalConfig.Translate.TranslateProvideEnum.Ai.ToString();
        AiProviderComboBox.Visibility = isAi ? Visibility.Visible : Visibility.Collapsed;
        AiModelComboBox.Visibility = isAi ? Visibility.Visible : Visibility.Collapsed;

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
        else if (provide.Equals(GlobalConfig.Translate.TranslateProvideEnum.GoogleCloud.ToString()))
            list = GoogleTranslateLanguageExtension.TranslateLanguageAttributeList;
        else if (provide.Equals(GlobalConfig.Translate.TranslateProvideEnum.Deeplx.ToString()))
            list = DeeplxTranslateLanguageExtension.TranslateLanguageAttributeList;
        else if (provide.Equals(GlobalConfig.Translate.TranslateProvideEnum.Ai.ToString()))
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

    /// <summary>
    ///     默认 AI 服务商 / 模型的选择
    /// </summary>
    private void AiProviderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_pageLoaded) return;
        RefreshAiModels();
        var tag = TagOf(AiProviderComboBox);
        if (tag == null) return;

        GlobalConfig.Translate.DefaultAiProvider = tag;
        // 切换服务商后原来的默认模型可能不在新服务商下，重置为该服务商的第一个模型
        GlobalConfig.Translate.DefaultAiModel = TagOf(AiModelComboBox) ?? "";
    }

    private void AiModelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_pageLoaded) return;
        var tag = TagOf(AiModelComboBox);
        if (tag != null) GlobalConfig.Translate.DefaultAiModel = tag;
    }

    /// <summary>
    ///     生成 AI 服务商配置卡片：服务商（名称 / Url / ApiKey）+ 其下的多个模型
    /// </summary>
    private void BuildAiProviderCards()
    {
        AiProviderPanel.Children.Clear();
        if (GlobalConfig.Translate.AiProviders.Count == 0)
        {
            AiProviderPanel.Children.Add(new TextBlock
            {
                Text = Strings.Setting_NoAiProvider,
                Opacity = 0.75,
                TextWrapping = TextWrapping.Wrap
            });
            return;
        }

        foreach (var provider in GlobalConfig.Translate.AiProviders)
            AiProviderPanel.Children.Add(BuildAiProviderCard(provider));
    }

    private UIElement BuildAiProviderCard(AiProvider provider)
    {
        var headerLabel = new TextBlock { Text = provider.ToString(), FontWeight = Microsoft.UI.Text.FontWeights.SemiBold };
        var removeProviderButton = new Button { Content = Strings.Setting_RemoveAiProvider };

        var header = new Grid();
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        headerLabel.VerticalAlignment = VerticalAlignment.Center;
        Grid.SetColumn(removeProviderButton, 1);
        header.Children.Add(headerLabel);
        header.Children.Add(removeProviderButton);

        var nameInput = new TextBox { Text = provider.Name, Header = Strings.Setting_AiProviderName };
        var urlInput = new TextBox { Text = provider.Url, Header = "Url" };
        var apiKeyInput = new PasswordBox { Password = provider.ApiKey, Header = "API Key" };

        var modelPanel = new StackPanel { Spacing = 4 };
        foreach (var model in provider.Models) modelPanel.Children.Add(BuildAiModelRow(provider, model));

        var addModelButton = new Button { Content = Strings.Setting_AddAiModel };

        nameInput.TextChanged += (_, _) =>
        {
            provider.Name = nameInput.Text ?? "";
            headerLabel.Text = provider.ToString();
            RefreshAiProviderBoxes();
        };
        urlInput.TextChanged += (_, _) => provider.Url = urlInput.Text ?? "";
        apiKeyInput.PasswordChanged += (_, _) => provider.ApiKey = apiKeyInput.Password;
        addModelButton.Click += (_, _) =>
        {
            var model = new AiProvider.AiModel();
            provider.Models.Add(model);
            modelPanel.Children.Add(BuildAiModelRow(provider, model));
            RefreshAiProviderBoxes();
        };
        removeProviderButton.Click += (_, _) =>
        {
            GlobalConfig.Translate.AiProviders.Remove(provider);
            GlobalConfig.Translate.NormalizeAiSelection();
            RefreshAiProviderBoxes();
            BuildAiProviderCards();
        };

        var content = new StackPanel { Spacing = 8, Margin = new Thickness(0, 8, 0, 0) };
        content.Children.Add(nameInput);
        content.Children.Add(urlInput);
        content.Children.Add(apiKeyInput);
        content.Children.Add(modelPanel);
        content.Children.Add(addModelButton);

        var root = new StackPanel();
        root.Children.Add(header);
        root.Children.Add(content);

        return new Border
        {
            Child = root,
            Padding = new Thickness(12),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(4),
            BorderBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Gray)
        };
    }

    private UIElement BuildAiModelRow(AiProvider provider, AiProvider.AiModel model)
    {
        var modelInput = new TextBox { Text = model.Name, PlaceholderText = Strings.Setting_AiModel };
        var removeModelButton = new Button { Content = Strings.Setting_RemoveAiModel };

        var row = new Grid();
        row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        Grid.SetColumn(removeModelButton, 1);
        row.ColumnSpacing = 8;
        row.Children.Add(modelInput);
        row.Children.Add(removeModelButton);

        modelInput.TextChanged += (_, _) =>
        {
            model.Name = modelInput.Text ?? "";
            RefreshAiModels();
        };
        removeModelButton.Click += (_, _) =>
        {
            provider.Models.Remove(model);
            if (row.Parent is Panel panel) panel.Children.Remove(row);
            GlobalConfig.Translate.NormalizeAiSelection();
            RefreshAiProviderBoxes();
        };

        return row;
    }

    private void AddAiProviderButton_Click(object sender, RoutedEventArgs e)
    {
        var provider = new AiProvider();
        provider.Models.Add(new AiProvider.AiModel());
        GlobalConfig.Translate.AiProviders.Add(provider);
        GlobalConfig.Translate.NormalizeAiSelection();
        RefreshAiProviderBoxes();
        BuildAiProviderCards();
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
}
