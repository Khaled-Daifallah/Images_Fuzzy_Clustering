using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FClus
{
    class MATRIX
    {
        private double[][] data;

        public MATRIX(double[][] data)
        {
            this.data = data;
        }

        public MATRIX(int rows, int cols)
        {
            data = new double[rows][];
            for (int h = 0; h < rows; h++)
                data[h] = new double[cols];
        }

        public double this[int i,int j]
        {
            get { return data[i][j]; }
            set { data[i][j] = value; }
        }

        public int Rows
        {
            get { return data.Length; }
        }

        public int Cols
        {
            get { return data[0].Length; }
        }

        public static MATRIX operator +(MATRIX m1, MATRIX m2)
        {
            MATRIX temp = new MATRIX(m1.Rows, m1.Cols);
            for (int k = 0; k < m1.Rows; k++)
                for (int h = 0; h < m1.Cols; h++)
                    temp[k, h] = m1[k, h] + m2[k, h];
            return temp;
        }

        public static MATRIX operator *(double d, MATRIX m1)
        {
            MATRIX temp = new MATRIX(m1.Rows, m1.Cols);
            for (int k = 0; k < m1.Rows; k++)
                for (int h = 0; h < m1.Cols; h++)
                    temp[k, h] = m1[k, h] * d;
            return temp;
        }

        public double[][] Data
        {
            get { return data; }
        }

        public static MATRIX operator /(MATRIX m1,double d)
        {
            MATRIX temp = new MATRIX(m1.Rows, m1.Cols);
            for (int k = 0; k < m1.Rows; k++)
                for (int h = 0; h < m1.Cols; h++)
                    temp[k, h] = m1[k, h] / d;
            return temp;
        }
    }
}
