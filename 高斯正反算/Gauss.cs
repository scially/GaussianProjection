using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathMethod;

namespace Geodesy
{
    public class Gauss : ArcLength
    {
        public Gauss(double a, double e2)
            : base(a, e2)
        {

        }
        public Gauss()
        {

        }

        //获取带号,并得到真正y值
        //pkm取值为500或者为0
        public static int n(ref double y, double pkm)
        {
            int n = (int)(y / 1e6);
            y -= pkm * 1000;
            y -= n * 1e6;
            return n;
        }

        //根据带号获取中央子午线经度
        public static double l0rad(int n)
        {
            if (n >= 13 && n <= 23)
            {
                return (6 * n - 3) * 3600 / DmsRad.p;
            }
            else
            {
                return (3 * n) * 3600 / DmsRad.p;
            }
        }
        #region 高斯正算
        //计算x坐标
        //采用龙贝格算法，精度为1e-5
        public double Get_x(double Brad, double lrad)
        {
            double t = Math.Tan(Brad);
            double et2 = e2 / (1 - e2);
            double n2 = et2 * Math.Pow(Math.Cos(Brad), 2);
            double x = X_r(1e-5, Brad) + N(Brad) / 2 * t * Math.Pow(Math.Cos(Brad) * lrad, 2)
                + N(Brad) / 24 * t * (5 - t * t + 9 * n2 + 4 * n2 * n2) * Math.Pow(Math.Cos(Brad) * lrad, 4)
                + N(Brad) / 720 * t * (61 - 58 * t * t + Math.Pow(t, 4)) * Math.Pow(Math.Cos(Brad) * lrad, 6);
            return x;
        }

        //计算y 坐标
        //不加带号，不加500km
        public double Get_y(double Brad, double lrad)
        {
            double t = Math.Tan(Brad);
            double et2 = e2 / (1 - e2);
            double n2 = et2 * Math.Pow(Math.Cos(Brad), 2);
            double y = N(Brad) * Math.Cos(Brad) * lrad
                    + N(Brad) / 6 * (1 - t * t + n2) * Math.Pow(Math.Cos(Brad) * lrad, 3)
                    + N(Brad) / 120 * (5 - 18 * t * t + Math.Pow(t, 4) + 14 * n2 - 58 * n2 * t * t) * Math.Pow(Math.Cos(Brad) * lrad, 5);
            return y;
        }

        //计算y 坐标
        //加带号，加p(km) p取值为500或0
        public double Get_y(double Brad, double lrad, double n, double pkm)
        {
            double y = Get_y(Brad, lrad);
            y += pkm * 1000 + n * 1e6;
            return y;
        }
        #endregion

        #region 高斯反算

        //y不加带号，不加500km
        //得到结果为弧度制
        public double Get_Brad(double x, double y)
        {
            double Bf = B_newton(x, 3);
            double tf = Math.Tan(Bf);
            double et2 = e2 / (1 - e2);
            double nf2 = et2 * Math.Pow(Math.Cos(Bf), 2);
            double B = Bf - y * y * tf / 2 / M(Bf) / N(Bf)
                + Math.Pow(y, 4) * tf / 24 / M(Bf) / Math.Pow(N(Bf), 3) * (5 + 3 * tf * tf + nf2 - 9 * nf2 * tf * tf)
                - Math.Pow(y, 6) * tf / 720 / M(Bf) / Math.Pow(N(Bf), 5) * (61 + 90 * tf * tf + 45 * Math.Pow(tf, 4));
            return B;
        }

        //y不加带号，不加500km
        //得到结果为弧度制
        public double Get_lrad(double x, double y)
        {
            double Bf = B_newton(x, 3);
            double tf = Math.Tan(Bf);
            double et2 = e2 / (1 - e2);
            double nf2 = et2 * Math.Pow(Math.Cos(Bf), 2);

            double lrad = y / N(Bf) / Math.Cos(Bf)
                       - Math.Pow(y, 3) / 6 / Math.Cos(Bf) / Math.Pow(N(Bf), 3) * (1 + 2 * tf * tf + nf2)
                       + Math.Pow(y, 5) / 120 / Math.Pow(N(Bf), 5) / Math.Cos(Bf) * (5 + 28 * tf * tf + 24 * Math.Pow(tf, 4) + 6 * nf2 + 8 * nf2 * tf * tf);
            return lrad;
        }


        //y加带号，不加p(km)
        //得到纬度为弧度制
        public double Get_Brad(double x, double y, double p)
        {
            Gauss.n(ref y, p);
            return Get_Brad(x, y);
        }

        //y加带号，不加p(km)
        //得到经度为弧度制
        public double Get_lrad(double x, double y, double p)
        {
            int N=Gauss.n(ref y, p);
            double lrad= Get_lrad(x, y);
            lrad += l0rad(N);
            return lrad;
        }

        #endregion
    }
}
