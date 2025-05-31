using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace DIP
{
    public partial class Rotation : Form
    {
        Bitmap npBitmap;
        DIPSample sample;
        public Rotation()
        {
            InitializeComponent();
        }

        public Rotation(Bitmap tmp, DIPSample sample)
        {
            InitializeComponent();
            npBitmap = tmp;
            pictureBox1.Image = npBitmap;
            this.sample = sample;
            textBox1.Text = hScrollBar1.Value.ToString();
        }

        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void rotations(int* f, int w, int h, int nw, int nh, int* g, int theta);
        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void salt_and_pepper(int* f, int w, int h, int* g);

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = hScrollBar1.Value.ToString();   
            int[] f;
            int[] g;
            int[] g1;
            int w = npBitmap.Width;
            int h = npBitmap.Height;
            int theta = hScrollBar1.Value;
            double theta_rad = theta * Math.PI / 180;
            //int nw = (int)Math.Round(w * Math.Cos(theta) + h * Math.Sin(theta));
            //int nh = (int)Math.Round(w * Math.Cos(theta) + h * Math.Sin(theta));
            int nw = ((int)Math.Round((w * Math.Abs(Math.Cos(theta_rad)) + h * Math.Abs(Math.Sin(theta_rad)))));
            int nh = ((int)Math.Round((w * Math.Abs(Math.Cos(theta_rad)) + h * Math.Abs(Math.Sin(theta_rad)))));
            f = sample.bmp2array(npBitmap);
            g = new int[(nw) * (nh)];
            g1 = new int[(nw) * (nh)];
            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g) fixed (int* g1_0 = g1)
                {
                    rotations(f0, w, h, nw, nh, g0, theta);
                    salt_and_pepper(g0, nw, nh, g1_0);
                }
            }
            pictureBox1.Image = DIPSample.array2bmp(g1);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            sample.set_Bitmap(pictureBox1.Image as Bitmap);
            this.Close();
        }

    }
}
