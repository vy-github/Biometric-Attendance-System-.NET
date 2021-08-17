using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WindowsFormsApplication1
{
    public partial class frmPin : Form
    {
        public frmPin()
        {
            InitializeComponent();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lblGO_Click(sender, e);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void lblGO_MouseEnter(object sender, EventArgs e)
        {
            lblGO.BackColor = Color.FromArgb(143, 188, 219);
        }

        private void lblGO_MouseLeave(object sender, EventArgs e)
        {
            lblGO.BackColor = Color.FromArgb(141, 141, 141);
        }

        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Biometric Attendance Project New\FingerprintAtendance\WindowsFormsApplication1\Database1.accdb");
        private void lblGO_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Enter the PIN first", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }
            try
            {
                //DBDataSetTableAdapters.PinTableTableAdapter pinObj = new DBDataSetTableAdapters.PinTableTableAdapter();
                //DBDataSet.PinTableDataTable dt = pinObj.GetDataByPIN(textBox1.Text);
                //if (dt.Rows.Count > 0)
                //{
                //    MessageBox.Show("Successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //else
                //{
                //    MessageBox.Show("Unsuccessful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    textBox1.Text = "";
                //    textBox1.Focus();
                //    return;
                //}

                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT PIN FROM PinTable";
                cmd.ExecuteNonQuery();
                OleDbDataReader reader = cmd.ExecuteReader();
                //string PinStr = "";
                if(reader.Read())
                {
                    //PinStr = reader["PIN"].ToString();
                    if (textBox1.Text.Equals(reader["PIN"].ToString()))
                    {
                        MessageBox.Show("Successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //MainForm mf = new MainForm();
                        //mf.PinForm();
                        if (MainDummyClass.dumForReg == 1)
                        {
                            MainForm.PinForm();
                            MainDummyClass.dumForReg = 0;
                        }
                        if (MainDummyClass.dumForReg == 2)
                        {
                            MainForm.regiCall();
                            MainDummyClass.dumForReg = 3;
                        }
                        if(Registration.dumManualAtte==1)
                        {
                            MainForm.atteCall();
                            //Registration.dumManualAtte = 0;
                        }

                        Dispose();

                        Application.OpenForms["frmTransprent"].Close();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect PIN", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox1.Text = "";
                        textBox1.Focus();
                    }
                }                
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnChangePin_MouseEnter(object sender, EventArgs e)
        {
            btnChangePin.BackColor = Color.FromArgb(255, 204, 153);
        }

        private void btnChangePin_MouseLeave(object sender, EventArgs e)
        {
            btnChangePin.BackColor = Color.FromArgb(255, 229, 204);
        }

        private void btnChangePin_Click(object sender, EventArgs e)
        {
            frmPinChange pc = new frmPinChange();
            pc.Show();
            Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
            Application.OpenForms["frmTransprent"].Close();
        }

        private void btnCancel_MouseEnter(object sender, EventArgs e)
        {
            btnCancel.BackColor = Color.FromArgb(255, 204, 153);
        }

        private void btnCancel_MouseLeave(object sender, EventArgs e)
        {
            btnCancel.BackColor = Color.FromArgb(255, 229, 204);
        }
    }
}
