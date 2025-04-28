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
    public partial class ContrastANDBrightness: Form
    {
        Bitmap NpBitmap;
        Bitmap tmp;
        DIPSample dip_;
        public ContrastANDBrightness()
        {
            InitializeComponent();
        }
        public ContrastANDBrightness(Bitmap tmp, DIPSample dip_)
        {
            InitializeComponent();
            pictureBox1.Image = tmp;
            NpBitmap = tmp;
            this.dip_ = dip_;
            textBox1.Text = a.ToString();
            textBox2.Text = b.ToString();
        }

        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void contrast_AND_Bright(int* f, int w, int h, int* g, float a, int b);

        private float a = 0.1f;
        private int b = 0;

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            int[] f;
            int[] g;
            int w = NpBitmap.Width;
            int h = NpBitmap.Height;
            tmp = NpBitmap;
            f = dip_.bmp2array(tmp);
            g = new int[w * h];
            a = (float)(hScrollBar1.Value)/10.0f;
            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g)
                {
                    contrast_AND_Bright(f0, w, h, g0, a, b);
                }
            }
            tmp = DIPSample.array2bmp(g);
            textBox1.Text = a.ToString();
            pictureBox1.Image = tmp;      
              
        }

        private void hScrollBar2_ValueChanged(object sender, EventArgs e)
        {
            int[] f;
            int[] g;
            int w = NpBitmap.Width;
            int h = NpBitmap.Height;
            tmp = NpBitmap;
            f = dip_.bmp2array(tmp);
            g = new int[w * h];
            b = hScrollBar2.Value;
            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g)
                {
                    contrast_AND_Bright(f0, w, h, g0, a, b);
                }
            }
            tmp = DIPSample.array2bmp(g);
            textBox2.Text = b.ToString();
            pictureBox1.Image = tmp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dip_.set_Bitmap(tmp);
            this.Close();
        }
    }
}
