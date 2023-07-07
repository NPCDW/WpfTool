using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfTool.Page.Setting;

public partial class GlobalHotkeyPage : System.Windows.Controls.Page
{
    private bool WindowLoaded = false;
    private byte hotkeysModifiers;
    private int hotkeysKey;
    private string hotkeysText = "";

    public GlobalHotkeyPage()
    {
        InitializeComponent();
    }

    private void GlobalHotkey_OnLoaded(object sender, RoutedEventArgs e)
    {
        this.OcrHotKeyTextBox.Text = GlobalConfig.HotKeys.Ocr.Text;
        this.GetWordsTranslateHotKeyTextBox.Text = GlobalConfig.HotKeys.GetWordsTranslate.Text;
        this.ScreenshotTranslateHotKeyTextBox.Text = GlobalConfig.HotKeys.ScreenshotTranslate.Text;
        this.TopMostHotKeyTextBox.Text = GlobalConfig.HotKeys.TopMost.Text;

        HotKeyConflictCheck();

        this.WindowLoaded = true;
    }

    private void HotKeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        e.Handled = true;
        hotkeysModifiers = 0;
        hotkeysKey = 0;
        hotkeysText = "";
        Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
        if (key == Key.LeftShift || key == Key.RightShift
                                 || key == Key.LeftCtrl || key == Key.RightCtrl
                                 || key == Key.LeftAlt || key == Key.RightAlt
                                 || key == Key.LWin || key == Key.RWin)
        {
            return;
        }

        StringBuilder shortcutText = new StringBuilder();
        if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
        {
            hotkeysModifiers += 2;
            shortcutText.Append("Ctrl + ");
        }

        if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
        {
            hotkeysModifiers += 4;
            shortcutText.Append("Shift + ");
        }

        if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0)
        {
            hotkeysModifiers += 1;
            shortcutText.Append("Alt + ");
        }

        if (hotkeysModifiers == 0 && (key < Key.F1 || key > Key.F12))
        {
            hotkeysKey = 0;
            shortcutText.Clear();
            ((TextBox)sender).Text = hotkeysText = "";
            return;
        }

        hotkeysKey = KeyInterop.VirtualKeyFromKey(key);
        shortcutText.Append(key.ToString());
        ((TextBox)sender).Text = hotkeysText = shortcutText.ToString();
    }

    private void OcrHotKeyTextBox_KeyUp(object sender, KeyEventArgs e)
    {
        Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
        if (key == Key.LeftShift || key == Key.RightShift
                                 || key == Key.LeftCtrl || key == Key.RightCtrl
                                 || key == Key.LeftAlt || key == Key.RightAlt
                                 || key == Key.LWin || key == Key.RWin)
        {
            return;
        }

        GlobalConfig.HotKeys.Ocr.Modifiers = hotkeysModifiers;
        GlobalConfig.HotKeys.Ocr.Key = hotkeysKey;
        GlobalConfig.HotKeys.Ocr.Text = hotkeysText.ToString();
        HotKeysUtil.ReRegisterHotKey();
        HotKeyConflictCheck();
    }

    private void GetWordsTranslateHotKeyTextBox_KeyUp(object sender, KeyEventArgs e)
    {
        Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
        if (key == Key.LeftShift || key == Key.RightShift
                                 || key == Key.LeftCtrl || key == Key.RightCtrl
                                 || key == Key.LeftAlt || key == Key.RightAlt
                                 || key == Key.LWin || key == Key.RWin)
        {
            return;
        }

        GlobalConfig.HotKeys.GetWordsTranslate.Modifiers = hotkeysModifiers;
        GlobalConfig.HotKeys.GetWordsTranslate.Key = hotkeysKey;
        GlobalConfig.HotKeys.GetWordsTranslate.Text = hotkeysText.ToString();
        HotKeysUtil.ReRegisterHotKey();
        HotKeyConflictCheck();
    }

    private void ScreenshotTranslateHotKeyTextBox_KeyUp(object sender, KeyEventArgs e)
    {
        Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
        if (key == Key.LeftShift || key == Key.RightShift
                                 || key == Key.LeftCtrl || key == Key.RightCtrl
                                 || key == Key.LeftAlt || key == Key.RightAlt
                                 || key == Key.LWin || key == Key.RWin)
        {
            return;
        }

        GlobalConfig.HotKeys.ScreenshotTranslate.Modifiers = hotkeysModifiers;
        GlobalConfig.HotKeys.ScreenshotTranslate.Key = hotkeysKey;
        GlobalConfig.HotKeys.ScreenshotTranslate.Text = hotkeysText.ToString();
        HotKeysUtil.ReRegisterHotKey();
        HotKeyConflictCheck();
    }

    private void TopMostHotKeyTextBox_KeyUp(object sender, KeyEventArgs e)
    {
        Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
        if (key == Key.LeftShift || key == Key.RightShift
                                 || key == Key.LeftCtrl || key == Key.RightCtrl
                                 || key == Key.LeftAlt || key == Key.RightAlt
                                 || key == Key.LWin || key == Key.RWin)
        {
            return;
        }

        GlobalConfig.HotKeys.TopMost.Modifiers = hotkeysModifiers;
        GlobalConfig.HotKeys.TopMost.Key = hotkeysKey;
        GlobalConfig.HotKeys.TopMost.Text = hotkeysText.ToString();
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
        GlobalConfig.HotKeys.Ocr.Modifiers = 0;
        GlobalConfig.HotKeys.Ocr.Key = 115;
        GlobalConfig.HotKeys.Ocr.Text = "F4";
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
        this.OcrHotKeyConflictLabel.Visibility =
            GlobalConfig.HotKeys.Ocr.Conflict ? Visibility.Visible : Visibility.Hidden;
        this.GetWordsTranslateHotKeyConflictLabel.Visibility = GlobalConfig.HotKeys.GetWordsTranslate.Conflict
            ? Visibility.Visible
            : Visibility.Hidden;
        this.ScreenshotTranslateHotKeyConflictLabel.Visibility = GlobalConfig.HotKeys.ScreenshotTranslate.Conflict
            ? Visibility.Visible
            : Visibility.Hidden;
        this.TopMostHotKeyConflictLabel.Visibility =
            GlobalConfig.HotKeys.TopMost.Conflict ? Visibility.Visible : Visibility.Hidden;
    }
}