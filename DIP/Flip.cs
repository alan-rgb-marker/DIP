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
    public partial class Flip : Form
    {
        Bitmap npbitmap, tmp;
        DIPSample dIP;
        int w, h;
        public Flip()
        {
            InitializeComponent();
        }
        public Flip(Bitmap tmp, DIPSample dIP)
        {
            InitializeComponent();
            npbitmap = tmp;
            this.tmp = tmp;
            pictureBox1.Image = tmp;
            this.dIP = dIP;
            w = tmp.Width;
            h = tmp.Height;
        }

        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void flip_horizontal(int* f, int w, int h, int* g);
        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void flip_transpose_right(int* f, int w, int h, int* g);
        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void flip_transpose_left(int* f, int w, int h, int* g);

        private void rotate_clockwise_90_Click(object sender, EventArgs e)
        {
            int[] f;
            int[] g;
            f = dIP.bmp2array(tmp);
            g = new int[w * h];
            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g)
                {
                    flip_transpose_right(f0, w, h, g0);
                }
            }
            tmp = DIPSample.array2bmp(g);
            pictureBox1.Image = tmp;
        }

        private void rotate_counter_clockwise_90_Click(object sender, EventArgs e)
        {
            int[] f;
            int[] g;
            f = dIP.bmp2array(tmp);
            g = new int[w * h];
            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g)
                {
                    flip_transpose_left(f0, w, h, g0);
                }
            }
            tmp = DIPSample.array2bmp(g);
            pictureBox1.Image = tmp;
        }
        
        private void horizontal_Click(object sender, EventArgs e)
        {
            int[] f;
            int[] g;
            f = dIP.bmp2array(tmp);
            g = new int[w * h];
            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g)
                {
                    flip_horizontal(f0, w, h, g0);
                }
            }
            tmp = DIPSample.array2bmp(g);
            pictureBox1.Image = tmp;
        }

        
    }
}
