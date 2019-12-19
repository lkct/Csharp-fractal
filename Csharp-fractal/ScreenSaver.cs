using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;
using System.IO;

namespace Csharp_fractal
{
    public partial class ScreenSaver : Form
    {
        public ScreenSaver()
        {
            InitializeComponent();

            // 设定显示区域大小
            Rectangle rect = Screen.GetWorkingArea(this);
            pictureBox.Location = new Point(0, 0);
            pictureBox.Size = new Size(rect.Width, rect.Height);

            // 变化参数初始化
            rnd = new Random();
            c = RandComplex(1);
            mouseX = 0;
            mouseY = 0;

            // 固定参数初始化
            double step = 0.002;
            region = new ImgRegion
            {
                Step = step,
                Left = -rect.Width * step / 2,
                Right = rect.Width * step / 2,
                Down = -rect.Height * step / 2,
                Up = rect.Height * step / 2,
            };
            speedRate = 0.2;
            IsFirstRun = true;
            imageIndex = 0;
        }

        // 可变参数
        private Random rnd; // 随机数生成器
        private Complex c; // Julia参数
        private int mouseX; // 当前鼠标位置屏幕坐标X
        private int mouseY; // 当前鼠标位置屏幕坐标Y
        private bool IsFirstRun; // 是否第一次绘图，因为第一次绘图效果不太好，所以用这个把它略过
        private int imageIndex; // 图的编号

        // 固定参数
        private readonly ImgRegion region; // 图像绘制区域
        private readonly double speedRate; // c的移动速度与和鼠标位置差的比例

        // 随机生成模为[0,1)的复数
        private Complex RandComplex(double mo)
        {
            double modulus = mo * rnd.NextDouble(); // 模，[0,mo)
            double argument = rnd.NextDouble() * 2 * Math.PI; // 幅角，[0,2π)
            return new Complex(modulus * Math.Cos(argument), modulus * Math.Sin(argument)); // 转化为直角坐标
        }

        // 绘制Julia集放入显示
        private void DrawJulia()
        {
            int rand = rnd.Next(ColorMaps.RAND_MAX);

            ColorMaps.ResetArgs(rnd, rand);
            Julia.ResetArgs(rand);

            ColorMap cmap = ColorMaps.GetCmap();

            Complex mouseC = new Complex(mouseX * region.Step + region.Left, region.Up - mouseY * region.Step); // 鼠标在复平面中坐标
            //Console.WriteLine(c);
            c += speedRate * (mouseC - c); // 更新c
            if (IsFirstRun)
                IsFirstRun = false;
            else
            {
                pictureBox.Image = Julia.GetJulia(region, c, cmap); // 重新绘图
                pictureBox.Image.Save("../../Images/image" + imageIndex++ + ".bmp"); // 保存图片
            }
        }

        // 鼠标移动时更新鼠标位置
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
        }

        // 每一定时间重新绘图
        private void TmrDraw_Tick(object sender, EventArgs e)
        {
            tmrDraw.Stop();
            GC.Collect();
            DrawJulia();
            tmrDraw.Start(); // 只等待100ms就绘制下一帧，等待时间主要为缓冲区绘制时间
        }

        // 按下任意键退出
        private void ScreenSaver_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            Application.Exit();
        }
    }
}
