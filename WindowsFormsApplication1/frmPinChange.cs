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
    public partial class frmPinChange : Form
    {
        public frmPinChange()
        {
            InitializeComponent();
        }

        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Biometric Attendance Project New\FingerprintAtendance\WindowsFormsApplication1\Database1.accdb");
        private void btnChangePin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Enter the Pin", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Enter the New Pin", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Enter the Confirm Pin", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
                return;
            }
            try
            {
                //DBDataSetTableAdapters.PinTableTableAdapter pinObj = new DBDataSetTableAdapters.PinTableTableAdapter();
                //DBDataSet.PinTableDataTable dt = pinObj.UpdateQueryByPIN(textBox1.Text);
                //if (dt.Rows.Count > 0)
                //{
                //    MessageBox.Show("Successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //else
                //{
                //    MessageBox.Show("Unsuccessful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                DBDataSetTableAdapters.PinTableTableAdapter pinObj = new DBDataSetTableAdapters.PinTableTableAdapter();
                DBDataSet.PinTableDataTable dt = pinObj.GetDataByPIN(textBox1.Text);
                if (dt.Rows.Count > 0)
                {
                    if (textBox2.Text.Equals(textBox3.Text))
                    {
                        conn.Open();
                        OleDbCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "update PinTable set PIN=" + textBox3.Text + "";
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        frmPin p = new frmPin();
                        p.Show();
                        Dispose();
                    }
                    else
                    {
                        MessageBox.Show("PIN not matched", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox2.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect PIN", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Text = "";
                    textBox1.Focus();
                }

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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            frmPin fp = new frmPin();
            fp.Show();
            Dispose();
        }

        private void btnCancel_MouseEnter(object sender, EventArgs e)
        {
            btnCancel.BackColor = Color.FromArgb(255, 204, 153);
        }

        private void btnCancel_MouseLeave(object sender, EventArgs e)
        {
            btnCancel.BackColor = Color.FromArgb(255, 229, 204);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox2.Focus();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox3.Focus();
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnChangePin_Click(sender, e);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
