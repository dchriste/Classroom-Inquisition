using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SrP_ClassroomInq
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        byte i = 0;
        byte k = 0;
        byte j = 0;
        bool textbox1WASclicked = new bool();
        bool grpbxRPL_WASclicked = new bool();
        bool btnCLS_WASclicked = new bool();

        #region Click Events
        
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Leaving so soon?";

            string caption = "Are you Sure you want to do that...?";

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show(this, message, caption, buttons, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            timer.Enabled = true;
            textbox1WASclicked = true;
            
        }

        private void trayICON_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                this.Hide();
                trayICON.ShowBalloonTip(500);
                this.WindowState = FormWindowState.Minimized;
            }

        }

        #endregion Click Events

        private void timer_Tick(object sender, EventArgs e)
        {
            #region Send Button
            if ((btnSend.Visible == false) && (textbox1WASclicked == true))// if not visible
            {
                if (i < 3)
                {
                    grpbxFeed.SetBounds(grpbxFeed.Location.X, grpbxFeed.Location.Y + 5, grpbxFeed.Size.Width, grpbxFeed.Size.Height - 2);
                    i++;
                }
                else if ((i > 2)&&(i < 9))
                {
                    grpbxFeed.SetBounds(grpbxFeed.Location.X, grpbxFeed.Location.Y + 4, grpbxFeed.Size.Width, grpbxFeed.Size.Height -3);
                    i++;
                }
                else
                {
                    i = 0;
                    timer.Enabled = false;
                    btnSend.Visible = true;
                    textbox1WASclicked = false;
                }
            }
            #endregion

            #region GrpBxRPL_Clicked
            if ((grpbx_Reply.Height < 137) && (grpbxRPL_WASclicked == true))// if not visible
            {
                if (k < 3)
                {
                    grpbx_Reply.SetBounds(grpbx_Reply.Location.X, grpbx_Reply.Location.Y, grpbx_Reply.Size.Width, grpbx_Reply.Size.Height + 3);
                    k++;
                }
                else if ((k > 2) && (k < 19))
                {
                    grpbx_Reply.SetBounds(grpbx_Reply.Location.X, grpbx_Reply.Location.Y, grpbx_Reply.Size.Width, grpbx_Reply.Size.Height + 6);
                    k++;
                }
                else
                {
                    k = 0;
                    timer.Enabled = false;
                    grpbxRPL_WASclicked = false;
                }
                textBox1.AppendText(grpbx_Reply.Height.ToString() + Environment.NewLine); //troubleshooting
            }
           
            #endregion

            #region Destroy Reply
            if ((grpbx_Reply.Height > 32) && (btnCLS_WASclicked == true))// if not visible
            {
                if (j < 3)
                {
                    grpbx_Reply.SetBounds(grpbx_Reply.Location.X, grpbx_Reply.Location.Y, grpbx_Reply.Size.Width, grpbx_Reply.Size.Height - 3);
                    j++;
                }
                else if ((j > 2) && (j < 19))
                {
                    grpbx_Reply.SetBounds(grpbx_Reply.Location.X, grpbx_Reply.Location.Y, grpbx_Reply.Size.Width, grpbx_Reply.Size.Height - 6);
                    j++;
                }
                else
                {
                    j = 0;
                    timer.Enabled = false;
                    btnCLS_WASclicked = false;
                }
                textBox1.AppendText(grpbx_Reply.Height.ToString() + Environment.NewLine); //troubleshooting
            }
            #endregion

        }

        private void btnCLR_Click(object sender, EventArgs e)
        {
            txtbx_Reply.ResetText(); //clear text
        }

        private void lbl_question_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            grpbxRPL_WASclicked = true; 
        }

        private void replyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            grpbxRPL_WASclicked = true; 
        }

        private void btnCLS_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            btnCLS_WASclicked = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

       



    }
}
