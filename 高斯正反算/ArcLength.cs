using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathMethod;

namespace Geodesy
{
    public class ArcLength
    {
        public double a;
        public double e2;
        public ArcLength()
        {

        }
        public ArcLength(double a, double e2)
        {
            this.a = a;
            this.e2 = e2;
        }


        #region 主曲率半径

        public double M(double rad)     //子午圈曲率半径
        {
            double M = 0;
            M = a * (1 - e2) / Math.Pow((1 - e2 * Math.Sin(rad) * Math.Sin(rad)), 3.0 / 2);
            return M;
        }

        public double N(double rad)
        {
            double N = 0;
            N = a / Math.Sqrt(1 - e2 * Math.Sin(rad) * Math.Sin(rad));
            return N;
        }     //卯酉圈曲率半径

        #endregion

        #region 子午圈弧长正算

        //经典算法
        public double X_standard(double rad)  
        {
            double X = 0;

            double m0 = a * (1 - e2);
            double m2 = 3.0 / 2 * e2 * m0;
            double m4 = 5.0 / 4 * e2 * m2;
            double m6 = 7.0 / 6 * e2 * m4;
            double m8 = 9.0 / 8 * e2 * m6;

            //将M展开为余弦的倍数函数后的各项系数
            double a0 = m0 + m2 / 2 + 3.0 / 8 * m4 + 5.0 / 16 * m6 + 35.0 / 128 * m8;
            double a2 = m2 / 2 + m4 / 2 + 15.0 / 32 * m6 + 7.0 / 16 * m8;
            double a4 = m4 / 8 + 3.0 / 16 * m6 + 7.0 / 32 * m8;
            double a6 = m6 / 32 + m8 / 16;
            double a8 = m8 / 128;

            X = a0 * rad - a2 / 2 * Math.Sin(2 * rad) + a4 / 4 * Math.Sin(4 * rad)
                - a6 / 6 * Math.Sin(6 * rad) + a8 / 8 * Math.Sin(8 * rad);
            return X;
        }

        //复化梯形解算
        public double X_ft(int N, double rad)   
        {
            double X = 0;

            X = Integral.FuHuaTiXing(M, N, rad, 0);
            return X;
        }

        //复化辛普森解算
        public double X_fs(int N, double rad)
        {
            double X = 0;
            X = Integral.FSimpson(M, N, rad, 0);

            return X;
        } 

        //龙贝格算法
        public double X_r(double e, double rad) 
        {
            double X = 0;

            X = Integral.Romberg(M, e, rad, 0);
            return X;
        }

        //科特斯算法
        public double X_fc(int N,double rad)
        {
            double x = 0;
            x = Integral.FCotes(M, N, rad, 0);
            return x;
        }

        //欧拉递推公式
        public double X_Ee(int N,double rad)
        {
            double X = 0;
            X = DifEquation.Euler_e(M, N, 0, 0, rad)[N-1];
            return X;
        }

        //欧拉预估-矫正公式
        public double X_Epre(int N,double rad)
        {
            double X = DifEquation.Euler_pre(M, N, 0, 0, rad)[N - 1];
            return X;
        }

        //二阶龙格-库塔算法
        public double X_RK2(int N,double rad)
        {
            double X = DifEquation.R_K2(M, N, 0, 0, rad)[N - 1];
            return X;
        }

        //四阶龙格-库塔算法
        public double X_RK4(int N,double rad)
        {
            double X = DifEquation.R_K4(M, N, 0, 0, rad)[N - 1];
            return X;
        }
        #endregion


        #region 子午圈弧长反算

        //二分法
        public double B_half(double X, double e)
        {
            double B = Equation.Half(
                (x) =>
                {
                    return X_standard(x) - X;
                }, this.B_standard(X, X) * 0.5, this.B_standard(X, X) * 1.5, e);
            return B;
        }
        public double B_standard(double X, double e)  //经典反算算法
        {
            double m0 = a * (1 - e2);
            double m2 = 3.0 / 2 * e2 * m0;
            double m4 = 5.0 / 4 * e2 * m2;
            double m6 = 7 / 6 * e2 * m4;
            double m8 = 9 / 8 * e2 * m6;

            //将M展开为余弦的倍数函数后的各项系数
            double a0 = m0 + m2 / 2 + 3.0 / 8 * m4 + 5.0 / 16 * m6 + 35.0 / 128 * m8;
            double a2 = m2 / 2 + m4 / 2 + 15.0 / 32 * m6 + 7.0 / 16 * m8;
            double a4 = m4 / 8 + 3.0 / 16 * m6 + 7.0 / 32 * m8;
            double a6 = m6 / 32 + m8 / 16;
            double a8 = m8 / 128;

            double Bf = 0;
            double B = X / a0;
            while (Math.Abs(Bf - B) >= e)
            {
                Bf = B;
                B = (X + a2 / 2 * Math.Sin(2 * Bf) - a4 / 4 * Math.Sin(4 * Bf) + a6 / 6 * Math.Sin(6 * Bf) - a8 / 8 * Math.Sin(8 * Bf)) / a0;
            }
            return B;
        }

        //单点弦截法迭代法
        //this.B_standard(X, X) 不参与迭代，为了得到X/a0，即方程的初解
        public double B_single(double X, int n) 
        {
            double B = Equation.Single((x) =>
            {
                return X_standard(x) - X;
            }, this.B_standard(X, X), Math.PI / 2, n);

            return B;
        }

        public double B_sec(double X, int n) //割线迭代法
        {
            double B = Equation.Sec((x) =>
            {
                return X_standard(x) - X;

            },0 , Math.PI / 2, n);
            return B;
        }

        public double B_newton(double X,int n) //牛顿迭代法 
        {
            double m0 = a * (1 - e2);
            double m2 = 3.0 / 2 * e2 * m0;
            double m4 = 5.0 / 4 * e2 * m2;
            double m6 = 7 / 6 * e2 * m4;
            double m8 = 9 / 8 * e2 * m6;

            //将M展开为余弦的倍数函数后的各项系数
            double a0 = m0 + m2 / 2 + 3.0 / 8 * m4 + 5.0 / 16 * m6 + 35.0 / 128 * m8;
            double a2 = m2 / 2 + m4 / 2 + 15.0 / 32 * m6 + 7.0 / 16 * m8;
            double a4 = m4 / 8 + 3.0 / 16 * m6 + 7.0 / 32 * m8;
            double a6 = m6 / 32 + m8 / 16;
            double a8 = m8 / 128;

            double B = Equation.Newton((x) =>
            {
                return X_standard(x) - X;
            }, (x) =>
            {
                double k = a0 - a2 * Math.Cos(2 * x) + a4 * Math.Cos(4 * x) - a6 * Math.Cos(6 * x) + a8 * Math.Cos(8 * x);
                return k;
            },this.B_standard(X,X), n);

            return B;
        }
        //欧拉迭代算法
        public double B_Euler_ex(double X,int N)
        {
            double Brad = DifEquation.Euler_ex(M, N, 0, 0, Math.PI / 2, X);
            return Brad;
        }

        //欧拉预估-校正算法
        public double B_Euler_prex(double X, int N)
        {
            double Brad = DifEquation.Euler_prex(M, N, 0, 0, Math.PI / 2, X);
            return Brad;
        }

        //2阶R_K算法
        public double B_RK2x(double X, int N)
        {
            double Brad = DifEquation.R_K2x(M, N, 0, 0, Math.PI / 2, X);
            return Brad;
        }

        //4阶R_K算法
        public double B_RK4x(double X, int N)
        {
            double Brad = DifEquation.R_K4x(M, N, 0, 0, Math.PI / 2, X);
            return Brad;
        }

        //欧拉迭代算法
        //牛顿迭代公式
        public double B_Euler_EX(double X,int N)
        {
            double Brad = DifEquation.Euler_eX(M, M, 1000, 0, 0, Math.PI / 2, X,N);
            return Brad;
        }

        //欧拉预估-校正算法
        //牛顿迭代公式
        public double B_Euler_preX(double X, int N)
        {
            double Brad = DifEquation.Euler_preX(M, M,1000, 0, 0, Math.PI / 2, X,N);
            return Brad;
        }

        //2阶R_K算法
        public double B_RK2X(double X, int N)
        {
            double Brad = DifEquation.R_K2X(M, M, 100, 0, 0, Math.PI / 2, X, N);
            return Brad;
        }

        //4阶R_K算法
        public double B_RK4X(double X, int N)
        {
            double Brad = DifEquation.R_K4X(M, M, 10, 0, 0, Math.PI / 2, X, N);
            return Brad;
        }
        #endregion

    }

}
