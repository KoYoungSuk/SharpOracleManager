using Oracle.ManagedDataAccess.Client;
using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;

namespace SharpOracleManager
{
    public partial class MainForm : Form
    {
        private OracleConnection conn;
        private String address;
        private String id;
        private String port;
        private String sid;
        private String choosed_sql;
        Global g = new Global();
        Form1 frm = new Form1();
        public MainForm(String address, String port, String id, String sid, OracleConnection conn, String choosed_sql)
        {
            InitializeComponent();
            this.address = address;
            this.port = port;
            this.id = id;
            this.sid = sid;
            this.conn = conn;
            this.choosed_sql = choosed_sql;
            label8.Text = g.checkOS();
            label4.Text = "Success Connected!(" + address + "(PORT: " + port + ") Connected ID: " + id + ")";
            listBox1.Items.Add(label4.Text);
            textBox1.Text = choosed_sql;
        }

       
        private void runQuery()
        {
            listBox3.Items.Clear();
            dataGridView1.DataSource = null;
            try
            {
                String sql = textBox1.Text;
                String[] dcmdarr = sql.Split(' ');
                String dcmd = dcmdarr[0];
                listBox1.Items.Add("Execute Query: " + sql);
                OracleCommand ocmd = null;
                int result = 0;
                if (dcmd.Equals("select"))
                {
                    OracleDataAdapter oda = new OracleDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    oda.Fill(dt);
                   
                    dataGridView1.DataSource = dt;
                    result = 1;
                    listBox3.Items.Add("Degree: " + dt.Columns.Count);
                    listBox3.Items.Add("Cardinality: " + dt.Rows.Count);
                }
                else
                {
                    ocmd = new OracleCommand(sql, conn);
                    ocmd.ExecuteNonQuery();
                    result = 1;
                }
                listBox2.Items.Add(sql);
                
                if(result == 1)
                {
                    g.informationmessage("Execute Query Successfully.");
                    listBox1.Items.Add("Execute Query Successfully.");
                }
                else
                {
                    listBox1.Items.Add("Unknown Query Execute Error");
                }

            }
            catch (Exception ex)
            {
                listBox1.Items.Add("Error: " + ex.Message);
                listBox1.Items.Add("Execute Query Failed.");
                g.errormessage(ex.Message);
            }
        }

        private void saveList(ListBox listbox, Boolean b)
        {
            if (b)
            {
                saveFileDialog1.Filter = "Text Document(*.txt)|*.txt|All Files(*.*)|*.*";
            }
            else
            {
                saveFileDialog1.Filter = "SQL Files(*.sql)|*.sql|Text Document(*.txt)|*.txt|All Files(*.*)|*.*";
            }
            saveFileDialog1.Title = "Save List as Text Document";
            saveFileDialog1.FileName = DateTime.Now.ToShortDateString();
            try
            {
                StreamWriter sw = null;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    sw = File.CreateText(saveFileDialog1.FileName);
                    if (b)
                    {
                        sw.WriteLine("SharpOracleManager Oracle DataBase Input Query List \r\n");
                        sw.WriteLine("Address: " + address + " Port: " + port + " ID: " + id + " \r\n");
                        sw.WriteLine("Save Date: " + DateTime.Now + " \r\n");
                        sw.WriteLine("=====================================");
                    }
                    for (int i = 0; i < listbox.Items.Count; i++)
                    {
                        sw.Write(listbox.Items[i]);
                        if (!b)
                        {
                            sw.Write(";");
                        }
                        sw.Write("\r\n");
                    }
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                g.errormessage(ex.Message);
            }
        }

      
        private void label2_Click(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            runQuery();
        }

        private void exitXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveListAsTextDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveList(listBox2, false);
        }

        private void dataBaseDisConnectDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(conn != null)
            {
                conn.Close();
                conn = null;
                listBox1.Items.Add("DataBase is disconnected at [ " + DateTime.Now.ToString() + " ]");
            }
            else
            {
                g.errormessage("DataBase is already disconnected.");
            }
        }


        private void queryExecuteXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            runQuery();
        }

        private void aboutSharpOracleManagerAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm2 af = new AboutForm2();
            af.Show();
        }

        private void queryClearCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void listClearLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            saveList(listBox2, false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveList(listBox1, true);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            saveList(listBox1, true);
        }

        private void textBox1_Leave(object sender, EventArgs e) {}
        private void Form_KeyDown(object sender, KeyEventArgs e) {}

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            QuerySelectionForm qsf = new QuerySelectionForm(address, port, id, sid, conn);
            qsf.Show();
            this.Hide();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void dataGridViewClearDCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e) //Save as Excel File 
        {
            saveFileDialog1.Filter = "Microsoft Excel 97~2003 File(*.xls)|*.xls|All Files(*.*)|*.*";
            saveFileDialog1.Title = "Save as Excel File";
            saveFileDialog1.OverwritePrompt = true;
            Excel.Application excelApp = null;    
            Excel.Workbook wb = null;              
            Excel._Worksheet workSheet = null;		

            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    excelApp = new Excel.Application();
                    if (excelApp == null)
                    {
                        g.informationmessage("Not Excel Installed!");
                        return;
                    }

                    wb = excelApp.Workbooks.Add(true);

                    workSheet = wb.Worksheets.get_Item(1) as Excel._Worksheet;
                    workSheet.Name = "Excel WorkSheet";

                    if (dataGridView1.Rows.Count == 0)
                    {
                        g.informationmessage("Empty Data.");
                    }

                    // Header(Schema)
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        workSheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                    }

                    // Instance
                    for (int r = 0; r < dataGridView1.Rows.Count; r++)
                    {
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            workSheet.Cells[r + 2, i + 1] = dataGridView1.Rows[r].Cells[i].Value;
                        }
                    }

                    wb.SaveAs(saveFileDialog1.FileName, Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                   
                    wb.Close(Type.Missing, Type.Missing, Type.Missing);
                    excelApp.Quit();
                    releaseObject(excelApp);
                    releaseObject(workSheet);
                    releaseObject(wb);

                    g.informationmessage("Success!");

                }
            }
            catch(Exception ex)
            {
                g.errormessage(ex.Message); 
            }
            
       }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }

            catch (Exception e)
            {
                obj = null;
                g.informationmessage(e.Message);
            }

            finally
            {
                GC.Collect();
            }
        }
    }
}
