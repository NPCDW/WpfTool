using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfTool.Util;

namespace WpfTool
{
    /// <summary>
    /// Screenshot.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenshotWindow : Window
    {
        private Rect Rectangle = new Rect();         //保存的矩形
        private System.Windows.Point StartPoint;        //鼠标按下的点
        private new bool MouseDown = false;         //鼠标是否被按下
        private Bitmap bitmap;  // 截屏图片
        private ScreenshotGoalEnum goal;
        private double dpiScale = 1;

        public ScreenshotWindow(ScreenshotGoalEnum goal)
        {
            this.goal = goal;

            // 获取鼠标所在屏幕
            System.Drawing.Point ms = System.Windows.Forms.Control.MousePosition;
            Rect bounds = new Rect();
            int x = 0, y = 0, width = 0, height = 0;
            foreach (WpfScreenHelper.Screen screen in WpfScreenHelper.Screen.AllScreens)
            {
                bounds = screen.WpfBounds;
                dpiScale = screen.ScaleFactor;
                x = (int)(bounds.X * dpiScale);
                y = (int)(bounds.Y * dpiScale);
                width = (int)(bounds.Width * dpiScale);
                height = (int)(bounds.Height * dpiScale);
                if (x <= ms.X && ms.X < x + width && y <= ms.Y && ms.Y < y + height)
                {
                    break;
                }
            }

            InitializeComponent();

            // 设置窗体位置、大小（实际宽高，单位unit）
            this.Top = bounds.X;
            this.Left = bounds.Y;
            this.Width = bounds.Width;
            this.Height = bounds.Height;

            // 设置遮罩
            Canvas.SetLeft(this, bounds.X);
            Canvas.SetTop(this, bounds.Y);
            LeftMask.Width = bounds.Width;
            LeftMask.Height = bounds.Height;

            // 设置窗体背景（像素宽高，单位px）
            bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);
            }
            this.Background = Utils.BitmapToImageBrush(bitmap);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Escape))
            {
                this.Close();
            }
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseDown)
            {
                System.Windows.Point CurrentPoint = Mouse.GetPosition(this);
                Rectangle = new Rect(StartPoint, CurrentPoint);

                Canvas.SetLeft(LeftMask, 0);
                Canvas.SetTop(LeftMask, 0);
                LeftMask.Width = Rectangle.X;
                LeftMask.Height = bitmap.Height;

                Canvas.SetLeft(RightMask, Rectangle.Left + Rectangle.Width);
                Canvas.SetTop(RightMask, 0);
                RightMask.Width = bitmap.Width - Rectangle.Left - Rectangle.Width;
                RightMask.Height = bitmap.Height;

                Canvas.SetLeft(UpMask, Rectangle.Left);
                Canvas.SetTop(UpMask, 0);
                UpMask.Width = Rectangle.Width;
                UpMask.Height = Rectangle.Y;

                Canvas.SetLeft(DownMask, Rectangle.Left);
                Canvas.SetTop(DownMask, Rectangle.Y + Rectangle.Height);
                DownMask.Width = Rectangle.Width;
                DownMask.Height = bitmap.Height - Rectangle.Height - Rectangle.Y;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MouseDown = true;
            StartPoint = Mouse.GetPosition(this);
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MouseDown = false;

            int x = (int)(Rectangle.X * dpiScale);
            int y = (int)(Rectangle.Y * dpiScale);
            int width = (int)(Rectangle.Width * dpiScale);
            int height = (int)(Rectangle.Height * dpiScale);
            if (width <= 0 || height <= 0)
            {
                return;
            }
            Bitmap bmpOut = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmpOut);
            g.DrawImage(bitmap,
                new System.Drawing.Rectangle(0, 0, width, height),
                new System.Drawing.Rectangle(x, y, width, height),
                GraphicsUnit.Pixel);

            this.Close();

            ResultWindow window = null;
            foreach (Window item in Application.Current.Windows)
            {
                if (item is ResultWindow)
                {
                    window = (ResultWindow)item;
                    window.WindowState = WindowState.Normal;
                    window.Activate();
                    break;
                }
            }
            if (window == null)
            {
                window = new ResultWindow();
                window.Show();
                window.Activate();
            }
            if (goal == ScreenshotGoalEnum.translate)
            {
                window.screenshotTranslate(bmpOut);
            }
            else
            {
                window.ocr(bmpOut);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Utils.FlushMemory();
        }
    }

    public enum ScreenshotGoalEnum
    {
        translate,
        ocr,
    }
}
