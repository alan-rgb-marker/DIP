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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DIP
{
    public partial class BitPlane: Form
    {
        public BitPlane()
        {
            InitializeComponent();
        }

        public BitPlane(Bitmap tmp, DIPSample dIP)
        {
            InitializeComponent();
            NpBitmap = tmp;
            dip_ = dIP;
            textBox1.Text = "b0";
            pictureBox1.Image = tmp;
        }


        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void bitplane(int* f, int w, int h, int* g, int n);

        Bitmap NpBitmap;
        Bitmap tmp;
        DIPSample dip_;

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            int[] f;
            int[] g;
            int w = NpBitmap.Width;
            int h = NpBitmap.Height;
            f = dip_.bmp2array(NpBitmap);
            g = new int[w * h];
            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g)
                {
                    bitplane(f0, w, h, g0, hScrollBar1.Value);
                }
            }
            tmp = DIPSample.array2bmp(g);
            textBox1.Text = "b" + hScrollBar1.Value.ToString();
            pictureBox1.Image = tmp;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
