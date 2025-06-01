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
    public partial class ConnectedComponent : Form
    {
        public ConnectedComponent(int sum)
        {
            InitializeComponent();
            this.textBox1.Text = sum.ToString();
        }
    }
}
