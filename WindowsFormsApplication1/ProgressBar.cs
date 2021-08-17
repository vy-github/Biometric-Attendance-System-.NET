using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class ProgressBar : UserControl
    {
        int progress;
        public ProgressBar()
        {
            progress = 0;
            InitializeComponent();
        }

        private void ProgressBar_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(this.Width / 2, this.Height / 2);
            e.Graphics.RotateTransform(-90);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen obj_pen = new Pen(Color.FromArgb(137,61,137));
            Rectangle rect1 = new Rectangle(0 - this.Width / 2 + 20, 0 - this.Height / 2 + 20, this.Width - 40, this.Height - 40);
            e.Graphics.DrawPie(obj_pen, rect1, 0, (int)(this.progress*3.6));//360/100=3.6
            e.Graphics.FillPie(new SolidBrush(Color.FromArgb(250, 202, 190)), rect1, 0, (int)(this.progress * 3.6));

            obj_pen = new Pen(Color.FromArgb(110, 110, 110));
            rect1 = new Rectangle(0 - this.Width / 2 + 30, 0 - this.Height / 2 + 30, this.Width - 60, this.Height - 60);
            e.Graphics.DrawPie(obj_pen, rect1, 0, 360);
            e.Graphics.FillPie(new SolidBrush(Color.FromArgb(100,110,110)), rect1, 0, 360);
            e.Graphics.RotateTransform(90);
            StringFormat ft = new StringFormat();
            ft.LineAlignment = StringAlignment.Center;
            ft.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(this.progress.ToString() + "%", new Font("Comic Sans MS", 20), new SolidBrush(Color.FromArgb(250, 202, 190)), rect1, ft);
        }

        double attend=0, Total=0, perCount=0;
        public void UpdateProgress(int progress,double attend,double Total)
        {
            this.attend = attend;
            this.Total = Total;
            perCount = this.attend / this.Total * 100;
            if (this.attend != 0)
            {
                i = 0;
                timer1.Start();
                this.progress = progress;
                this.Invalidate();
            }

            toolTip1.SetToolTip(this, "Today's attendance " + this.attend + " out of " + this.Total);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Stop")
            {
                timer1.Stop();
                button1.Text = "Start";
            }
            else
            {
                timer1.Start();
                button1.Text = "Stop";
            }
        }

        int i = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.progress = this.progress + 1;
            this.Invalidate();
            i = i + 1;
            if (i == Math.Round(perCount))
                timer1.Stop();
        }
    }
}
