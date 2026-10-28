using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using wotool.CloudService;
using wotool.Entity;
using wotool.Util;
using WinRT.Interop;

namespace wotool.Views;

/// <summary>
///     识别 / 翻译结果窗口
/// </summary>
public sealed partial class ResultWindow : Window
{
    private Bitmap? _bmp;
    private bool _windowLoaded;
    private readonly List<Control> _initialTabStops = new();

    public ResultWindow()
    {
        InitializeComponent();
        SuppressInitialFocus();
        Title = Strings.ResultWindows_Title;
        TitleTextBlock.Text = Strings.ResultWindows_Title;

        if (AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.IsResizable = true;
            presenter.IsMinimizable = true;
            presenter.IsMaximizable = true;
        }

        var handle = WindowNative.GetWindowHandle(this);
        AppWindow.Resize(new Windows.Graphics.SizeInt32((int)(800 * ScreenshotHelper.GetScaleFactor(handle)),
            (int)(900 * ScreenshotHelper.GetScaleFactor(handle))));

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleTextBlock);

        DefaultOcrSettingCheck.Content = Strings.ResultWindows_SetAsDefault;
        DefaultTranslateSettingCheck.Content = Strings.ResultWindows_SetAsDefault;
        StreamingOutputCheckBox.Content = Strings.ResultWindows_StreamingOutput;
        OcrButton.Content = Strings.ResultWindows_ReOCR;
        TranslateButton.Content = Strings.ResultWindows_Translate;

        InitializeComboBoxes();
        Activated += (_, _) =>
        {
            SuppressInitialFocus();
            if (!_windowLoaded)
            {
                _windowLoaded = true;
                LoadDefaults();
            }
            DispatcherQueue.TryEnqueue(RestoreInitialTabStops);
            RootGrid.Focus(FocusState.Programmatic);
        };
        Closed += (_, _) =>
        {
            _bmp?.Dispose();
            Utils.FlushMemory();
        };
    }

    public void SetOcrText(string text) => OcrTextBox.Text = text;

    private void InitializeComboBoxes()
    {
        AddItem(DefaultOcrProvideComboBox, "BaiduCloud", Strings.CloudService_BaiduCloud);
        AddItem(DefaultOcrProvideComboBox, "TencentCloud", Strings.CloudService_TencentCloud);
        AddItem(DefaultOcrProvideComboBox, "SpaceOcr", Strings.CloudService_SpaceOCR);

        AddItem(DefaultTranslateProvideComboBox, "BaiduAi", Strings.CloudService_BaiduAI);
        AddItem(DefaultTranslateProvideComboBox, "GoogleCloud", Strings.CloudService_GoogleCloud);
        AddItem(DefaultTranslateProvideComboBox, "Deeplx", Strings.CloudService_Deeplx);
        AddItem(DefaultTranslateProvideComboBox, "Ai", Strings.CloudService_AI);

        AiProviderComboBox.SelectionChanged += (_, _) =>
        {
            RefreshAiModelItems();
            RefreshTranslateDefaultCheckState();
        };
        AiModelComboBox.SelectionChanged += (_, _) => RefreshTranslateDefaultCheckState();
        AiProviderComboBox.Items.Clear();
        foreach (var provider in GlobalConfig.Translate.AiProviders)
            AddItem(AiProviderComboBox, provider.Id, provider.ToString());
    }

    private static void AddItem(ComboBox comboBox, string tag, string content)
    {
        comboBox.Items.Add(new ComboBoxItem { Tag = tag, Content = content });
    }

    private static string? TagOf(ComboBox comboBox)
    {
        return (comboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
    }

    private void LoadDefaults()
    {
        SelectByTag(DefaultOcrProvideComboBox, GlobalConfig.Ocr.DefaultOcrProvide.ToString());
        RefreshOcrItems();
        SelectByTag(DefaultOcrTypeComboBox, GlobalConfig.Ocr.DefaultOcrType);
        SelectByTag(DefaultOcrLanguageComboBox, GlobalConfig.Ocr.DefaultOcrLanguage);

        SelectByTag(DefaultTranslateProvideComboBox, GlobalConfig.Translate.DefaultTranslateProvide.ToString());
        SelectByTag(AiProviderComboBox, GlobalConfig.Translate.DefaultAiProvider);
        if (AiProviderComboBox.SelectedItem == null && AiProviderComboBox.Items.Count > 0)
            AiProviderComboBox.SelectedIndex = 0;
        RefreshAiModelItems();
        SelectByTag(AiModelComboBox, GlobalConfig.Translate.DefaultAiModel);
        if (AiModelComboBox.SelectedItem == null && AiModelComboBox.Items.Count > 0)
            AiModelComboBox.SelectedIndex = 0;
        StreamingOutputCheckBox.IsChecked = GlobalConfig.Translate.DefaultAiStreaming;
        RefreshTranslateItems();
        SelectByTag(SourceLanguageComboBox, GlobalConfig.Translate.DefaultTranslateSourceLanguage);
        SelectByTag(TargetLanguageComboBox, GlobalConfig.Translate.DefaultTranslateTargetLanguage);

        _windowLoaded = true;
        RefreshDefaultCheckState();
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

    public void StartTranslate()
    {
        Translate(TagOf(DefaultTranslateProvideComboBox), TagOf(SourceLanguageComboBox),
            TagOf(TargetLanguageComboBox), TagOf(AiProviderComboBox), TagOf(AiModelComboBox));
    }

    /// <summary>
    ///     按当前选中的 AI 服务商重建模型选项
    /// </summary>
    private void RefreshAiModelItems()
    {
        AiModelComboBox.Items.Clear();
        var provider = GlobalConfig.Translate.GetAiProvider(TagOf(AiProviderComboBox));
        foreach (var model in provider.Models) AddItem(AiModelComboBox, model.Id, model.ToString());
        if (AiModelComboBox.Items.Count > 0) AiModelComboBox.SelectedIndex = 0;
    }

    public void Translate(string? translateProvideStr = null, string? sourceLanguage = null,
        string? targetLanguage = null, string? aiProviderId = null, string? aiModelId = null)
    {
        TranslateTextBox.Text = Strings.ResultWindows_translating;
        _ = TranslateAsync(translateProvideStr, sourceLanguage, targetLanguage, aiProviderId, aiModelId);
    }

    private async Task TranslateAsync(string? translateProvideStr, string? sourceLanguage, string? targetLanguage,
        string? aiProviderId, string? aiModelId)
    {
        // Let WinUI render the result window and its progress state before starting network work.
        await Task.Yield();
        var ocrText = OcrTextBox.Text?.Trim();
        if (string.IsNullOrEmpty(ocrText))
        {
            TranslateTextBox.Text = string.Empty;
            return;
        }

        GlobalConfig.Translate.TranslateProvideEnum translateProvide;
        if (string.IsNullOrWhiteSpace(translateProvideStr))
            translateProvide = GlobalConfig.Translate.DefaultTranslateProvide;
        else
            translateProvide = (GlobalConfig.Translate.TranslateProvideEnum)Enum.Parse(
                typeof(GlobalConfig.Translate.TranslateProvideEnum), translateProvideStr);
        sourceLanguage ??= GlobalConfig.Translate.DefaultTranslateSourceLanguage!;
        targetLanguage ??= GlobalConfig.Translate.DefaultTranslateTargetLanguage!;

        if (translateProvide == GlobalConfig.Translate.TranslateProvideEnum.BaiduAi &&
            (string.IsNullOrEmpty(GlobalConfig.Translate.BaiduAi.AppId) ||
             string.IsNullOrEmpty(GlobalConfig.Translate.BaiduAi.AppSecret)))
        {
            DialogService.Show(Strings.ResultWindows_EmptyKeyMessage);
            return;
        }

        if (translateProvide == GlobalConfig.Translate.TranslateProvideEnum.Deeplx &&
            string.IsNullOrEmpty(GlobalConfig.Translate.Deeplx.Url))
        {
            DialogService.Show(Strings.ResultWindows_EmptyKeyMessage);
            return;
        }

        var aiProvider = GlobalConfig.Translate.GetAiProvider(aiProviderId);
        var aiModel = GlobalConfig.Translate.GetAiModel(aiProvider, aiModelId);
        if (translateProvide == GlobalConfig.Translate.TranslateProvideEnum.Ai &&
            GlobalConfig.Translate.AiProviders.Count == 0)
        {
            DialogService.Show(Strings.ResultWindows_EmptyAiProviderMessage);
            return;
        }

        if (translateProvide == GlobalConfig.Translate.TranslateProvideEnum.Ai &&
            (string.IsNullOrEmpty(aiProvider.Url) || string.IsNullOrEmpty(aiProvider.ApiKey) ||
             string.IsNullOrEmpty(aiModel.Name)))
        {
            DialogService.Show(Strings.ResultWindows_EmptyKeyMessage);
            return;
        }

        try
        {
            if (translateProvide == GlobalConfig.Translate.TranslateProvideEnum.Ai)
            {
                if (StreamingOutputCheckBox.IsChecked != true)
                {
                    TranslateTextBox.Text = await OpenAiHelper.Translate(ocrText, sourceLanguage, targetLanguage,
                        aiProvider.Id, aiModel.Id);
                    return;
                }

                var pending = new StringBuilder();
                var rendered = new StringBuilder();
                var sync = new object();
                var renderQueued = false;
                var firstDelta = true;
                var hasRenderedContent = false;

                void QueueRender(string delta)
                {
                    lock (sync)
                    {
                        pending.Append(delta);
                        if (renderQueued) return;
                        renderQueued = true;
                    }

                    DispatcherQueue.TryEnqueue(() =>
                    {
                        string batch;
                        lock (sync)
                        {
                            batch = pending.ToString();
                            pending.Clear();
                            renderQueued = false;
                        }

                        if (firstDelta)
                        {
                            TranslateTextBox.Text = string.Empty;
                            firstDelta = false;
                        }

                        rendered.Append(batch);
                        TranslateTextBox.Text = rendered.ToString();
                        hasRenderedContent = true;
                        TranslateTextBox.SelectionStart = TranslateTextBox.Text.Length;
                        TranslateTextBox.SelectionLength = 0;
                    });
                }

                void ShowThinking()
                {
                    DispatcherQueue.TryEnqueue(() =>
                    {
                        if (!hasRenderedContent) TranslateTextBox.Text = Strings.ResultWindows_Thinking;
                    });
                }

                var aiResult = await OpenAiHelper.TranslateStream(ocrText, sourceLanguage, targetLanguage, QueueRender,
                    aiProvider.Id, aiModel.Id, onThinking: ShowThinking);
                if (aiResult.StartsWith("AI API error:", StringComparison.Ordinal))
                    TranslateTextBox.Text = aiResult;
                return;
            }

            TranslateTextBox.Text = translateProvide switch
            {
                GlobalConfig.Translate.TranslateProvideEnum.BaiduAi =>
                    await BaiduAiHelper.Translate(ocrText, sourceLanguage, targetLanguage),
                GlobalConfig.Translate.TranslateProvideEnum.GoogleCloud =>
                    await GoogleCloudHelper.Translate(ocrText, sourceLanguage, targetLanguage),
                GlobalConfig.Translate.TranslateProvideEnum.Deeplx =>
                    await DeeplxHelper.Translate(ocrText, sourceLanguage, targetLanguage),
                _ => string.Empty
            };
        }
        catch (Exception ex)
        {
            TranslateTextBox.Text = ex.Message;
        }
    }

    public void Ocr(Bitmap bmp, string? ocrProvideStr = null, string? ocrType = null, string? ocrLanguage = null)
    {
        _bmp = bmp;
        OcrTextBox.Text = Strings.ResultWindows_ocring;
        _ = OcrAsync(bmp, ocrProvideStr, ocrType, ocrLanguage);
    }

    private void SuppressInitialFocus()
    {
        Control[] controls =
        {
            DefaultOcrProvideComboBox, DefaultOcrTypeComboBox, DefaultOcrLanguageComboBox,
            DefaultOcrSettingCheck, OcrButton, OcrTextBox,
            DefaultTranslateProvideComboBox, AiProviderComboBox, AiModelComboBox,
            StreamingOutputCheckBox, SourceLanguageComboBox, TargetLanguageComboBox,
            DefaultTranslateSettingCheck, TranslateButton, TranslateTextBox
        };

        foreach (var control in controls)
        {
            if (!control.IsTabStop) continue;
            control.IsTabStop = false;
            _initialTabStops.Add(control);
        }
    }

    private void RestoreInitialTabStops()
    {
        foreach (var control in _initialTabStops) control.IsTabStop = true;
        _initialTabStops.Clear();
    }

    private async Task OcrAsync(Bitmap bmp, string? ocrProvideStr, string? ocrType, string? ocrLanguage)
    {
        // Keep the progress state visible before invoking an OCR provider.
        await Task.Yield();
        GlobalConfig.Ocr.OcrProvideEnum ocrProvide;
        if (string.IsNullOrWhiteSpace(ocrProvideStr))
            ocrProvide = GlobalConfig.Ocr.DefaultOcrProvide;
        else
            ocrProvide = (GlobalConfig.Ocr.OcrProvideEnum)Enum.Parse(
                typeof(GlobalConfig.Ocr.OcrProvideEnum), ocrProvideStr);
        ocrType ??= GlobalConfig.Ocr.DefaultOcrType;
        ocrLanguage ??= GlobalConfig.Ocr.DefaultOcrLanguage;

        if (ocrProvide == GlobalConfig.Ocr.OcrProvideEnum.TencentCloud &&
            (string.IsNullOrEmpty(GlobalConfig.Ocr.TencentCloud.SecretId) ||
             string.IsNullOrEmpty(GlobalConfig.Ocr.TencentCloud.SecretKey)))
        {
            DialogService.Show(Strings.ResultWindows_EmptyKeyMessage);
            return;
        }

        if (ocrProvide == GlobalConfig.Ocr.OcrProvideEnum.BaiduCloud &&
            (string.IsNullOrEmpty(GlobalConfig.Ocr.BaiduCloud.ClientId) ||
             string.IsNullOrEmpty(GlobalConfig.Ocr.BaiduCloud.ClientSecret)))
        {
            DialogService.Show(Strings.ResultWindows_EmptyKeyMessage);
            return;
        }

        if (ocrProvide == GlobalConfig.Ocr.OcrProvideEnum.SpaceOcr &&
            string.IsNullOrEmpty(GlobalConfig.Ocr.SpaceOcr.ApiKey))
        {
            DialogService.Show(Strings.ResultWindows_EmptyKeyMessage);
            return;
        }

        try
        {
            OcrTextBox.Text = ocrProvide switch
            {
                GlobalConfig.Ocr.OcrProvideEnum.TencentCloud => await TencentCloudHelper.Ocr(bmp, ocrType),
                GlobalConfig.Ocr.OcrProvideEnum.BaiduCloud => await BaiduCloudHelper.Ocr(bmp, ocrType),
                GlobalConfig.Ocr.OcrProvideEnum.SpaceOcr => await SpaceOcrHelper.Ocr(bmp, ocrType, ocrLanguage),
                _ => string.Empty
            };
        }
        catch (Exception ex)
        {
            OcrTextBox.Text = ex.Message;
        }
    }

    private void OcrButton_Click(object sender, RoutedEventArgs e)
    {
        if (_bmp == null)
        {
            DialogService.Show(Strings.ResultWindows_NotFoundImage);
            return;
        }

        Ocr(_bmp, TagOf(DefaultOcrProvideComboBox), TagOf(DefaultOcrTypeComboBox), TagOf(DefaultOcrLanguageComboBox));
    }

    private void TranslateButton_Click(object sender, RoutedEventArgs e)
    {
        StartTranslate();
    }

    private void DefaultOcrProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
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

    private void OcrTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshDefaultCheckState();
    }

    /// <summary>
    ///     当前选择与配置一致时勾选并禁用「设为默认」
    /// </summary>
    private void RefreshDefaultCheckState()
    {
        RefreshOcrDefaultCheckState();
        RefreshTranslateDefaultCheckState();
    }

    private void RefreshOcrDefaultCheckState()
    {
        if (!_windowLoaded) return;
        var ocrProvide = TagOf(DefaultOcrProvideComboBox);
        var ocrType = TagOf(DefaultOcrTypeComboBox);
        var ocrLanguage = TagOf(DefaultOcrLanguageComboBox);
        if (ocrProvide == null || ocrType == null || ocrLanguage == null) return;

        var isDefault = ocrProvide.Equals(GlobalConfig.Ocr.DefaultOcrProvide.ToString())
                        && ocrType.Equals(GlobalConfig.Ocr.DefaultOcrType)
                        && ocrLanguage.Equals(GlobalConfig.Ocr.DefaultOcrLanguage);
        DefaultOcrSettingCheck.IsChecked = isDefault;
        DefaultOcrSettingCheck.IsEnabled = !isDefault;
    }

    private void DefaultOcrSettingCheck_Checked(object sender, RoutedEventArgs e)
    {
        if (DefaultOcrSettingCheck.IsChecked != true)
        {
            DefaultOcrSettingCheck.IsEnabled = true;
            return;
        }

        DefaultOcrSettingCheck.IsEnabled = false;
        if (!_windowLoaded) return;

        GlobalConfig.Ocr.DefaultOcrProvide = (GlobalConfig.Ocr.OcrProvideEnum)Enum.Parse(
            typeof(GlobalConfig.Ocr.OcrProvideEnum), TagOf(DefaultOcrProvideComboBox)!);
        GlobalConfig.Ocr.DefaultOcrType = TagOf(DefaultOcrTypeComboBox)!;
        GlobalConfig.Ocr.DefaultOcrLanguage = TagOf(DefaultOcrLanguageComboBox)!;
        GlobalConfig.SaveConfig();
    }

    private void DefaultTranslateProvideComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var isAi = TagOf(DefaultTranslateProvideComboBox) == GlobalConfig.Translate.TranslateProvideEnum.Ai.ToString();
        AiProviderComboBox.Visibility = isAi ? Visibility.Visible : Visibility.Collapsed;
        AiModelComboBox.Visibility = isAi ? Visibility.Visible : Visibility.Collapsed;
        StreamingOutputCheckBox.Visibility = isAi ? Visibility.Visible : Visibility.Collapsed;
        RefreshTranslateItems();
    }

    /// <summary>
    ///     按当前选中的翻译服务商重建源 / 目标语言选项
    /// </summary>
    private void RefreshTranslateItems()
    {
        var provide = TagOf(DefaultTranslateProvideComboBox);
        SourceLanguageComboBox.Items.Clear();
        TargetLanguageComboBox.Items.Clear();
        if (provide == null) return;

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

    private void TranslateLanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshTranslateDefaultCheckState();
    }

    private void RefreshTranslateDefaultCheckState()
    {
        if (!_windowLoaded) return;
        var provide = TagOf(DefaultTranslateProvideComboBox);
        var source = TagOf(SourceLanguageComboBox);
        var target = TagOf(TargetLanguageComboBox);
        if (provide == null || source == null || target == null) return;

        var isDefault = provide.Equals(GlobalConfig.Translate.DefaultTranslateProvide.ToString())
                        && IsDefaultAiSelection(provide)
                        && source.Equals(GlobalConfig.Translate.DefaultTranslateSourceLanguage)
                        && target.Equals(GlobalConfig.Translate.DefaultTranslateTargetLanguage);
        DefaultTranslateSettingCheck.IsChecked = isDefault;
        DefaultTranslateSettingCheck.IsEnabled = !isDefault;
    }

    /// <summary>
    ///     非 AI 服务商恒为真；AI 服务商需服务商与模型都与默认配置一致
    /// </summary>
    private bool IsDefaultAiSelection(string provide)
    {
        if (provide != GlobalConfig.Translate.TranslateProvideEnum.Ai.ToString()) return true;
        return TagOf(AiProviderComboBox) == GlobalConfig.Translate.DefaultAiProvider
               && TagOf(AiModelComboBox) == GlobalConfig.Translate.DefaultAiModel
               && StreamingOutputCheckBox.IsChecked == GlobalConfig.Translate.DefaultAiStreaming;
    }

    private void StreamingOutputCheckBox_Click(object sender, RoutedEventArgs e)
    {
        RefreshTranslateDefaultCheckState();
    }

    private void DefaultTranslateSettingCheck_Checked(object sender, RoutedEventArgs e)
    {
        if (DefaultTranslateSettingCheck.IsChecked != true)
        {
            DefaultTranslateSettingCheck.IsEnabled = true;
            return;
        }

        DefaultTranslateSettingCheck.IsEnabled = false;
        if (!_windowLoaded) return;

        GlobalConfig.Translate.DefaultTranslateProvide = (GlobalConfig.Translate.TranslateProvideEnum)Enum.Parse(
            typeof(GlobalConfig.Translate.TranslateProvideEnum), TagOf(DefaultTranslateProvideComboBox)!);
        GlobalConfig.Translate.DefaultTranslateSourceLanguage = TagOf(SourceLanguageComboBox);
        GlobalConfig.Translate.DefaultTranslateTargetLanguage = TagOf(TargetLanguageComboBox);
        if (GlobalConfig.Translate.DefaultTranslateProvide == GlobalConfig.Translate.TranslateProvideEnum.Ai)
        {
            GlobalConfig.Translate.DefaultAiProvider = TagOf(AiProviderComboBox) ?? GlobalConfig.Translate.DefaultAiProvider;
            GlobalConfig.Translate.DefaultAiModel = TagOf(AiModelComboBox) ?? GlobalConfig.Translate.DefaultAiModel;
            GlobalConfig.Translate.DefaultAiStreaming = StreamingOutputCheckBox.IsChecked == true;
        }

        GlobalConfig.SaveConfig();
    }
}
