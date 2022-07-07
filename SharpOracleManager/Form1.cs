using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpOracleManager
{
    public partial class Form1 : Form
    {
        OracleConnection conn;
        Global g = new Global();
        public Form1()
        {
            InitializeComponent();
        }
        
        public void connectDB(String address, String port, String sid)
        {
            try
            {
                String strconn = g.connstr(address, port, sid, textBox2.Text, textBox3.Text);
                conn = new OracleConnection(strconn);
                conn.Open();
                MainForm mf = new MainForm(address, port, textBox2.Text, sid, conn, "");
                this.Hide();
                mf.Show();
            }
            catch (Exception ex)
            {
                g.errormessage(ex.Message);
            }
        }

        private void login()
        {
            try
            {
                String[] strconnarr = textBox1.Text.Split(':');
                String[] strconnarr2 = strconnarr[1].Split('/');
                String address = strconnarr[0];
                String port = strconnarr2[0];
                String sid = strconnarr2[1];
                connectDB(address, port, sid);
            }
            catch (Exception ex)
            {
                g.errormessage(ex.Message);
            }
        }
  
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            login();
        }
        private void Form1_Load(object sender, EventArgs e){}

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void groupBox1_Enter(object sender, EventArgs e){}
        private void Form1_KeyDown(object sender, KeyEventArgs e) { }
        private void text_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                login();
            }
        }
    }
}
