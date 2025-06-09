using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ScottPlot;
using static System.Net.WebRequestMethods;

namespace DIP
{
    public partial class DIPSample : Form
    {
        public DIPSample()
        {
            InitializeComponent();
        }

        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void negative(int* f0, int w, int h, int* g0);
        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void histogram_equalization(int* f, int w, int h, int* g);
        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void histograms(int* f, int w, int h, int* g);
        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void flip_horizontal(int* f, int w, int h, int* g);
        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void mosaic(int* f, int w, int h, int* g);
        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void Filter(int* f, int w, int h, int* g, double* kernel, double sigma);
        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int Otsu(int* f, int w, int h, int* g, int histSize);
        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int connected_component(int* f, int w, int h);
        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void salt_and_pepper(int* f, int w, int h, int* g);
        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int hough_line_transform(int* f, int w, int h, int* g);
        [DllImport("dip_proc.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int hough_circle_transform(int* f, int w, int h, int* g);

        Bitmap NpBitmap;
        int[] f;
        int[] g;
        int w, h;

        private void DIPSample_Load(object sender, EventArgs e)
        {
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            this.stStripLabel.Text = "";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            oFileDlg.CheckFileExists = true;
            oFileDlg.CheckPathExists = true;
            oFileDlg.Title = "Open File - DIP Sample";
            oFileDlg.ValidateNames = true;
            oFileDlg.Filter = "bmp files (*.bmp)|*.bmp";
            oFileDlg.FileName = "";

            if (oFileDlg.ShowDialog() == DialogResult.OK)
            {
                MSForm childForm = new MSForm();
                childForm.MdiParent = this;
                childForm.pf1 = stStripLabel;
                NpBitmap = bmp_read(oFileDlg);
                childForm.pBitmap = NpBitmap;
                w = NpBitmap.Width;
                h = NpBitmap.Height;
                childForm.Show();
            }
        }

        private Bitmap bmp_read(OpenFileDialog oFileDlg)
        {
            Bitmap pBitmap;
            string fileloc = oFileDlg.FileName;
            pBitmap = new Bitmap(fileloc);
            w = pBitmap.Width;
            h = pBitmap.Height;
            return pBitmap;
        }

        //灰階影像轉陣列
        public int[] bmp2array(Bitmap myBitmap)
        {
            int[] ImgData = new int[myBitmap.Width * myBitmap.Height];
            BitmapData byteArray = myBitmap.LockBits(new Rectangle(0, 0, myBitmap.Width, myBitmap.Height),
                                          ImageLockMode.ReadWrite,
                                          myBitmap.PixelFormat);
            int ByteOfSkip = byteArray.Stride - byteArray.Width * (int)(byteArray.Stride / myBitmap.Width);
            unsafe
            {
                byte* imgPtr = (byte*)(byteArray.Scan0);
                for (int y = 0; y < byteArray.Height; y++)
                {
                    for (int x = 0; x < byteArray.Width; x++)
                    {
                        ImgData[x + byteArray.Height * y] = (int)*(imgPtr);
                        //ImgData[x, y] = (int)*(imgPtr + 1);
                        //ImgData[x, y] = (int)*(imgPtr + 2);
                        imgPtr += (int)(byteArray.Stride / myBitmap.Width);
                    }
                    imgPtr += ByteOfSkip;
                }
            }
            myBitmap.UnlockBits(byteArray);
            return ImgData;
        }

        //黑白影像轉陣列
        public int[] onebit_bmp2array(Bitmap myBitmap)
        {
            
            int[] ImgData = new int[myBitmap.Width * myBitmap.Height];
            BitmapData byteArray = myBitmap.LockBits(new Rectangle(0, 0, myBitmap.Width, myBitmap.Height),
                                                     ImageLockMode.ReadOnly,  // 僅讀取影像資料
                                                     myBitmap.PixelFormat);

            if (myBitmap.PixelFormat != PixelFormat.Format1bppIndexed)
            {
                throw new NotSupportedException("僅支援 1 位元索引影像格式 (1bpp indexed)。");
            }

            int width = byteArray.Width;
            int height = byteArray.Height;
            int stride = byteArray.Stride;

            unsafe
            {
                byte* scan0 = (byte*)byteArray.Scan0;
                for (int y = 0; y < height; y++)
                {
                    byte* rowStart = scan0 + y * stride;
                    for (int x = 0; x < width; x++)
                    {
                        int byteIndex = x / 8;           // 計算像素所在的位元組索引
                        int bitIndex = 7 - (x % 8);      // 計算該像素在位元組中的位元位置
                        byte b = rowStart[byteIndex];    // 讀取該位元組
                        int pixelValue = (b >> bitIndex) & 1;  // 提取位元值（0 或 1）
                        ImgData[y * width + x] = pixelValue;   // 儲存到陣列中
                    }
                }
            }

            myBitmap.UnlockBits(byteArray);
            return ImgData;
        }


        public static Bitmap array2bmp(int[] ImgData)
        {
            int Width = (int)Math.Sqrt(ImgData.GetLength(0));
            int Height = (int)Math.Sqrt(ImgData.GetLength(0));
            Bitmap myBitmap = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            BitmapData byteArray = myBitmap.LockBits(new Rectangle(0, 0, Width, Height),
                                           ImageLockMode.WriteOnly,
                                           PixelFormat.Format24bppRgb);
            //Padding bytes的長度
            int ByteOfSkip = byteArray.Stride - myBitmap.Width * 3;
            unsafe
            {                                   // 指標取出影像資料
                byte* imgPtr = (byte*)byteArray.Scan0;
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        *imgPtr = (byte)ImgData[x + Height * y];       //B
                        *(imgPtr + 1) = (byte)ImgData[x + Height * y]; //G 
                        *(imgPtr + 2) = (byte)ImgData[x + Height * y]; //R  
                        imgPtr += 3;
                    }
                    imgPtr += ByteOfSkip; // 跳過Padding bytes
                }
            }
            myBitmap.UnlockBits(byteArray);
            return myBitmap;
        }

        internal static int[] array2bmp(Bitmap mypitureBox)
        {
            throw new NotImplementedException();
        }

        private void rGBtoGrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] f;
            foreach (MSForm cF in MdiChildren)
            {
                if (cF.Focused)
                {
                    f = bmp2array(cF.pBitmap);
                    NpBitmap = array2bmp(f);
                    break;
                }
            }
            MSForm childForm = new MSForm();
            childForm.MdiParent = this;
            childForm.pf1 = stStripLabel;
            childForm.pBitmap = NpBitmap;
            childForm.Show();
        }

        private void NegativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] f;
            int[] g;
            foreach (MSForm cF in MdiChildren)
            {
                if (cF.Focused)
                {
                    f = bmp2array(cF.pBitmap);
                    g = new int[w * h];
                    unsafe
                    {
                        fixed (int* f0 = f) fixed (int* g0 = g)
                        {
                            negative(f0, w, h, g0);
                        }
                    }
                    NpBitmap = array2bmp(g);
                    break;
                }
            }
            MSForm childForm = new MSForm();
            childForm.MdiParent = this;
            childForm.pf1 = stStripLabel;
            childForm.pBitmap = NpBitmap;
            childForm.Show();
        }

        private void ContrastANDBrightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {

            foreach (MSForm cF in MdiChildren)
            {
                if (cF.Focused)
                {
                    ContrastANDBrightness contrastANDBrightness = new ContrastANDBrightness(cF.pBitmap, this);
                    contrastANDBrightness.Show();
                }
            }
        }

        public void set_Bitmap(Bitmap b)
        {
            MSForm childForm = new MSForm();
            childForm.MdiParent = this;
            childForm.pf1 = stStripLabel;
            childForm.pBitmap = b;
            childForm.Show();
        }

        private void BitPlaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (MSForm cF in MdiChildren)
            {
                if (cF.Focused)
                {
                    BitPlane bitPlane = new BitPlane(cF.pBitmap, this);
                    bitPlane.Show();
                }
            }
        }

        private void HistogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] f;
            int[] g = new int[256];

            foreach (MSForm cF in MdiChildren)
            {
                if (cF.Focused)
                {
                    f = bmp2array(cF.pBitmap);
                    unsafe
                    {
                        fixed (int* f0 = f) fixed (int* g0 = g)
                        {
                            histograms(f0, w, h, g0);
                        }
                    }
                }
            }
            HistogramUI plt = new HistogramUI();
            plt.MdiParent = this;
            // 3. 準備X軸 (0,1,2,...,255)
            double[] bins = Enumerable.Range(0, 256).Select(x => (double)x).ToArray();
            // 4. 畫圖
            var barPlot = plt.formsPlot1.Plot.AddBar(g.Select(x => (double)x).ToArray(), bins);
            plt.formsPlot1.Plot.SetAxisLimits(yMin: 0); // Y軸從0開始
            plt.formsPlot1.Plot.XLabel("像素值");
            plt.formsPlot1.Plot.YLabel("數量");
            plt.formsPlot1.Refresh();
            plt.Show();

        }

        private void HistogramEqualizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] f;
            int[] g = null; // Initialize 'g' to avoid CS0165 error  
            foreach (MSForm cF in MdiChildren)
            {
                if (cF.Focused)
                {
                    f = bmp2array(cF.pBitmap);
                    g = new int[w * h];
                    unsafe
                    {
                        fixed (int* f0 = f) fixed (int* g0 = g)
                        {
                            histogram_equalization(f0, w, h, g0);
                        }
                    }
                    NpBitmap = array2bmp(g);
                    break;
                }
            }

            MSForm childForm = new MSForm();
            childForm.MdiParent = this;
            childForm.pf1 = stStripLabel;
            childForm.pBitmap = NpBitmap;
            childForm.Show();

            if (g != null) // Ensure 'g' is not null before proceeding  
            {
                HistogramUI plt = new HistogramUI();
                plt.MdiParent = this;
                //double[] g1 = new double[w*h]; // Adjusted size to match bins

                int[] counts = new int[256];
                unsafe
                {
                    fixed (int* f0 = g) fixed (int* g0 = counts)
                    {
                        histograms(f0, w, h, g0);
                    }


                    // 3. 準備X軸 (0,1,2,...,255)
                    double[] bins = Enumerable.Range(0, 256).Select(x => (double)x).ToArray();

                    // 4. 畫圖
                    var barPlot = plt.formsPlot1.Plot.AddBar(counts.Select(x => (double)x).ToArray(), bins);
                    plt.formsPlot1.Plot.SetAxisLimits(yMin: 0); // Y軸從0開始
                    plt.formsPlot1.Plot.XLabel("像素值");
                    plt.formsPlot1.Plot.YLabel("數量");
                    plt.formsPlot1.Refresh();
                    plt.Show();
                }
            }
        }

        private void FlipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            foreach (MSForm cF in MdiChildren)
            {
                if (cF.Focused)
                {
                    Flip flip = new Flip(cF.pBitmap, this);
                    flip.Show();
                }
            }
            
        }

        private void mosaicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] f;
            int[] g;
            foreach (MSForm cF in MdiChildren)
            {
                if (cF.Focused)
                {
                    f = bmp2array(cF.pBitmap);
                    g = new int[w * h];
                    unsafe
                    {
                        fixed (int* f0 = f) fixed (int* g0 = g)
                        {
                            mosaic(f0, w, h, g0);
                        }
                    }
                    NpBitmap = array2bmp(g);
                    break;
                }
            }
            MSForm childForm = new MSForm();
            childForm.MdiParent = this;
            childForm.pf1 = stStripLabel;
            childForm.pBitmap = NpBitmap;
            childForm.Show();
        }
        //========================================================================================
        public Bitmap origin;
        public Bitmap tmp;
        public Bitmap FilterPicture(double[] kernel,double sigma)
        {
            int[] f;
            int[] g;
            f = bmp2array(tmp);
            g = new int[w * h];
            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g) fixed (double* k0 = kernel)
                {
                    Filter(f0, w, h, g0, k0, sigma);
                }
            }
            tmp = array2bmp(g);
            return tmp;
        }
        private void FilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new Filter();
            filter.DS = this;
            foreach (MSForm cF in MdiChildren)
            {
                if (cF.Focused)
                {
                    filter.pictureBox1.Image = cF.pBitmap;
                    origin = cF.pBitmap;
                    tmp = cF.pBitmap;
                    break;
                }
            }
            if (filter.ShowDialog() == DialogResult.OK)
            {
                double[] kernel = filter.KernelValues;
                Bitmap processedImage = tmp;

                MSForm childForm = new MSForm();
                childForm.MdiParent = this;
                childForm.pf1 = stStripLabel;
                childForm.pBitmap = processedImage;
                childForm.Show();
            }    
        }

        private void RotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (MSForm cF in MdiChildren)
            {
                if (cF.Focused)
                {
                    Rotation rotation = new Rotation(cF.pBitmap, this);
                    rotation.Show();
                }
            }
        }
        int Threshold=-1;
        private void OtsuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            int[] f;
            int[] g;
            foreach (MSForm cF in MdiChildren)
            {
                if (cF.Focused)
                {
                    f = bmp2array(cF.pBitmap);
                    g = new int[w * h];
                    unsafe
                    {
                        fixed (int* f0 = f) fixed (int* g0 = g)
                        {
                            Threshold= Otsu(f0, w, h, g0, 256);
                        }
                        Console.WriteLine($"Otsu threshold: {Threshold}");
                    }
                    NpBitmap = array2bmp(g);
                    break;
                }
            }
            MSForm childForm = new MSForm();
            childForm.MdiParent = this;
            childForm.pf1 = stStripLabel;
            childForm.pBitmap = NpBitmap;
            childForm.Text = $"最佳閾值: {Threshold}";
            childForm.Show();
        }

        private void zoomToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            foreach (MSForm cF in MdiChildren)
            {
                if (cF.Focused)
                {
                    Zoom zoom = new Zoom(cF.pBitmap, this);
                    zoom.Show();
                }
            }
        }

        private void Salt_And_PepperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] f;
            int[] g;

            foreach (MSForm cF in MdiChildren)
            {
                if (cF.Focused)
                {
                    f = bmp2array(cF.pBitmap);
                    w = cF.pBitmap.Width;
                    h = cF.pBitmap.Height;
                    g = new int[w * h];
                    unsafe
                    {
                        fixed (int* f0 = f) fixed (int* g0 = g)
                        {
                            salt_and_pepper(f0, w, h, g0);
                        }
                    }
                    NpBitmap = array2bmp(g);
                    break;
                }
            }
            MSForm childForm = new MSForm();
            childForm.MdiParent = this;
            childForm.pf1 = stStripLabel;
            childForm.pBitmap = NpBitmap;
            childForm.Show();
        }

        private void HoughTransformLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] f;
            int[] g;
            int ct = 0;
            foreach (MSForm cF in MdiChildren)
            {
                if (cF.Focused)
                {
                    f = bmp2array(cF.pBitmap);
                    w = cF.pBitmap.Width;
                    h = cF.pBitmap.Height;
                    g = new int[w * h];
                    unsafe
                    {
                        fixed (int* f0 = f) fixed (int* g0 = g)
                        {
                            ct = hough_line_transform(f0, w, h, g0);
                        }
                    }
                    NpBitmap = array2bmp(g);
                    break;
                }
            }
            MSForm childForm = new MSForm();
            childForm.MdiParent = this;
            childForm.pf1 = stStripLabel;
            childForm.pBitmap = NpBitmap;
            childForm.Show();
            ConnectedComponent houghlinetransform = new ConnectedComponent(ct);
            houghlinetransform.Show();
        }

        private void HoughTransformCircleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] f;
            int[] g;
            int ct = 0;
            foreach (MSForm cF in MdiChildren)
            {
                if (cF.Focused)
                {
                    f = bmp2array(cF.pBitmap);
                    w = cF.pBitmap.Width;
                    h = cF.pBitmap.Height;
                    g = new int[w * h];
                    unsafe
                    {
                        fixed (int* f0 = f) fixed (int* g0 = g)
                        {
                            ct = hough_circle_transform(f0, w, h, g0);
                        }
                    }
                    NpBitmap = array2bmp(g);
                    break;
                }
            }
            MSForm childForm = new MSForm();
            childForm.MdiParent = this;
            childForm.pf1 = stStripLabel;
            childForm.pBitmap = NpBitmap;
            childForm.Show();
            ConnectedComponent houghlinetransform = new ConnectedComponent(ct);
            houghlinetransform.Show();
        }

        private void ConComToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] f;
            int sum = 0;
            foreach (MSForm cF in MdiChildren)
            {
                if (cF.Focused)
                {
                    f =onebit_bmp2array(cF.pBitmap);
                    w = cF.pBitmap.Width;
                    h = cF.pBitmap.Height;
                    unsafe
                    {
                        fixed (int* f0 = f)
                        {
                            sum = connected_component(f0, w, h);
                        }
                    }

                    break;
                }
            }
            ConnectedComponent conCom = new ConnectedComponent(sum);
            conCom.Show();
        }

    }
}
