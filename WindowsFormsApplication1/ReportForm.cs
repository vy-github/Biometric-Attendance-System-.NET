using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApplication1
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
        }

        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Biometric Attendance Project New\FingerprintAtendance\WindowsFormsApplication1\Database1.accdb");
        private void ReportForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dBDataSet.Attendance' table. You can move, or remove it, as needed.
            //this.attendanceTableAdapter.Fill(this.dBDataSet.Attendance);
            panel1.Left = (this.ClientSize.Width - panel1.Width) / 2;

            conn.Open();
            OleDbCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Attendance";

            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            bunifuCustomDataGrid1.DataSource = dt;
            conn.Close();

            bunifuCustomDataGrid1.Columns[41].DisplayIndex = 0;
            for (int j = 1; j < 32; j++)
            {
                dt.Columns.Add("Day " + j.ToString(), typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    row["Day " + j.ToString()] = (int)row["day" + j.ToString()] == 0 ? "A" : "P";
                }
            }
            for (int i = 44; i < 75; i++)
            {
                bunifuCustomDataGrid1.Columns[i].DisplayIndex = i - 43;
            }
            //dataGridView1.Columns[44].DisplayIndex = 1;

            dt.Columns.Add("ID", typeof(string));
            bunifuCustomDataGrid1.Columns[75].DisplayIndex = 0;
            foreach (DataRow row in dt.Rows)
            {
                row["ID"] = (int)row["id"];
            }

            for (int i = 1; i < 32; i++)
            {
                bunifuCustomDataGrid1.Columns["day" + i.ToString()].Visible = false;
            }
            //bunifuCustomDataGrid1.Columns[40].DisplayIndex = 0;
            bunifuCustomDataGrid1.Columns["id"].Visible = false;
            bunifuCustomDataGrid1.Columns["fingerBytes"].Visible = false;

            //bunifuCustomDataGrid1.Columns["id"].HeaderText = "ID";
            bunifuCustomDataGrid1.Columns["name"].HeaderText = "Name";
            bunifuCustomDataGrid1.Columns["curMonth"].HeaderText = "Current Month";
            bunifuCustomDataGrid1.Columns["preMonth"].HeaderText = "Previous Month";
            bunifuCustomDataGrid1.Columns["ereMonth"].HeaderText = "Two Months Ago";
            bunifuCustomDataGrid1.Columns["Quarter1"].HeaderText = "Jan-Mar";
            bunifuCustomDataGrid1.Columns["Quarter2"].HeaderText = "Apr-Jun";
            bunifuCustomDataGrid1.Columns["Quarter3"].HeaderText = "Jul-Sep";
            bunifuCustomDataGrid1.Columns["Quarter4"].HeaderText = "Oct-Dec";
            bunifuCustomDataGrid1.Columns["curYear"].HeaderText = "Current Year";
            bunifuCustomDataGrid1.Columns["preYear"].HeaderText = "Previous Year";

            bunifuCustomDataGrid1.DataSource = dt;

            DataGridViewDisableButtonColumn delButton = new DataGridViewDisableButtonColumn();
            delButton.Name = "btnDelete";
            delButton.HeaderText = "";
            delButton.Text = "Delete";
            delButton.UseColumnTextForButtonValue = true;
            bunifuCustomDataGrid1.Columns.Add(delButton);

            for (int i = 1; i < 32; i++)
            {
                foreach (DataGridViewRow row in bunifuCustomDataGrid1.Rows)
                {
                    if (row.Cells["Day " + i.ToString()].Value.ToString() == "A")
                        row.Cells["Day " + i.ToString()].Style.ForeColor = Color.FromArgb(228, 31, 87);
                    else
                        row.Cells["Day " + i.ToString()].Style.ForeColor = Color.FromArgb(0, 200, 0); 
                }
            }

            int rowIndex = 0;
            foreach (DataGridViewRow row in bunifuCustomDataGrid1.Rows)
            {
                if (row.Cells[43].Value.ToString() != "Active")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(241, 190, 190);
                }
                else
                {
                    DataGridViewDisableButtonCell buttonCell = (DataGridViewDisableButtonCell)bunifuCustomDataGrid1.Rows[rowIndex].Cells["btnDelete"];
                    buttonCell.Enabled = !(Boolean)true;
                    bunifuCustomDataGrid1.Invalidate();
                }
                rowIndex++;
            }


            //foreach (DataGridViewRow row in bunifuCustomDataGrid1.Rows.OfType<DataGridViewRow>().Where(c => c.Cells[btnDelete.Index != null)

            //foreach (DataGridViewColumn column in dataGridView1.Columns)
            //{
            //    column.ValueType = typeof(string);
            //}

            //this.dataGridView1.Columns[1].ValueType = typeof(string);
            //Console.WriteLine(dataGridView1.Columns[1]);

            //for (int i=1;i<31;i++)
            //{
            //    foreach (DataGridViewRow row in dataGridView1.Rows)
            //    {
            //        int d = Convert.ToInt32(row.Cells[i].Value);
            //        if (d == 0)
            //        {
            //            row.Cells[i].Value = "A";
            //            //row.Cells[i].Value = row.Cells[i].Value.ToString() == "0" ? "A" : "P";
            //        }
            //    }
            //}

            //DataGridViewCell cell = null;
            //DataGridViewRow row1 = cell.OwningRow;
            //dataGridView1.Rows[1].Cells[1].Value = 'A';
        }
        
        void callll()
        {
            //foreach (DataGridViewCell cell in bunifuCustomDataGrid1.SelectedCells)
            //{
            //    if(cell.Value.ToString()=="A")

            //    //Console.WriteLine(string.Format("{0}", cell.Value));
            //}
            ////for (int i = 1; i < 32; i++)
            ////{
            ////    Console.WriteLine(bunifuCustomDataGrid1.Columns["Day " + i.ToString()].Name + " :- " + bunifuCustomDataGrid1.Rows[0].Cells[v].Value.ToString());
            ////}
            ////v++;
            for (int i = 1; i < 32; i++)
            {
                foreach (DataGridViewRow row in bunifuCustomDataGrid1.SelectedRows)
                {
                    //Console.WriteLine(bunifuCustomDataGrid1.Columns["Day " + i.ToString()].Name+" :- "+ row.Cells["Day " + i.ToString()].Value.ToString());
                    try
                    {
                        if (row.Cells["Day " + i.ToString()].Value.ToString() == "A")
                            row.Cells["Day " + i.ToString()].Style.SelectionForeColor = Color.FromArgb(228, 31, 87);
                        else
                            row.Cells["Day " + i.ToString()].Style.SelectionForeColor = Color.FromArgb(0, 200, 0);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void bunifuCustomDataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == bunifuCustomDataGrid1.Columns["btnDelete"].Index && e.RowIndex >= 0)
            {
                DataGridViewDisableButtonCell buttonCell = (DataGridViewDisableButtonCell)bunifuCustomDataGrid1.Rows[e.RowIndex].Cells["btnDelete"];
                if (buttonCell.Enabled)
                {
                    int idValue = Convert.ToInt32(bunifuCustomDataGrid1.Rows[e.RowIndex].Cells["id"].Value);
                    DialogResult dr = MessageBox.Show("Are you show to delete record!", "Record Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        conn.Open();
                        OleDbCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "DELETE FROM Attendance WHERE id = " + idValue.ToString() + "";
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Record Deleted","Deleted",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        foreach (DataGridViewRow row in bunifuCustomDataGrid1.SelectedRows)
                            bunifuCustomDataGrid1.Rows.RemoveAt(row.Index);
                    }
                }
            }
        }
        
        private void bunifuCustomDataGrid1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //foreach (DataGridViewRow row in bunifuCustomDataGrid1.Rows)
            //{
            //    //if (col.DefaultCellStyle.SelectionForeColor == col.DefaultCellStyle.SelectionForeColor)
            //    //{
            //    //    col.DefaultCellStyle.SelectionForeColor = Color.Empty;
            //    //}
            //    if(row.Cells[0].Value.ToString() == "A")
            //    {
            //        row.DefaultCellStyle.SelectionForeColor = Color.Red;
            //    }

            //}
            callll();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            (bunifuCustomDataGrid1.DataSource as DataTable).DefaultView.RowFilter = string.Format("ID LIKE '%{0}%'", textBox1.Text);
            foreColorOnChange();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.Black;
            label2.Visible = true;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Search by ID";
                textBox1.ForeColor = Color.Gray;
                label2.Visible = false;
            }
            (bunifuCustomDataGrid1.DataSource as DataTable).DefaultView.RowFilter = string.Format("ID LIKE '%{0}%'", "");
            foreColorOnChange();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            textBox2.Text = "Search by Name";
            textBox2.ForeColor = Color.Gray;
            label3.Visible = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            (bunifuCustomDataGrid1.DataSource as DataTable).DefaultView.RowFilter = string.Format("Name LIKE '%{0}%'", textBox2.Text);
            foreColorOnChange();
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox2.ForeColor = Color.Black;
            label3.Visible = true;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Search by Name";
                textBox2.ForeColor = Color.Gray;
                label3.Visible = false;
            }
            (bunifuCustomDataGrid1.DataSource as DataTable).DefaultView.RowFilter = string.Format("Name LIKE '%{0}%'", "");
            foreColorOnChange();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBox1.Text = "Search by ID";
            textBox1.ForeColor = Color.Gray;
            label2.Visible = false;
        }

        void foreColorOnChange()
        {
            for (int i = 1; i < 32; i++)
            {
                foreach (DataGridViewRow row in bunifuCustomDataGrid1.Rows)
                {
                    if (row.Cells["Day " + i.ToString()].Value.ToString() == "A")
                        row.Cells["Day " + i.ToString()].Style.ForeColor = Color.FromArgb(228, 31, 87);
                    else
                        row.Cells["Day " + i.ToString()].Style.ForeColor = Color.FromArgb(0, 200, 0);
                }
            }
            callll();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //if (textBox1.Text != "Search by ID")
            //{
            //    searchData(textBox1.Text,40);
            //}

            //if (textBox2.Text != "Search by Name")
            //{
            //    searchData(textBox2.Text,41);
            //}
        }

        public void Maximize(Form frmReport)
        {
            frmReport.WindowState = FormWindowState.Maximized;
        }

        private void ReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.dumForRep = 0;
        }

        //void searchData(string seDum,int seRow)
        //{
        //    bunifuCustomDataGrid1.ClearSelection();
        //    bunifuCustomDataGrid1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        //    try
        //    {
        //        foreach (DataGridViewRow row in bunifuCustomDataGrid1.Rows)
        //        {
        //            if (row.Cells[seRow].Value.ToString().Equals(seDum))
        //            {
        //                row.Selected = true;
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        //private void SetDGVButtonColumnEnable(bool enabled)
        //{
        //    foreach (DataGridViewRow row in bunifuCustomDataGrid1.Rows)
        //    {
        //        // Set Enabled property of the fourth column in the DGV.
        //        ((DataGridViewDisableButtonCell)row.Cells[75]).Enabled = enabled;
        //    }
        //    bunifuCustomDataGrid1.Refresh();
        //}
    }

    public class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
    {
        public DataGridViewDisableButtonColumn()
        {
            this.CellTemplate = new DataGridViewDisableButtonCell();
        }
    }

    public class DataGridViewDisableButtonCell : DataGridViewButtonCell
    {
        private bool enabledValue;
        public bool Enabled
        {
            get
            {
                return enabledValue;
            }
            set
            {
                enabledValue = value;
            }
        }

        // Override the Clone method so that the Enabled property is copied.
        public override object Clone()
        {
            DataGridViewDisableButtonCell cell =
                (DataGridViewDisableButtonCell)base.Clone();
            cell.Enabled = this.Enabled;
            return cell;
        }

        // By default, enable the button cell.
        public DataGridViewDisableButtonCell()
        {
            this.enabledValue = true;
        }

        protected override void Paint(Graphics graphics,
            Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value,
            object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // The button cell is disabled, so paint the border,
            // background, and disabled button for the cell.
            if (!this.enabledValue)
            {
                // Draw the cell background, if specified.
                if ((paintParts & DataGridViewPaintParts.Background) ==
                    DataGridViewPaintParts.Background)
                {
                    SolidBrush cellBackground =
                        new SolidBrush(cellStyle.BackColor);
                    graphics.FillRectangle(cellBackground, cellBounds);
                    cellBackground.Dispose();
                }

                // Draw the cell borders, if specified.
                if ((paintParts & DataGridViewPaintParts.Border) ==
                    DataGridViewPaintParts.Border)
                {
                    PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                        advancedBorderStyle);
                }

                // Calculate the area in which to draw the button.
                Rectangle buttonArea = cellBounds;
                Rectangle buttonAdjustment =
                    this.BorderWidths(advancedBorderStyle);
                buttonArea.X += buttonAdjustment.X;
                buttonArea.Y += buttonAdjustment.Y;
                buttonArea.Height -= buttonAdjustment.Height;
                buttonArea.Width -= buttonAdjustment.Width;

                // Draw the disabled button.
                ButtonRenderer.DrawButton(graphics, buttonArea,
                    PushButtonState.Disabled);

                // Draw the disabled button text.
                if (this.FormattedValue is String)
                {
                    TextRenderer.DrawText(graphics,
                        (string)this.FormattedValue,
                        this.DataGridView.Font,
                        buttonArea, SystemColors.GrayText);
                }
            }
            else
            {
                // The button cell is enabled, so let the base class
                // handle the painting.
                base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                    elementState, value, formattedValue, errorText,
                    cellStyle, advancedBorderStyle, paintParts);
            }
        }
    }
}
