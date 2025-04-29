using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DIP
{
    public partial class Filter: Form
    {

        public Filter()
        {
            InitializeComponent();
            textBoxes = new TextBox[]
            {
            textBox1, textBox2, textBox3, textBox4, textBox5,
            textBox6, textBox7, textBox8, textBox9, textBox10,
            textBox11, textBox12, textBox13, textBox14, textBox15,
            textBox16, textBox17, textBox18, textBox19, textBox20,
            textBox21, textBox22, textBox23, textBox24, textBox25,
            };
        }
        public DIPSample DS { get; set; }
        public double[] KernelValues { get; private set; } = new double[25];
        private TextBox[] textBoxes;
        public double Sigma { get;private set; }
        private void Apply_Button(object sender, EventArgs e)
        {
            for (int i = 0; i < 25; i++)
            {
                string input = textBoxes[i].Text.Trim();
                if (input.Contains("/") && input.Split('/').Length == 2)
                {
                    var parts = input.Split('/');
                    if (double.TryParse(parts[0], out double num) && double.TryParse(parts[1], out double den) && den != 0)
                        KernelValues[i] = num / den;
                    else
                        KernelValues[i] = 0;
                }
                else if (double.TryParse(input, out double value))
                {
                    KernelValues[i] = value;
                }
                else
                {
                    textBoxes[i].Text = "0";
                    KernelValues[i] = 0;
                }
            }

            pictureBox1.Image = DS.FilterPicture(KernelValues,0.0);
            this.DialogResult = DialogResult.None;
        }

        private void Confirm_Botton(object sender, EventArgs e)
        {
            if (KernelValues != null) 
            {
                this.DialogResult = DialogResult.OK;
            }
        }
        private void Reset_Botton(object sender, EventArgs e)
        {
            DS.tmp = DS.origin;
            pictureBox1.Image = DS.tmp;
            this.DialogResult = DialogResult.None;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Mean_3X3(object sender, EventArgs e)
        {
            for (int i = 0; i < 25; i++)
            {
                int row = i / 5;
                int col = i % 5;

                if (row >= 1 && row <= 3 && col >= 1 && col <= 3)
                {
                    KernelValues[i] = 1.0 / 9.0;
                    textBoxes[i].Text = "1/9";
                }
                else
                {
                    KernelValues[i] = 0.0;
                    textBoxes[i].Text = "0";
                }
            }
            pictureBox1.Image = DS.FilterPicture(KernelValues, 0.0);

        }
        private void Mean_5X5(object sender, EventArgs e)
        {
            double value = 1.0 / 25.0;
            for (int i = 0; i < 25; i++)
            {
                KernelValues[i] = value;
                textBoxes[i].Text = "1/25";
            }
            pictureBox1.Image = DS.FilterPicture(KernelValues, 0.0);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            double Sigma_Value = hScrollBar1.Value / 10.0; 
            textBox26.Text = Sigma_Value.ToString();
        }

        private void Gauss_Button(object sender, EventArgs e)
        {
            Sigma = double.Parse(textBox26.Text);
            if (Sigma != 0)
            {
                pictureBox1.Image = DS.FilterPicture(KernelValues, Sigma);
                this.DialogResult = DialogResult.None;
            }
        }

        private void Clear_Button_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 25; i++)
            {
                KernelValues[i] = 0;
                textBoxes[i].Text = "0";
            }
        }

        private void Solbel_H_Click(object sender, EventArgs e)
        {
            KernelValues = new double[25]
            {
            -1, -2, 0,  2,  1,
            -4, -8, 0,  8,  4,
            -6, -12, 0, 12,  6,
            -4, -8, 0,  8,  4,
            -1, -2, 0,  2,  1
            };

            // 更新 TextBox 顯示
            TextBox[] textBoxes = new TextBox[]
            {
            textBox1, textBox2, textBox3, textBox4, textBox5,
            textBox6, textBox7, textBox8, textBox9, textBox10,
            textBox11, textBox12, textBox13, textBox14, textBox15,
            textBox16, textBox17, textBox18, textBox19, textBox20,
            textBox21, textBox22, textBox23, textBox24, textBox25,
            };

            for (int i = 0; i < 25; i++)
            {
                textBoxes[i].Text = KernelValues[i].ToString();
            }
        }

        private void Solbel_V_Click(object sender, EventArgs e)
        {
            // 定義 5x5 Sobel 垂直濾波器
            KernelValues = new double[25]
            {
            -1, -4, -6, -4, -1,
            -2, -8, -12, -8, -2,
            0,  0,  0,  0,  0,
            2,  8,  12,  8,  2,
            1,  4,  6,  4,  1
            };

            // 更新 TextBox 顯示
            TextBox[] textBoxes = new TextBox[]
            {
            textBox1, textBox2, textBox3, textBox4, textBox5,
            textBox6, textBox7, textBox8, textBox9, textBox10,
            textBox11, textBox12, textBox13, textBox14, textBox15,
            textBox16, textBox17, textBox18, textBox19, textBox20,
            textBox21, textBox22, textBox23, textBox24, textBox25,
            };

            for (int i = 0; i < 25; i++)
            {
                textBoxes[i].Text = KernelValues[i].ToString();
            }
        }
    }
}
