using System.Drawing;
using System.Numerics;
using System;

namespace Csharp_fractal
{
    // 颜色映射函数的委托，将3个复数映射到一个颜色
    public delegate Color ColorMap(Complex z0, Complex z1, Complex z2);

    // 颜色映射
    static class ColorMaps
    {
        public const int MaxColorRoundValue = 512;
        public const int RAND_MAX = 0x7fff;
        // 三个有关变量的系数
        private const double dL1_coef = 0.6;
        private const double dL2_coef = 0.6;
        private const double dL3_coef = 2.0;

        // 颜色变化强度
        public static double m_kR { get; set; }  
        public static double m_kG { get; set; }
        public static double m_kB { get; set; }
        // 初始颜色
        public static double m_R0 { get; set; }  
        public static double m_G0 { get; set; }
        public static double m_B0 { get; set; }
        // 三个变量的比例
        public static double m_ColorK1 { get; set; }
        public static double m_ColorK2 { get; set; }
        public static double m_ColorK3 { get; set; }

        private static int[] ColorRound_table = new int[(MaxColorRoundValue + 1) * 2];

        static ColorMaps()
        {
            Creat_colorRound_table();
        }
        
        // 参数随机重置
        public static void ResetArgs(Random rnd, int rand)
        {
            m_ColorK1 = 0.216;
            m_ColorK2 = 0.6;
            m_ColorK3 = 0.6;
            if (rnd.NextDouble() < 0.5)
                m_ColorK1 *= -1;
            if (rnd.NextDouble() < 0.5)
                m_ColorK2 *= -1;
            if (rnd.NextDouble() < 0.5)
                m_ColorK3 *= -1;

            double r = 1.0 / (double)(1 << (int)(Julia.Power - 3));
            r = Math.Pow(r, 0.095); //r大概在0.8到1之间
            m_ColorK1 *= r;
            m_ColorK2 *= r;
            m_ColorK3 *= r;

            ColorMoverInit(rnd, 50 * r, 90 * r); //初始化颜色
        }

        public static void ColorMoverInit(Random rnd, double kMin, double kMax)
        //参数取值：第一个不到50 第二个不到90  
        {
            //颜色变化强度！
            m_kR = rnd.Next(RAND_MAX) * (1.0 / RAND_MAX) * (kMax - kMin) + kMin; //40多到90之间的随机浮点数
            m_kG = rnd.Next(RAND_MAX) * (1.0 / RAND_MAX) * (kMax - kMin) + kMin;
            m_kB = rnd.Next(RAND_MAX) * (1.0 / RAND_MAX) * (kMax - kMin) + kMin;

            //初始颜色
            m_R0 = rnd.Next(RAND_MAX) * (1.0 / RAND_MAX) * MaxColorRoundValue; //0到512之间的随机浮点数
            m_G0 = rnd.Next(RAND_MAX) * (1.0 / RAND_MAX) * MaxColorRoundValue;
            m_B0 = rnd.Next(RAND_MAX) * (1.0 / RAND_MAX) * MaxColorRoundValue;

        }

        private static void Creat_colorRound_table()
        {
            for (int i = 0; i < ColorRound_table.Length; i++) //首尾相接，为了柔和
                ColorRound_table[i] = Round_color(i);
                //ColorRound_table[i] = Round_color(i - (MaxColorRoundValue + 1)); //取值是-513到+512
        }

        private static int Round_color(int x)
        {
            //if (x < 0) x = -x;//取值变成  513到0，再到512。首尾相接
            //while (x > MaxColorRoundValue) x -= MaxColorRoundValue;//最后x范围是 1 512到0 再到512
            double rd = (Math.Sin(x * (2.0 * Math.PI / MaxColorRoundValue)) + 1.1) / 2.1;//色环，正好2pi转了一圈
            //rd取值从 -0.1到1到0.1到1到-0.1
            int ri = (int)(rd * 255 + 0.5);
            //long ri=abs(x-csMaxColorRoundValue/2);
            if (ri < 0) return 0;
            else if (ri > 255) return 255;
            else return ri;
        }


        private static int GetColor(double Color0, double k, double Gene)
        {
            uint colorIndex = (uint)(Color0 + k * Gene) % (uint)MaxColorRoundValue;
            return ColorRound_table[colorIndex];
        }

        // 根据传入的颜色参数获取颜色映射函数
        public static ColorMap GetCmap()
        {
            return (z0, z1, z2) =>
            {
                Complex diff1 = z1 - z0;
                Complex diff2 = z2 - z1;
                double dL1 = Math.Log(Math.Abs(diff1.Real * diff1.Imaginary)) * dL1_coef;
                double dL2 = Math.Log(diff1.Magnitude * diff1.Magnitude) * dL2_coef;
                double dL3 = Math.Log(Math.Abs(diff2.Real) + Math.Abs(diff2.Imaginary)) * dL3_coef;


                double kR = dL1 * m_ColorK1 + dL2 * m_ColorK2 - dL3 * m_ColorK3;
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
