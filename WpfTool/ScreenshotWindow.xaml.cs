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

        public ScreenshotWindow(ScreenshotGoalEnum goal)
        {
            this.goal = goal;

            // 获取鼠标所在屏幕
            System.Drawing.Point ms = System.Windows.Forms.Control.MousePosition;
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.PrimaryScreen;
            foreach (System.Windows.Forms.Screen item in System.Windows.Forms.Screen.AllScreens)
            {
                if (item.Bounds.X < ms.X && ms.X < item.Bounds.X + item.Bounds.Width && item.Bounds.Y < ms.Y && ms.Y < item.Bounds.Y + item.Bounds.Height)
                {
                    screen = item;
                    break;
                }
            }

            InitializeComponent();

            // 设置窗体位置、大小
            this.Top = screen.Bounds.X;
            this.Left = screen.Bounds.Y;
            this.Width = screen.Bounds.Width;
            this.Height = screen.Bounds.Height;

            // 设置窗体背景
            bitmap = new Bitmap(screen.Bounds.Width, screen.Bounds.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, screen.Bounds.Size, CopyPixelOperation.SourceCopy);
            }
            this.Background = Utils.BitmapToImageBrush(bitmap);

            // 设置遮罩
            Canvas.SetLeft(this, screen.Bounds.X);
            Canvas.SetTop(this, screen.Bounds.Y);
            LeftMask.Width = bitmap.Width;
            LeftMask.Height = bitmap.Height;
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

            double dpi = NativeMethod.GetDpi();
            int width = (int)(Rectangle.Width * dpi);
            int height = (int)(Rectangle.Height * dpi);
            if (width <= 0 || height <= 0)
            {
                return;
            }
            Bitmap bmpOut = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmpOut);
            g.DrawImage(bitmap,
                new System.Drawing.Rectangle(0, 0, width, height),
                new System.Drawing.Rectangle((int)(Rectangle.X * dpi), (int)(Rectangle.Y * dpi), width, height),
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
