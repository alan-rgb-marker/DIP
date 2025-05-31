using ScottPlot.Drawing.Colormaps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DIP
{
    public partial class Zoom: Form
    {
        public Zoom(Bitmap pBitmap, DIPSample dIPSample)
        {
            InitializeComponent();
            npBitmap = pBitmap;
            pictureBox1.Image = npBitmap;
        }
        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void scale_up_bilinear(int* f, int w, int h, int* g, int new_w, int new_h, double scale_factor);
        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void scale_down_bilinear(int* f, int w, int h, int* g, int new_w, int new_h, double shrink_factor);

        Bitmap npBitmap;
        DIPSample sample;
        double factor = 1.0;
        private Bitmap array2bmp(int[] pixelArray, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int pixelValue = pixelArray[y * width + x];
                    Color color = Color.FromArgb(pixelValue, pixelValue, pixelValue);
                    bmp.SetPixel(x, y, color);
                }
            }
            return bmp;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            factor = Convert.ToDouble(textBox2.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int w = npBitmap.Width;  // 取得原始影像寬度
            int h = npBitmap.Height; // 取得原始影像高度

            // 初始化像素陣列 f 和 g
            int[] f = new int[w * h];  // 原始影像的像素陣列
            double scaleFactor = double.Parse(textBox1.Text);  // 從 TextBox 控制項獲取放大倍數


            // 將影像轉換為灰階像素陣列 f
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color pixelColor = npBitmap.GetPixel(x, y);
                    // 灰階公式：Y = 0.3 * R + 0.59 * G + 0.11 * B
                    f[y * w + x] = (int)(0.3 * pixelColor.R + 0.59 * pixelColor.G + 0.11 * pixelColor.B);
                }
            }

            // 計算新的影像尺寸
            int newWidth = (int)(w * scaleFactor);
            int newHeight = (int)(h * scaleFactor);
            int[] g = new int[newWidth * newHeight];  // 儲存放大後的影像像素

            // 呼叫放大函數進行處理
            
            unsafe
            {
                fixed (int* f0 = f)  
                fixed (int* g0 = g)
                {
                    // 放大影像
                    scale_up_bilinear(f0, w, h, g0, newWidth, newHeight, scaleFactor);
                }
            }

            // 將處理後的像素陣列轉換為 Bitmap
            Bitmap tmp = array2bmp(g, newWidth, newHeight);

            // 顯示處理後的影像
            pictureBox1.Image = tmp;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int w = npBitmap.Width;  // 取得原始影像寬度
            int h = npBitmap.Height; // 取得原始影像高度

            // 初始化像素陣列 f 和 g
            int[] f = new int[w * h];  // 原始影像的像素陣列
            double shinkFactor =1/ double.Parse(textBox2.Text);  // 從 TextBox 控制項獲取放大倍數

            // 將影像轉換為灰階像素陣列 f
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color pixelColor = npBitmap.GetPixel(x, y);
                    // 灰階公式：Y = 0.3 * R + 0.59 * G + 0.11 * B
                    f[y * w + x] = (int)(0.3 * pixelColor.R + 0.59 * pixelColor.G + 0.11 * pixelColor.B);
                }
            }

            // 計算新的影像尺寸
            int newWidth = (int)(w * shinkFactor);
            int newHeight = (int)(h * shinkFactor);
            int[] g = new int[newWidth * newHeight];  


            unsafe
            {
                fixed (int* f0 = f)
                fixed (int* g0 = g)
                {
                    scale_down_bilinear(f0, w, h, g0, newWidth, newHeight, shinkFactor);
                }
            }

            // 將處理後的像素陣列轉換為 Bitmap
            Bitmap tmp = array2bmp(g, newWidth, newHeight);

            // 顯示處理後的影像
            pictureBox1.Image = tmp;
        }
    }
}
