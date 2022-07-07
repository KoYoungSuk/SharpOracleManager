using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpOracleManager
{
    public partial class QuerySelectionForm : Form
    {
        String address;
        String port;
        String id;
        String sid;
        OracleConnection conn;
        Global g = new Global();
        public QuerySelectionForm(String address, String port, String id, String sid, OracleConnection conn)
        {
            this.address = address;
            this.port = port;
            this.id = id;
            this.sid = sid;
            this.conn = conn;
            this.Text = g.checkOS();
            InitializeComponent();
        }
        
        public void choose()
        {
            try
            {
                String sql = textBox1.Text;
                MainForm mf = new MainForm(address, port, id, sid, conn, sql);
                mf.Show();
                this.Hide();
            }
            catch(Exception ex)
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
            choose();
        }

        private void QuerySelectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                choose();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "SQL Files(*.sql)|*.sql|Text Document(*.txt)|*.txt|All Files(*.*)|*.*";
                openFileDialog1.Title = "Open Query List";
                StreamReader sr = null;
                String fulltxt = null;
                String[] fulltxtarr = null;
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    sr = new StreamReader(openFileDialog1.FileName);
                    fulltxt = sr.ReadToEnd();
                    fulltxtarr = fulltxt.Split(';');
                    sr.Close();
                    for(int i=0; i<fulltxtarr.Length; i++)
                    {
                        listBox1.Items.Add(fulltxtarr[i].Replace('\r', ' ').Replace('\n', ' ').Trim());
                    }
                }

            }catch(Exception ex)
            {
                g.errormessage(ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = listBox1.SelectedItem.ToString();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void QuerySelectionForm_Load(object sender, EventArgs e)
        {

        }
    }
}
