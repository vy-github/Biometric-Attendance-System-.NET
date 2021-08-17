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
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label7.Text = "Date : " + DateTime.Now.ToString("dd-MM-yyyy");
            label10.Text = "Time : " + DateTime.Now.ToString("hh-mm-ss tt");
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            //toolTip1.SetToolTip(pb_progress_bar, "Today's attendance 80 out of 100");
            toolTip1.SetToolTip(this, "Home page");
        }
    }
}
