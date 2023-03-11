using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ImageProcessing
{
    static class ImageProcesses
    {
        public static int[] Histogram(Bitmap loaded)
        {
            if (loaded == null) { return null; }
            Bitmap loadedCopy = new Bitmap(loaded);
            Color pixel;
            byte average;
            int[] histogram = new int[256];

            for (int x = 0; x < loadedCopy.Width; x++)
            {
                for (int y = 0; y < loadedCopy.Height; y++)
                {
                    pixel = loadedCopy.GetPixel(x, y);
                    average = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                    loadedCopy.SetPixel(x, y, Color.FromArgb(average, average, average));

                    pixel = loadedCopy.GetPixel(x, y);
                    histogram[pixel.R]++;
                }
            }

            return histogram;
        } 
    }
}
