using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfTool.Entity;
using WpfTool.Util;

namespace WpfTool.Page.Setting;

public partial class GlobalHotkeyPage
{
    private int _hotkeysKey;
    private byte _hotkeysModifiers;
    private string _hotkeysText = "";

    public GlobalHotkeyPage()
    {
        InitializeComponent();
    }

    private void GlobalHotkey_OnLoaded(object sender, RoutedEventArgs e)
    {
        OcrHotKeyTextBox.Text = GlobalConfig.HotKeys.OcrHotKey.Text;
        GetWordsTranslateHotKeyTextBox.Text = GlobalConfig.HotKeys.GetWordsTranslate.Text;
        ScreenshotTranslateHotKeyTextBox.Text = GlobalConfig.HotKeys.ScreenshotTranslate.Text;
        TopMostHotKeyTextBox.Text = GlobalConfig.HotKeys.TopMost.Text;

        HotKeyConflictCheck();
    }

    private void HotKeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        e.Handled = true;
        _hotkeysModifiers = 0;
        _hotkeysKey = 0;
        _hotkeysText = "";
        var key = e.Key == Key.System ? e.SystemKey : e.Key;
        if (key == Key.LeftShift || key == Key.RightShift
                                 || key == Key.LeftCtrl || key == Key.RightCtrl
                                 || key == Key.LeftAlt || key == Key.RightAlt
                                 || key == Key.LWin || key == Key.RWin)
            return;

        var shortcutText = new StringBuilder();
        if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
        {
            _hotkeysModifiers += 2;
            shortcutText.Append("Ctrl + ");
        }

        if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
        {
            _hotkeysModifiers += 4;
            shortcutText.Append("Shift + ");
        }

        if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0)
        {
            _hotkeysModifiers += 1;
            shortcutText.Append("Alt + ");
        }

        if (_hotkeysModifiers == 0 && (key < Key.F1 || key > Key.F12))
        {
            _hotkeysKey = 0;
            shortcutText.Clear();
            ((TextBox)sender).Text = _hotkeysText = "";
            return;
        }

        _hotkeysKey = KeyInterop.VirtualKeyFromKey(key);
        shortcutText.Append(key.ToString());
        ((TextBox)sender).Text = _hotkeysText = shortcutText.ToString();
    }

    private void OcrHotKeyTextBox_KeyUp(object sender, KeyEventArgs e)
    {
        var key = e.Key == Key.System ? e.SystemKey : e.Key;
        if (key == Key.LeftShift || key == Key.RightShift
                                 || key == Key.LeftCtrl || key == Key.RightCtrl
                                 || key == Key.LeftAlt || key == Key.RightAlt
                                 || key == Key.LWin || key == Key.RWin)
            return;

        GlobalConfig.HotKeys.OcrHotKey.Modifiers = _hotkeysModifiers;
        GlobalConfig.HotKeys.OcrHotKey.Key = _hotkeysKey;
        GlobalConfig.HotKeys.OcrHotKey.Text = _hotkeysText;
        HotKeysUtil.ReRegisterHotKey();
        HotKeyConflictCheck();
    }

    private void GetWordsTranslateHotKeyTextBox_KeyUp(object sender, KeyEventArgs e)
    {
        var key = e.Key == Key.System ? e.SystemKey : e.Key;
        if (key == Key.LeftShift || key == Key.RightShift
                                 || key == Key.LeftCtrl || key == Key.RightCtrl
                                 || key == Key.LeftAlt || key == Key.RightAlt
                                 || key == Key.LWin || key == Key.RWin)
            return;

        GlobalConfig.HotKeys.GetWordsTranslate.Modifiers = _hotkeysModifiers;
        GlobalConfig.HotKeys.GetWordsTranslate.Key = _hotkeysKey;
        GlobalConfig.HotKeys.GetWordsTranslate.Text = _hotkeysText;
        HotKeysUtil.ReRegisterHotKey();
        HotKeyConflictCheck();
    }

    private void ScreenshotTranslateHotKeyTextBox_KeyUp(object sender, KeyEventArgs e)
    {
        var key = e.Key == Key.System ? e.SystemKey : e.Key;
        if (key == Key.LeftShift || key == Key.RightShift
                                 || key == Key.LeftCtrl || key == Key.RightCtrl
                                 || key == Key.LeftAlt || key == Key.RightAlt
                                 || key == Key.LWin || key == Key.RWin)
            return;

        GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers = _hotkeysModifiers;
        GlobalConfig.HotKeys.ScreenshotTranslate.Key = _hotkeysKey;
        GlobalConfig.HotKeys.ScreenshotTranslate.Text = _hotkeysText;
        HotKeysUtil.ReRegisterHotKey();
        HotKeyConflictCheck();
    }

    private void TopMostHotKeyTextBox_KeyUp(object sender, KeyEventArgs e)
    {
        var key = e.Key == Key.System ? e.SystemKey : e.Key;
        if (key == Key.LeftShift || key == Key.RightShift
                                 || key == Key.LeftCtrl || key == Key.RightCtrl
                                 || key == Key.LeftAlt || key == Key.RightAlt
                                 || key == Key.LWin || key == Key.RWin)
            return;

        GlobalConfig.HotKeys.TopMost.Modifiers = _hotkeysModifiers;
        GlobalConfig.HotKeys.TopMost.Key = _hotkeysKey;
        GlobalConfig.HotKeys.TopMost.Text = _hotkeysText;
        HotKeysUtil.ReRegisterHotKey();
        HotKeyConflictCheck();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
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
            GlobalConfig.HotKeys.OcrHotKey.Conflict ? Visibility.Visible : Visibility.Hidden;
        GetWordsTranslateHotKeyConflictLabel.Visibility = GlobalConfig.HotKeys.GetWordsTranslate.Conflict
            ? Visibility.Visible
            : Visibility.Hidden;
        ScreenshotTranslateHotKeyConflictLabel.Visibility = GlobalConfig.HotKeys.ScreenshotTranslate.Conflict
            ? Visibility.Visible
            : Visibility.Hidden;
        TopMostHotKeyConflictLabel.Visibility =
            GlobalConfig.HotKeys.TopMost.Conflict ? Visibility.Visible : Visibility.Hidden;
    }
}