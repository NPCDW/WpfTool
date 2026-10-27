using System;
using System.Drawing;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;
using Windows.System;
using WpfTool.Util;
using WinRT.Interop;
using Point = Windows.Foundation.Point;
using Rectangle = System.Drawing.Rectangle;

namespace WpfTool.Views;

public enum ScreenshotGoalEnum
{
    Translate,
    Ocr
}

/// <summary>
///     全屏截图窗口：铺一张当前屏幕的静态截图做背景，四块遮罩裁出选区
/// </summary>
public sealed partial class ScreenshotWindow : Window
{
    private readonly ScreenshotGoalEnum _goal;
    private readonly double _dpiScale;
    private readonly Bitmap _bitmap;
    private bool _mouseDown;
    private Point _startPoint;
    private Rect _rectangle;

    public ScreenshotWindow(ScreenshotGoalEnum goal)
    {
        _goal = goal;

        var screenBounds = ScreenshotHelper.GetScreenBoundsOfCursor();
        InitializeComponent();

        var hwnd = WindowNative.GetWindowHandle(this);
        _dpiScale = ScreenshotHelper.GetScaleFactor(hwnd);
        if (_dpiScale <= 0) _dpiScale = 1;

        AppWindow.MoveAndResize(ScreenshotHelper.ToRectInt32(screenBounds));

        if (AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.SetBorderAndTitleBar(false, false);
            presenter.IsResizable = false;
            presenter.IsAlwaysOnTop = true;
            presenter.IsMinimizable = false;
            presenter.IsMaximizable = false;
        }

        try { AppWindow.IsShownInSwitchers = false; } catch { }

        _bitmap = ScreenshotHelper.CaptureScreen(screenBounds);
        ScreenBrush.ImageSource = ScreenshotHelper.BitmapToBitmapImage(_bitmap);

        var initialized = false;
        Activated += (_, _) =>
        {
            if (initialized) return;
            initialized = true;
            RootGrid.Focus(FocusState.Programmatic);
            ApplyMask(new Rect(0, 0, 0, 0));
        };

        Activated += OnActivated;
        Closed += OnClosed;
    }

    private void OnActivated(object sender, WindowActivatedEventArgs args)
    {
        RootGrid.Focus(FocusState.Programmatic);
    }

    private void RootGrid_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Escape) Close();
    }

    private void RootGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        var point = e.GetCurrentPoint(RootGrid);
        if (point.Properties.IsRightButtonPressed)
        {
            Close();
            return;
        }

        if (!point.Properties.IsLeftButtonPressed) return;
        _mouseDown = true;
        _startPoint = point.Position;
        SelectionBorder.Visibility = Visibility.Visible;
    }

    private void RootGrid_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (!_mouseDown) return;
        var current = e.GetCurrentPoint(RootGrid).Position;
        _rectangle = new Rect(
            Math.Min(_startPoint.X, current.X),
            Math.Min(_startPoint.Y, current.Y),
            Math.Abs(current.X - _startPoint.X),
            Math.Abs(current.Y - _startPoint.Y));
        ApplyMask(_rectangle);
    }

    private void RootGrid_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        if (!_mouseDown) return;
        _mouseDown = false;

        var x = (int)(_rectangle.X * _dpiScale);
        var y = (int)(_rectangle.Y * _dpiScale);
        var width = (int)(_rectangle.Width * _dpiScale);
        var height = (int)(_rectangle.Height * _dpiScale);
        if (width <= 0 || height <= 0) return;

        var cropped = ScreenshotHelper.Crop(_bitmap, new Rectangle(x, y, width, height));
        AppWindow.Hide();
        var resultWindow = WindowHelper.Activate<ResultWindow>();
        if (_goal == ScreenshotGoalEnum.Translate)
            _ = resultWindow.ScreenshotTranslateAsync(cropped);
        else
            resultWindow.Ocr(cropped);
        Close();
    }

    /// <summary>
    ///     四块遮罩 + 选框，坐标单位都是 DIP
    /// </summary>
    private void ApplyMask(Rect rect)
    {
        var width = RootGrid.ActualWidth;
        var height = RootGrid.ActualHeight;

        LeftMask.Width = Math.Max(rect.X, 0);
        LeftMask.Height = height;

        RightMask.Margin = new Thickness(rect.X + rect.Width, 0, 0, 0);
        RightMask.Width = Math.Max(width - rect.X - rect.Width, 0);
        RightMask.Height = height;

        UpMask.Margin = new Thickness(rect.X, 0, 0, 0);
        UpMask.Width = rect.Width;
        UpMask.Height = Math.Max(rect.Y, 0);

        DownMask.Margin = new Thickness(rect.X, rect.Y + rect.Height, 0, 0);
        DownMask.Width = rect.Width;
        DownMask.Height = Math.Max(height - rect.Y - rect.Height, 0);

        SelectionBorder.Margin = new Thickness(rect.X, rect.Y, 0, 0);
        SelectionBorder.Width = rect.Width;
        SelectionBorder.Height = rect.Height;
    }

    private void OnClosed(object sender, WindowEventArgs args)
    {
        _bitmap.Dispose();
        Utils.FlushMemory();
    }
}
