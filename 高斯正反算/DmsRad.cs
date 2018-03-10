using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geodesy
{
    public class DmsRad
    {
        public const double p = 180 / Math.PI * 3600;
        //将度分秒化为弧度值
        public static double dms2rad(string a)
        {
            double dms = Convert.ToDouble(a);

            int d = (int)((dms + 0.00001) / 10000.0);
            dms -= d * 10000.0;

            int m = (int)((dms + 0.00001) / 100.0);
            dms -= m * 100.0;

            dms = (d * 3600.0 + m * 60.0 + dms) / p;
            return dms;
        }
        //将角度的弧度制化为度分秒连写的角度
        public static string rad2dms(double a)
        {
            double rad = a* p;

            int d = (int)(rad / 3600.0 + 0.00001);
            rad -= d*3600.0;

            int m = (int)(rad / 60.0 + 0.00001);
            rad -= m * 60.0;

            rad = d * 10000.0 + m * 100.0 + rad;
            return rad.ToString();
        }
    }
}
