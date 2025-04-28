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
        }
        public DIPSample DS { get; set; }
        public double[] KernelValues { get; private set; }
        private void button1_Click(object sender, EventArgs e)
        {
            KernelValues = new double[25];

            TextBox[] textBoxes = new TextBox[]
            {
           textBox1, textBox2, textBox3,textBox4, textBox5, 
           textBox6,textBox7, textBox8, textBox9, textBox10, 
           textBox11, textBox12, textBox13, textBox14, textBox15,
           textBox16, textBox17, textBox18, textBox19, textBox20,
           textBox21, textBox22, textBox23, textBox24, textBox25,
            };

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

            pictureBox1.Image = DS.FilterPicture(KernelValues);
            this.DialogResult = DialogResult.None;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (KernelValues != null) 
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
