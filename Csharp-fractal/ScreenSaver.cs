using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Csharp_fractal
{
    public partial class ScreenSaver : Form
    {
        public ScreenSaver()
        {
            InitializeComponent();

            Rectangle rect = Screen.GetWorkingArea(this);
            this.Size = new Size(rect.Width, rect.Height);
            pictureBox.Location = new Point(0, 0);
            pictureBox.Size = new Size(rect.Width, rect.Height);

            rnd = new Random();
            c = 2 * new Complex(rnd.NextDouble() - 0.5, rnd.NextDouble() - 0.5);
            mouseX = 0;
            mouseY = 0;
        }

        private Random rnd;
        private Complex c;
        private int mouseX;
        private int mouseY;

        private void DrawJulia()
        {
            int width = pictureBox.Width;
            int height = pictureBox.Height;
            double step = 0.002; // TODO:
            ImgRegion region = new ImgRegion // TODO:
            {
                Step = step,
                Left = -width * step / 2,
                Right = width * step / 2,
                Down = -height * step / 2,
                Up = height * step / 2,
            };
            double[] bg_rgb = new double[3] { 0.5, 1.5, 1.5 }; // TODO:
            double[] fg_rgb = new double[3] { 0.7, 0.8, 0.9 }; // TODO:
            Double2IntColor d2i = (v, coef) => (int)(Math.Pow(v, coef) * 255); // TODO:
            ColorMap cmap = ColorMaps.GetCmap(bg_rgb, fg_rgb, d2i);

            double speedRate = 0.2; // TODO:
            double randRate = 0.5; // TODO:
            Complex mouseC = new Complex(mouseX * step + region.Left, region.Up - mouseY * step);
            Complex randC = new Complex(rnd.NextDouble() - 0.5, rnd.NextDouble() - 0.5);
            c += speedRate * (mouseC - c) + randRate * randC;
            pictureBox.Image = Julia.GetJulia(region, c, cmap);
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void tmrDraw_Tick(object sender, EventArgs e)
        {
            GC.Collect();
            DrawJulia();
        }

        private void ScreenSaver_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            Application.Exit();
        }
    }
}
