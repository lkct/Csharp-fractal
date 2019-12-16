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
    public partial class Form1 : Form // TODO: Form1 naming
    {
        public Form1()
        {
            InitializeComponent();
            DrawJulia();
        }

        private void DrawJulia()
        {
            double step = 0.01; // TODO:
            int width = pictureBox1.Width / 2; // TODO: // TODO: pictureBox1 naming
            int height = pictureBox1.Height / 2; // TODO:
            ImgRegion region = new ImgRegion
            {
                Step = step,
                Left = -width * step / 2,
                Right = width * step / 2,
                Down = -height * step / 2,
                Up = height * step / 2,
            };
            Complex c = new Complex(0.0, -0.8); // TODO:
            double[] bg_rgb = new double[3] { 0.5, 1.5, 1.5 }; // TODO:
            double[] fg_rgb = new double[3] { 0.9, 0.9, 0.9 }; // TODO:
            Double2IntColor d2i = (v, coef) => (int)(Math.Pow(v, coef) * 255); // TODO:
            ColorMap cmap = ColorMaps.GetCmap(bg_rgb, fg_rgb, d2i);
            pictureBox1.Image = new Bitmap(Julia.GetJulia(region, c, cmap), pictureBox1.Width, pictureBox1.Height);
        }
    }
}
