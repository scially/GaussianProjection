using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathMethod
{
    public class Integral
    {
        //梯形公式
        public static double TiXing(Func<double, double> fun, double up, double down)
        {
            return (up - down) / 2 * (fun(up) + fun(down));
        }
        //辛浦生公式
        public static double Simpson(Func<double, double> fun, double up, double down)
        {
            return (up - down) / 6 * (fun(up) + fun(down) + 4 * fun((up + down) / 2));
        }

        public static double Cotes(Func<double, double> fun, double up, double down)
        {
            double C = (up - down) / 90 * (7 * fun(up) + 7 * fun(down) + 32 * fun((up + 3 * down) / 4)
                     + 12 * fun((up + down) / 2) + 32 * fun((3 * up + down) / 4));
            return C;
        }
        //复化梯形公式
        public static double FuHuaTiXing(Func<double, double> fun, int N, double up, double down)
        {
            double h = (up - down) / N;
            double result = 0;
            double x = down;
            for (int i = 0; i < N - 1; i++)
            {
                x += h;
                result += fun(x);
            }
            result = (fun(up) + result * 2 + fun(down)) * h / 2;
            return result;
        }
        //复化辛浦生公式
        public static double FSimpson(Func<double, double> fun, int N, double up, double down)
        {
            double h = (up - down) / N;
            double result = 0;
            for (int n = 0; n < N; n++)
            {
                result += h / 6 * (fun(down) + 4 * fun(down + h / 2) + fun(down + h));
                down += h;
            }
            return result;
        }
        //复化科特斯公式
        public static double FCotes(Func<double, double> fun, int N, double up, double down)
        {
            double h = (up - down) / N;
            double result = 0;
            for (int n = 0; n < N; n++)
            {
                result += h / 90 * (7 * fun(down) + 32 * fun(down + h / 4) + 12 * fun(down + h / 2) +
                        32 * fun(down + 3 * h / 4) + 7 * fun(down + h));
                down += h;
            }
            return result;
        }
        //龙贝格算法
        public static double Romberg(Func<double, double> fun, double e, double up, double down)
        {
            double R1 = 0, R2 = 0;
            int k = 0; //2的k次方即为N（划分的子区间数）
            R1 = (64 * C(fun, 2 * (int)Math.Pow(2, k), up, down) - C(fun, (int)Math.Pow(2, k++), up, down)) / 63;
            R2 = (64 * C(fun, 2 * (int)Math.Pow(2, k), up, down) - C(fun, (int)Math.Pow(2, k++), up, down)) / 63;
            while (Math.Abs(R2 - R1) > e)
            {
                R1 = R2;
                R2 = (64 * C(fun, 2 * (int)Math.Pow(2, k), up, down) - C(fun, (int)Math.Pow(2, k++), up, down)) / 63;
            }
            return R2;
        }
        private static double S(Func<double, double> fun, int N, double up, double down)
        {
            return (4 * FuHuaTiXing(fun, 2 * N, up, down) - FuHuaTiXing(fun, N, up, down)) / 3;
        }
        private static double C(Func<double, double> fun, int N, double up, double down)
        {
            return (16 * S(fun, 2 * N, up, down) - S(fun, N, up, down)) / 15;
        }
    }
}
