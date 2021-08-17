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
using System.Globalization;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public partial class Registration : UserControl
    {
        public Registration()
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
        int MatchThreshold = 1400;

        private void Registration_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(picFinger, "Fingerprint");          
            toolTip1.SetToolTip(textBox1, "Search by ID");
            toolTip1.SetToolTip(textBox2, "Search by Name");
            toolTip1.SetToolTip(textBox3, "Searched ID");
            toolTip1.SetToolTip(textBox4, "Searched Name");

            picFinger.BackColor = Color.Transparent;
            getIDandNAME();

            //Graphics g = pictureBox1.CreateGraphics();
            //g.Clear(Color.White);
            //Pen p = new Pen(Color.Black, 1);
            //g.DrawRectangle(p, 29, 20, 270, 470);
            //g.DrawRectangle(p, 466, 20, 270, 470);

            Control.CheckForIllegalCrossThreadCalls = false;
            //lblSerial.Text = "";
            lblStatus.Text = "";
            lblStatus.Left = 65;
            lblStatus.Width = 210;
            picFinger.Image = null;
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
        private void btnStartCapture_Click(object sender, EventArgs e)
        {
            btnInit();
            lblStatus.Visible = true;
            btnStartCapture.Enabled = false;
            //this.lblStatus.Location = new Point(125, lblStatus.Location.Y);
            try
            {
                dumRefreshTimer = 0;
                //lblStatus.Text = "";
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
                int ret = mfs100.StartCapture(80, 10000, chkShowPreview.Checked);
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

        private void btnStopCapture_Click(object sender, EventArgs e)
        {
            //lblStatus.Visible = false;
            //this.lblStatus.Location = new Point(125, lblStatus.Location.Y);
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
            //lblStatus.Visible = false;
            btnStartCapture.Enabled = true;
            //foo();
        }

        //private void AutoCapture()
        //{
        //    lblStatus.Text = "";
        //    picFinger.Image = null;
        //    Thread trd = new Thread(() =>
        //    {
        //        try
        //        {
        //            FingerData fingerprintData = null;
        //            ISOTemplate = null;
        //            //ANSITemplate = null;
        //            lblStatus.Text = "";
        //            picFinger.Image = null;
        //            //if (setQuality() == false)
        //            //{
        //            //    return;
        //            //}
        //            int ret = mfs100.AutoCapture(ref fingerprintData, 10000, chkShowPreview.Checked, chkIsDetectFinger.Checked);
        //            if (ret != 0)
        //            {
        //                ShowMessage(mfs100.GetErrorMsg(ret), true);
        //            }
        //            else
        //            {
        //                //lblStatus.Text = "Success: Quality: " + fingerprintData.Quality.ToString() + " Nfiq: " + fingerprintData.Nfiq.ToString();
        //                File.WriteAllBytes(datapath + "//ISOTemplate.iso", fingerprintData.ISOTemplate);
        //                File.WriteAllBytes(datapath + "//ISOImage.iso", fingerprintData.ISOImage);
        //                File.WriteAllBytes(datapath + "//AnsiTemplate.ansi", fingerprintData.ANSITemplate);
        //                File.WriteAllBytes(datapath + "//RawData.raw", fingerprintData.RawData);
        //                fingerprintData.FingerImage.Save(datapath + "//FingerImage.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
        //                File.WriteAllBytes(datapath + "//WSQImage.wsq", fingerprintData.WSQImage);
        //                ISOTemplate = new byte[fingerprintData.ISOTemplate.Length];
        //                fingerprintData.ISOTemplate.CopyTo(ISOTemplate, 0);
        //                ANSITemplate = new byte[fingerprintData.ANSITemplate.Length];
        //                fingerprintData.ANSITemplate.CopyTo(ANSITemplate, 0);
        //                string info = "Quality: " + fingerprintData.Quality.ToString() + "     Nfiq: " + fingerprintData.Nfiq.ToString() + "     Bpp: " + fingerprintData.Bpp.ToString() + "     GrayScale:" + fingerprintData.GrayScale.ToString() + "\nW(in):" + fingerprintData.InWidth.ToString() + "     H(in):" + fingerprintData.InHeight.ToString() + "     area(in):" + fingerprintData.InArea.ToString() + "     Dpi/Ppi:" + fingerprintData.Resolution.ToString() + "     Compress Ratio:" + fingerprintData.WSQCompressRatio.ToString() + "     WSQ Info:" + fingerprintData.WSQInfo.ToString();
        //                lblStatus.Text = info;
        //                ShowMessage("Capture Success.\nFinger data is saved at application path", false);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ShowMessage(ex.ToString(), true);
        //        }
        //        finally
        //        {
        //            GC.Collect();
        //        }
        //    });
        //    trd.Start();
        //}

        private void MatchISO()
        {
            conn.Open();
            OleDbCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;

            //cmd.CommandText = "SELECT fingerBytes,id,Name FROM Attendance";
            cmd.CommandText = "SELECT * FROM Attendance";
            cmd.ExecuteNonQuery();
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ////nameString = reader1["Name"].ToString();
                ////byte[] fingerbuffer = Encoding.ASCII.GetBytes(reader["fingerBytes"].ToString());
                
                string day = "day" + DateTime.Today.Day.ToString();

                string getMonthForQuarter = DateTime.Today.Month.ToString();
                string MonthForQuarter = "";
                int QuarterInt=0;

                if (getMonthForQuarter == "1" || getMonthForQuarter == "2" || getMonthForQuarter == "3")
                {
                    MonthForQuarter = "Quarter" + "1";
                    QuarterInt = Convert.ToInt32(reader["Quarter1"]) + 1;
                }
                if (getMonthForQuarter == "4" || getMonthForQuarter == "5" || getMonthForQuarter == "6")
                {
                    MonthForQuarter = "Quarter" + "2";
                    QuarterInt = Convert.ToInt32(reader["Quarter2"]) + 1;
                }
                if (getMonthForQuarter == "7" || getMonthForQuarter == "8" || getMonthForQuarter == "9")
                {
                    MonthForQuarter = "Quarter" + "3";
                    QuarterInt = Convert.ToInt32(reader["Quarter3"]) + 1;
                }
                if (getMonthForQuarter == "10" || getMonthForQuarter == "11" || getMonthForQuarter == "12")
                {
                    MonthForQuarter = "Quarter" + "4";
                    QuarterInt = Convert.ToInt32(reader["Quarter4"]) + 1;
                }

                string str = reader["fingerBytes"].ToString();
                //int[] res = new int[str.Length];

                //for (int i = 0; i < str.Length; i++)
                //{
                //    res[i] = Convert.ToInt32(str.Substring(i, 1));
                //}

                //byte[] fingerbuffer = res.Select(i => (byte)i).ToArray();
                byte[] fingerbuffer = str.Split(new char[] {'-'}).Select(hexChar => 
                byte.Parse(hexChar, NumberStyles.HexNumber)).ToArray();
                try
                {
                    if (ISOTemplate != null && ISOTemplate.Length > 0)
                    {
                        int score = 0;
                        // you can pass here two different ISOTemplates
                        int ret = mfs100.MatchISO(ISOTemplate, fingerbuffer, ref score);
                        if (ret == 0)
                        {
                            if (score >= MatchThreshold)
                            {
                                //ShowMessage("Finger matched with score: " + score.ToString(), false);
                                textBox3.Text = reader["id"].ToString();
                                textBox4.Text = reader["Name"].ToString();
                                string dayStr = reader[day].ToString();
                                conn.Close();

                                if(dayStr=="1")
                                {
                                    ShowMessage(textBox4.Text+" already attended", true);
                                    return;
                                }

                                conn.Open();
                                OleDbCommand cmd1 = conn.CreateCommand();
                                cmd1.CommandType = CommandType.Text;
                                //string day = "day" + DateTime.Today.Day.ToString();
                                cmd1.CommandText = "UPDATE Attendance SET " + day + " = 1,"+ MonthForQuarter + " = "+ QuarterInt.ToString() + " WHERE id=" + textBox3.Text + "";
                                cmd1.ExecuteNonQuery();
                                conn.Close();
                                ShowMessage("Welcome "+textBox4.Text, false);

                                return;
                            }
                            else
                            {
                                //ShowMessage("Finger not matched, score: " + score.ToString() + " is too low", false);
                            }
                        }
                        else
                        {
                            ShowMessage(mfs100.GetErrorMsg(ret), true);
                        }
                    }
                    else
                    {
                        ShowMessage("Please capture finger first", true);
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
            conn.Close();
            ShowMessage("No record found", true);
        }

        //private void MatchANSI()
        //{
        //    try
        //    {
        //        if (ISOTemplate != null && ISOTemplate.Length > 0)
        //        {
        //            int score = 0;
        //            // you can pass here two different ANSITemplate
        //            int ret = mfs100.MatchANSI(ANSITemplate, ANSITemplate, ref score);
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

        private void Uninit()
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

        //void OnMFS100Attached()
        //{
        //    ShowMessage("MFS100 found, You can initialized now", false);
        //}
        //void OnMFS100Detached()
        //{
        //    ShowMessage("MFS100 removed", true);
        //}

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

                    //string info = "Quality: " + fingerprintData.Quality.ToString() + "     Nfiq: " + fingerprintData.Nfiq.ToString() + "     Bpp: " + fingerprintData.Bpp.ToString() + "     GrayScale:" + fingerprintData.GrayScale.ToString() + "\nW(in):" + fingerprintData.InWidth.ToString() + "     H(in):" + fingerprintData.InHeight.ToString() + "     area(in):" + fingerprintData.InArea.ToString() + "     Dpi/Ppi:" + fingerprintData.Resolution.ToString() + "     Compress Ratio:" + fingerprintData.WSQCompressRatio.ToString() + "     WSQ Info:" + fingerprintData.WSQInfo.ToString();
                    //lblStatus.Text = info;
                    //ShowMessage("Capture Success.\nFinger data is saved at application path", false);
                    btnStartCapture.Enabled = true;
                    MatchISO();
                    dumRefreshTimer = 1;
                    foo();
                }
                else
                {
                    //this.lblStatus.Location = new Point(65, lblStatus.Location.Y);
                    lblStatus.Text = "Failed: error: " + errorCode.ToString() + " (" + errorMsg + ")";
                    //lblStatus.Visible = false;
                    btnStartCapture.Enabled = true;
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
                textBox3.Text = "";
                textBox4.Text = "";
            }
        }

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
                    //MessageBox.Show(ex.ToString());
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

        //bool setQuality()
        //{
        //    try
        //    {
        //        quality = Convert.ToInt32(80);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage("Invalid Quality Value", true);
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //    }
        //    return false;

        //}

        //bool setTimeout()
        //{
        //    try
        //    {
        //        timeout = Convert.ToInt32(10000);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage("Invalid Timeout Value", true);
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //    }
        //    return false;

        //}

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

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox2.ForeColor = Color.Black;
            label3.Visible = true;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text=="")
            {
                textBox2.Text = "Search by Name";
                textBox2.ForeColor = Color.Gray;
                label3.Visible = false;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBox1.Text = "Search by ID";
            textBox1.ForeColor = Color.Gray;
            label2.Visible = false;
        }


        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Biometric Attendance Project New\FingerprintAtendance\WindowsFormsApplication1\Database1.accdb");
        public void getIDandNAME()
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                
                cmd.CommandText = "SELECT ID,Name FROM Registration";
                cmd.ExecuteNonQuery();
                OleDbDataReader reader = cmd.ExecuteReader();

                //int idInt = 0;
                //string nameString = "";
                AutoCompleteStringCollection txt1 = new AutoCompleteStringCollection();
                AutoCompleteStringCollection txt2 = new AutoCompleteStringCollection();
                while (reader.Read())
                {
                    //idInt = Convert.ToInt32(reader["ID"]);
                    txt1.Add(reader["ID"].ToString());
                    //nameString = reader["Name"].ToString();
                    txt2.Add(reader["Name"].ToString());
                }
                //textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                //textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox1.AutoCompleteCustomSource = txt1;
                textBox2.AutoCompleteCustomSource = txt2;

                conn.Close();


                //conn.Open();
                //OleDbCommand cmd1 = conn.CreateCommand();
                //cmd1.CommandType = CommandType.Text;

                //cmd1.CommandText = "SELECT Name FROM Registration";
                //cmd1.ExecuteNonQuery();
                //OleDbDataReader reader1 = cmd1.ExecuteReader();

                //string nameString = "";
                //AutoCompleteStringCollection txt2 = new AutoCompleteStringCollection();
                //while (reader1.Read())
                //{
                //    nameString = reader1["Name"].ToString();
                //    txt2.Add(nameString);
                //}
                //textBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                //textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
                //textBox2.AutoCompleteCustomSource = txt2;
                //conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {           
            if (textBox1.Text != "Search by ID")
            {
                editRun = 0;
                editByIdOrName("id = " + textBox1.Text);
                if (editRun == 0)
                {
                    frmTransprent ft = new frmTransprent();
                    ft.Show();
                    editData.idData = Convert.ToInt32(textBox1.Text);
                    ////MainForm.regiCall();
                    editData.dumForEditid = 1;
                    MainDummyClass.dumForReg = 2;
                    textBox1.Text = "";
                    textBox1_Leave(sender, e);
                }
            }

            if (textBox2.Text != "Search by Name")
            {
                editRun = 0;
                editByIdOrName("Name = '" + textBox2.Text+"'");
                if (editRun == 0)
                {
                    frmTransprent ft = new frmTransprent();
                    ft.Show();
                    editData.nameData = textBox2.Text;
                    ////MainForm.regiCall();
                    editData.dumForEditName = 1;
                    MainDummyClass.dumForReg = 2;
                    textBox2.Text = "";
                    textBox2_Leave(sender, e);
                }
            }
        }

        int editRun = 0;
        void editByIdOrName(string val)
        {
            conn.Open();
            OleDbCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Registration WHERE "+val;
            cmd.ExecuteNonQuery();
            int countRow = Convert.ToInt32(cmd.ExecuteScalar());
            if (countRow < 1)
            {
                MessageBox.Show("Record not found", "Edit Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                editRun = 1;
            }
            conn.Close();
        }

        public static int dumManualAtte = 0, dumGetID = 0;
        public static string dumGetName = "";
        private void btnGiveAttend_Click(object sender, EventArgs e)
        {
            editRun = 0;
            if (textBox1.Text != "Search by ID")
            {
                editByIdOrName("id = " + textBox1.Text);
                if (editRun == 0)
                {
                    dumGetID = Convert.ToInt32(textBox1.Text);
                    frmTransprent ft = new frmTransprent();
                    ft.Show();
                    dumManualAtte = 1;
                    dumRefreshTimer = 0;
                }
                textBox1.Text = "";
                textBox1_Leave(sender, e);
            }

            if (textBox2.Text != "Search by Name")
            {
                editByIdOrName("Name = '" + textBox2.Text + "'");
                if (editRun == 0)
                {
                    dumGetName = textBox2.Text;
                    frmTransprent ft = new frmTransprent();
                    ft.Show();
                    dumManualAtte = 1;
                    dumRefreshTimer = 0;
                }
                textBox2.Text = "";
                textBox2_Leave(sender, e);
            }
        }

        public void SetIDAndName(string id, string nm)
        {
            textBox3.Text = id;
            textBox4.Text = nm;
            dumRefreshTimer = 1;
            foo();
        }

        //public string SetTextOfID
        //{
        //    get { return textBox1.Text; }
        //    set { textBox1.Text = value; }
        //}

        //public string SetTextOfName
        //{
        //    get { return textBox2.Text; }
        //    set { textBox2.Text = value; }
        //}
    }

    public static class editData
    {
        public static int dumForEditid = 0;
        public static int dumForEditName = 0;
        public static int idData = 0;
        public static string nameData = "";
    }
}
