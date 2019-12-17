using System.Drawing;
using System.Numerics;

namespace Csharp_fractal
{
    // 颜色映射函数的委托，将3个复数映射到一个颜色
    public delegate Color ColorMap(Complex z0, Complex z1, Complex z2);
    // TODO: 在ScreenSaver中使用的其它必要的类型，设定为public

    // 颜色映射
    static class ColorMaps
    {
        // TODO: GetCmap中需要用到的定义（属性，方法等），设定为private static
        // TODO: 简写属性初始化方式  private static int MaxIter { get; set; } = 10;
        static ColorMaps()
        {
            // TODO: 静态构造函数，初始化必要参数
        }

        // 根据传入的颜色参数获取颜色映射函数
        public static ColorMap GetCmap(/* TODO: 从DrawJulia中传入必要的参数 */)
        {
            return (z0, z1, z2) =>
            {
                // TODO: 颜色映射
                int r = 0;
                int g = 0;
                int b = 0;
                return Color.FromArgb(r, g, b);
            };
        }
    }
}
