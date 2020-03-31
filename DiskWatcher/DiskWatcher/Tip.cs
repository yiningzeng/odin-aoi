using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiskWatcher
{
    public partial class Tip : Form
    {
        public Tip(string str)
        {
            InitializeComponent();
            this.label1.Text = str;
            this.DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
