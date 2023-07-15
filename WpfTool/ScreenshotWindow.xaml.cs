using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfTool.Util;
using Control = System.Windows.Forms.Control;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;
using Screen = WpfScreenHelper.Screen;
using Size = System.Drawing.Size;

namespace WpfTool;

/// <summary>
///     Screenshot.xaml 的交互逻辑
/// </summary>
public partial class ScreenshotWindow
{
    private readonly Bitmap _bitmap; // 截屏图片
    private readonly double _dpiScale = 1;
    private readonly ScreenshotGoalEnum _goal;
    private bool _mouseDown; //鼠标是否被按下
    private Rect _rectangle; //保存的矩形
    private Point _startPoint; //鼠标按下的点

    public ScreenshotWindow(ScreenshotGoalEnum goal)
    {
        this._goal = goal;

        // 获取鼠标所在屏幕
        var ms = Control.MousePosition;
        var bounds = new Rect();
        int x = 0, y = 0, width = 0, height = 0;
        foreach (var screen in Screen.AllScreens)
        {
            bounds = screen.WpfBounds;
            _dpiScale = screen.ScaleFactor;
            x = (int)(bounds.X * _dpiScale);
            y = (int)(bounds.Y * _dpiScale);
            width = (int)(bounds.Width * _dpiScale);
            height = (int)(bounds.Height * _dpiScale);
            if (x <= ms.X && ms.X < x + width && y <= ms.Y && ms.Y < y + height) break;
        }

        InitializeComponent();

        // 设置窗体位置、大小（实际宽高，单位unit）
        Top = bounds.X;
        Left = bounds.Y;
        Width = bounds.Width;
        Height = bounds.Height;

        // 设置遮罩
        Canvas.SetLeft(this, bounds.X);
        Canvas.SetTop(this, bounds.Y);
        LeftMask.Width = bounds.Width;
        LeftMask.Height = bounds.Height;

        // 设置窗体背景（像素宽高，单位px）
        _bitmap = new Bitmap(width, height);
        using (var g = Graphics.FromImage(_bitmap))
        {
            g.CopyFromScreen(x, y, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
        }

        Background = Utils.BitmapToImageBrush(_bitmap);
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyStates == Keyboard.GetKeyStates(Key.Escape)) Close();
    }

    private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        Close();
    }

    private void Window_MouseMove(object sender, MouseEventArgs e)
    {
        if (_mouseDown)
        {
            var currentPoint = Mouse.GetPosition(this);
            _rectangle = new Rect(_startPoint, currentPoint);

            Canvas.SetLeft(LeftMask, 0);
            Canvas.SetTop(LeftMask, 0);
            LeftMask.Width = _rectangle.X;
            LeftMask.Height = _bitmap.Height;

            Canvas.SetLeft(RightMask, _rectangle.Left + _rectangle.Width);
            Canvas.SetTop(RightMask, 0);
            RightMask.Width = _bitmap.Width - _rectangle.Left - _rectangle.Width;
            RightMask.Height = _bitmap.Height;

            Canvas.SetLeft(UpMask, _rectangle.Left);
            Canvas.SetTop(UpMask, 0);
            UpMask.Width = _rectangle.Width;
            UpMask.Height = _rectangle.Y;

            Canvas.SetLeft(DownMask, _rectangle.Left);
            Canvas.SetTop(DownMask, _rectangle.Y + _rectangle.Height);
            DownMask.Width = _rectangle.Width;
            DownMask.Height = _bitmap.Height - _rectangle.Height - _rectangle.Y;
        }
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _mouseDown = true;
        _startPoint = Mouse.GetPosition(this);
    }

    private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _mouseDown = false;

        var x = (int)(_rectangle.X * _dpiScale);
        var y = (int)(_rectangle.Y * _dpiScale);
        var width = (int)(_rectangle.Width * _dpiScale);
        var height = (int)(_rectangle.Height * _dpiScale);
        if (width <= 0 || height <= 0) return;
        var bmpOut = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        var g = Graphics.FromImage(bmpOut);
        g.DrawImage(_bitmap,
            new Rectangle(0, 0, width, height),
            new Rectangle(x, y, width, height),
            GraphicsUnit.Pixel);

        Close();

        ResultWindow? window = null;
        foreach (Window item in Application.Current.Windows)
            if (item is ResultWindow)
            {
                window = (ResultWindow)item;
                window.WindowState = WindowState.Normal;
                window.Activate();
                break;
            }

        if (window == null)
        {
            window = new ResultWindow();
            window.Show();
            window.Activate();
        }

        if (_goal == ScreenshotGoalEnum.Translate)
            window.ScreenshotTranslate(bmpOut);
        else
            window.Ocr(bmpOut);
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        Utils.FlushMemory();
    }
}

public enum ScreenshotGoalEnum
{
    Translate,
    Ocr
}