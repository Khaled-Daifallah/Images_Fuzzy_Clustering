using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FClus
{
    class VECTOR
    {
        private double[] val;

        public VECTOR(double[] val)
        {
            this.val = val;
        }

        public int Length
        {
            get { return val.Length; }
        }

        public double this[int i]
        {
            get { return val[i]; }
        }

        public double[] Val
        {
            get { return val; }
        }

        public static VECTOR operator +(VECTOR v1, VECTOR v2)
        {
            // must be the same length.
            double[] temp = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
                temp[i] = v1[i] + v2[i];
            return new VECTOR(temp);
        }

        public static VECTOR operator -(VECTOR v1, VECTOR v2)
        {
            // must be the same length.
            double[] temp = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
                temp[i] = v1[i] - v2[i];
            return new VECTOR(temp);
        }

        public static VECTOR operator *(double d, VECTOR v2)
        {
            double[] temp = new double[v2.Length];
            for (int i = 0; i < v2.Length; i++)
                temp[i] = v2[i] * d;
            return new VECTOR(temp);
        }

       /* public static VECTOR operator /(VECTOR v1, VECTOR v2)
        {
            // must be the same length.
            double[] temp = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
                temp[i] = v1[i] / v2[i];
            return new VECTOR(temp);
        }*/

        public static VECTOR operator /(VECTOR v1, double d)
        {
            double[] temp = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
                temp[i] = v1[i] / d;
            return new VECTOR(temp);
        }

    }
}
