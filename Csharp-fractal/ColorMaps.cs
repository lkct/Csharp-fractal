using System.Drawing;
using System.Numerics;
using System;

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
        public const int MaxColorRoundValue = 512;
        public const int RAND_MAX = 0x7fff;
        private const double dL1_coef = 0.6;
        private const double dL2_coef = 0.6;
        private const double dL3_coef = 2.0;

        public static double m_kR { get; set; }  //颜色变化强度
        public static double m_kG { get; set; }
        public static double m_kB { get; set; }
        public static double m_R0 { get; set; }  //初始颜色
        public static double m_G0 { get; set; }
        public static double m_B0 { get; set; }
        public static double m_ColorK1 { get; set; }
        public static double m_ColorK2 { get; set; }
        public static double m_ColorK3 { get; set; }

        private static int[] colorRound_table = new int[(MaxColorRoundValue + 1) * 2];

        static ColorMaps()
        {
            // TODO: 静态构造函数，初始化必要参数
            Creat_colorRound_table();
        }

        public static void ColorMoverInit(int rand, double kMin, double kMax)
        //参数取值！第一个不到50 第二个 不到90  
        {
            //颜色变化强度！
            m_kR = rand * (1.0 / RAND_MAX) * (kMax - kMin) + kMin; //40多到90之间的随机浮点数
            m_kG = rand * (1.0 / RAND_MAX) * (kMax - kMin) + kMin;
            m_kB = rand * (1.0 / RAND_MAX) * (kMax - kMin) + kMin;

            //初始颜色
            m_R0 = rand * (1.0 / RAND_MAX) * MaxColorRoundValue; //0到512之间的随机浮点数
            m_G0 = rand * (1.0 / RAND_MAX) * MaxColorRoundValue;
            m_B0 = rand * (1.0 / RAND_MAX) * MaxColorRoundValue;

        }

        private static void Creat_colorRound_table()
        {
            for (int i = 0; i < (MaxColorRoundValue + 1) * 2; i++)//首尾相接！为了柔和！太强大了！哈哈，明白了
                colorRound_table[i] = round_color(i - (MaxColorRoundValue + 1));//取值是 -513到+510！！太强大了！
        }//i是0到1023

        private static int round_color(int x)
        {
            if (x < 0) x = -x;//取值变成  513到0，再到512。首尾相接
            while (x > MaxColorRoundValue) x -= MaxColorRoundValue;//最后x范围是 1 512 到0 再到512
            const double PI = 3.1415926535897932384626433832795;
            double rd = (Math.Sin(x * (2.0 * PI / MaxColorRoundValue)) + 1.1) / 2.1;//色环！正好2pi转了一圈！
            //rd取值从 -0.1到1到0.1到1到-0.1
            int ri = (int)(rd * 255 + 0.5);
            //long ri=abs(x-csMaxColorRoundValue/2);
            if (ri < 0) return 0;
            else if (ri > 255) return 255;
            else return ri;
        }

        private static int GetColor(double Color0,double k,double Gene)
        {
            uint colorIndex = (uint)(Color0 + k * Gene) % (uint)MaxColorRoundValue;
            return colorRound_table[colorIndex];
        }

        // 根据传入的颜色参数获取颜色映射函数
        public static ColorMap GetCmap(/* TODO: 从DrawJulia中传入必要的参数 */)
        {
            return (z0, z1, z2) =>
            {
                Complex diff1 = z1 - z0;
                Complex diff2 = z2 - z1;
                double dL1 = Math.Log(Math.Abs(diff1.Real * diff1.Imaginary)) * dL1_coef;
                double dL2 = Math.Log(diff1.Magnitude * diff1.Magnitude) * dL2_coef;
                double dL3 = Math.Log(Math.Abs(diff2.Real) + Math.Abs(diff2.Imaginary)) * dL3_coef;


                double kR = dL1 * m_ColorK1 + dL2 * m_ColorK2 - dL3 * m_ColorK3;//这个配比很奇怪！不懂为啥这样
                double kG = dL1 * m_ColorK1 - dL2 * m_ColorK2 + dL3 * m_ColorK3;
                double kB = -dL1 * m_ColorK1 + dL2 * m_ColorK2 + dL3 * m_ColorK3;

                int red = (int)(GetColor(m_R0, m_kR, kR));
                int green = (int)(GetColor(m_G0, m_kG, kG));
                int blue = (int)GetColor(m_B0, m_kB, kB);
                return Color.FromArgb(red, green, blue);
            };
        }
    }
}
