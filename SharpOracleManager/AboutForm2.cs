using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpOracleManager
{
    public partial class AboutForm2 : Form
    {
        Global g = new Global();
        public AboutForm2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Text = "Triggered!";
            g.informationmessage(g.checkOS());
            label7.Text = "You Triggered Easter Egg!";
        }
    }
}
