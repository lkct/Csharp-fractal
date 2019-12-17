using System.Drawing;
using System.Numerics;

namespace Csharp_fractal
{
    // 复平面上的图像绘制区域
    struct ImgRegion
    {
        public double Left { get; set; } // 左侧边界（-1方向）
        public double Right { get; set; } // 右侧边界（+1方向）
        public double Down { get; set; } // 下方边界（-i方向）
        public double Up { get; set; } // 上方边界（+i方向）
        public double Step { get; set; } // 每像素在复平面中的步长
    }

    // 绘制Julia集
    static class Julia
    {
        // public static bool IsTanRev { get; set; } // 是否X、Y轴颠倒，可外部修改
        public static int MaxIter { get; set; } // 迭代次数，可外部修改
        public static double Power { get; set; } // 迭代方程幂次，可外部修改
        public static int[] PowerNumberList = new int[] { 3, 3, 3, 3, 4, 4, 5, 5, 6, 7, 8 };

        // 传入复平面绘制区域，复数参数c，颜色映射函数，返回绘制好的位图
        public static Bitmap GetJulia(ImgRegion region, Complex c, ColorMap cmap)
        {
            int width = (int)((region.Right - region.Left) / region.Step); // 复平面中绘图区域宽度
            int height = (int)((region.Up - region.Down) / region.Step); // 同上，高度
            Bitmap img = new Bitmap(width, height); // 图像

            int i, j; // 图像坐标
            double x, y; // 复平面坐标
            for (i = 0, x = region.Left; i < width; i++, x += region.Step)
            {
                for (j = 0, y = region.Down; j < height; j++, y += region.Step)
                {
                    //Complex z0 = new Complex(x, y); // 第MaxIter次的迭代值
                    //Complex z1 = Complex.Pow(z0, Power) + c; // 第MaxIter+1次的迭代值
                    //Complex z2 = Complex.Pow(z1, Power) + c; // 第MaxIter+2次的迭代值
                    //for (int iters = 0; iters < MaxIter; iters++)
                    //{
                    //    z0 = z1;
                    //    z1 = z2;
                    //    z2 = Complex.Pow(z1, Power) + c;
                    //}

                    // 牛顿法迭代，求z^n + c = 0的根，n是Power
                    Complex z0 = new Complex(x, y); // 第MaxIter次的迭代值
                    Complex z1 = F(z0, c); // 第MaxIter+1次的迭代值
                    Complex z2 = F(z1, c); // 第MaxIter+2次的迭代值
                    for (int iters = 0; iters < MaxIter; iters++)
                    {
                        z0 = z1;
                        z1 = z2;
                        z2 = F(z1, c);
                    }
                    img.SetPixel(i, j, cmap(z0, z1, z2)); // 映射为颜色，放入图像
                }
            }

            return img;
        }

        private static Complex F(Complex z,Complex c)
        {
            return z - (Complex.Pow(z, Power) + c) / (Power * Complex.Pow(z, Power - 1));
        }
    }
}
