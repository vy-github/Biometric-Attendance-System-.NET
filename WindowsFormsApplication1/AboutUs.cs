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
    public partial class AboutUs : UserControl
    {
        public AboutUs()
        {
            InitializeComponent();
        }

        private void AboutUs_Load(object sender, EventArgs e)
        {
            left = label6.Location.X;
        }

        int left = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            left = left - 1;
            label6.SetBounds(left, label6.Location.Y, label6.Width, label6.Height);
            if (left == -108)
                left = 132;
        }

        int progressX = -181, dum = 1;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (dum == 1 && progressX <= 181)
            {
                this.progressX = this.progressX + 1;
                label10.SetBounds(progressX, label10.Location.Y, label10.Width, label10.Height);
                if (progressX == 181)
                    dum = 0;
            }
            else
            {
                this.progressX = this.progressX - 1;
                label10.SetBounds(progressX, label10.Location.Y, label10.Width, label10.Height);
                if (progressX == -181)
                    dum = 1;
            }
        }

        public void aboutTimerSet()
        {
            left = 14;
            progressX = -181;
            dum = 1;
        }
    }
}
