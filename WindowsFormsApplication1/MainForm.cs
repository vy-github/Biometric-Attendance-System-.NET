using System;
using MANTRA;
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
    public partial class MainForm : Form
    {
        int mov, movx, movy;
        public MainForm()
        {
            InitializeComponent();
            //MonthAndYearSetting();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //string getDay = DateTime.Today.Day.ToString();
            string getMonth = DateTime.Today.Month.ToString();
            string getYear = DateTime.Now.Year.ToString();
            string getMonthFromDB = "";
            string getYearFromDB = "";
            //string storeMonthYear = getMonth + "-" + DateTime.Now.Year.ToString();

            conn.Open();
            OleDbCommand cmd1 = conn.CreateCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "SELECT * FROM OpeningDateUpdate";
            cmd1.ExecuteNonQuery();
            OleDbDataReader reader = cmd1.ExecuteReader();
            if(reader.Read())
            {
                getMonthFromDB = reader["updateMonth"].ToString();
                getYearFromDB = reader["updateYear"].ToString();
                reader.Close();
            }
            cmd1.CommandText = "UPDATE OpeningDateUpdate SET updateMonth = '" + getMonth + "', updateYear = '" + getYear + "'";
            cmd1.ExecuteNonQuery();
            conn.Close();


            if (getMonthFromDB != getMonth || getMonthFromDB == getMonth && getYearFromDB != getYear)
            {
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE Attendance SET day1 = 0,day2 = 0,day3 = 0,day4 = 0,day5 = 0,day6 = 0,day7 = 0,day8 = 0,day9 = 0,day10 = 0,day11 = 0,day12 = 0,day13 = 0,day14 = 0,day15 = 0,day16 = 0,day17 = 0,day18 = 0,day19 = 0,day20 = 0,day21 = 0,day22 = 0,day23 = 0,day24 = 0,day25 = 0,day26 = 0,day27 = 0,day28 = 0,day29 = 0,day30 = 0,day31 = 0, preMonth = curMonth, ereMonth = preMonth";
                cmd.ExecuteNonQuery();
                if (getYearFromDB != getYear)
                {
                    cmd.CommandText = "UPDATE Attendance SET Quarter1 = 0, Quarter2 = 0, Quarter3 = 0, Quarter4 = 0, preYear = curYear";
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

            toolTip1.SetToolTip(pictureBox1, "Home");
            toolTip1.SetToolTip(label8, "Close");
            toolTip1.SetToolTip(label9, "Minimise");

            label1.Parent = label3;
            label2.Parent = panel1;
            label4.Parent = pictureBox1;

            //label5.Parent = panel2;
            //label6.Parent = panel2;
            //label7.Parent = panel2;
            //label10.Parent = panel2;
                        
            //attendanceForm1.Parent = panel2;
            //deviceInfo1.Parent = panel2;


            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;

            homePage1.BackColor = Color.Transparent;
            registration1.BackColor = Color.Transparent;
            attendanceForm1.BackColor = Color.Transparent;
            deviceInfor1.BackColor = Color.Transparent;
            aboutUs1.BackColor = Color.Transparent;

            //label5.BackColor = Color.Transparent;
            //label6.BackColor = Color.Transparent;
            //label7.BackColor = Color.Transparent;
            //label10.BackColor = Color.Transparent;
            //progressBar1.BackColor = Color.Transparent;
            //panel2.BackColor = Color.Transparent;

            //attendanceForm1.BackColor = Color.Transparent;
            //deviceInfo1.BackColor = Color.Transparent;

            pictureBox1_Click(sender, e);
        }

        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Biometric Attendance Project New\FingerprintAtendance\WindowsFormsApplication1\Database1.accdb");
        void MonthAndYearSetting()
        {
            
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movx = e.X+230;
            movy = e.Y;
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mov == 1)
                this.SetDesktopLocation(MousePosition.X - movx, MousePosition.Y - movy);
        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movx = e.X;
            movy = e.Y;
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            if (mov == 1)
                this.SetDesktopLocation(MousePosition.X - movx, MousePosition.Y - movy);
        }

        private void label2_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        private void label4_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movx = e.X+60;
            movy = e.Y+13;
        }

        private void label4_MouseMove(object sender, MouseEventArgs e)
        {
            if (mov == 1)
                this.SetDesktopLocation(MousePosition.X - movx, MousePosition.Y - movy);
        }

        private void label4_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        private void label8_MouseMove(object sender, MouseEventArgs e)
        {
            label8.BackColor = Color.FromArgb(232, 26, 40);
            //label9.BackColor = Color.FromArgb(250, 251, 253); ;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Close();
            uninitDevice();
        }

        private void label9_MouseMove(object sender, MouseEventArgs e)
        {
            //label8.BackColor = Color.FromArgb(250, 251, 253); 
            label9.BackColor = SystemColors.ControlDark;
        }

        private void label9_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            uninitDevice();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            registration1.BringToFront();
            buttonEffect(1, 0, 0, 0, 0);
            registration1.Invoke((MethodInvoker)delegate { registration1.getIDandNAME(); });
            //attendanceForm1.Focus();
            //backImg(0);
        }

        public void button2_Click(object sender, EventArgs e)
        {
            //attendanceForm1.Visible = true;
            //backImg(0);

            //attendanceForm1.BringToFront();
            //buttonEffect(0, 1, 0, 0, 0);

            MainDummyClass.dumForReg = 1;
            frmTransprent ft = new frmTransprent();
            ft.Show();
        }

        //public void pinForm()
        //{
        //    attendanceForm1.BringToFront();
        //    buttonEffect(0, 1, 0, 0, 0);
        //}

        public static int dumRegi1 = 0, dumDevInfo = 0, dumRegi2 = 0,dumAtte=0;
        public static void PinForm()
        {
            //attendanceForm1.BringToFront();
            //buttonEffect(0, 1, 0, 0, 0);
            dumRegi1 = 1;
        }

        public static void statusCall()
        {
            dumDevInfo = 1;
        }

        public static void regiCall()
        {
            dumRegi2 = 1;
        }

        public static void atteCall()
        {
            dumAtte = 1;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (dumRegi1 == 1)
            {
                attendanceForm1.Invoke((MethodInvoker)delegate { attendanceForm1.submitSetting(); });
                attendanceForm1.BringToFront();
                buttonEffect(0, 1, 0, 0, 0);
                dumRegi1 = 0;
            }
            if (dumDevInfo == 1)
            {
                deviceInfor1.BringToFront();
                buttonEffect(0, 0, 0, 1, 0);
                dumDevInfo = 0;
            }
            if (dumRegi2 == 1)
            {
                attendanceForm1.Invoke((MethodInvoker)delegate { attendanceForm1.editSetting(); });
                attendanceForm1.BringToFront();
                buttonEffect(0, 1, 0, 0, 0);
                dumRegi2 = 0;
            }
            if (dumAtte == 1)
            {
                dumAtte = 0;
                if (Registration.dumManualAtte == 1)
                {
                    conn.Open();
                    OleDbCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    if (Registration.dumGetID != 0)
                        cmd.CommandText = "SELECT * FROM Attendance WHERE id = " + Registration.dumGetID;

                    if (Registration.dumGetName != "")
                        cmd.CommandText = "SELECT * FROM Attendance WHERE name = '" + Registration.dumGetName + "'";

                    cmd.ExecuteNonQuery();
                    OleDbDataReader reader = cmd.ExecuteReader();

                    string day = "day" + DateTime.Today.Day.ToString();
                    string getMonthForQuarter = DateTime.Today.Month.ToString();
                    string MonthForQuarter = "";
                    int QuarterInt = 0;
                    string dumTemGetName = "";
                    string dumTemGetID = "";

                    if (reader.Read())
                    {
                        string dayStr = reader[day].ToString();
                        dumTemGetName = reader["name"].ToString();
                        dumTemGetID = reader["id"].ToString();

                        if (dayStr == "1")
                        {
                            MessageBox.Show(dumTemGetName + " already attended", "Attendance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            registration1.Invoke((MethodInvoker)delegate { registration1.SetIDAndName(dumTemGetID, dumTemGetName); });
                            reader.Close();
                            conn.Close();
                            return;
                        }

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
                        reader.Close();
                    }

                    if (Registration.dumGetID != 0)
                    {
                        cmd.CommandText = "UPDATE Attendance SET " + day + " = 1," + MonthForQuarter + " = " + QuarterInt.ToString() + " WHERE id=" + Registration.dumGetID + "";
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Welcome " + dumTemGetName, "Attendance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Registration.dumGetID = 0;

                        registration1.Invoke((MethodInvoker)delegate { registration1.SetIDAndName(dumTemGetID, dumTemGetName); });
                    }

                    if (Registration.dumGetName != "")
                    {
                        cmd.CommandText = "UPDATE Attendance SET " + day + " = 1," + MonthForQuarter + " = " + QuarterInt.ToString() + " WHERE name='" + Registration.dumGetName + "'";
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Welcome " + Registration.dumGetName, "Attendance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Registration.dumGetName = "";

                        registration1.Invoke((MethodInvoker)delegate { registration1.SetIDAndName(dumTemGetID, dumTemGetName); });
                    }
                    conn.Close();

                    Registration.dumManualAtte = 0;
                }
                else
                {
                    registration1.BringToFront();
                    buttonEffect(1, 0, 0, 0, 0);
                }
            }
        }

        public static int dumForRep = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            buttonEffect(0, 0, 1, 0, 0);
            ReportForm rf = new ReportForm();
            if (MainForm.dumForRep == 1)
            {
                rf.Maximize(rf);
                return;
            }
            rf.Show();
            MainForm.dumForRep = 1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            deviceInfor1.BringToFront();
            buttonEffect(0, 0, 0, 1, 0);
            deviceInfor1.Invoke((MethodInvoker)delegate { deviceInfor1.checkConnection(); });
            //backImg(1);
        }

        private void button5_Click(object sender, EventArgs e)
        { 
            //deviceInfo1.Visible = true;
            aboutUs1.BringToFront();
            buttonEffect(0, 0, 0, 0, 1);
            //deviceInfo1.BringToFront();
            //backImg(1);
            
            aboutUs1.Invoke((MethodInvoker)delegate { aboutUs1.aboutTimerSet(); });
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            getTodayPercentage();
            //timer1.Start();
            //num = 0;
            this.ProgressUpgrade(num, attend, Total);
            attend = 0;
            Total = 0;
            homePage1.BringToFront();
            buttonEffect(0, 0, 0, 0, 0);
            //registration1.Visible = false;
            //attendanceForm1.Visible = false;
            //deviceInfo1.Visible = false;

            //backImg(0);
        }

        public void ProgressUpgrade(object progress,int attend,int Total)
        {
            homePage1.Invoke((MethodInvoker)delegate { homePage1.pb_progress_bar.UpdateProgress(Convert.ToInt32(progress), attend, Total); });
        }

        int num = 0, attend = 0, Total = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //num = num + 1;
            //this.ProgressUpgrade(num);
            //if (num > 99)
            //{
            //    timer1.Stop();
            //    //num = 0;
            //}
        }
        
        void getTodayPercentage()
        {
            string day = "day" + DateTime.Today.Day.ToString();

            conn.Open();
            OleDbCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT " + day + " FROM Attendance";
            cmd.ExecuteNonQuery();
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int a = Convert.ToInt32(reader[day]);
                if (a == 1)
                    attend++;
                Total++;
            }
            conn.Close();
        }

        private void label9_MouseLeave(object sender, EventArgs e)
        {
            moveBack();
        }

        private void label8_MouseLeave(object sender, EventArgs e)
        {
            moveBack();
        }

        public void moveBack()
        {
            label8.BackColor = Color.FromArgb(250, 251, 253);
            label9.BackColor = Color.FromArgb(250, 251, 253);
        }

        //void backImg(int a)
        //{
        //    if(a==1)
        //        this.BackgroundImage=new Bitmap(@"D:\Biometric Attendance Project New\Background\asdf.png");
        //    else
        //        this.BackgroundImage = new Bitmap(@"D:\Biometric Attendance Project New\Background\back.jpg");
        //}

        void uninitDevice()
        {
            MFS100 mfs100 = new MFS100();
            try
            {
                int ret = mfs100.Uninit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        
        void buttonEffect(int a,int b,int c,int d,int e)
        {
            if(a==1)
            {
                button1.BackColor = Color.FromArgb(240, 244, 247);
                button1.ForeColor = Color.Black;
                button1.Image = new Bitmap(@"D:\Biometric Attendance Project New\fingerprint-with-crosshair-focus.png");
            }
            else
            {
                button1.BackColor = Color.FromArgb(64, 64, 64);
                button1.ForeColor = Color.White;
                button1.Image = new Bitmap(@"D:\Biometric Attendance Project New\fingerprintwhite1.png");
            }

            if (b == 1)
            {
                button2.BackColor = Color.FromArgb(246, 247, 251);
                button2.ForeColor = Color.Black;
                button2.Image = new Bitmap(@"D:\Biometric Attendance Project New\resume.png");
            }
            else
            {
                button2.BackColor = Color.FromArgb(64, 64, 64);
                button2.ForeColor = Color.White;
                button2.Image = new Bitmap(@"D:\Biometric Attendance Project New\\Icon\resume.png");
            }

            if (c == 1)
            {
                button3.BackColor = Color.FromArgb(244,248,251);
                button3.ForeColor = Color.Black;
                button3.Image = new Bitmap(@"D:\Biometric Attendance Project New\medical-history.png");
            }
            else
            {
                button3.BackColor = Color.FromArgb(64, 64, 64);
                button3.ForeColor = Color.White;
                button3.Image = new Bitmap(@"D:\Biometric Attendance Project New\Icon\report.png");
            }

            if (d == 1)
            {
                button4.BackColor = Color.FromArgb(248,249,251);
                button4.ForeColor = Color.Black;
                button4.Image = new Bitmap(@"D:\Biometric Attendance Project New\smartwatch.png");
            }
            else
            {
                button4.BackColor = Color.FromArgb(64, 64, 64);
                button4.ForeColor = Color.White;
                button4.Image = new Bitmap(@"D:\Biometric Attendance Project New\Icon\smartwatch.png");
            }

            if (e == 1)
            {
                button5.BackColor = Color.FromArgb(255,255,255);
                button5.ForeColor = Color.Black;
                button5.Image = new Bitmap(@"D:\Biometric Attendance Project New\about-us (1).png");
            }
            else
            {
                button5.BackColor = Color.FromArgb(64, 64, 64);
                button5.ForeColor = Color.White;
                button5.Image = new Bitmap(@"D:\Biometric Attendance Project New\Icon\about-us (4).png");
            }
        }
    }
}

public static class MainDummyClass
{
    public static int dumForReg = 0;
}