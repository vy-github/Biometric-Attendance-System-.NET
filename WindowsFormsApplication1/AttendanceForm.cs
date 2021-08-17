using MANTRA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public partial class AttendanceForm : UserControl
    {
        string stringImage = "";
        public AttendanceForm()
        {
            InitializeComponent();
        }

        MFS100 mfs100 = null;
        //int quality = 60;
        //int timeout = 10000;
        string datapath = Application.StartupPath + "\\FingerData";
        byte[] ISOTemplate = null;
        byte[] ANSITemplate = null;
        //byte[] com = null;
        DeviceInfo deviceInfo = null;
        string key = "";
        //int MatchThreshold = 1400;

        private void AttendanceForm_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(label17, "Set quality of fingerprint we recommend quality should 90 or more");
            toolTip1.SetToolTip(coboxQuality, "Set quality of fingerprint we recommend quality should 90 or more");
            toolTip1.SetToolTip(label18, "Set time for fingerprint capture timing");
            toolTip1.SetToolTip(coboxTime, "Set time for fingerprint capture timing");
            toolTip2.SetToolTip(textBox1, "ID of current registration");
            toolTip2.SetToolTip(textBox2, "Enter the Name");
            toolTip2.SetToolTip(textBox3, "Enter the Mobile No");
            toolTip2.SetToolTip(textBox4, "Enter the Address");
            toolTip2.SetToolTip(dateTimePicker1, "Enter the joining date");
            //toolTip2.SetToolTip(Search, "Edit Previous Record");

            dateTimePicker1.Parent = panel4;
            picFinger.Parent = panel5;

            dateTimePicker1.CalendarTitleBackColor = Color.Transparent;
            picFinger.BackColor = Color.Transparent;

            textBox1.BackColor = Color.FromArgb(241, 245, 250);
            textBox2.BackColor = Color.FromArgb(241, 245, 250);
            textBox3.BackColor = Color.FromArgb(241, 245, 250);
            textBox4.BackColor = Color.FromArgb(237, 242, 250);

            coboxQuality.BackColor= Color.FromArgb(214, 226, 240);
            coboxTime.BackColor = Color.White;

            dateTimePicker1.MaxDate = DateTime.Now;


            Control.CheckForIllegalCrossThreadCalls = false;
            //lblSerial.Text = "";
            lblStatus.Left = 62;
            lblStatus.Width = 208;
            //resetControl();
            mfs100 = new MFS100(key);
            //mfs100.OnMFS100Attached += OnMFS100Attached;
            //mfs100.OnMFS100Detached += OnMFS100Detached;
            mfs100.OnPreview += OnPreview;
            mfs100.OnCaptureCompleted += OnCaptureCompleted;
            try
            {
                if (!Directory.Exists(datapath))
                {
                    Directory.CreateDirectory(datapath);
                }
            }
            catch (Exception ex)
            {
                //ShowMessage(ex.Message, true);
            }
        }

        private void btnInit()
        {
            try
            {
                int ret = mfs100.Init();
                if (ret != 0)
                {
                    ShowMessage(mfs100.GetErrorMsg(ret), true);
                }
                else
                {
                    deviceInfo = mfs100.GetDeviceInfo();
                    /*if (deviceInfo != null)
                    {
                        //string scannerInfo = "SERIAL NO.: " + deviceInfo.SerialNo + "     MAKE: " + deviceInfo.Make + "     MODEL: " + deviceInfo.Model + "\nWIDTH: " + deviceInfo.Width.ToString() + "     HEIGHT: " + deviceInfo.Height.ToString() + "     CERT: " + mfs100.GetCertification();
                        //lblSerial.Text = scannerInfo;
                    }
                    else
                    {
                        //lblSerial.Text = "";
                    }*/
                    ShowMessage(mfs100.GetErrorMsg(ret), false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.ToString(), true);
            }
            finally
            {
                GC.Collect();
            }
        }

        int dumRefreshTimer = 1;
        private void btnStart_Click(object sender, EventArgs e)
        {
            btnInit();
            lblStatus.Visible = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnUnini.Enabled = false;
            coboxQuality.Enabled = false;
            coboxTime.Enabled = false;
            //this.lblStatus.Location = new Point(120, lblStatus.Location.Y);
            try
            {
                dumRefreshTimer = 0;
                //resetControl();
                picFinger.Image = null;
                ISOTemplate = null;
                //ANSITemplate = null;
                //if (setQuality() == false)
                //{
                //    return;
                //}
                //if (setTimeout() == false)
                //{
                //    return;
                //}
                int ret = mfs100.StartCapture(Convert.ToInt32 (coboxQuality.Text), (Convert.ToInt32(coboxTime.Text)*1000), chkShowPreview.Checked);
                if (ret != 0)
                {
                    ShowMessage(mfs100.GetErrorMsg(ret), true);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.ToString(), true);
            }
            finally
            {
                GC.Collect();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //lblStatus.Visible = false;
            //this.lblStatus.Location = new Point(120, lblStatus.Location.Y);
            try
            {
                int ret = mfs100.StopCapture();
                //ShowMessage(mfs100.GetErrorMsg(ret), false);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.ToString(), true);
            }
            finally
            {
                GC.Collect();
            }
            btnStart.Enabled = true;
            //btnStop.Enabled = false;
            btnUnini.Enabled = true;
            coboxQuality.Enabled = true;
            coboxTime.Enabled = true;
        }

        //private void MatchISO()
        //{
        //    try
        //    {
        //        if (ISOTemplate != null && ISOTemplate.Length > 0)
        //        {
        //            int score = 0;
        //            // you can pass here two different ISOTemplates
        //            int ret = mfs100.MatchISO(ISOTemplate, ISOTemplate, ref score);
        //            if (ret == 0)
        //            {
        //                if (score >= MatchThreshold)
        //                {
        //                    ShowMessage("Finger matched with score: " + score.ToString(), false);
        //                }
        //                else
        //                {
        //                    ShowMessage("Finger not matched, score: " + score.ToString() + " is too low", false);
        //                }
        //            }
        //            else
        //            {
        //                ShowMessage(mfs100.GetErrorMsg(ret), true);
        //            }
        //        }
        //        else
        //        {
        //            ShowMessage("Please capture finger first", true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage(ex.ToString(), true);
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //    }
        //}

        private void btnUnini_Click(object sender, EventArgs e)
        {
            try
            {
                int ret = mfs100.Uninit();
                //ShowMessage(mfs100.GetErrorMsg(ret), false);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.ToString(), true);
            }
            finally
            {
                GC.Collect();
            }
        }

        void OnCaptureCompleted(bool status, int errorCode, string errorMsg, FingerData fingerprintData)
        {
            try
            {
                if (status)
                {
                    picFinger.Image = fingerprintData.FingerImage;
                    picFinger.Refresh();
                    //lblStatus.Text = "Success: Quality: " + fingerprintData.Quality.ToString() + " Nfiq: " + fingerprintData.Nfiq.ToString();

                    File.WriteAllBytes(datapath + "//ISOTemplate.iso", fingerprintData.ISOTemplate);
                    File.WriteAllBytes(datapath + "//ISOImage.iso", fingerprintData.ISOImage);
                    File.WriteAllBytes(datapath + "//AnsiTemplate.ansi", fingerprintData.ANSITemplate);
                    File.WriteAllBytes(datapath + "//RawData.raw", fingerprintData.RawData);
                    fingerprintData.FingerImage.Save(datapath + "//FingerImage.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                    File.WriteAllBytes(datapath + "//WSQImage.wsq", fingerprintData.WSQImage);

                    ISOTemplate = new byte[fingerprintData.ISOTemplate.Length];
                    fingerprintData.ISOTemplate.CopyTo(ISOTemplate, 0);
                    ANSITemplate = new byte[fingerprintData.ANSITemplate.Length];
                    fingerprintData.ANSITemplate.CopyTo(ANSITemplate, 0);

                    byte[] byteImage = File.ReadAllBytes(datapath + @"\ISOTemplate.iso");
                    stringImage = BitConverter.ToString(byteImage); ;

                    //string info = "Quality: " + fingerprintData.Quality.ToString() + "     Nfiq: " + fingerprintData.Nfiq.ToString() + "     Bpp: " + fingerprintData.Bpp.ToString() + "     GrayScale:" + fingerprintData.GrayScale.ToString() + "\nW(in):" + fingerprintData.InWidth.ToString() + "     H(in):" + fingerprintData.InHeight.ToString() + "     area(in):" + fingerprintData.InArea.ToString() + "     Dpi/Ppi:" + fingerprintData.Resolution.ToString() + "     Compress Ratio:" + fingerprintData.WSQCompressRatio.ToString() + "     WSQ Info:" + fingerprintData.WSQInfo.ToString();
                    //lblStatus.Text = info;
                    //ShowMessage("Capture Success.\nFinger data is saved at application path", false);
                    btnStart.Enabled = true;
                    btnUnini.Enabled = true;
                    coboxQuality.Enabled = true;
                    coboxTime.Enabled = true;
                    dumRefreshTimer = 1;
                    foo();
                }
                else
                {
                    //this.lblStatus.Location = new Point(60, lblStatus.Location.Y);
                    lblStatus.Text = "Failed: error: " + errorCode.ToString() + " (" + errorMsg + ")";
                    btnStart.Enabled = true;
                    btnUnini.Enabled = true;
                    coboxQuality.Enabled = true;
                    coboxTime.Enabled = true;
                    //textBox2.Text = lblStatus.Text;
                    //System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                    //timer.Interval = 5000;
                    //timer.Tick += new EventHandler(timer_Tick);
                    //timer.Start();
                    dumRefreshTimer = 1;
                    foo();
                }
                lblStatus.Refresh();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.ToString(), true);
            }
            finally
            {
                GC.Collect();
                //timer1.Start();
            }
        }

        private void foo()
        {
            Task.Delay(5000).ContinueWith(t => Bar());
        }

        private void Bar()
        {
            if (dumRefreshTimer == 1)
            {
                picFinger.Image = null;
                lblStatus.Visible = false;
            }
        }

        //void timer_Tick(object sender, EventArgs e)
        //{
        //    if (errmsg == 1)
        //    {
        //        lblStatus.Visible = false;
        //        errmsg = -1;
        //        timer1.Stop();
        //    }
        //    errmsg++;
        //}

        void OnPreview(FingerData fingerprintData)
        {
            Thread trd = new Thread(() =>
            {
                try
                {
                    if (fingerprintData != null)
                    {
                        picFinger.Image = fingerprintData.FingerImage;
                        picFinger.Refresh();
                        lblStatus.Text = "Quality :- " + fingerprintData.Quality.ToString();
                        lblStatus.Refresh();
                    }
                }
                catch (Exception ex)
                {
                    //ShowMessage(ex.ToString(), true);
                }
            });
            trd.Start();
        }

        //void resetControl()
        //{
        //    lblStatus.Text = "";
        //    picFinger.Image = null;
        //}

        void ShowMessage(string msg, bool iserror)
        {
            MessageBox.Show(msg, "MFS100", MessageBoxButtons.OK, (iserror ? MessageBoxIcon.Error : MessageBoxIcon.Information), MessageBoxDefaultButton.Button1);
        }

        //int errmsg = 0;
        //private void timer1_Tick(object sender, EventArgs e)
        //{
            //if(errmsg==1)
            //{
            //    lblStatus.Visible = false;
            //    errmsg = -1;
            //    timer1.Stop();
            //}
            //errmsg++;
        //}

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Graphics g = e.Graphics)
            {
                var p = new Pen(Color.Black, 1);
                g.DrawRectangle(p, 117, 84, 191, 25);
                g.DrawRectangle(p, 117, 131, 191, 25);
                g.DrawRectangle(p, 117, 178, 191, 25);

                g.DrawRectangle(p, 117, 230, 191, 81);
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            dateTimePicker1.Value = DateTime.Now.Date;
            picFinger.Image = null;
            stringImage = "";
            lblStatus.Visible = false;
            textBox2.Focus();
        }

        private void btnStart_MouseMove(object sender, MouseEventArgs e)
        {
            btnStart.BackColor = Color.FromArgb(10, 110, 110);
            btnStart.ForeColor = Color.FromArgb(200, 220, 200);
        }

        private void btnStart_MouseLeave(object sender, EventArgs e)
        {
            btnStart.BackColor = Color.FromArgb(185, 185, 185);
            btnStart.ForeColor = Color.FromArgb(103, 40, 128);
        }

        private void btnStop_MouseMove(object sender, MouseEventArgs e)
        {
            btnStop.BackColor = Color.FromArgb(10, 110, 110);
            btnStop.ForeColor = Color.FromArgb(200, 220, 200);
        }

        private void btnStop_MouseLeave(object sender, EventArgs e)
        {
            btnStop.BackColor = Color.FromArgb(185, 185, 185);
            btnStop.ForeColor = Color.FromArgb(103, 40, 128);
        }

        private void btnUnini_MouseMove(object sender, MouseEventArgs e)
        {
            btnUnini.BackColor = Color.FromArgb(10, 110, 110);
            btnUnini.ForeColor = Color.FromArgb(200, 220, 200);
        }

        private void btnUnini_MouseLeave(object sender, EventArgs e)
        {
            btnUnini.BackColor = Color.FromArgb(185, 185, 185);
            btnUnini.ForeColor = Color.FromArgb(103, 40, 128);
        }

        private void btnStatus_MouseMove(object sender, MouseEventArgs e)
        {
            btnStatus.BackColor = Color.FromArgb(10, 110, 110);
            btnStatus.ForeColor = Color.FromArgb(200, 220, 200);
        }

        private void btnStatus_MouseLeave(object sender, EventArgs e)
        {
            btnStatus.BackColor = Color.FromArgb(185, 185, 185);
            btnStatus.ForeColor = Color.FromArgb(103, 40, 128);
        }

        private void btnSubmit_MouseMove(object sender, MouseEventArgs e)
        {
            btnSubmit.BackColor = Color.FromArgb(255,217,179);
            btnSubmit.ForeColor = Color.FromArgb(23,127,117);
        }

        private void btnSubmit_MouseLeave(object sender, EventArgs e)
        {
            btnSubmit.BackColor = Color.FromArgb(110, 110, 110);
            btnSubmit.ForeColor = Color.FromArgb(206, 235, 200); 
        }

        private void btnClear_MouseMove(object sender, MouseEventArgs e)
        {
            btnClear.BackColor = Color.FromArgb(255, 217, 179);
            btnClear.ForeColor = Color.FromArgb(23, 127, 117);
        }

        private void btnClear_MouseLeave(object sender, EventArgs e)
        {
            btnClear.BackColor = Color.FromArgb(110, 110, 110);
            btnClear.ForeColor = Color.FromArgb(206, 235, 200);
        }

        private void btnDelete_MouseMove(object sender, MouseEventArgs e)
        {
            btnDelete.BackColor = Color.FromArgb(255, 217, 179);
            btnDelete.ForeColor = Color.FromArgb(23, 127, 117);
        }

        private void btnDelete_MouseLeave(object sender, EventArgs e)
        {
            btnDelete.BackColor = Color.FromArgb(110, 110, 110);
            btnDelete.ForeColor = Color.FromArgb(206, 235, 200);
        }

        //private void Search_Click(object sender, EventArgs e)
        //{
        //    textBox1.Enabled = true;
        //    textBox1.Focus();
        //}

        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Biometric Attendance Project New\FingerprintAtendance\WindowsFormsApplication1\Database1.accdb");
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Enter the Name", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Enter the Mobile No.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
                return;
            }
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Enter the Address", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox4.Focus();
                return;
            }
            //if(picFinger.Image==null)
            //{
            //    MessageBox.Show("Fingerprint is compulsory", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    picFinger.Focus();
            //    return;
            //}
            if (string.IsNullOrEmpty(stringImage))
            {
                MessageBox.Show("Fingerprint is compulsory", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                picFinger.Focus();
                return;
            }

            try
                {
                
                //foreach (byte a in byteImage)
                //    stringImage = stringImage + a;

                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;

                if (btnSubmit.Text == "Submit")
                {
                    cmd.CommandText = "insert into Registration (ID,Name,MobNo,Address,Joidate,fingerBytes) values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + dateTimePicker1.Value.ToString("dd-MM-yyyy") + "','" + stringImage + "')";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "insert into Attendance (ID,Name,fingerBytes,Status) values('" + textBox1.Text + "','" + textBox2.Text + "','" + stringImage + "','Active')";
                    cmd.ExecuteNonQuery();

                    conn.Close();

                    MessageBox.Show("Registration Successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnClear.PerformClick();
                }
                if (btnSubmit.Text == "Update")
                {
                    cmd.CommandText = "UPDATE Registration SET ID='" + textBox1.Text + "',Name='" + textBox2.Text + "',MobNo='" + textBox3.Text + "',Address='" + textBox4.Text + "',Joidate='" + dateTimePicker1.Value.ToString("dd-MM-yyyy") + "',fingerBytes='" + stringImage + "' WHERE id="+textBox1.Text+"";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "UPDATE Attendance SET ID='" + textBox1.Text + "',Name='" + textBox2.Text + "',fingerBytes='" + stringImage + "' WHERE id=" + textBox1.Text + "";
                    cmd.ExecuteNonQuery();

                    conn.Close();

                    MessageBox.Show("Record Updated", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //btnClear.PerformClick();
                    btnUnini.PerformClick();
                    MainForm.atteCall();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void getId()
        {
            if (MainDummyClass.dumForReg != 3)
            {
                try
                {
                    conn.Open();
                    OleDbCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "SELECT id FROM Registration";
                    cmd.ExecuteNonQuery();
                    OleDbDataReader reader = cmd.ExecuteReader();
                    int idInt = 0;
                    while (reader.Read())
                    {
                        idInt = Convert.ToInt32(reader["id"]);
                    }
                    textBox1.Text = (idInt + 1).ToString();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            getId();
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            btnClear.PerformClick();
            btnUnini.PerformClick();
            MainForm.statusCall();
        }

        public void submitSetting()
        {
            btnSubmit.Location = new Point(218,btnSubmit.Location.Y);
            btnClear.Location = new Point(383, btnSubmit.Location.Y);
            btnDelete.Visible = false;
            btnSubmit.Text = "Submit";
            //textBox1.Text = "";
            //textBox2.Text = "";
            btnClear.PerformClick();
        }

        public void editSetting()
        {
            btnDelete.Visible = true;
            btnSubmit.Text = "Update";
            btnSubmit.Location = new Point(134, btnSubmit.Location.Y);
            btnDelete.Location = new Point(299, btnSubmit.Location.Y);
            btnClear.Location = new Point(465, btnSubmit.Location.Y);
            if (editData.dumForEditid == 1)
            {
                textBox1.Text = editData.idData.ToString();
                editData.dumForEditid = 0;
                GetDataIdOrName("id = " + textBox1.Text);
            }

            if (editData.dumForEditName == 1)
            {
                textBox2.Text = editData.nameData.ToString();
                editData.dumForEditName = 0;
                GetDataIdOrName("Name = '" + textBox2.Text + "'");
            }
            //stringImage = "";
        }

        void GetDataIdOrName(string val)
        {
            conn.Open();
            OleDbCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Registration WHERE " + val;
            cmd.ExecuteNonQuery();
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                textBox1.Text = reader["id"].ToString();
                textBox2.Text = reader["Name"].ToString();
                textBox3.Text = reader["MobNo"].ToString();
                textBox4.Text = reader["Address"].ToString();
                dateTimePicker1.Value = (DateTime)reader["Joidate"];
                stringImage = reader["fingerBytes"].ToString(); 
            }
            conn.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you show to delete record!", "Record Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE Attendance SET id = '0',Status = 'Leaved' WHERE id=" + textBox1.Text + "";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Registration WHERE id = " + textBox1.Text;
                cmd.ExecuteNonQuery();
                conn.Close();
                ShowMessage("Record Deleted", false);
                //btnClear.PerformClick();
                btnUnini.PerformClick();
                MainForm.atteCall();
            }
        }
    }
}
