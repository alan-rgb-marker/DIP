using System.Windows.Forms;

namespace DIP
{
    partial class DIPSample
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rGBtoGrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NegativeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mosaicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ContrastANDBrightnessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BitPlaneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HistogramEqualizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HistogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FlipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RotationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OtsuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.ConComToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stStripLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 325);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(657, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // stStripLabel
            // 
            this.stStripLabel.Name = "stStripLabel";
            this.stStripLabel.Size = new System.Drawing.Size(128, 17);
            this.stStripLabel.Text = "toolStripStatusLabel1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.iPToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(657, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // iPToolStripMenuItem
            // 
            this.iPToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rGBtoGrayToolStripMenuItem,
            this.NegativeToolStripMenuItem,
            this.mosaicToolStripMenuItem,
            this.ContrastANDBrightnessToolStripMenuItem,
            this.BitPlaneToolStripMenuItem,
            this.HistogramEqualizationToolStripMenuItem,
            this.HistogramToolStripMenuItem,
            this.FlipToolStripMenuItem,
            this.FilterToolStripMenuItem,
            this.RotationToolStripMenuItem,
            this.OtsuToolStripMenuItem,
            this.ConComToolStripMenuItem,
            this.zoomToolStripMenuItem});
            this.iPToolStripMenuItem.Name = "iPToolStripMenuItem";
            this.iPToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.iPToolStripMenuItem.Text = "樣式";
            // 
            // rGBtoGrayToolStripMenuItem
            // 
            this.rGBtoGrayToolStripMenuItem.Name = "rGBtoGrayToolStripMenuItem";
            this.rGBtoGrayToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.rGBtoGrayToolStripMenuItem.Text = "RGBtoGray";
            this.rGBtoGrayToolStripMenuItem.Click += new System.EventHandler(this.rGBtoGrayToolStripMenuItem_Click);
            // 
            // NegativeToolStripMenuItem
            // 
            this.NegativeToolStripMenuItem.Name = "NegativeToolStripMenuItem";
            this.NegativeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.NegativeToolStripMenuItem.Text = "負片";
            this.NegativeToolStripMenuItem.Click += new System.EventHandler(this.NegativeToolStripMenuItem_Click);
            // 
            // mosaicToolStripMenuItem
            // 
            this.mosaicToolStripMenuItem.Name = "mosaicToolStripMenuItem";
            this.mosaicToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.mosaicToolStripMenuItem.Text = "馬賽克";
            this.mosaicToolStripMenuItem.Click += new System.EventHandler(this.mosaicToolStripMenuItem_Click);
            // 
            // ContrastANDBrightnessToolStripMenuItem
            // 
            this.ContrastANDBrightnessToolStripMenuItem.Name = "ContrastANDBrightnessToolStripMenuItem";
            this.ContrastANDBrightnessToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ContrastANDBrightnessToolStripMenuItem.Text = "對比與亮度";
            this.ContrastANDBrightnessToolStripMenuItem.Click += new System.EventHandler(this.ContrastANDBrightnessToolStripMenuItem_Click);
            // 
            // BitPlaneToolStripMenuItem
            // 
            this.BitPlaneToolStripMenuItem.Name = "BitPlaneToolStripMenuItem";
            this.BitPlaneToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.BitPlaneToolStripMenuItem.Text = "位元切面";
            this.BitPlaneToolStripMenuItem.Click += new System.EventHandler(this.BitPlaneToolStripMenuItem_Click);
            // 
            // HistogramEqualizationToolStripMenuItem
            // 
            this.HistogramEqualizationToolStripMenuItem.Name = "HistogramEqualizationToolStripMenuItem";
            this.HistogramEqualizationToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.HistogramEqualizationToolStripMenuItem.Text = "直方圖等化";
            this.HistogramEqualizationToolStripMenuItem.Click += new System.EventHandler(this.HistogramEqualizationToolStripMenuItem_Click);
            // 
            // HistogramToolStripMenuItem
            // 
            this.HistogramToolStripMenuItem.Name = "HistogramToolStripMenuItem";
            this.HistogramToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.HistogramToolStripMenuItem.Text = "直方圖";
            this.HistogramToolStripMenuItem.Click += new System.EventHandler(this.HistogramToolStripMenuItem_Click);
            // 
            // FlipToolStripMenuItem
            // 
            this.FlipToolStripMenuItem.Name = "FlipToolStripMenuItem";
            this.FlipToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.FlipToolStripMenuItem.Text = "翻轉";
            this.FlipToolStripMenuItem.Click += new System.EventHandler(this.FlipToolStripMenuItem_Click);
            // 
            // FilterToolStripMenuItem
            // 
            this.FilterToolStripMenuItem.Name = "FilterToolStripMenuItem";
            this.FilterToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.FilterToolStripMenuItem.Text = "濾波器";
            this.FilterToolStripMenuItem.Click += new System.EventHandler(this.FilterToolStripMenuItem_Click);
            // 
            // RotationToolStripMenuItem
            // 
            this.RotationToolStripMenuItem.Name = "RotationToolStripMenuItem";
            this.RotationToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.RotationToolStripMenuItem.Text = "旋轉";
            this.RotationToolStripMenuItem.Click += new System.EventHandler(this.RotationToolStripMenuItem_Click);
            // 
            // OtsuToolStripMenuItem
            // 
            this.OtsuToolStripMenuItem.Name = "OtsuToolStripMenuItem";
            this.OtsuToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.OtsuToolStripMenuItem.Text = "二值化";
            this.OtsuToolStripMenuItem.Click += new System.EventHandler(this.OtsuToolStripMenuItem_Click);
            // 
            // oFileDlg
            // 
            this.oFileDlg.FileName = "openFileDialog1";
            // 
            // ConComToolStripMenuItem
            // 
            this.ConComToolStripMenuItem.Name = "ConComToolStripMenuItem";
            this.ConComToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ConComToolStripMenuItem.Text = "組別標籤";
            this.ConComToolStripMenuItem.Click += new System.EventHandler(this.ConComToolStripMenuItem_Click);
            // 
            // zoomToolStripMenuItem
            // 
            this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            this.zoomToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zoomToolStripMenuItem.Text = "縮放";
            this.zoomToolStripMenuItem.Click += new System.EventHandler(this.zoomToolStripMenuItem_Click_1);
            // 
            // DIPSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 347);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DIPSample";
            this.Text = "DIPSample";
            this.Load += new System.EventHandler(this.DIPSample_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel stStripLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iPToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog oFileDlg;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;

        public ToolStripMenuItem ConnectedComponentToolStripMenuItem { get; private set; }

        private System.Windows.Forms.ToolStripMenuItem rGBtoGrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NegativeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ContrastANDBrightnessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BitPlaneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HistogramEqualizationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HistogramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FlipToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mosaicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OtsuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RotationToolStripMenuItem;
        private ToolStripMenuItem ConComToolStripMenuItem;
        private ToolStripMenuItem zoomToolStripMenuItem;
    }
}