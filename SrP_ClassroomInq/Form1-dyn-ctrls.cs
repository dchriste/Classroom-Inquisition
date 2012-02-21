using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace SrP_ClassroomInq
{
	public partial class Form1 : Form
	{

        
        public Form1()
		{
			InitializeComponent();
            textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys); // so you can send with enter in the main txtbx
            PanelStudents.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            PanelFAQ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            PanelPrefs.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            DirectMsgPanel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
		}
        #region Initialization

        string[] Students_Name = new string[classSize];
        string[] Students_ID = new string[classSize];
        string[] Qs_to_create = new string[classSize];
        bool Qs_to_Make = false;//flag to create controls
        string[] addr_tbl = {"\x20","\x02","\x03","\x1F","\x05","\x06","\x07","\x22","\x09","\x21",
                           "\x0B","\x0C","\x0D","\x0E","\x0F","\x10","\x11","\x12","\x13","\x14",
                           "\x15","\x16","\x17","\x18","\x19","\x1A","\x1B","\x1C","\x1D","\x1E",
                            };
        bool[] Q_status = new bool[classSize]; //records the read status of a question
        bool[] Q_Replied = new bool[classSize]; //records if the question has been replied to
        string[] Q_sender = new string[classSize]; // records the name of the sender of each question
        string brdcst_addr = "\x01";
        string[] logFiles = new string[classSize];
        string EncryptedData = @"StudentInfo_Encrypted.txt";
        string PlainData = @"StudentInfo.txt";
        string[] portNames = new string[10];
        string tempString = "";
        string tempString2 = "";
        string RX_Data = "";
        string SecretKey;
        SoundPlayer JukeBox = new System.Media.SoundPlayer(); //for alert tone
        bool WeGotData = false;
		public const int classSize = 50;  //will be max questions handled at once
		byte i = 0;
		byte k = 0;
		byte j = 0;
        byte x = 0;
        byte iAck = 0;
        int ix = 0; //only for serial tick timer...
        int jx = 0; //only for the make controls timer
        string addr = "\xFF";
        byte NumQuestions = 0;
        byte UnreadCount = 0;
        byte timesClicked = 0; //allows for double click of question
        byte DMtimesClicked = 0;
        byte PrefsTimesClicked = 0;
        byte FAQTimesClicked = 0;
        byte StuMgmtTimesClicked = 0;
        bool DesirePrefs = new bool();
        bool DesireDM = new bool();
        bool DesireBrdcst = new bool();
        bool FAQClicked = new bool();
        bool FAQShowing = new bool();
        bool StuMgmtClicked = new bool();
        bool StuMgmtShowing = new bool();
        bool DMclicked = new bool();
        bool DMPanelShowing = new bool();
        bool PrefsShowing = new bool();
        bool PrefsClicked = new bool();
		bool textbox1WASclicked = new bool();
		bool grpbxRPL_WASclicked = new bool();
		bool btnCLS_WASclicked = new bool();
        bool NEW_grpbx = new bool();
        bool AckRXd = false;
        bool timeout_ack = false;
        int old_lblID = 0;
        bool DesireID = false;
        int new_lblID = 0;
		int lbl_ID = 0;
        int lbl_ID_2 = 0;// for unread counter

        #region Dyn-Ctrls-Init_DoNotChange
        System.Windows.Forms.GroupBox[] group_arr = new System.Windows.Forms.GroupBox[classSize]; //add const for max_lns
		System.Windows.Forms.Button[] reply_arr = new System.Windows.Forms.Button[classSize]; //add const for max_lns
		System.Windows.Forms.Button[] close_arr = new System.Windows.Forms.Button[classSize]; //add const for max_lns
		System.Windows.Forms.Button[] clear_arr = new System.Windows.Forms.Button[classSize]; //add const for max_lns
		System.Windows.Forms.TextBox[] txtbx_reply_arr = new System.Windows.Forms.TextBox[classSize]; //add const for max_lns
		System.Windows.Forms.Label[] lbl_arr = new System.Windows.Forms.Label[classSize]; //add const for max_lns
        System.Windows.Forms.PictureBox[] picbx_arr = new System.Windows.Forms.PictureBox[classSize];
        System.Drawing.Point origingrouparr = new System.Drawing.Point(6, -13); //originally 6,19 but this gives questions an entering animation
        System.Drawing.Point tempgrouparr = new System.Drawing.Point(6, 19);
        System.Drawing.Point tempreplyarr = new System.Drawing.Point(230, 98);
        System.Drawing.Point tempclosearr = new System.Drawing.Point(129, 98);
        System.Drawing.Point tempcleararr = new System.Drawing.Point(23, 98);
        System.Drawing.Point txtbx_reply_temp = new System.Drawing.Point(23, 32);
        System.Drawing.Point lbl_arr_temp = new System.Drawing.Point(0, -13);
        System.Drawing.Point picbx_arr_tmp = new System.Drawing.Point(2, 9);
        #endregion

        #endregion
        
        public void MoveCtrlsDown()
        {
            //move grpbxs down if new question received to implement LIFO
                NEW_grpbx = true;
                timer.Enabled = true;            
        }

        public void MakeCtrls(string question){
            reply_arr[NumQuestions] = new System.Windows.Forms.Button();
            reply_arr[NumQuestions].BackColor = System.Drawing.Color.Black;
            reply_arr[NumQuestions].ForeColor = System.Drawing.Color.DarkGoldenrod;
            reply_arr[NumQuestions].FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            reply_arr[NumQuestions].Location = tempreplyarr;
            reply_arr[NumQuestions].Name = "reply_arr_" + NumQuestions.ToString();
            reply_arr[NumQuestions].Size = new System.Drawing.Size(67, 25);
            reply_arr[NumQuestions].TabIndex = 2;
            reply_arr[NumQuestions].Text = "Repl&y";
            reply_arr[NumQuestions].UseVisualStyleBackColor = true;
            reply_arr[NumQuestions].Click += new System.EventHandler(this.btnRepl_Click);
            //reply_arr[NumQuestions].KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys2);

            close_arr[NumQuestions] = new Button();
            close_arr[NumQuestions].BackColor = System.Drawing.Color.Black;
            close_arr[NumQuestions].ForeColor = System.Drawing.Color.DarkGoldenrod;
            close_arr[NumQuestions].FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            close_arr[NumQuestions].Location = tempclosearr;
            close_arr[NumQuestions].Name = "close_arr_" + NumQuestions.ToString();
            close_arr[NumQuestions].Size = new System.Drawing.Size(67, 25);
            close_arr[NumQuestions].TabIndex = 3;
            close_arr[NumQuestions].Text = "Clos&e";
            close_arr[NumQuestions].UseVisualStyleBackColor = true;
            close_arr[NumQuestions].Click += new System.EventHandler(this.btnCLS_Click);

            clear_arr[NumQuestions] = new Button();
            clear_arr[NumQuestions].BackColor = System.Drawing.Color.Black;
            clear_arr[NumQuestions].ForeColor = System.Drawing.Color.DarkGoldenrod;
            clear_arr[NumQuestions].FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            clear_arr[NumQuestions].Location = tempcleararr;
            clear_arr[NumQuestions].Name = "clear_arr_" + NumQuestions.ToString();
            clear_arr[NumQuestions].Size = new System.Drawing.Size(67, 25);
            clear_arr[NumQuestions].TabIndex = 4;
            clear_arr[NumQuestions].Text = "Clea&r";
            clear_arr[NumQuestions].UseVisualStyleBackColor = true;
            clear_arr[NumQuestions].Click += new System.EventHandler(this.btnCLR_Click);

            txtbx_reply_arr[NumQuestions] = new TextBox();
            txtbx_reply_arr[NumQuestions].AcceptsReturn = false;
            txtbx_reply_arr[NumQuestions].Location = txtbx_reply_temp;
            txtbx_reply_arr[NumQuestions].Multiline = true;
            txtbx_reply_arr[NumQuestions].Name = " txtbx_reply_arr_" + NumQuestions.ToString();
            txtbx_reply_arr[NumQuestions].Size = new System.Drawing.Size(274, 51);
            txtbx_reply_arr[NumQuestions].TabIndex = 1;
            txtbx_reply_arr[NumQuestions].KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);

            group_arr[NumQuestions] = new System.Windows.Forms.GroupBox();  // Instantiate next index of groupbox array
            group_arr[NumQuestions].BackColor = System.Drawing.Color.Black;
            group_arr[NumQuestions].ForeColor = System.Drawing.SystemColors.HighlightText;
            group_arr[NumQuestions].Location = origingrouparr; //insert new question at the top
            group_arr[NumQuestions].Name = "group_arr_" + NumQuestions.ToString();
            group_arr[NumQuestions].Size = new System.Drawing.Size(328, 32);
            group_arr[NumQuestions].TabIndex = 2;
            group_arr[NumQuestions].TabStop = false;

            lbl_arr[NumQuestions] = new Label();
            lbl_arr[NumQuestions].AutoSize = false;
            lbl_arr[NumQuestions].AutoEllipsis = true;
            lbl_arr[NumQuestions].TextAlign = System.Drawing.ContentAlignment.TopCenter;
            lbl_arr[NumQuestions].BackColor = System.Drawing.Color.Black;
            lbl_arr[NumQuestions].ForeColor = System.Drawing.Color.DarkGoldenrod;
            lbl_arr[NumQuestions].FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            lbl_arr[NumQuestions].Dock = DockStyle.Top;
            lbl_arr[NumQuestions].Location = lbl_arr_temp;
            lbl_arr[NumQuestions].Name = "lbl_arr_" + NumQuestions.ToString(); 
            lbl_arr[NumQuestions].Size = new System.Drawing.Size(330, 15);
            lbl_arr[NumQuestions].TabIndex = 0;            
            lbl_arr[NumQuestions].Text = "***" + "          " + question + "  -" + tempString2; //passed to this function by the sender, eventually add the name of student here            
            lbl_arr[NumQuestions].Click += new System.EventHandler(this.lbl_question_Click);
            lbl_arr[NumQuestions].MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbl_question_MouseMove);

            picbx_arr[NumQuestions] = new PictureBox();
            picbx_arr[NumQuestions].Image = global::SrP_ClassroomInq.Properties.Resources.reply_arrow;
            picbx_arr[NumQuestions].Location = picbx_arr_tmp;
            picbx_arr[NumQuestions].Name = "picbx_arr_" + NumQuestions.ToString();
            picbx_arr[NumQuestions].Size = new System.Drawing.Size(24, 20); //half the actual image size
            picbx_arr[NumQuestions].SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            picbx_arr[NumQuestions].TabIndex = 0;
            picbx_arr[NumQuestions].TabStop = false;
            picbx_arr[NumQuestions].Visible = false; //set to visible after marked read
           
            group_arr[NumQuestions].Controls.Add(reply_arr[NumQuestions]);
            group_arr[NumQuestions].Controls.Add(close_arr[NumQuestions]);
            group_arr[NumQuestions].Controls.Add(clear_arr[NumQuestions]);
            group_arr[NumQuestions].Controls.Add(txtbx_reply_arr[NumQuestions]);
            group_arr[NumQuestions].Controls.Add(lbl_arr[NumQuestions]);
            group_arr[NumQuestions].Controls.Add(picbx_arr[NumQuestions]);
            MoveCtrlsDown(); //move ctrls for lifo operation
            grpbxFeed.Controls.Add(group_arr[NumQuestions]);

            if (chkbxRXSound.Checked) //eventually have preference here
            {
                JukeBox.SoundLocation = @"blip.wav"; // this is in [projdir]/bin/debug/blip.wav
                JukeBox.Play();
            }
            if (UnreadCount > 0)
            {
                picbxStatus.Visible = false;
                tlstrplbl_Unread.Text = "Unread " + UnreadCount.ToString();
                tlstrplbl_Unread.Visible = true;
                 //Form1.Text = "Classroom Inquisition | Unread " + UnreadCount.ToString();
            }
            else
            {
                picbxStatus.Visible = true;
                tlstrplbl_Unread.Visible = false;
            }

            NumQuestions++; //increment number of ctrls
        }

        public string HandleBCKSPC(string RAW_DATA) // this works as of 10-25-11
        {
            string tmpString = RAW_DATA;
            string finalString = "";
            string[] strARRAY = new string[RAW_DATA.Length];
            char tmp=' ';
            char tmp_response=' ';
            strARRAY = tmpString.Split('\b');

            foreach (string str in strARRAY){
                if (finalString.Length > 1)
                {
                    finalString = finalString.Substring(0, (finalString.Length - 1)) + str; //remove char to delete and concat strings
                }
                else if (finalString == "")
                {
                    finalString = str; // initial value
                }
                else
                {
                    finalString += str.Substring(0, (str.Length -1)); // if you fudge and backspace the second letter you type
                }
            }

            tmp = finalString[1]; //this should be the address
            tmp_response = finalString[2]; //if this is a flowcontrol string otherwise this would be the first char of the message
            finalString = finalString.TrimStart('\x02'); //clean up rx'd string start of transmit char
            finalString = finalString.TrimEnd('\x03'); //trim end of transmit

            if ((tmp_response != 0x05) && (tmp_response != 0x06)) //if not enq or ack
            {
                Q_sender[NumQuestions] = tmp.ToString().TrimStart('\x27').TrimEnd('\x27'); //the trims are to fix the mess ToString makes of the unprintable char
                finalString = finalString.TrimStart(tmp); //trim off the address

                for (int i = 0; i < Students_ID.Length; i++)
                {
                    if (String.CompareOrdinal(addr_tbl[i], Q_sender[NumQuestions]) == 0) //CompareOrdinal Compares Numerical value
                    {
                        tempString2 = Students_Name[i];
                        break;
                    }
                }
                return finalString;
            }
            else // the string received is part of the flow control
            {
                if (tmp_response == 0x05) //enq has been received
                {
                    FlowCtrl_Send(tmp.ToString().TrimStart('\x27').TrimEnd('\x27'), "ack"); //ack the enq
                }
                else //ack is received
                {
                    AckRXd = true; //proceed with the sending
                }
                return "";
            }
        }
        
        private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)

        {
            if (e.KeyChar == (char)13) //enter key in windows
            {
                if (textBox1.Focused) //then it's the sender
                {
                    if ((textBox1.Text != "") && (textBox1.Text != Environment.NewLine))
                    {
                        if (SendMsg(textBox1.Text, brdcst_addr))
                        {
                            textBox1.Clear();
                            grpbxFeed.Focus(); //get cursor out of text box
                            textbox1WASclicked = true;
                            timer.Enabled = true; // hide the send button
                        }
                    }
                    else
                    {
                        MessageBox.Show("You Haven't typed a message  yet..");
                    }
                }
                else
                {
                    btnRepl_Click(sender, e);
                }
            }
            else if (e.KeyChar == (char)27) //escape key
            {
                if (StuMgmtShowing)
                {
                    StuMgmtClicked = true;
                    timer.Enabled = true;
                }                
                else if (FAQShowing)
                {
                    FAQClicked = true;
                    timer.Enabled = true;
                }
                else if (PrefsShowing)
                {
                    PrefsClicked = true;
                    timer.Enabled = true;
                }
                else if (DMPanelShowing)
                {
                    DMclicked = true;
                    timer.Enabled = true;
                }
            }
        }

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
            //grpbxRPL_WASclicked = false;
            textBox1.Clear();        
			
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
                    if (!chkbxLameMode.Checked) //animations
                    {
                        if (i < 3)
                        {
                            grpbxFeed.SetBounds(grpbxFeed.Location.X, grpbxFeed.Location.Y + 5, grpbxFeed.Size.Width, grpbxFeed.Size.Height - 2);
                            DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X, DirectMsgPanel.Location.Y + 5, DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height - 2);
                            PanelPrefs.SetBounds(PanelPrefs.Location.X, PanelPrefs.Location.Y + 5, PanelPrefs.Size.Width, PanelPrefs.Size.Height - 2);
                            i++;
                        }
                        else if ((i > 2) && (i < 9))
                        {
                            grpbxFeed.SetBounds(grpbxFeed.Location.X, grpbxFeed.Location.Y + 4, grpbxFeed.Size.Width, grpbxFeed.Size.Height - 3);
                            DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X, DirectMsgPanel.Location.Y + 4, DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height - 3);
                            PanelPrefs.SetBounds(PanelPrefs.Location.X, PanelPrefs.Location.Y + 4, PanelPrefs.Size.Width, PanelPrefs.Size.Height - 3);
                            i++;
                        }
                        else
                        {
                            i = 0;
                            timer.Enabled = false;
                            btnSend.Visible = true;
                            btnCLS.Visible = true;
                            serialCOMcmbbx.Visible = true;
                            textbox1WASclicked = false;
                        }
                    }
                    else //no animations
                    {
                        //jump straight to whatever position..
                        grpbxFeed.SetBounds(grpbxFeed.Location.X, grpbxFeed.Location.Y + 39, grpbxFeed.Size.Width, grpbxFeed.Size.Height - 24);
                        DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X, DirectMsgPanel.Location.Y + 39, DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height - 24);
                        PanelPrefs.SetBounds(PanelPrefs.Location.X, PanelPrefs.Location.Y + 39, PanelPrefs.Size.Width, PanelPrefs.Size.Height - 24);
                        timer.Enabled = false;
                        btnSend.Visible = true;
                        btnCLS.Visible = true;
                        serialCOMcmbbx.Visible = true;
                        textbox1WASclicked = false;
                    }
                }

                if ((btnSend.Visible == true) && (textbox1WASclicked == true))// if visible, destroy
                {
                    if (!chkbxLameMode.Checked)
                    {
                        if (i < 3)
                        {
                            grpbxFeed.SetBounds(grpbxFeed.Location.X, grpbxFeed.Location.Y - 5, grpbxFeed.Size.Width, grpbxFeed.Size.Height + 2);
                            DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X, DirectMsgPanel.Location.Y - 5, DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height + 2);
                            PanelPrefs.SetBounds(PanelPrefs.Location.X, PanelPrefs.Location.Y - 5, PanelPrefs.Size.Width, PanelPrefs.Size.Height + 2);
                            i++;
                        }
                        else if ((i > 2) && (i < 9))
                        {
                            grpbxFeed.SetBounds(grpbxFeed.Location.X, grpbxFeed.Location.Y - 4, grpbxFeed.Size.Width, grpbxFeed.Size.Height + 3);
                            DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X, DirectMsgPanel.Location.Y - 4, DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height + 3);
                            PanelPrefs.SetBounds(PanelPrefs.Location.X, PanelPrefs.Location.Y - 4, PanelPrefs.Size.Width, PanelPrefs.Size.Height + 3);
                            i++;
                        }
                        else
                        {
                            i = 0;
                            btnSend.Visible = false;
                            btnCLS.Visible = false;
                            serialCOMcmbbx.Visible = false;
                            timer.Enabled = false;
                            textbox1WASclicked = false;
                            grpbxFeed.Focus(); //get cursor out of textbox if it's there...
                        }
                    }
                    else //no animations
                    {
                        grpbxFeed.SetBounds(grpbxFeed.Location.X, grpbxFeed.Location.Y - 39, grpbxFeed.Size.Width, grpbxFeed.Size.Height + 24);
                        DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X, DirectMsgPanel.Location.Y - 39, DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height + 24);
                        PanelPrefs.SetBounds(PanelPrefs.Location.X, PanelPrefs.Location.Y - 39, PanelPrefs.Size.Width, PanelPrefs.Size.Height + 24);
                        btnSend.Visible = false;
                        btnCLS.Visible = false;
                        serialCOMcmbbx.Visible = false;
                        timer.Enabled = false;
                        textbox1WASclicked = false;
                        grpbxFeed.Focus(); //get cursor out of textbox if it's there...
                    }
                }
            
			#endregion

			#region GrpBxRPL_Clicked
			if(grpbxFeed.Controls.Count > 0) //allows textbox click with no questions
            {
                if ((group_arr[lbl_ID].Height < 141) && (grpbxRPL_WASclicked == true))// if not visible
                {
                    Point tmp = new Point();
                    if (!chkbxLameMode.Checked)
                    {
                        if (k < 3) //use groupboxID..
                        {
                            group_arr[lbl_ID].SetBounds(group_arr[lbl_ID].Location.X, group_arr[lbl_ID].Location.Y, group_arr[lbl_ID].Size.Width, group_arr[lbl_ID].Size.Height + 2);
                            if (lbl_ID != 0)
                            {
                                for (int i = lbl_ID - 1; i >= 0; i--) // to achieve LIFO
                                {
                                    tmp.X = group_arr[i].Location.X;
                                    tmp.Y = group_arr[i].Location.Y + 2;
                                    group_arr[i].Location = tmp;
                                }
                            }
                            k++;
                        }
                        else if ((k > 2) && (k < 36))
                        {
                            group_arr[lbl_ID].SetBounds(group_arr[lbl_ID].Location.X, group_arr[lbl_ID].Location.Y, group_arr[lbl_ID].Size.Width, group_arr[lbl_ID].Size.Height + 3);
                            //fire ctrl resize event
                            if (lbl_ID != 0)
                            {
                                for (int i = lbl_ID - 1; i >= 0; i--)// to achieve LIFO
                                {
                                    tmp.X = group_arr[i].Location.X;
                                    tmp.Y = group_arr[i].Location.Y + 3;
                                    group_arr[i].Location = tmp;
                                }

                            }
                            k++;
                        }
                        else
                        {
                            k = 0;
                            timer.Enabled = false;
                            grpbxRPL_WASclicked = false;
                            txtbx_reply_arr[lbl_ID].Focus(); //places cursor in question reply box on open
                        }
                    }
                    else //no animations
                    {
                        group_arr[lbl_ID].SetBounds(group_arr[lbl_ID].Location.X, group_arr[lbl_ID].Location.Y, group_arr[lbl_ID].Size.Width, group_arr[lbl_ID].Size.Height + 105);
                        if (lbl_ID != 0)
                        {
                            for (int i = lbl_ID - 1; i >= 0; i--)// to achieve LIFO
                            {
                                tmp.X = group_arr[i].Location.X;
                                tmp.Y = group_arr[i].Location.Y + 105;
                                group_arr[i].Location = tmp;
                            }
                        }
                        timer.Enabled = false;
                        grpbxRPL_WASclicked = false;
                        txtbx_reply_arr[lbl_ID].Focus(); //places cursor in question reply box on open
                    }
                }
			}
		   
			#endregion

			#region Destroy Reply
            if(grpbxFeed.Controls.Count > 0) //allows textbox click with no questions
            { 
                if ((group_arr[lbl_ID].Height > 31) && (btnCLS_WASclicked == true))// if open and clicked
                {
                    Point tmp = new Point(); // for dynamic moving of all controls below the clicked one
                    if (!chkbxLameMode.Checked)
                    {

                        if (j < 3)
                        {
                            group_arr[lbl_ID].SetBounds(group_arr[lbl_ID].Location.X, group_arr[lbl_ID].Location.Y, group_arr[lbl_ID].Size.Width, group_arr[lbl_ID].Size.Height - 2);
                            if (lbl_ID != 0)
                            {
                                for (int x = lbl_ID - 1; x >= 0; x--)
                                {
                                    tmp.X = group_arr[x].Location.X;
                                    tmp.Y = group_arr[x].Location.Y - 2;
                                    group_arr[x].Location = tmp;
                                }
                            }
                            j++;
                        }
                        else if ((j > 2) && (j < 36)) //was 19 with 3,6 pixel increments
                        {
                            group_arr[lbl_ID].SetBounds(group_arr[lbl_ID].Location.X, group_arr[lbl_ID].Location.Y, group_arr[lbl_ID].Size.Width, group_arr[lbl_ID].Size.Height - 3);
                            if (lbl_ID != 0)
                            {
                                for (int x = lbl_ID - 1; x >= 0; x--)
                                {
                                    tmp.X = group_arr[x].Location.X;
                                    tmp.Y = group_arr[x].Location.Y - 3;
                                    group_arr[x].Location = tmp;
                                }
                            }
                            j++;
                        }
                        else
                        {
                            j = 0;
                            btnCLS_WASclicked = false;
                            if (!DesireID)
                            {
                                timer.Enabled = false;
                                timesClicked = 0; //reset
                            }
                            else
                            {
                                lbl_ID = new_lblID;
                                grpbxRPL_WASclicked = true;
                                DesireID = false;
                            }
                        }
                    }
                    else //no animations :(
                    {
                        group_arr[lbl_ID].SetBounds(group_arr[lbl_ID].Location.X, group_arr[lbl_ID].Location.Y, group_arr[lbl_ID].Size.Width, group_arr[lbl_ID].Size.Height - 105);
                        if (lbl_ID != 0)
                        {
                            for (int x = lbl_ID - 1; x >= 0; x--)
                            {
                                tmp.X = group_arr[x].Location.X;
                                tmp.Y = group_arr[x].Location.Y - 105;
                                group_arr[x].Location = tmp;
                            }
                        }
                        j = 0;
                        btnCLS_WASclicked = false;
                        if (!DesireID)
                        {
                            timer.Enabled = false;
                            timesClicked = 0; //reset
                        }
                        else
                        {
                            lbl_ID = new_lblID;
                            grpbxRPL_WASclicked = true;
                        }
                    }
                }
				//textBox1.AppendText(group_arr[lbl_ID].Height.ToString() + Environment.NewLine); //troubleshooting
			}
			#endregion

            #region MoveCtrlsDown
            if ((NEW_grpbx == true))
            {
                Point TEMP = new Point();
                if (k < 3) //use groupboxID..
                {
                    for (int i = NumQuestions - 1; i >=0; i--) // to achieve LIFO
                    {
                        TEMP.X = group_arr[i].Location.X;
                        TEMP.Y = group_arr[i].Location.Y + 2;
                        group_arr[i].Location = TEMP;
                    }

                    k++;
                }
                else if ((k > 2) && (k < 12)) //was 19 with 3,6 pixel increments
                {
                    for (int i = NumQuestions - 1; i >=0; i--)// to achieve LIFO
                    {
                        TEMP.X = group_arr[i].Location.X;
                        TEMP.Y = group_arr[i].Location.Y + 3;
                        group_arr[i].Location = TEMP;
                    }


                    k++;
                }
                else
                {
                    k = 0;
                    timer.Enabled = false;
                    NEW_grpbx = false;
                }
            }

            #endregion

            #region DMPanel Animate
            if ((DirectMsgPanel.Location.X <16) && (DMclicked == true) && (DMtimesClicked==0) && (DMPanelShowing ==false))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X + 16, DirectMsgPanel.Location.Y, DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X + 16, grpbxFeed.Location.Y, grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X + 15, DirectMsgPanel.Location.Y, DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X + 15, grpbxFeed.Location.Y, grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X + 10, DirectMsgPanel.Location.Y, DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X + 10, grpbxFeed.Location.Y, grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        DMPanelShowing = true;
                        DMclicked = false;
                        DMtimesClicked = 1;
                        timer.Enabled = false;
                        DirectMsgPanel.Focus();
                    }
                }
                else //no animations, boo!
                {
                    DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X + 410, DirectMsgPanel.Location.Y, DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height);
                    grpbxFeed.SetBounds(grpbxFeed.Location.X + 410, grpbxFeed.Location.Y, grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                    DMPanelShowing = true;
                    DMclicked = false;
                    DMtimesClicked = 1;
                    timer.Enabled = false;
                    DirectMsgPanel.Focus();
                }
            }
            else if ((DirectMsgPanel.Location.X > -396) && (DMclicked == true) && (DMtimesClicked==1) && (DMPanelShowing == true) )
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X - 10, DirectMsgPanel.Location.Y, DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X - 10, grpbxFeed.Location.Y, grpbxFeed.Size.Width, grpbxFeed.Size.Height);

                        x++;
                    }
                    else if (x < 20)
                    {
                        DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X - 15, DirectMsgPanel.Location.Y, DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X - 15, grpbxFeed.Location.Y, grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X - 16, DirectMsgPanel.Location.Y, DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X - 16, grpbxFeed.Location.Y, grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        DMPanelShowing = false;
                        DMclicked = false;
                        DMtimesClicked = 0;
                        if (DesirePrefs)
                        {
                            if (!PrefsShowing)
                            {
                                PrefsClicked = true; //show prefs now                                
                            }
                            DesirePrefs = false;
                        }
                        else
                        {
                            timer.Enabled = false; //all done
                        }
                    }
                }
                else //no animations, boo!
                {
                    DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X - 410, DirectMsgPanel.Location.Y, DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height);
                    grpbxFeed.SetBounds(grpbxFeed.Location.X - 410, grpbxFeed.Location.Y, grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                    DMPanelShowing = false;
                    DMclicked = false;
                    DMtimesClicked = 0;
                    if (DesirePrefs)
                    {
                        if (!PrefsShowing)
                        {
                            PrefsClicked = true; //show prefs now                                
                        }
                        DesirePrefs = false;
                    }
                    else
                    {
                        timer.Enabled = false; //all done
                    }
                }
            }


            #endregion

            #region Prefs Animate default coordinates (395,80)

            if ((PanelPrefs.Location.X > 14) && (PrefsClicked == true) && (PrefsTimesClicked == 0) && (PrefsShowing == false))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        PanelPrefs.SetBounds(PanelPrefs.Location.X - 16, PanelPrefs.Location.Y, PanelPrefs.Size.Width, PanelPrefs.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X - 16, grpbxFeed.Location.Y, grpbxFeed.Size.Width, grpbxFeed.Size.Height);

                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelPrefs.SetBounds(PanelPrefs.Location.X - 12, PanelPrefs.Location.Y, PanelPrefs.Size.Width, PanelPrefs.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X - 12, grpbxFeed.Location.Y, grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelPrefs.SetBounds(PanelPrefs.Location.X - 10, PanelPrefs.Location.Y, PanelPrefs.Size.Width, PanelPrefs.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X - 10, grpbxFeed.Location.Y, grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        PrefsShowing = true;
                        PrefsClicked = false;
                        PrefsTimesClicked = 1;
                        timer.Enabled = false;
                        PanelPrefs.Focus();
                    }
                }
                else //no animations
                {
                    PanelPrefs.SetBounds(PanelPrefs.Location.X - 380, PanelPrefs.Location.Y, PanelPrefs.Size.Width, PanelPrefs.Size.Height);
                    grpbxFeed.SetBounds(grpbxFeed.Location.X - 380, grpbxFeed.Location.Y, grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                    PrefsShowing = true;
                    PrefsClicked = false;
                    PrefsTimesClicked = 1;
                    timer.Enabled = false;
                    PanelPrefs.Focus();
                }
            }
            else if ((PanelPrefs.Location.X < 396) && (PrefsClicked == true) && (PrefsTimesClicked == 1) && (PrefsShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        PanelPrefs.SetBounds(PanelPrefs.Location.X + 10, PanelPrefs.Location.Y, PanelPrefs.Size.Width, PanelPrefs.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X + 10, grpbxFeed.Location.Y, grpbxFeed.Size.Width, grpbxFeed.Size.Height);

                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelPrefs.SetBounds(PanelPrefs.Location.X + 12, PanelPrefs.Location.Y, PanelPrefs.Size.Width, PanelPrefs.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X + 12, grpbxFeed.Location.Y, grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelPrefs.SetBounds(PanelPrefs.Location.X + 16, PanelPrefs.Location.Y, PanelPrefs.Size.Width, PanelPrefs.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X + 16, grpbxFeed.Location.Y, grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        PrefsShowing = false;
                        PrefsClicked = false;
                        PrefsTimesClicked = 0;
                        SavePrefs(); //when hiding the panel save the preferences
                        if (DesireDM)
                        {
                            if (!DMPanelShowing)
                            {
                                DMclicked = true; //show DM now
                            }
                            DesireDM = false;
                        }
                        else if (DesireBrdcst)
                        {
                            textbox1WASclicked = true; // show broadcast box
                            DesireBrdcst = false;
                        }
                        else
                        {
                            timer.Enabled = false; //all done
                        }
                    }
                }
                else //no animations, boo!
                {
                    PanelPrefs.SetBounds(PanelPrefs.Location.X + 380, PanelPrefs.Location.Y, PanelPrefs.Size.Width, PanelPrefs.Size.Height);
                    grpbxFeed.SetBounds(grpbxFeed.Location.X + 380, grpbxFeed.Location.Y, grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                    PrefsShowing = false;
                    PrefsClicked = false;
                    PrefsTimesClicked = 0;
                    SavePrefs(); //when hiding the panel save the preferences
                    if (DesireDM)
                    {
                        if (!DMPanelShowing)
                        {
                            DMclicked = true; //show DM now
                        }
                        DesireDM = false;
                    }
                    else if (DesireBrdcst)
                    {
                        textbox1WASclicked = true; // show broadcast box
                        DesireBrdcst = false;
                    }
                    else
                    {
                        timer.Enabled = false; //all done
                    }
                }                
            }


            #endregion

            #region FAQ Animate

            if ((PanelFAQ.Location.Y < 26) && (FAQClicked == true) && (FAQTimesClicked == 0) && (FAQShowing == false))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y + 24, PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y + 15, PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y + 12, PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        FAQShowing = true;
                        FAQClicked = false;
                        FAQTimesClicked = 1;
                        timer.Enabled = false;
                        PanelFAQ.Focus();
                    }
                }
                else //no animations
                {
                    PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y + 510, PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                    FAQShowing = true;
                    FAQClicked = false;
                    FAQTimesClicked = 1;
                    timer.Enabled = false;
                    PanelFAQ.Focus();
                }
            }
            else if( (PanelFAQ.Location.Y > -486) && (FAQClicked == true) && (FAQTimesClicked == 1) && (FAQShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y - 12, PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y - 15, PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y - 24, PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        FAQShowing = false;
                        FAQClicked = false;
                        FAQTimesClicked = 0;
                        if (DesireBrdcst)
                        {
                            if (!btnSend.Visible)
                            {
                                textbox1WASclicked = true; //show broadcast now                                
                            }
                            DesireBrdcst = false;
                        }
                        else if (DesireDM)
                        {
                            if (!DMPanelShowing)
                            {
                                DMclicked = true; //show DM now
                            }
                            else
                            {
                                DirectMsgPanel.Focus();
                            }
                            DesireDM = false;
                        }
                        else if (DesirePrefs)
                        {
                            if (!PrefsShowing)
                            {
                                PrefsClicked = true; //show prefs now                                
                            }
                            else
                            {
                                PanelPrefs.Focus();
                            }
                            DesirePrefs = false;
                        }
                        else
                        {
                            timer.Enabled = false; //all done
                        }
                    }
                }
                else //no animations
                {
                    PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y - 510, PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                    FAQShowing = false;
                    FAQClicked = false;
                    FAQTimesClicked = 0;
                    if (DesireBrdcst)
                    {
                        if (!btnSend.Visible)
                        {
                            textbox1WASclicked = true; //show broadcast now                                
                        }
                        DesireBrdcst = false;
                    }
                    else if (DesireDM)
                    {
                        if (!DMPanelShowing)
                        {
                            DMclicked = true; //show DM now
                        }
                        DesireDM = false;
                    }
                    else if (DesirePrefs)
                    {
                        if (!PrefsShowing)
                        {
                            PrefsClicked = true; //show prefs now                                
                        }
                        DesirePrefs = false;
                    }
                    else
                    {
                        timer.Enabled = false; //all done
                    }
                }
             }
            
            
            #endregion

            #region Student Management Animate

            if ((PanelStudents.Location.Y > 24) && (StuMgmtClicked == true) && (StuMgmtTimesClicked == 0) && (StuMgmtShowing == false))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y - 24, PanelStudents.Size.Width, PanelStudents.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y - 15, PanelStudents.Size.Width, PanelStudents.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y - 12, PanelStudents.Size.Width, PanelStudents.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        StuMgmtShowing = true;
                        StuMgmtClicked = false;
                        StuMgmtTimesClicked = 1;
                        timer.Enabled = false;
                        PanelStudents.Focus();
                    }
                }
                else //no animations, boo!
                {
                    PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y - 510, PanelStudents.Size.Width, PanelStudents.Size.Height);
                    StuMgmtShowing = true;
                    StuMgmtClicked = false;
                    StuMgmtTimesClicked = 1;
                    timer.Enabled = false;
                    PanelStudents.Focus();
                }
            }
            else if ((PanelStudents.Location.Y < 536) && (StuMgmtClicked == true) && (StuMgmtTimesClicked == 1) && (StuMgmtShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y + 12, PanelStudents.Size.Width, PanelStudents.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y + 15, PanelStudents.Size.Width, PanelStudents.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y + 24, PanelStudents.Size.Width, PanelStudents.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        StuMgmtShowing = false;
                        StuMgmtClicked = false;
                        StuMgmtTimesClicked = 0;
                        if (DesirePrefs)
                        {
                            if (PrefsShowing)
                            {
                                PrefsClicked = true; //hide prefs now, only for StuMgmt                                
                            }
                            DesirePrefs = false;
                        }
                        else
                        {
                            timer.Enabled = false; //all done
                        }
                    }
                }
                else //no animations
                {
                    PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y + 510, PanelStudents.Size.Width, PanelStudents.Size.Height);
                    StuMgmtShowing = false;
                    StuMgmtClicked = false;
                    StuMgmtTimesClicked = 0;
                    if (DesirePrefs)
                    {
                        if (PrefsShowing)
                        {
                            PrefsClicked = true; //hide prefs now, only for StuMgmt                                
                        }
                        DesirePrefs = false;
                    }
                    else
                    {
                        timer.Enabled = false; //all done
                    }
                }
            }


            #endregion
        }//end of timer_Tick

		private void btnCLR_Click(object sender, EventArgs e)
		{
            if ((NumQuestions - 1) > 0)
            {
                for (int i = 0; i < NumQuestions - 1; i++)
                {

                    txtbx_reply_arr[i].ResetText(); //clear text

                }
            }
            else 
            {
                txtbx_reply_arr[0].ResetText(); 
            }
		}

        private void btnRepl_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lbl_arr.Length - 1; i++) //loop through to find the clicked one
            {
                if (sender.Equals(lbl_arr[i]))
                {
                    lbl_ID = i;
                }
            }

            addr = Q_sender[lbl_ID]; // find out who sent the question clicked

            if ((txtbx_reply_arr[lbl_ID].Text!=Environment.NewLine) && (txtbx_reply_arr[lbl_ID].Text!="")) // we have communications and data to send
            {
                //FlowCtrl_Send(addr, "enq");
               // timeout_ack = false; //so that the following statement will work
              //  while (!AckRXd && !timeout_ack) ;
              //  if (AckRXd)
               // {
                    if (SendMsg(txtbx_reply_arr[lbl_ID].Text.TrimEnd('\x0A'), addr)) //SendMsg returns true for a successful send
                    {
                        LogQandA(txtbx_reply_arr[lbl_ID].Text.TrimEnd('\x0A'), true, addr); //log message
                        txtbx_reply_arr[lbl_ID].Clear();
                        btnCLS_WASclicked = true; //close question
                        timer.Enabled = true; // hide the send button

                        if (!Q_Replied[lbl_ID])
                        {
                            Q_Replied[lbl_ID] = true;
                            picbx_arr[lbl_ID].BringToFront();//to display on the label
                            picbx_arr[lbl_ID].Visible = true; //come out from hiding
                        }
                    }
                    //AckRXd = false; //reset
                    
             //   }
              //  else
             //   {
              //      MessageBox.Show("Student System Not Responding." + Environment.NewLine + "Message not sent.");
              //  }
            }
            else
            {
                MessageBox.Show("You must type a message before sending.");
            }


        }

		private void lbl_question_Click(object sender, EventArgs e)
		{
            if (rdbtnClick.Checked)
            {
                UnreadDecrement(sender);
            }
			for (int i = 0; i < lbl_arr.Length - 1; i++) //loop through to find the clicked one
			{
				if ( sender.Equals(lbl_arr[i])){
                    old_lblID = lbl_ID; //save in case a separate question is clicked
                    new_lblID = i; //store new to use later
				}
			}
            
            if (timesClicked == 0)
            {
                grpbxRPL_WASclicked = true;
                timesClicked = 1;
                lbl_ID = new_lblID; //there isn't possibly one open yet
            }
            else
            {
                if (old_lblID == new_lblID)
                {
                    btnCLS_WASclicked = true; //second click closes question.
                    timesClicked = 0;
                }
                else
                {
                    DesireID = true; //queue dynamic animations
                    btnCLS_WASclicked = true; //second click closes question.
                    //the animation will use lbl_ID which is the old one still
                }
            }
            timer.Enabled = true;
		}

		private void btnCLS_Click(object sender, EventArgs e)
		{
			timer.Enabled = true;
			btnCLS_WASclicked = true;
            timesClicked = 0;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
            
            //auto fill feed box or something... for testing if needed

            Properties.Settings.Default.Reload(); //load the key..
            SecretKey = Properties.Settings.Default.key;

            /*Check the state of previous settings and reset them*/
            chkbxLameMode.Checked = Properties.Settings.Default.Animations;
            chkbxRXSound.Checked = Properties.Settings.Default.SoundRX;
            chkbxTXSound.Checked = Properties.Settings.Default.SoundTX;
            rdbtnClick.Checked = Properties.Settings.Default.ClickRead;
            rdbtnHover.Checked = Properties.Settings.Default.HoverRead;
            /***************************************************/

            string[] tmpstring = new string[classSize];
            try
            {
                using (StreamReader sr = File.OpenText(PlainData))
                {
                    string tempstr = "";
                    while ((tempstr = sr.ReadLine()) != null)
                    {
                        tempstr = tempstr.TrimEnd('\x0A');
                        tmpstring = tempstr.Split(',');// first item in tmp string should always be name
                        Students_Name[i] = tmpstring[0]; // store student names as they are read from file
                        Students_ID[i++] = tmpstring[1]; //store ID's
                    }
                    sr.Close();
                }
                i = j = 0;
            }
            catch 
            {
                MessageBox.Show("Student Data File is missing!");
            }

		}

        private void PrefsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((StuMgmtShowing == true) && (FAQShowing == false) && (DMPanelShowing == false)) 
            {
                SaveStudentData(); //if the panel is hiding save data
                StuMgmtClicked = true;
                timer.Enabled = true;
                PanelPrefs.Focus(); //for use of esc key
            }
            else if ((FAQShowing == false) && (DMPanelShowing == false))
            {
                PrefsClicked = true;
                timer.Enabled = true;
            }
            else if ((FAQShowing ==true) && (DMPanelShowing == false))
            {
                DesirePrefs = true;//show me after hiding FAQ
                FAQClicked = true; //hide
                timer.Enabled = true;
            }
            else if ((FAQShowing == false) && (DMPanelShowing == true))
            {
                DesirePrefs = true;//show me after hiding DM
                DMclicked = true; //hide
                timer.Enabled = true;
            } 
        }

        private void serialCOMcmbbx_Click(object sender, EventArgs e)
        {
            serialCOMcmbbx.Items.Clear();
            portNames = System.IO.Ports.SerialPort.GetPortNames();
            foreach (string name in portNames)
            {
                serialCOMcmbbx.Items.Add(name);
            }
        }

        private void serialCOMcmbbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SerialPort.IsOpen == true)  //make sure we don't try to open a port that is already
                SerialPort.Close();

            SerialPort.PortName = portNames[serialCOMcmbbx.SelectedIndex];
           
            try
            {
                SerialPort.Open();
                textBox1.Focus(); //put cursor in txtbx
            }
            catch (Exception E)
            {
                MessageBox.Show("Serial Port selected could not be opened...");
                SerialPort.Close();
            }


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SerialPort.Close(); //tie up loose ends..
            SavePrefs();

            if (StuMgmtShowing)
            {
                SaveStudentData(); //if the panel is showing, on close, save data
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Show About Information
            AboutBox_CI About = new AboutBox_CI();
            About.ShowDialog();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text != "") && (textBox1.Text != Environment.NewLine))
            {
                if (SendMsg(textBox1.Text, brdcst_addr)) //Sending Function, returns true if send succeeds
                {
                    textBox1.Clear();
                    grpbxFeed.Focus(); //get cursor out of textbox
                    textbox1WASclicked = true;
                    timer.Enabled = true; // hide the send button
                }
            }
            else
            {
                MessageBox.Show("You Haven't typed a message  yet..");
            }
        }

        private void timer_SerialRead_Tick(object sender, EventArgs e) //my very own SerialDataRX'd event
        {
            string tmp_str = "";
            if (SerialPort.IsOpen == true)
            {
                SerialPort.ReadTimeout = 25; //in miliseconds
                tempString = null;
                try
                {
                    tempString = SerialPort.ReadLine();
                }
                catch (Exception E)
                {
                    //hopefully we don't freeze up now
                }
                if (tempString != null)
                {
                    //MessageBox.Show(tempString); //testing
                    RX_Data = tempString;
                    WeGotData = true;
                    tempString = null;
                }
                                
                if (WeGotData != false)
                {
                    //we just had data, call some event
                    UnreadCount++;
                    tmp_str = HandleBCKSPC(RX_Data);
                    if (tmp_str == "")// handle backspace (0x08) before printing questions
                    {
                        //if "" is true then the string received was a flwctrl string, and it's taken care of
                    }
                    else // we can make controls because this was a question
                    {
                        Qs_to_create[ix++] = tmp_str; //store and increment pointer
                        Qs_to_Make = true; //set the flag
                        timer_ControlsCreate.Enabled = true;
                    }
                    WeGotData = false;
                    
                }
                
            }
        }

        private void lbl_question_MouseMove(object sender, MouseEventArgs e)
        {
            if (rdbtnHover.Checked)
            {
                UnreadDecrement(sender);
            }
        }

        private void directMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((StuMgmtShowing == true) && (PrefsShowing == true))
            {
                DesireDM = true; //show after hiding stumgmt
                StuMgmtClicked = true; //hide
                DesirePrefs = true; //tells stumgmt to hide prefs
                timer.Enabled = true;
            }
            else if ((FAQShowing == true) && (PrefsShowing == false))
            {
                DesireDM = true; //show me after hiding FAQ
                FAQClicked = true; //hide me
                timer.Enabled = true;
            }
            else if ((FAQShowing == false) && (PrefsShowing == true))
            {
                DesireDM = true; //show me after hiding Prefs
                PrefsClicked = true; //hide me
                timer.Enabled = true;
            }
            else
            {
                DMclicked = true;
                timer.Enabled = true;
            }
        }

        private void btnDM_Cls_Click(object sender, EventArgs e)
        {
            DMclicked = true;
            timer.Enabled = true;
        }

        private void broadcastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((FAQShowing == false) && (StuMgmtShowing == false))
            {
                timer.Enabled = true;
                textbox1WASclicked = true;
                textBox1.Clear();
                textBox1.Focus();
            }
            else if (FAQShowing == true)
            {
                DesireBrdcst = true;
                FAQClicked = true; //hide
                timer.Enabled = true;
            }
            else if (StuMgmtShowing == true)
            {
                SaveStudentData(); //if the panel is hiding save data
                StuMgmtClicked = true; //hide
                DesirePrefs = true; // hide me too...
                DesireBrdcst = true; //show me last
                timer.Enabled = true;
            }
        }

        private void btnCLS_Click_1(object sender, EventArgs e)
        {
            textbox1WASclicked = true;
            timer.Enabled = true;
            grpbxFeed.Focus();
        }

        private void btnPrefs_Cls_Click(object sender, EventArgs e)
        {
            PrefsClicked = true;
            timer.Enabled = true;
        }

        private void btnDM_Clr_Click(object sender, EventArgs e)
        {
            txtbxDM.ResetText();
        }
 
        private void btnDM_Send_Click(object sender, EventArgs e)
        {
            //send stuff obviously
            if ((txtbxDM.Text != "") && (txtbxDM.Text != Environment.NewLine))
            {
                //FlowCtrl_Send(addr, "enq");
                //timeout_ack = false; //so that the following statement will work
                //while (!timeout_ack);
                //if (AckRXd)
                //{
                    if (SendMsg(txtbxDM.Text, addr)) //Sending Function, returns true if send succeeds
                    {
                        LogQandA(txtbxDM.Text, true, addr);
                        txtbxDM.ResetText(); //clear the textbox after sending
                        DMclicked = true; //to close the direct message panel
                        timer.Enabled = true;
                    }
                  //  AckRXd = false;//reset
                //}
                //else
                //{
                //    MessageBox.Show("Student System Not Responding." + Environment.NewLine + "Message not sent.");
                //}
            }
            else
            {
                MessageBox.Show("You Haven't typed a message  yet..");
            }
            
        }

        private void btnFAQcls_Click(object sender, EventArgs e)
        {
            FAQClicked = true;
            timer.Enabled = true;

            if (StuMgmtShowing)
            {
                PanelStudents.Focus();
            }
        }

        private void generalFAQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FAQClicked = true;
            timer.Enabled = true;
            PanelFAQ.Focus();
        }

        private void lnklblFAQ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("mailto:bondslaveofJesus@gmail.com");
            }
            catch { }
        }

        private void btnStuMgmt_Prefs_Click(object sender, EventArgs e)
        {
            string[] tmpstring = new string[classSize];
            int i = 0;
            //open StuMgmtPanel
            if (StuMgmtTimesClicked == 0)
            {
                //try
                //{
                //    if (File.Exists(EncryptedData))
                //    {
                //        DecryptFile(EncryptedData, PlainData, SecretKey);
                //        File.Delete(EncryptedData); //remove old encrypted data, on panel open
                //    }
                //}
                //catch (Exception E)
                //{
                //    MessageBox.Show("Decrypt failed..." + Environment.NewLine + E);
                //}
                try
                {
                    using (StreamReader sr = File.OpenText(PlainData))
                    {
                        string tempstr = "";
                        while ((tempstr = sr.ReadLine()) != null)
                        {
                            tempstr = tempstr.TrimEnd('\x0A');
                            tmpstring = tempstr.Split(',');// first item in tmp string should always be name
                            Students_Name[i] = tmpstring[0]; // store student names as they are read from file
                            Students_ID[i++] = tmpstring[1]; //store ID's
                        }
                        sr.Close();
                    }
                    
                    lstbxStudents.Items.Clear();
                    for (j = 0; j < i; j++)
                    {
                        lstbxStudents.Items.Add(Students_Name[j]); //repopulate the lstbx
                    }
                    //File.Delete(PlainData); //otherwise while in the panel decrypted sensitive data would be vulnerable..
                    i = j = 0;
                }
                catch (Exception E)
                {
                    MessageBox.Show("File open for filling Listbox Failed..." + Environment.NewLine + E);
                }

            }
            StuMgmtClicked = true;
            timer.Enabled = true;
        }

        private void btnCLSStudents_Click(object sender, EventArgs e)
        {
            StuMgmtClicked = true;
            timer.Enabled = true;
            
            SaveStudentData();
            PanelPrefs.Focus();
        }

        private void SaveStudentData()
        {
           
            if (File.Exists(PlainData) == false)
            {
                File.Create(PlainData);
            }

            using (StreamWriter sw = new StreamWriter(PlainData))
            {
                //build and write the strings for the data file
                for (int i = 0; i < lstbxStudents.Items.Count; i++)
                {
                    sw.WriteLine(Students_Name[i] + ',' + Students_ID[i]);
                }
                sw.Flush();
                sw.Close();
            }
            
            ////get key for file encryption
            //SecretKey = GenerateKey();

            ////for additional security pin the key
            //GCHandle gch = GCHandle.Alloc(SecretKey, GCHandleType.Pinned);

            ////encrypt away!
            //EncryptFile(PlainData, EncryptedData, SecretKey);

            //Properties.Settings.Default.key = SecretKey;
            //Properties.Settings.Default.Save();

            //ZeroMemory(gch.AddrOfPinnedObject(), SecretKey.Length);
            ////SecretKey.Remove(0); //starting with first character, zero the string
            //gch.Free();

            //File.Delete(PlainData); //get rid of the unencrypted data
        }

        //courtesy of msdn: http://support.microsoft.com/kb/307010
        #region Crypto from Msdn
        static string GenerateKey()
        {
            //create an instance of Symetric Algorithm. Key and IV are generated automatically.
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

            //Use the Automatically generated key for encryption..
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key); 
        }

        static void EncryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            //input filestream
            FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
            //output filestream
            FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);

            //actual crypto provider to be used on the file
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            //give the cryptographic provider the secret key and init vector otherwise it will make them up...
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            //create a cryptostream class using the previous cryptographic provider and existing filestream obj
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptoctream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);

            //read in the input and write the encypted output file by passing it to cryptostream
            byte[] bytearrayinput = new byte[fsInput.Length];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptoctream.Write(bytearrayinput, 0, bytearrayinput.Length);

        }

        static void DecryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            //create file stream to read the encrypted file back
            FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);

            //create aes decryptor from the aes instance
            ICryptoTransform desdecrypt = DES.CreateDecryptor();

            //create crypto stream set to read and do a aes decryption transform on incoming bytes
            CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);

            //prints the contents of the decrypted file
            StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
            fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
            fsDecrypted.Flush();
            fsDecrypted.Close();
        }

        //  Call this function to remove the key from memory after use for security.
        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);

        #endregion

        private void btnEDTStudents_Click(object sender, EventArgs e)
        {
            //edit the currently selected student in Listbox, messagebox to select one if they haven't yet
            if (lstbxStudents.SelectedIndex == -1)
            {
                MessageBox.Show("You haven't selected a Student to edit yet.");
            }
            else
            {
                txtbxStudentsName.Text = Students_Name[lstbxStudents.SelectedIndex];
                txtbxStudentsName.Focus();
                grpbxEDITStudents.Text = "Editing Student " + (lstbxStudents.SelectedIndex + 1).ToString();
            }
            
        }

        private void btnCLRStudents_Click(object sender, EventArgs e)
        {
            //reset the student name element to the default StudentNum...
            if (lstbxStudents.SelectedIndex == -1)
            {
                MessageBox.Show("You haven't selected a Student to edit yet.");
            }
            else
            {
                Students_Name[lstbxStudents.SelectedIndex] = "Student  " + (lstbxStudents.SelectedIndex +1 ).ToString();
                lstbxStudents.Items.Insert(lstbxStudents.SelectedIndex, Students_Name[lstbxStudents.SelectedIndex]);
                lstbxStudents.Items.RemoveAt(lstbxStudents.SelectedIndex);
                lstbxStudents.Refresh();
            }
            PanelStudents.Focus(); //for esc to work
        }

        private void btnDoneStudents_Click(object sender, EventArgs e)
        {
            //save the changes made in the textbox to the name back into the "database"
            Students_Name[lstbxStudents.SelectedIndex] = txtbxStudentsName.Text;
            lstbxStudents.Items.Insert(lstbxStudents.SelectedIndex, txtbxStudentsName.Text);
            lstbxStudents.Items.RemoveAt(lstbxStudents.SelectedIndex);
            txtbxStudentsName.Text = "<----- Select a Student";
            lstbxStudents.Refresh();
            grpbxEDITStudents.Text = "Student #";

            PanelStudents.Focus();
        }

        private void cmbxDM_Click(object sender, EventArgs e)
        {
            string[] tmpstring = new string[classSize];
            try
            {
                int i = 0;
                using (StreamReader sr = File.OpenText(PlainData))
                {
                    string tempstr = "";
                    while ((tempstr = sr.ReadLine()) != null)
                    {
                        tempstr = tempstr.TrimEnd('\x0A');
                        tmpstring = tempstr.Split(',');// first item in tmp string should always be name
                        Students_Name[i] = tmpstring[0]; // store student names as they are read from file
                        Students_ID[i++] = tmpstring[1]; //store ID's
                    }
                    sr.Close();
                }
                cmbxDM.Items.Clear();
                for (j = 0; j < i; j++)
                {
                    cmbxDM.Items.Add(Students_Name[j]); //repopulate the lstbx
                }
                i = j = 0;
            }
            catch (Exception E)
            {
                MessageBox.Show("File open for filling Combobox Failed..." + Environment.NewLine + E);
            }
            
        }

        private void cmbxDM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbxDM.SelectedIndex != -1)
            {
                addr = addr_tbl[cmbxDM.SelectedIndex];
            }
            DirectMsgPanel.Focus(); //so esc key will work
        }

        private void FlowCtrl_Send(string Address_Student,string desire)
        {
            //0x05 = Enquiry 0x06= Acknowledgement
            string flwctrl_send = "";
            if (desire == "enq")
            {
                flwctrl_send = "\x05"; //enquire
            }
            else
            {
                flwctrl_send = "\x06"; //reply with Ack
            }

            if (SerialPort.IsOpen == true)
            {
                SerialPort.WriteLine('\x02' + Address_Student + flwctrl_send + '\x03'); //Enquire if it's clear to send
            }
            else
            {
                MessageBox.Show("You must select a COM port before sending communications.");
            }
        
        }

        private void timer_ControlsCreate_Tick(object sender, EventArgs e) //this event goes off when timer enabled, every 1 seconds
        {
            if (Qs_to_Make)
            {
                if (ix > jx) // we haven't made all the controls yet
                {
                    LogQandA(Qs_to_create[jx], false, Q_sender[jx]);
                    MakeCtrls(Qs_to_create[jx++]); //make controls and increment creation counter
                }
                else
                {
                    Qs_to_Make = false; //stop trying to make things
                }
            }
            else //No more changes to make!
            {
                timer_ControlsCreate.Enabled = false;
            }            
        }

        private bool SendMsg(string Message2Send, string Address2Send)
        {
            bool SuccessOfSending = new bool();
            if (SerialPort.IsOpen == true)
            {
                SerialPort.WriteLine('\x02' + Address2Send + Message2Send + '\x03'); //framing with stx addr message etx
                SuccessOfSending = true;
                if (chkbxTXSound.Checked)
                {
                    JukeBox.SoundLocation = @"SentMsg.wav"; // this is in [projdir]/bin/debug/blip.wav
                    JukeBox.Play();
                }
            }
            else
            {
                MessageBox.Show("You must select a COM port before sending communications.");
                SuccessOfSending = false;
            }
            return SuccessOfSending; //lets sender know if sending succeeded
        }

        private void SavePrefs()
        {
            Properties.Settings.Default.Animations = chkbxLameMode.Checked;
            Properties.Settings.Default.SoundRX = chkbxRXSound.Checked;
            Properties.Settings.Default.SoundTX = chkbxTXSound.Checked;
            Properties.Settings.Default.ClickRead = rdbtnClick.Checked;
            Properties.Settings.Default.HoverRead = rdbtnHover.Checked;
            Properties.Settings.Default.Save();
        }

        private void UnreadDecrement(object sender)
        {
            //UnreadCount needs smartly decremented here
            for (int i = 0; i < lbl_arr.Length - 1; i++) //loop through to find the clicked one
            {
                if (sender.Equals(lbl_arr[i]))
                {
                    lbl_ID_2 = i;
                }
            }

            if (Q_status[lbl_ID_2] != true) //if status is true then it has already been read
            {
                Q_status[lbl_ID_2] = true;
                UnreadCount--;
                lbl_arr[lbl_ID_2].Text = lbl_arr[lbl_ID_2].Text.TrimStart('*').TrimStart(' '); //removes unread indicator in the label

                if (UnreadCount > 0)
                {
                    picbxStatus.Visible = false;
                    tlstrplbl_Unread.Text = "Unread " + UnreadCount.ToString();
                    tlstrplbl_Unread.Visible = true;
                }
                else
                {
                    picbxStatus.Visible = true;
                    tlstrplbl_Unread.Visible = false;
                }
            }
        }

        private void lnklblIssues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/dchriste/Classroom-Inquisition/issues");
            }
            catch { }
        }

        private bool LogQandA(string dialog, bool teacher, string recipient)
        {
            bool success = false;
            byte student = 0;
            string name2write = "";
            try
            {
                for (int i = 0; i < Students_ID.Length; i++) //loop to find student name by comparing addresses
                {
                    if (String.CompareOrdinal(addr_tbl[i], recipient) == 0) //CompareOrdinal Compares Numerical value
                    {
                        name2write = Students_Name[i]; //student name
                        student = (byte)i; //for use as a index below
                        break;
                    }
                }

                //make the generic filename
                logFiles[student] = @".logs\Student" + (student + 1).ToString() + "-log.txt"; //Student#-log.txt in hidden folder .logs

                if (File.Exists(logFiles[student]) == false)
                {
                    using (FileStream fs = File.Create(logFiles[student]))
                    {
                        fs.Close();
                    }
                }

                if (!teacher)
                {
                    //then I have the name already from the loop above.
                }
                else
                {
                    name2write = "Teacher";
                }

                using (StreamWriter sw = File.AppendText(logFiles[student]))
                {
                    sw.NewLine = "\x0A"; //sets the writeline terminator
                    sw.WriteLine(name2write + " ( " + DateTime.Now.ToString("HH:mm:ss") + "  |  " + DateTime.Today.Date.ToString("d") + " )" + ":" + "\x0A" + dialog + "\x0A");  //this will leave one blank line between each message
                    /*Example:
                     * Teacher (13:10:12  |  4/1/2012):
                     * It's okay johnny, you're dad will be here soon.. April Fools!
                     * ...
                     * */
                    sw.Flush();
                    sw.Close();
                }
                success = true;
            }
            catch
            {
                success = false;
                MessageBox.Show("Log creation // writing failed!!!");
            }
            return success;
        }
   } //end of partial class
} //end of namespace
