using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        private double DpiScale = 1;

        public ScreenshotWindow(ScreenshotGoalEnum goal)
        {
            this.goal = goal;

            // 获取鼠标所在屏幕
            System.Drawing.Point ms = System.Windows.Forms.Control.MousePosition;
            Rect bounds = new Rect();
            foreach (WpfScreenHelper.Screen screen in WpfScreenHelper.Screen.AllScreens)
            {
                bounds = screen.WpfBounds;
                if (bounds.X < ms.X && ms.X < bounds.X + bounds.Width && bounds.Y < ms.Y && ms.Y < bounds.Y + bounds.Height)
                {
                    DpiScale = screen.ScaleFactor;
                    break;
                }
            }

            InitializeComponent();

            // 测试
            foreach (var item in WpfScreenHelper.Screen.AllScreens)
            {
                Console.WriteLine(item.WpfBounds);
                Console.WriteLine(item.Bounds);
                Console.WriteLine(item.ScaleFactor);
            }

            // 设置窗体位置、大小（实际宽高，单位unit）
            this.Top = bounds.X;
            this.Left = bounds.Y;
            this.Width = bounds.Width;
            this.Height = bounds.Height;

            // 设置窗体背景（像素宽高，单位px）
            int width = (int)(bounds.Width * DpiScale);
            int height = (int)(bounds.Height * DpiScale);
            bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen((int)bounds.X, (int)bounds.Y, 0, 0, new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);
            }
            bitmap.Save("E:/Temp/1.png");
            this.Background = Utils.BitmapToImageBrush(bitmap);

            // 设置遮罩
            Canvas.SetLeft(this, bounds.X);
            Canvas.SetTop(this, bounds.Y);
            LeftMask.Width = bounds.Width;
            LeftMask.Height = bounds.Height;
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

            int width = (int)(Rectangle.Width * DpiScale);
            int height = (int)(Rectangle.Height * DpiScale);
            if (width <= 0 || height <= 0)
            {
                return;
            }
            Bitmap bmpOut = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmpOut);
            g.DrawImage(bitmap,
                new System.Drawing.Rectangle(0, 0, width, height),
                new System.Drawing.Rectangle((int)(Rectangle.X * DpiScale), (int)(Rectangle.Y * DpiScale), width, height),
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
