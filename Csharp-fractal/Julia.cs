using System.Drawing;
using System.Numerics;

namespace Csharp_fractal
{
    struct ImgRegion
    {
        public double Left { get; set; }
        public double Right { get; set; }
        public double Down { get; set; }
        public double Up { get; set; }
        public double Step { get; set; }
    }

    static class Julia
    {
        public static double R { get; set; } = 4.0;
        public static int MaxIter { get; set; } = 100;

        public static Bitmap GetJulia(ImgRegion region, Complex c, ColorMap cmap)
        {
            int width = (int)((region.Right - region.Left) / region.Step);
            int height = (int)((region.Up - region.Down) / region.Step);
            Bitmap img = new Bitmap(width, height);

            int i, j;
            double x, y;
            for (i = 0, x = region.Left; i < width; i++, x += region.Step)
            {
                for (j = 0, y = region.Down; j < height; j++, y += region.Step)
                {
                    Complex z = new Complex(x, y);
                    int iters;
                    for (iters = 0; iters < MaxIter; iters++)
                    {
                        if (z.Magnitude >= R)
                        {
                            break;
                        }
                        z = z * z + c;
                    }
                    img.SetPixel(i, j, cmap(z.Magnitude, iters));
                }
            }

            return img;
        }
    }
}
