using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MANTRA;

namespace WindowsFormsApplication1
{
    public partial class DeviceInfor : UserControl
    {
        public DeviceInfor()
        {
            InitializeComponent();
        }

        MFS100 mfs100 = new MFS100();
        public void checkConnection()
        {
            try
            {
                if (mfs100.IsConnected())
                {
                    label1.Text = "Device name :- Mantra";
                    label3.Text = ":-   MFS100\n:-   1128272\n:-   316\n:-   354\n:-   STQC.PIV";
                    label4.Text = "Version      :-   9025";
                    label5.Text = "Status        :-   Connected";
                }
                else
                {
                    label1.Text = "Device name :- null";
                    label3.Text = ":-   null\n:-   null\n:-   null\n:-   null\n:-   null";
                    label4.Text = "Version      :-   null";
                    label5.Text = "Status        :-    null";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
