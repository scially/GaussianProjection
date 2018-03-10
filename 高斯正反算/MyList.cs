using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 高斯正反算
{
    //类似与泛型Dictionary类似，但是键值可以重复
    public class MyList<T>
    {
        private List<T> listX = new List<T>();
        private List<T> listY = new List<T>();
        public int Count
        {
            get
            {
                return listX.Count;
            }
        }

        public List<T> ListX
        {
            get { return listX; }
        }
        public List<T> ListY
        {
            get { return listY; }
        }
        public void Add(T x, T y)
        {
            listX.Add(x);
            listY.Add(y);
        }

        public void ToArray(out T[] xs, out T[] ys)
        {
            xs = listX.ToArray();
            ys = listY.ToArray();
        }

        //public T Get1T(int index)
        //{
        //    if (index < listX.Count)
        //    {
        //        return listX[index];
        //    }
        //    return default(T);
        //}
    }
}
