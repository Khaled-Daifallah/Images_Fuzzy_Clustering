using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FClus
{
    class ReadImage
    {
        private string path;
        private double[][] data = null;
        private UnsafeBitmap map;
        
        public ReadImage(string pa)
        {
            path = pa;
        }

        public UnsafeBitmap IMAGE
        {
            get { return map; }
        }

        private void loadImage()
        {
            map = new UnsafeBitmap(new System.Drawing.Bitmap(@path));
            map.LockBitmap();
            PixelData pi;
            data = new double[map.Width * map.Height][];
            for(int y=0;y<map.Height;y++)
                for (int x = 0; x < map.Width; x++)
                {
                    pi = map.GetPixel(x, y);
                    data[y * map.Width + x] = new double[5];
                    data[y * map.Width + x][0] = pi.red;
                    data[y * map.Width + x][1] = pi.green;
                    data[y * map.Width + x][2] = pi.blue;
                    data[y * map.Width + x][3] = x;
                    data[y * map.Width + x][4] = y;
                }
        }

        public double[][] getData
        {
            get
            {
                if (data == null)
                    loadImage();
                return data;
            }
        }


    }
}
