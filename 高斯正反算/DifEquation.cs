using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathMethod
{
    public class DifEquation
    {
        //欧拉迭代公式
        //求出区间(x0,x]中每隔步长h的精度为e的近似值
        //fun为x的函数即 dy/dx=fun(x,y)
        //f(x0)=y0
        public static double[] Euler_e(Func<double, double, double> fun, int N, double x0, double y0, double x, double e = 1e-10)
        {
            double[] res = new double[N];
            double h = (x - x0) / N;
            for (int i = 0; i < N; i++)
            {
                x0 = x0 + h;
                double yn = y0;
                if (i - 1 >= 0)
                    yn = res[i - 1];
                res[i] = yn + h * fun(x0 - h, yn);
                double res_e = 0;
                while (Math.Abs(res_e - res[i]) >= e)
                {
                    res_e = res[i];
                    res[i] = yn + h / 2 * (fun(x0 - h, yn) + fun(x0, res_e));
                }
            }
            return res;
        }

        //求出区间(x0,x]中每隔步长h的精度为e的近似值
        //fun为x的函数即 dy/dx=fun(x)
        //f(x0)=y0
        public static double[] Euler_e(Func<double, double> fun, int N, double x0, double y0, double x, double e = 1e-10)
        {
            double[] res = new double[N];
            double h = (x - x0) / N;
            for (int i = 0; i < N; i++)
            {
                x0 = x0 + h;
                double yn = y0;
                if (i - 1 >= 0)
                    yn = res[i - 1];
                res[i] = yn + h * fun(x0 - h);
                double res_e = 0;
                while (Math.Abs(res_e - res[i]) >= e)
                {
                    res_e = res[i];
                    res[i] = yn + h / 2 * (fun(x0 - h) + fun(x0));
                }
            }
            return res;
        }

        //欧拉预估-矫正公式
        //求出区间(x0,x]中每隔步长h的近似值
        //fun为x的函数即 dy/dx=fun(x,y)
        //f(x0)=y0
        public static double[] Euler_pre(Func<double, double, double> fun, int N, double x0, double y0, double x)
        {
            double[] res = new double[N];
            double h = (x - x0) / N;
            for (int i = 0; i < N; i++)
            {
                x0 = x0 + h;
                double yn = y0;
                if (i - 1 >= 0)
                    yn = res[i - 1];
                res[i] = yn + h * fun(x0 - h, yn);
                res[i] = yn + h / 2 * (fun(x0 - h, yn) + fun(x0, res[i]));

            }
            return res;
        }
        //欧拉预估-矫正公式
        //求出区间(x0,x]中每隔步长h的近似值
        //fun为x的函数即 dy/dx=fun(x)
        //f(x0)=y0
        public static double[] Euler_pre(Func<double, double> fun, int N, double x0, double y0, double x)
        {
            double[] res = new double[N];
            double h = (x - x0) / N;
            for (int i = 0; i < N; i++)
            {
                x0 = x0 + h;
                double yn = y0;
                if (i - 1 >= 0)
                    yn = res[i - 1];
                res[i] = yn + h * fun(x0 - h);
                res[i] = yn + h / 2 * (fun(x0 - h) + fun(x0));
            }
            return res;
        }

        //二阶龙格-库塔算法
        //求出区间(x0,x]中每隔步长h的近似值
        //fun为x的函数即 dy/dx=fun(x,y)
        //f(x0)=y0
        public static double[] R_K2(Func<double, double, double> fun, int N, double x0, double y0, double x)
        {
            double[] res = new double[N];
            double h = (x - x0) / N;
            for (int i = 0; i < N; i++)
            {
                double yn = y0;
                if (i - 1 >= 0)
                    yn = res[i - 1];
                double k1 = h * fun(x0, yn);
                double k2 = h * fun(x0 + 2.0 / 3 * h, yn + 2.0 / 3 * k1);
                res[i] = yn + 1.0 / 4 * (k1 + 3 * k2);
                x0 += h;
            }
            return res;
        }
        //二阶龙格-库塔算法
        //求出区间(x0,x]中每隔步长h的近似值
        //fun为x的函数即 dy/dx=fun(x)
        //f(x0)=y0
        public static double[] R_K2(Func<double, double> fun, int N, double x0, double y0, double x)
        {
            double[] res = new double[N];
            double h = (x - x0) / N;
            for (int i = 0; i < N; i++)
            {
                double yn = y0;
                if (i - 1 >= 0)
                    yn = res[i - 1];
                double k1 = h * fun(x0);
                double k2 = h * fun(x0 + 2.0 / 3 * h);
                res[i] = yn + 1.0 / 4 * (k1 + 3 * k2);
                x0 += h;
            }
            return res;
        }
        //四阶龙格-库塔算法
        public static double[] R_K4(Func<double, double, double> fun, int N, double x0, double y0, double x)
        {
            double[] res = new double[N];
            double h = (x - x0) / N;
            for (int i = 0; i < N; i++)
            {
                double yn = y0;
                if (i - 1 >= 0)
                    yn = res[i - 1];
                double k1 = h * fun(x0, yn);
                double k2 = h * fun(x0 + 0.5 * h, yn + 0.5 * k1);
                double k3 = h * fun(x0 + 0.5 * h, yn + 0.5 * k2);
                double k4 = h * fun(x0 + h, yn + k3);
                res[i] = yn + 1.0 / 6 * (k1 + 2 * (k2 + k3) + k4);
                x0 += h;
            }
            return res;
        }
        //四阶龙格-库塔算法re
        public static double[] R_K4(Func<double, double> fun, int N, double x0, double y0, double x)
        {
            double[] res = new double[N];
            double h = (x - x0) / N;
            for (int i = 0; i < N; i++)
            {
                double yn = y0;
                if (i - 1 >= 0)
                    yn = res[i - 1];
                double k1 = h * fun(x0);
                double k2 = h * fun(x0 + 0.5 * h);
                double k3 = h * fun(x0 + 0.5 * h);
                double k4 = h * fun(x0 + h);
                res[i] = yn + 1.0 / 6 * (k1 + 2 * (k2 + k3) + k4);
                x0 += h;
            }
            return res;
        }

        //====================================================================================//
        //欧拉迭代公式，根据y求x
        public static double Euler_ex(Func<double, double, double> fun, int N, double x0, double y0, double x, double y, double e = 1e-10)
        {
            double h = (x - x0) / N;
            double[] res = Euler_e(fun, N, x0, y0, x, e);
            return ArrayOfy(res, h, x0, y);
        }


        //欧拉迭代公式，根据y求x
        public static double Euler_ex(Func<double, double> fun, int N, double x0, double y0, double x, double y, double e = 1e-10)
        {
            double h = (x - x0) / N;
            double[] res = Euler_e(fun, N, x0, y0, x, e);
            return ArrayOfy(res, h, x0, y);
        }

        //欧拉预估-矫正公式
        public static double Euler_prex(Func<double, double, double> fun, int N, double x0, double y0, double x, double y)
        {
            double h = (x - x0) / N;
            double[] res = Euler_pre(fun, N, x0, y0, x);
            return ArrayOfy(res, h, x0, y);
        }
        //欧拉预估-矫正公式
        public static double Euler_prex(Func<double, double> fun, int N, double x0, double y0, double x, double y)
        {
            double h = (x - x0) / N;
            double[] res = Euler_pre(fun, N, x0, y0, x);
            return ArrayOfy(res, h, x0, y);
        }

        //二阶龙格-库塔算法
        public static double R_K2x(Func<double, double, double> fun, int N, double x0, double y0, double x, double y)
        {
            double h = (x - x0) / N;
            double[] res = Euler_pre(fun, N, x0, y0, x);
            return ArrayOfy(res, h, x0, y);
        }
        //二阶龙格-库塔算法
        public static double R_K2x(Func<double, double> fun, int N, double x0, double y0, double x, double y)
        {
            double h = (x - x0) / N;
            double[] res = Euler_pre(fun, N, x0, y0, x);
            return ArrayOfy(res, h, x0, y);
        }
        //四阶龙格-库塔算法
        public static double R_K4x(Func<double, double, double> fun, int N, double x0, double y0, double x, double y)
        {
            double h = (x - x0) / N;
            double[] res = R_K4(fun, N, x0, y0, x);
            return ArrayOfy(res, h, x0, y);
        }
        //四阶龙格-库塔算法re
        public static double R_K4x(Func<double, double> fun, int N, double x0, double y0, double x, double y)
        {
            double h = (x - x0) / N;
            double[] res = R_K4(fun, N, x0, y0, x);
            return ArrayOfy(res, h, x0, y);
        }

        //找出数组res中与y最接近的Index
        private static double ArrayOfy(double[] res, double h, double x0, double y)
        {
            double min = Math.Abs(res[0] - y);
            int minIndex = 0;
            for (int i = 1; i < res.Length; i++)
            {
                if (Math.Abs(res[i] - y) <= min)
                {
                    min = Math.Abs(res[i] - y);
                    minIndex = i;
                }
            }
            return x0 + h * (minIndex + 1);
        }

        //=====================常微分方程与牛顿迭代公式================
        //欧拉迭代公式
        //数值迭代-牛顿迭代公式
        public static double Euler_eX(Func<double, double> fun, Func<double, double> fun_k,int N, double x0, double y0,double x, double y, int n=3)
        {
            return Equation.Newton(xpara =>
             {
                 double[] result = Euler_e(fun, N, x0, y0, xpara);
                 return result[result.Length - 1]-y;
             },fun_k, x, n);
        }
        //欧拉预估-矫正公式
        //数值迭代-牛顿迭代公式
        public static double Euler_preX(Func<double, double> fun, Func<double, double> fun_k,int N, double x0, double y0, double x, double y, int n=3)
        {
            return Equation.Newton(xpara =>
            {
                double[] result = Euler_pre(fun, N, x0, y0, xpara);
                return result[result.Length - 1] - y;
            }, fun_k, x, n);
        }

        //二阶龙格-库塔算法
        //数值迭代-牛顿迭代公式
        public static double R_K2X(Func<double, double> fun, Func<double, double> fun_k, int N, double x0, double y0, double x, double y, int n = 3)
        {
            return Equation.Newton(xpara =>
            {
                double[] result = R_K2(fun, N, x0, y0, xpara);
                return result[result.Length - 1] - y;
            }, fun_k, x, n);
        }
        //四阶龙格-库塔算法
        //数值迭代-牛顿迭代公式
        public static double R_K4X(Func<double, double> fun, Func<double, double> fun_k, int N, double x0, double y0, double x, double y, int n = 3)
        {
            return Equation.Newton(xpara =>
            {
                double[] result = R_K4(fun, N, x0, y0, xpara);
                return result[result.Length - 1] - y;
            }, fun_k, x, n);
        }
    }

}
