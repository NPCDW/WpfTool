using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using WpfTool.Entity;
using WpfTool.Pages;
using WpfTool.Util;
using WinRT.Interop;
namespace WpfTool.Views;
/// <summary>
///     设置窗口，左侧 NavigationView 对应原来 Wpf.Ui 的 NavigationFluent
/// </summary>
public sealed partial class SettingWindow : Window
{
    public SettingWindow()
    {
        InitializeComponent();
        Title = Strings.Setting_Title;
        AppTitleTextBlock.Text = Strings.Setting_Title;
        CommonNavText.Text = Strings.Setting_Common;
        OcrNavText.Text = Strings.Setting_OCR;
        TranslateNavText.Text = Strings.Setting_MachineTranslate;
        HotkeyNavText.Text = Strings.Setting_GlobalHotkey;
        AboutNavText.Text = Strings.Setting_about;
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        var handle = WindowNative.GetWindowHandle(this);
        var scale = ScreenshotHelper.GetScaleFactor(handle);
        AppWindow.Resize(new Windows.Graphics.SizeInt32((int)(900 * scale), (int)(650 * scale)));
        if (AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.IsResizable = true;
        }
        CenterOnScreen();
        RootNavigation.SelectedItem = CommonNavItem;
        Closed += (_, _) =>
        {
            GlobalConfig.SaveConfig();
            Utils.FlushMemory();
        };
    }
    private void CenterOnScreen()
    {
        var display = DisplayArea.GetFromWindowId(AppWindow.Id, DisplayAreaFallback.Primary);
        var work = display.WorkArea;
        var size = AppWindow.Size;
        AppWindow.Move(new Windows.Graphics.PointInt32(
            work.X + (work.Width - size.Width) / 2,
            work.Y + (work.Height - size.Height) / 2));
    }
    private void RootNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is NavigationViewItem { Tag: string tag }) Navigate(tag);
    }
    private void Navigate(string tag)
    {
        var transition = new EntranceNavigationTransitionInfo();
        switch (tag)
        {
            case "common":
                RootFrame.Navigate(typeof(CommonPage), null, transition);
                break;
            case "ocr":
                RootFrame.Navigate(typeof(OcrPage), null, transition);
                break;
            case "translate":
                RootFrame.Navigate(typeof(MachineTranslatePage), null, transition);
                break;
            case "hotkey":
                RootFrame.Navigate(typeof(GlobalHotkeyPage), null, transition);
                break;
            case "about":
                RootFrame.Navigate(typeof(AboutPage), null, transition);
                break;
        }
    }
    /// <summary>
    ///     语言切换后重建整窗文案
    /// </summary>
    public void Reload()
    {
        Title = Strings.Setting_Title;
        AppTitleTextBlock.Text = Strings.Setting_Title;
        CommonNavText.Text = Strings.Setting_Common;
        OcrNavText.Text = Strings.Setting_OCR;
        TranslateNavText.Text = Strings.Setting_MachineTranslate;
        HotkeyNavText.Text = Strings.Setting_GlobalHotkey;
        AboutNavText.Text = Strings.Setting_about;
        Navigate((RootNavigation.SelectedItem as NavigationViewItem)?.Tag as string ?? "common");
    }
}
