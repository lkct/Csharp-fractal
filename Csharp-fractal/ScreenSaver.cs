using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

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
            c = RandComplex();
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
            randRate = 0.2;
            // TODO: 新加入参数的初始化
        }

        // 可变参数
        private Random rnd; // 随机数生成器
        private Complex c; // Julia参数
        private int mouseX; // 当前鼠标位置屏幕坐标X
        private int mouseY; // 当前鼠标位置屏幕坐标Y

        // 固定参数
        private readonly ImgRegion region; // 图像绘制区域
        private readonly double speedRate; // c的移动速度与和鼠标位置差的比例
        private readonly double randRate; // 移动方向随机干扰的比例
        // TODO: 其它固定参数，全部设定为private readonly，在GetCmap前使用或在GetCmap中传入\

        // 随机生成模为[0,1)的复数
        private Complex RandComplex()
        {
            double modulus = rnd.NextDouble(); // 模，[0,1)
            double argument = rnd.NextDouble() * 2 * Math.PI; // 幅角，[0,2π)
            return new Complex(modulus * Math.Cos(argument), modulus * Math.Sin(argument)); // 转化为直角坐标
        }

        // 绘制Julia集放入显示
        private void DrawJulia()
        {
            // TODO: 必要的处理，如随机数
            ColorMap cmap = ColorMaps.GetCmap(/* TODO: 传入必要的参数，包括固定参数 */);

            Complex mouseC = new Complex(mouseX * region.Step + region.Left, region.Up - mouseY * region.Step); // 鼠标在复平面中坐标
            Complex randC = RandComplex(); // 随机扰动
            c += speedRate * (mouseC - c) + randRate * randC; // 更新c
            pictureBox.Image = Julia.GetJulia(region, c, cmap); // 重新绘图
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
            GC.Collect();
            DrawJulia();
        }

        // 按下任意键退出
        private void ScreenSaver_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            Application.Exit();
        }
    }
}
