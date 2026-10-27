using System;
using System.Text;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using WpfTool.Entity;
using WpfTool.Util;
using Windows.System;
using Windows.UI.Core;

namespace WpfTool.Pages;

public sealed partial class GlobalHotkeyPage : Page
{
    private byte _hotkeysModifiers;
    private int _hotkeysKey;
    private string _hotkeysText = "";

    public GlobalHotkeyPage()
    {
        InitializeComponent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        OcrHeader.Text = Strings.Setting_OCR;
        WordTranslationHeader.Text = Strings.Setting_WordTranslation;
        ScreenshotTranslationHeader.Text = Strings.Setting_ScreenshotTranslation;
        TopMostHeader.Text = Strings.Setting_TopMostToggle;
        ResetButton.Content = Strings.Setting_Reset;

        OcrHotKeyConflictLabel.Text = Strings.Setting_HotkeyConflict;
        GetWordsTranslateHotKeyConflictLabel.Text = Strings.Setting_HotkeyConflict;
        ScreenshotTranslateHotKeyConflictLabel.Text = Strings.Setting_HotkeyConflict;
        TopMostHotKeyConflictLabel.Text = Strings.Setting_HotkeyConflict;

        OcrHotKeyTextBox.Text = GlobalConfig.HotKeys.OcrHotKey.Text;
        GetWordsTranslateHotKeyTextBox.Text = GlobalConfig.HotKeys.GetWordsTranslate.Text;
        ScreenshotTranslateHotKeyTextBox.Text = GlobalConfig.HotKeys.ScreenshotTranslate.Text;
        TopMostHotKeyTextBox.Text = GlobalConfig.HotKeys.TopMost.Text;

        HotKeyConflictCheck();
    }

    /// <summary>
    ///     WinUI 的 KeyDown 无法可靠拿到修饰键状态，这里用 InputKeyboardSource
    /// </summary>
    private static VirtualKeyModifiers GetModifiers()
    {
        var modifiers = VirtualKeyModifiers.None;
        var state = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control);
        if (state.HasFlag(CoreVirtualKeyStates.Down)) modifiers |= VirtualKeyModifiers.Control;
        state = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift);
        if (state.HasFlag(CoreVirtualKeyStates.Down)) modifiers |= VirtualKeyModifiers.Shift;
        state = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Menu);
        if (state.HasFlag(CoreVirtualKeyStates.Down)) modifiers |= VirtualKeyModifiers.Menu;
        state = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.LeftWindows);
        if (state.HasFlag(CoreVirtualKeyStates.Down)) modifiers |= VirtualKeyModifiers.Windows;
        return modifiers;
    }

    private static bool IsModifierKey(VirtualKey key)
    {
        return key is VirtualKey.Shift or VirtualKey.LeftShift or VirtualKey.RightShift
            or VirtualKey.Control or VirtualKey.LeftControl or VirtualKey.RightControl
            or VirtualKey.Menu or VirtualKey.LeftMenu or VirtualKey.RightMenu
            or VirtualKey.LeftWindows or VirtualKey.RightWindows;
    }

    private void HotKeyTextBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        e.Handled = true;
        _hotkeysModifiers = 0;
        _hotkeysKey = 0;
        _hotkeysText = "";

        var key = e.Key;
        if (IsModifierKey(key)) return;

        var modifiers = GetModifiers();
        var shortcutText = new StringBuilder();
        if (modifiers.HasFlag(VirtualKeyModifiers.Control))
        {
            _hotkeysModifiers += 2;
            shortcutText.Append("Ctrl + ");
        }

        if (modifiers.HasFlag(VirtualKeyModifiers.Shift))
        {
            _hotkeysModifiers += 4;
            shortcutText.Append("Shift + ");
        }

        if (modifiers.HasFlag(VirtualKeyModifiers.Menu))
        {
            _hotkeysModifiers += 1;
            shortcutText.Append("Alt + ");
        }

        if (modifiers.HasFlag(VirtualKeyModifiers.Windows))
        {
            _hotkeysModifiers += 8;
            shortcutText.Append("Win + ");
        }

        if (_hotkeysModifiers == 0 && (key < VirtualKey.F1 || key > VirtualKey.F12))
        {
            ((TextBox)sender).Text = _hotkeysText = "";
            return;
        }

        _hotkeysKey = (int)key;
        shortcutText.Append(key);
        ((TextBox)sender).Text = _hotkeysText = shortcutText.ToString();

        if (ReferenceEquals(sender, OcrHotKeyTextBox)) ApplyOcr();
        else if (ReferenceEquals(sender, GetWordsTranslateHotKeyTextBox)) ApplyGetWordsTranslate();
        else if (ReferenceEquals(sender, ScreenshotTranslateHotKeyTextBox)) ApplyScreenshotTranslate();
        else if (ReferenceEquals(sender, TopMostHotKeyTextBox)) ApplyTopMost();
    }

    private void ApplyOcr()
    {
        GlobalConfig.HotKeys.OcrHotKey.Modifiers = _hotkeysModifiers;
        GlobalConfig.HotKeys.OcrHotKey.Key = _hotkeysKey;
        GlobalConfig.HotKeys.OcrHotKey.Text = _hotkeysText;
        HotKeysUtil.ReRegisterHotKey();
        HotKeyConflictCheck();
    }

    private void ApplyGetWordsTranslate()
    {
        GlobalConfig.HotKeys.GetWordsTranslate.Modifiers = _hotkeysModifiers;
        GlobalConfig.HotKeys.GetWordsTranslate.Key = _hotkeysKey;
        GlobalConfig.HotKeys.GetWordsTranslate.Text = _hotkeysText;
        HotKeysUtil.ReRegisterHotKey();
        HotKeyConflictCheck();
    }

    private void ApplyScreenshotTranslate()
    {
        GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers = _hotkeysModifiers;
        GlobalConfig.HotKeys.ScreenshotTranslate.Key = _hotkeysKey;
        GlobalConfig.HotKeys.ScreenshotTranslate.Text = _hotkeysText;
        HotKeysUtil.ReRegisterHotKey();
        HotKeyConflictCheck();
    }

    private void ApplyTopMost()
    {
        GlobalConfig.HotKeys.TopMost.Modifiers = _hotkeysModifiers;
        GlobalConfig.HotKeys.TopMost.Key = _hotkeysKey;
        GlobalConfig.HotKeys.TopMost.Text = _hotkeysText;
        HotKeysUtil.ReRegisterHotKey();
        HotKeyConflictCheck();
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        GetWordsTranslateHotKeyTextBox.Text = "F2";
        OcrHotKeyTextBox.Text = "F4";
        ScreenshotTranslateHotKeyTextBox.Text = "Ctrl + F2";
        TopMostHotKeyTextBox.Text = "F6";

        GlobalConfig.HotKeys.GetWordsTranslate.Modifiers = 0;
        GlobalConfig.HotKeys.GetWordsTranslate.Key = 113;
        GlobalConfig.HotKeys.GetWordsTranslate.Text = "F2";
        GlobalConfig.HotKeys.OcrHotKey.Modifiers = 0;
        GlobalConfig.HotKeys.OcrHotKey.Key = 115;
        GlobalConfig.HotKeys.OcrHotKey.Text = "F4";
        GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers = 2;
        GlobalConfig.HotKeys.ScreenshotTranslate.Key = 113;
        GlobalConfig.HotKeys.ScreenshotTranslate.Text = "Ctrl + F2";
        GlobalConfig.HotKeys.TopMost.Modifiers = 0;
        GlobalConfig.HotKeys.TopMost.Key = 117;
        GlobalConfig.HotKeys.TopMost.Text = "F6";

        HotKeysUtil.ReRegisterHotKey();
        HotKeyConflictCheck();
    }

    private void HotKeyConflictCheck()
    {
        OcrHotKeyConflictLabel.Visibility =
            GlobalConfig.HotKeys.OcrHotKey.Conflict ? Visibility.Visible : Visibility.Collapsed;
        GetWordsTranslateHotKeyConflictLabel.Visibility = GlobalConfig.HotKeys.GetWordsTranslate.Conflict
            ? Visibility.Visible
            : Visibility.Collapsed;
        ScreenshotTranslateHotKeyConflictLabel.Visibility = GlobalConfig.HotKeys.ScreenshotTranslate.Conflict
            ? Visibility.Visible
            : Visibility.Collapsed;
        TopMostHotKeyConflictLabel.Visibility =
            GlobalConfig.HotKeys.TopMost.Conflict ? Visibility.Visible : Visibility.Collapsed;
    }
}
