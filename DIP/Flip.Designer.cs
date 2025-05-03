namespace DIP
{
    partial class Flip
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.rotate_clockwise_90 = new System.Windows.Forms.Button();
            this.rotate_counter_clockwise_90 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Location = new System.Drawing.Point(99, 46);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(256, 256);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.Location = new System.Drawing.Point(134, 416);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(177, 39);
            this.button1.TabIndex = 2;
            this.button1.Text = "翻轉";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.horizontal_Click);
            // 
            // rotate_clockwise_90
            // 
            this.rotate_clockwise_90.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rotate_clockwise_90.Location = new System.Drawing.Point(252, 357);
            this.rotate_clockwise_90.Name = "rotate_clockwise_90";
            this.rotate_clockwise_90.Size = new System.Drawing.Size(151, 40);
            this.rotate_clockwise_90.TabIndex = 3;
            this.rotate_clockwise_90.Text = "順時針->";
            this.rotate_clockwise_90.UseVisualStyleBackColor = true;
            this.rotate_clockwise_90.Click += new System.EventHandler(this.rotate_clockwise_90_Click);
            // 
            // rotate_counter_clockwise_90
            // 
            this.rotate_counter_clockwise_90.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rotate_counter_clockwise_90.Location = new System.Drawing.Point(34, 357);
            this.rotate_counter_clockwise_90.Name = "rotate_counter_clockwise_90";
            this.rotate_counter_clockwise_90.Size = new System.Drawing.Size(151, 40);
            this.rotate_counter_clockwise_90.TabIndex = 4;
            this.rotate_counter_clockwise_90.Text = "<-逆時針";
            this.rotate_counter_clockwise_90.UseVisualStyleBackColor = true;
            this.rotate_counter_clockwise_90.Click += new System.EventHandler(this.rotate_counter_clockwise_90_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button2.Location = new System.Drawing.Point(134, 489);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(177, 39);
            this.button2.TabIndex = 5;
            this.button2.Text = "確定";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Flip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 572);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.rotate_counter_clockwise_90);
            this.Controls.Add(this.rotate_clockwise_90);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Flip";
            this.Text = "Flip";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button rotate_clockwise_90;
        private System.Windows.Forms.Button rotate_counter_clockwise_90;
        private System.Windows.Forms.Button button2;
    }
}