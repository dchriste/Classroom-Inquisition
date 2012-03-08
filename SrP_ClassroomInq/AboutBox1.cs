using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SrP_ClassroomInq
{
    public partial class AboutBox_CI : Form
    {
        public AboutBox_CI()
        {
            InitializeComponent();
        }

        private void AboutBox_CI_Load(object sender, EventArgs e)
        {
            //Version version = Assembly.GetExecutingAssembly().GetName().Version;
            //labelVersion.Text = version.ToString();

            labelVersion.BackColor = this.BackColor;
            labelVersion.ForeColor = this.ForeColor;            
        }


    }
}
