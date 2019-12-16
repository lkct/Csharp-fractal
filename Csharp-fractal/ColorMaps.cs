using System;
using System.Drawing;

namespace Csharp_fractal
{
    public delegate Color ColorMap(double norm, int iter);
    public delegate int Double2IntColor(double v, double coef);

    static class ColorMaps
    {
        public static ColorMap GetCmap(double[] bg_rgb, double[] fg_rgb, Double2IntColor d2i)
        {
            return (norm, iter) =>
            {
                if (norm < Julia.R)
                {
                    return Color.Black;
                }

                double v = Math.Log(iter - Math.Log(Math.Log(norm, 2), 2) + 2, 2) / 4;
                int r, g, b;
                if (v < 1.0)
                {
                    double conv = Math.Max(0, v); // 收敛率
                    r = d2i(conv, bg_rgb[0]);
                    g = d2i(conv, bg_rgb[1]);
                    b = d2i(conv, bg_rgb[2]);
                }
                else
                {
                    double div = Math.Max(0, 2 - v);
                    r = d2i(div, fg_rgb[0]); // 发散率
                    g = d2i(div, fg_rgb[1]);
                    b = d2i(div, fg_rgb[2]);
                }
                return Color.FromArgb(r, g, b);
            };
        }
    }
}
