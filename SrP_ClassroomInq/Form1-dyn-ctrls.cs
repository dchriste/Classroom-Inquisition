using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
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
	public partial class frmClassrromInq : Form
	{
        /****************************************************************************
          *  Classroom Inquisition Teacher Application 
          *  Programmer: David Christensen
          *  Bug Tracking: https://github.com/dchriste/Classroom-Inquisition/issues 
         *****************************************************************************/
        public frmClassrromInq()
		{
			InitializeComponent();
            
            textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys); // so you can send with enter in the main txtbx
            PanelStudents.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            PanelFAQ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            PanelPrefs.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            DirectMsgPanel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            PanelConvView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            PanelAttendance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            PanelQuizMaker.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            PanelQuizMode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            PanelClassVote.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
		}
        #region Initialization

        public StreamReader Stream2Print;
        public Font PrintFont;
        public Color ForeColorTheme = Color.DarkGoldenrod;
        public Color BackColorTheme = Color.Black;
        public AboutBox_CI About = new AboutBox_CI();

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
        bool[] Q_NameShowing = new bool[classSize];
        string[] Q_sender = new string[classSize]; // records the addr of the sender of each question
        string brdcst_addr = "\x01";
        string[] logFiles = new string[classSize];
        string EncryptedData = @".data\StudentInfo_Encrypted.txt";
        string PlainData = @".data\StudentInfo.txt";
        string[] portNames = new string[10];
        string tempString = "";
        string tempString2 = "";
        string RX_Data = "";
        string SecretKey;
        string StateMachine = "Normal"; //inital state
        SoundPlayer JukeBox = new System.Media.SoundPlayer(); //for alert tone
        bool WeGotData = false;
		public const int classSize = 50;  //will be max questions handled at once
		byte i = 0;
		byte k = 0;
		byte j = 0;
        byte x = 0;
        byte iAck = 0;
        static int logHistoryLength = 500; //this could be a preference, it determines the amount shown in conversation viewer
        string[] logTmpString = new string[logHistoryLength];
        int ix = 0; //only for serial tick timer...
        int jx = 0; //only for the make controls timer
        string addr = "\xFF";
        byte NumQuestions = 0;
        byte UnreadCount = 0;
        bool EDTClicked = false;
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
        bool ConvViewClicked = false;
        bool ConvViewShowing = false;
        byte ConvViewTimesClicked = 0;
        bool QuizMakerShowing = false;
        bool QuizMakerClicked = false;
        byte QuizMakerTimesClicked = 0;
        bool QuizModeShowing = false;
        bool QuizModeClicked = false;
        byte QuizModeTimesClicked = 0;
        bool AttendanceShowing = false;
        bool AttendanceClicked = false;
        byte AttendanceTimesClicked = 0;
        bool ClassVoteShowing = false;
        bool ClassVoteClicked = false;
        byte ClassVoteTimesClicked = 0;
		bool textbox1WASclicked = new bool();
		bool grpbxRPL_WASclicked = new bool();
		bool btnCLS_WASclicked = new bool();
        bool NEW_grpbx = new bool();
        bool AckRXd = false;
        bool timeout_ack = false;
        int old_lblID = 0;
        bool DesireID = false;
        bool DeleteQuestion = false;
        bool Question_Deleted = false;
        bool QuizQuestionDel = false;
        int QuizDelID = 255;
        string QzQuestionDel = "";
        bool Request_Undo = false;
        bool sudo_kill = false;
        byte Del_ID = 255;
        byte timesThrough = 0;
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
        System.Windows.Forms.PictureBox[] picbx_ConvView_arr = new System.Windows.Forms.PictureBox[classSize];
        System.Windows.Forms.ToolTip[] tt_picbxCV_arr = new System.Windows.Forms.ToolTip[classSize];
        /*This is for the quiz mode*/
        System.Windows.Forms.Panel[] PanelQuestion_arr = new System.Windows.Forms.Panel[classSize]; //limits number of quiz questions
        System.Windows.Forms.Button[] btnQuizQuestionSend_arr = new System.Windows.Forms.Button[classSize];// ^^
        System.Windows.Forms.Label[] lblQuestionNum_arr = new System.Windows.Forms.Label[classSize];
        System.Windows.Forms.TextBox[] txtbxQuestion_arr = new System.Windows.Forms.TextBox[classSize];
        /**************************/
        System.Drawing.Point origingrouparr = new System.Drawing.Point(6, -13); //originally 6,19 but this gives questions an entering animation
        System.Drawing.Point tempgrouparr = new System.Drawing.Point(6, 19);
        System.Drawing.Point tempreplyarr = new System.Drawing.Point(230, 98);
        System.Drawing.Point tempclosearr = new System.Drawing.Point(129, 98);
        System.Drawing.Point tempcleararr = new System.Drawing.Point(23, 98);
        System.Drawing.Point txtbx_reply_temp = new System.Drawing.Point(23, 32);
        System.Drawing.Point lbl_arr_temp = new System.Drawing.Point(0, -13);
        System.Drawing.Point picbx_arr_tmp = new System.Drawing.Point(2, 9);
        System.Drawing.Point picbx_ConvView_tmp = new System.Drawing.Point(296, 7);
        /*This is for quiz mode*/
        System.Drawing.Point lblQuestionNum_arr_tmp = new System.Drawing.Point(5, 21);
        System.Drawing.Point txtbxQuestion_arr_tmp = new System.Drawing.Point(32, 13);
        System.Drawing.Point btnQuizQuestionSend_arr_tmp = new System.Drawing.Point(288, 13);
        System.Drawing.Point PanelQuestion_arr_tmp = new System.Drawing.Point(7, 6);
        /***********************/
        #endregion

        #endregion
        
        /*This method allows for the moving down of controls when a new control is added*/
        public void MoveCtrlsDown()
        {
            //move grpbxs down if new question received to implement LIFO
                NEW_grpbx = true;
                timer.Enabled = true;            
        }
        
        /*This method makes the controls for the new question dynamically upon receiving the quesiton*/
        public void MakeCtrls(string question){
            reply_arr[NumQuestions] = new System.Windows.Forms.Button();
            reply_arr[NumQuestions].BackColor = BackColorTheme;
            reply_arr[NumQuestions].ForeColor = ForeColorTheme;
            reply_arr[NumQuestions].FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            reply_arr[NumQuestions].Location = tempreplyarr;
            reply_arr[NumQuestions].Name = "reply_arr_" + NumQuestions.ToString();
            reply_arr[NumQuestions].Size = new System.Drawing.Size(67, 25);
            reply_arr[NumQuestions].TabIndex = 2;
            reply_arr[NumQuestions].Text = "Repl&y";
            reply_arr[NumQuestions].UseVisualStyleBackColor = true;
            reply_arr[NumQuestions].Click += new System.EventHandler(this.btnRepl_Click);

            close_arr[NumQuestions] = new Button();
            close_arr[NumQuestions].BackColor = BackColorTheme;
            close_arr[NumQuestions].ForeColor = ForeColorTheme;
            close_arr[NumQuestions].FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            close_arr[NumQuestions].Location = tempclosearr;
            close_arr[NumQuestions].Name = "close_arr_" + NumQuestions.ToString();
            close_arr[NumQuestions].Size = new System.Drawing.Size(67, 25);
            close_arr[NumQuestions].TabIndex = 3;
            close_arr[NumQuestions].Text = "Clos&e";
            close_arr[NumQuestions].UseVisualStyleBackColor = true;
            close_arr[NumQuestions].Click += new System.EventHandler(this.btnCLS_Click);

            clear_arr[NumQuestions] = new Button();
            clear_arr[NumQuestions].BackColor = BackColorTheme;
            clear_arr[NumQuestions].ForeColor = ForeColorTheme;
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
            txtbx_reply_arr[NumQuestions].BorderStyle = BorderStyle.FixedSingle;
            txtbx_reply_arr[NumQuestions].Multiline = true;
            txtbx_reply_arr[NumQuestions].Name = " txtbx_reply_arr_" + NumQuestions.ToString();
            txtbx_reply_arr[NumQuestions].Size = new System.Drawing.Size(274, 51);
            txtbx_reply_arr[NumQuestions].TabIndex = 1;
            txtbx_reply_arr[NumQuestions].KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);

            group_arr[NumQuestions] = new System.Windows.Forms.GroupBox();  // Instantiate next index of groupbox array
            group_arr[NumQuestions].BackColor = BackColorTheme;
            group_arr[NumQuestions].ForeColor = ForeColorTheme;
            group_arr[NumQuestions].Location = origingrouparr; //insert new question at the top
            group_arr[NumQuestions].Name = "group_arr_" + NumQuestions.ToString();
            group_arr[NumQuestions].Size = new System.Drawing.Size(328, 32);
            group_arr[NumQuestions].TabIndex = 2;
            group_arr[NumQuestions].TabStop = false;
            group_arr[NumQuestions].MouseDown +=new MouseEventHandler(group_arr_MouseDown);

            lbl_arr[NumQuestions] = new Label();
            lbl_arr[NumQuestions].AutoSize = false;
            lbl_arr[NumQuestions].AutoEllipsis = true;
            lbl_arr[NumQuestions].TextAlign = System.Drawing.ContentAlignment.TopCenter;
            lbl_arr[NumQuestions].BackColor = BackColorTheme;
            lbl_arr[NumQuestions].ForeColor = ForeColorTheme;
            lbl_arr[NumQuestions].FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            lbl_arr[NumQuestions].Dock = DockStyle.Top;
            lbl_arr[NumQuestions].Location = lbl_arr_temp;
            lbl_arr[NumQuestions].Name = "lbl_arr_" + NumQuestions.ToString(); 
            lbl_arr[NumQuestions].Size = new System.Drawing.Size(330, 15);
            lbl_arr[NumQuestions].TabIndex = 0;
            if (showNamesToolStripMenuItem.Checked)
            {
                lbl_arr[NumQuestions].Text = "***" + "          " + question + "  -" + tempString2; 
                Q_NameShowing[NumQuestions] = true; //flag for name visibility
            }
            else
            {
                lbl_arr[NumQuestions].Text = "***" + "          " + question;
                Q_NameShowing[NumQuestions] = false;
            }
            lbl_arr[NumQuestions].MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbl_question_MouseDown);
            lbl_arr[NumQuestions].MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbl_question_MouseMove);

            picbx_arr[NumQuestions] = new PictureBox();
            picbx_arr[NumQuestions].BackColor = BackColorTheme;
            picbx_arr[NumQuestions].Image = global::SrP_ClassroomInq.Properties.Resources.reply_arrow;
            picbx_arr[NumQuestions].Location = picbx_arr_tmp;
            picbx_arr[NumQuestions].Name = "picbx_arr_" + NumQuestions.ToString();
            picbx_arr[NumQuestions].Size = new System.Drawing.Size(24, 20); 
            picbx_arr[NumQuestions].SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            picbx_arr[NumQuestions].TabIndex = 0;
            picbx_arr[NumQuestions].TabStop = false;
            picbx_arr[NumQuestions].Visible = false; //set to visible after marked read

            picbx_ConvView_arr[NumQuestions] = new PictureBox();
            picbx_ConvView_arr[NumQuestions].BackColor = BackColorTheme;
            picbx_ConvView_arr[NumQuestions].Image = global::SrP_ClassroomInq.Properties.Resources.bubbles2;
            picbx_ConvView_arr[NumQuestions].Location = picbx_ConvView_tmp;
            picbx_ConvView_arr[NumQuestions].Name = "picbx_ConvView_arr_" + NumQuestions.ToString();
            picbx_ConvView_arr[NumQuestions].Size = new System.Drawing.Size(30, 24); 
            picbx_ConvView_arr[NumQuestions].SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            picbx_ConvView_arr[NumQuestions].TabIndex = 0;
            picbx_ConvView_arr[NumQuestions].TabStop = false;
            picbx_ConvView_arr[NumQuestions].Visible = true; //set to visible after marked read
            picbx_ConvView_arr[NumQuestions].Click += new System.EventHandler(this.ConvViewPic_Click);
            picbx_ConvView_arr[NumQuestions].MouseEnter += new System.EventHandler(this.picbx_CV_MouseEnter);
            picbx_ConvView_arr[NumQuestions].MouseLeave += new System.EventHandler(this.picbx_CV_MouseLeave);

            tt_picbxCV_arr[NumQuestions] = new ToolTip();
            tt_picbxCV_arr[NumQuestions].BackColor = BackColorTheme;
            tt_picbxCV_arr[NumQuestions].ForeColor = ForeColorTheme;
            tt_picbxCV_arr[NumQuestions].IsBalloon = true;
            tt_picbxCV_arr[NumQuestions].ToolTipTitle = "Click the icon!";
            tt_picbxCV_arr[NumQuestions].AutoPopDelay = 2500;
            tt_picbxCV_arr[NumQuestions].InitialDelay = 300;
            tt_picbxCV_arr[NumQuestions].ReshowDelay = 1000;
            tt_picbxCV_arr[NumQuestions].UseFading = true;
            tt_picbxCV_arr[NumQuestions].UseAnimation = true;

            group_arr[NumQuestions].Controls.Add(reply_arr[NumQuestions]);
            group_arr[NumQuestions].Controls.Add(close_arr[NumQuestions]);
            group_arr[NumQuestions].Controls.Add(clear_arr[NumQuestions]);
            group_arr[NumQuestions].Controls.Add(txtbx_reply_arr[NumQuestions]);
            group_arr[NumQuestions].Controls.Add(lbl_arr[NumQuestions]);
            group_arr[NumQuestions].Controls.Add(picbx_arr[NumQuestions]);
            group_arr[NumQuestions].Controls.Add(picbx_ConvView_arr[NumQuestions]);
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
                this.Text = " Classroom Inquisition  |  Home (" + UnreadCount.ToString() + ")";
            }
            else
            {
                picbxStatus.Visible = true;
                tlstrplbl_Unread.Visible = false;
            }
            if (chkbxNotify.Checked)
            {
                trayICON.BalloonTipTitle = tempString2 + " asked:";
                trayICON.BalloonTipText = question;
                trayICON.ShowBalloonTip(1250); //show for 1.25 seconds
            }
            NumQuestions++; //increment number of ctrls
        }
        
        /*This method handles the backspace character, formats questions, and fills arrays for later use*/
        public string HandleBCKSPC(string RAW_DATA) 
        {
            string tmpString = RAW_DATA;
            string finalString = "";
            string[] strARRAY = new string[RAW_DATA.Length];
            char tmp=' ';
            char tmp_response=' ';
            strARRAY = tmpString.Split('\b');
            if (RAW_DATA.Length > 5)//contains at least stx,addr,msg,etx,LF
            {
                foreach (string str in strARRAY)
                {
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
                        finalString += str.Substring(0, (str.Length - 1)); // if you fudge and backspace the second letter you type
                    }
                }

                tmp = finalString[1]; //this should be the address
                tmp_response = finalString[2]; //if this is a flowcontrol string otherwise this would be the first char of the message
                finalString = finalString.TrimStart('\x02'); //clean up rx'd string start of transmit char
                finalString = finalString.TrimEnd('\x03'); //trim end of transmit

                if ((tmp_response != 0x05) && (tmp_response != 0x06)) //if not enq or ack
                {
                    switch(StateMachine){
                        
                        case("Quiz"):
                            finalString = finalString.TrimStart(tmp); //trim off the address
                            for (int i = 0; i < Students_ID.Length; i++)
                            {
                                if (String.CompareOrdinal(addr_tbl[i], tmp.ToString().TrimStart('\x27').TrimEnd('\x27')) == 0) //CompareOrdinal Compares Numerical value
                                {
                                    tempString2 = Students_Name[i];
                                    break;
                                }
                            }
                            break;

                        case("Normal"):
                        default:
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
                        break;
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
            else
            {
                return "";
            }
        }
        
        /*This method allows for key presses to be detected and used even by controls which don't have keypress events*/
        public void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)

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
                if (ClassVoteShowing)
                {
                    btnExitClassVote_Click(sender, e);
                }
                else if (AttendanceShowing)
                {
                    btnExitAttendance_Click(sender, e);
                }
                else if (QuizModeShowing)
                {
                    btnExitQuiz_Click(sender, e);
                }
                else if (QuizMakerShowing)
                {
                    btnQMcls_Click(sender, e);
                }
                else if (ConvViewShowing)
                {
                    ConvViewClicked = true;
                    timer.Enabled = true;
                }
                else if (StuMgmtShowing)
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
        
        /*This is the method which quits the app from the menu bar*/
		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
            this.Close(); // there is a prompt in form closing
		}
        
        /*This method determines what happens when you click the broadcast box*/
		private void textBox1_MouseClick(object sender, MouseEventArgs e)
		{
			timer.Enabled = true;
			textbox1WASclicked = true;
            //grpbxRPL_WASclicked = false;
            textBox1.Clear();        
			
		}
        
        /*This method allows for minimize to tray functionality*/
		private void trayICON_MouseDoubleClick(object sender, MouseEventArgs e)
		{
            trayICON.BalloonTipTitle ="Classroom Inquistion";
            trayICON.BalloonTipText = "Raising Hands is a thing of the past";

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

        /*This clears the question repl box of the specifically clicked question*/
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
        
        /*This handles the replies for every question in the feed box*/
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
        
        /*This allows the click to open/close questions as well as mark read and show context menus*/
		private void lbl_question_MouseDown(object sender, MouseEventArgs e)
		{
            bool rightClick = (e.Button == System.Windows.Forms.MouseButtons.Right);
            bool leftClick = (e.Button == System.Windows.Forms.MouseButtons.Left);
            
            for (int i = 0; i < lbl_arr.Length - 1; i++) //loop through to find the clicked one
                {
                    if (sender.Equals(lbl_arr[i]))
                    {
                        old_lblID = lbl_ID; //save in case a separate question is clicked
                        new_lblID = i; //store new to use later
                    }
                }
            
            if (rightClick)
            {
                //show context menu at mouse click location aligned right
                cntxtMenu.Show(lbl_arr[new_lblID], e.Location, LeftRightAlignment.Right);
            }
            else if (leftClick)
            {
                if (rdbtnClick.Checked)
                {
                    UnreadDecrement(sender);
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
		}
        
        /*Closes the question which it relates to*/
		private void btnCLS_Click(object sender, EventArgs e)
		{
			timer.Enabled = true;
			btnCLS_WASclicked = true;
            timesClicked = 0;
		}
       
        /*This method loads up prefs and other things needed to be initialized at startup*/
		private void Form1_Load(object sender, EventArgs e)
		{
            serialCOMcmbbx_Click(sender, e);//pre-load the combobox         
            //auto fill feed box or something... for testing if needed

            Properties.Settings.Default.Reload();           

            /*Check the state of previous settings and reset them*/
            SecretKey = Properties.Settings.Default.key;
            chkbxLameMode.Checked = Properties.Settings.Default.Animations;
            chkbxRXSound.Checked = Properties.Settings.Default.SoundRX;
            chkbxTXSound.Checked = Properties.Settings.Default.SoundTX;
            rdbtnClick.Checked = Properties.Settings.Default.ClickRead;
            rdbtnHover.Checked = Properties.Settings.Default.HoverRead;
            chkbxTooltips.Checked = Properties.Settings.Default.Tooltips;
            showNamesToolStripMenuItem.Checked = Properties.Settings.Default.ShowNames;
            chkbxNotify.Checked = Properties.Settings.Default.Notify;
            chkbxCtrlHide.Checked = Properties.Settings.Default.CtrlHide;
            ForeColorTheme = Properties.Settings.Default.ForeColorTheme;
            BackColorTheme = Properties.Settings.Default.BackColorTheme;
            timer.Interval = Properties.Settings.Default.AnimationSpeed;
            /***************************************************/

            ThemeApply(ForeColorTheme, BackColorTheme); //set from saved theme

            string[] tmpstring = new string[classSize];
            try
            {
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
                }
                catch (DirectoryNotFoundException dirEx)
                {
                    Directory.CreateDirectory(@".data"); //in case the directory didn't exist
                }
                catch (FileNotFoundException fileEx)
                {
                    File.Create(PlainData); //recreate the file
                }

                lstbxStudents.Items.Clear();
                for (j = 0; j < i; j++)
                {
                    lstbxStudents.Items.Add(Students_Name[j]); //intialize the lstbx
                }
                i = j = 0;
            }
            catch 
            {
                MessageBox.Show("Stuff's not right....");
            }

            try
            {
                try
                {
                    i = 0;
                    using (StreamReader sr = File.OpenText(@".data\Quiz\QuizMaker.txt"))
                    {
                        string tempstr = "";
                        while ((tempstr = sr.ReadLine()) != null)
                        {
                            tmpstring = tempstr.Split('\x0A'); //divvy it up by newline
                            logTmpString[i] = tmpstring[0]; //weird error where tmpstring is only using index 0...this fixes it
                            i++;
                        }
                        sr.Close();
                        for (j = 0; j <= (i - 1); j++)
                        {
                            MakeQuizQuestion(logTmpString[j], j); //populate quiz from data file
                        }
                        i = j = 0;
                    }
                }
                catch (DirectoryNotFoundException dirEx)
                {
                    Directory.CreateDirectory(@".data/Quiz"); // recreate the directory
                }
                catch (FileNotFoundException fileEX)
                {
                    File.Create(@".data\Quiz\QuizMaker.txt"); // recreate the quiz maker file
                }
            }
            catch 
            {
                MessageBox.Show("Oh No's....Quiz file reading broke!!");
            }
            this.Focus();
		}
        
        /*This method shows the PrefsPanel*/
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
        
        /*This method fills the serial port combo box and auto selects a port if there's only one*/
        private void serialCOMcmbbx_Click(object sender, EventArgs e)
        {
            byte tmp = 0;
            serialCOMcmbbx.Items.Clear();
            portNames = System.IO.Ports.SerialPort.GetPortNames();
            foreach (string name in portNames)
            {
                serialCOMcmbbx.Items.Add(name);
                tmp++;
            }
            if (tmp == 1) //there's only 1 serial port, let's connect
            {
                serialCOMcmbbx.SelectedIndex = 0; //choose the first one
                serialCOMcmbbx_SelectedIndexChanged(sender, e);
            }
        }
       
        /*Attempts to open the selected port from the combobox*/
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
        
        /*This method runs while the form is still running but close has been issued, allows cancel of close*/
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!sudo_kill)
            {
                string message = "Leaving so soon?";

                string caption = "Are you Sure you want to do that...?";

                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = MessageBox.Show(this, message, caption, buttons, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SerialPort.Close(); //tie up loose ends..
                    SavePrefs();

                    if (StuMgmtShowing)
                    {
                        SaveStudentData(); //if the panel is showing, on close, save data
                    }
                    if (QuizMakerShowing) // you'll want to save whatever changes they've made
                    {
                        try
                        {
                            using (StreamWriter sw = new StreamWriter(@".data\Quiz\QuizMaker.txt"))
                            {
                                //build and write the strings for the data file
                                for (int i = 0; i < lstbxQuizMaker.Items.Count; i++)
                                {
                                    sw.WriteLine(lstbxQuizMaker.Items[i]);
                                }
                                sw.Flush();
                                sw.Close();
                            }
                        }
                        catch {}
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                SerialPort.Close(); //tie up loose ends..
                SavePrefs();
                if (StuMgmtShowing)
                {
                    SaveStudentData(); //if the panel is showing, on close, save data
                }
                if (QuizMakerShowing) // you'll want to save whatever changes they've made
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(@".data\Quiz\QuizMaker.txt"))
                        {
                            //build and write the strings for the data file
                            for (int i = 0; i < lstbxQuizMaker.Items.Count; i++)
                            {
                                sw.WriteLine(lstbxQuizMaker.Items[i]);
                            }
                            sw.Flush();
                            sw.Close();
                        }
                    }
                    catch { }
                }
            }
        }
        
        /*This shows the about information in a separate dialog*/
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Show About Information            
            About.ShowDialog();
        }
        
        /*This sends a broadcast message from the broadcast box*/
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
        
        /*This is a timer which polls the serial port to see if there is data*/
        private void timer_SerialRead_Tick(object sender, EventArgs e) //my very own SerialDataRX'd event
        {
            string tmp_str = "";
            if (SerialPort.IsOpen == true)
            {
                SerialPort.ReadTimeout = 10; //in miliseconds
                tempString = null;
                try
                {
                    tempString = SerialPort.ReadLine();
                }
                catch {}

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
                    tmp_str = HandleBCKSPC(RX_Data);
                    if (tmp_str == "")// handle backspace (0x08) before printing questions
                    {
                        //if "" is true then the string received was a flwctrl string, and it's taken care of
                    }
                    else // we can decide what to do based on program state
                    {
                        switch (StateMachine)
                        {
                            case("Quiz"):
                                RecordQuizData(tmp_str, tempString2); //question, student's name
                                break;

                            case("Attendance"):
                                //we're in attendance mode
                                break;

                            case("ClassVote"):
                                //in class vote mode
                                break;

                            case("Normal"): //normal is the default
                            default:
                                 UnreadCount++;
                                 Qs_to_create[ix++] = tmp_str; //store and increment pointer
                                 Qs_to_Make = true; //set the flag
                                 timer_ControlsCreate.Enabled = true;
                                 break;                            
                        }
                    }
                    WeGotData = false;

                }
                else
                {
                    if (sb_send_status.Text == "Message Sent")
                    {
                        timesThrough++; 
                        if (timesThrough >= 5) //~500ms
                        {
                            sb_send_status.Text = "Ready to Communicate"; //reset the status bar
                            timesThrough = 0;
                        }
                    }
                }
                
            }
        }
        
        /*This allows for unread decrement by mouse hover*/
        private void lbl_question_MouseMove(object sender, MouseEventArgs e)
        {
            if (rdbtnHover.Checked)
            {
                UnreadDecrement(sender);
            }
        }
        
        /*This opens the direct message panel*/
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
        
        /*This closes the direct message panel*/
        private void btnDM_Cls_Click(object sender, EventArgs e)
        {
            DMclicked = true;
            timer.Enabled = true;
        }
        
        /*This opens the broadcast box*/
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
        
        /*This closes the broadcast box*/
        private void btnCLS_Click_1(object sender, EventArgs e)
        {
            textbox1WASclicked = true;
            timer.Enabled = true;
            grpbxFeed.Focus();
        }
        
        /*This closes preferences*/
        private void btnPrefs_Cls_Click(object sender, EventArgs e)
        {
            SavePrefs();
            PrefsClicked = true;
            timer.Enabled = true;
        }
        
        /*Clears the Direct message box*/
        private void btnDM_Clr_Click(object sender, EventArgs e)
        {
            txtbxDM.ResetText();
        }
        
        /*This sends a direct message to the student selected in the DMcombobx*/
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
        
        /*Closes the FAQ Panel*/
        private void btnFAQcls_Click(object sender, EventArgs e)
        {
            FAQClicked = true;
            timer.Enabled = true;

            if (StuMgmtShowing)
            {
                PanelStudents.Focus();
            }
        }
        
        /*Opens the FAQ Panel, button in the Help menu*/
        private void generalFAQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FAQClicked = true;
            timer.Enabled = true;
            PanelFAQ.Focus();
        }
        
        /*This allows the emailing of the developer*/
        private void lnklblFAQ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("mailto:bondslaveofJesus@gmail.com");
            }
            catch { }
        }
        
        /*This shows the Student Management Center and propigates the listbox from the data file*/
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
        
        /*This closes the Student Management Center*/
        private void btnCLSStudents_Click(object sender, EventArgs e)
        {
            StuMgmtClicked = true;
            timer.Enabled = true;
            
            SaveStudentData();
            PanelPrefs.Focus();
        }
        
        /*This saves the information which may have been changed about the students in StuMgmt*/
        public void SaveStudentData()
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
        
        /*This allows the editing of student names*/
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
        
        /*This allows for the resetting of the name to default*/
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
        
        /*This completes the rename*/
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
        
        /*Propigates the combobx from the student data file*/
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
        
        /*This selects to student to be messaged and set the address variable appropriately*/
        private void cmbxDM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbxDM.SelectedIndex != -1)
            {
                addr = addr_tbl[cmbxDM.SelectedIndex];
            }
            DirectMsgPanel.Focus(); //so esc key will work
        }
        
        /*This is intended to provide a software flow control (but isn't used yet...)*/
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
        
        /*This creates the controls in a timely manner so that they don't get created on top of one another */
        private void timer_ControlsCreate_Tick(object sender, EventArgs e) 
        { //this event goes off when timer enabled, every 1 seconds
            if (Qs_to_Make)
            {
                if (ix > jx) // we haven't made all the controls yet
                {
                    LogQandA(Qs_to_create[jx], false, Q_sender[jx]);
                    MakeCtrls(Qs_to_create[jx++]); //make controls and increment creation counter
                    sb_send_status.Text = "New Message Received!";
                    picbx_ConvView_arr[jx-1].BringToFront(); //shows the "show conv view pic"
                }
                else
                {
                    Qs_to_Make = false; //stop trying to make things
                }
            }
            else //No more changes to make!
            {
                timer_ControlsCreate.Enabled = false;
                sb_send_status.Text = "Ready to Communicate";
            }            
        }
        
        /*The method is responsible for sending all the serial messages*/
        public bool SendMsg(string Message2Send, string Address2Send)
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
            sb_send_status.Text = SuccessOfSending ? "Message Sent" : "Message Sending Failed!"; 
            return SuccessOfSending; //lets sender know if sending succeeded
        }
        
        /*Save the preferences whenever called*/
        public void SavePrefs()
        {
            Properties.Settings.Default.Animations = chkbxLameMode.Checked;
            Properties.Settings.Default.SoundRX = chkbxRXSound.Checked;
            Properties.Settings.Default.SoundTX = chkbxTXSound.Checked;
            Properties.Settings.Default.ClickRead = rdbtnClick.Checked;
            Properties.Settings.Default.HoverRead = rdbtnHover.Checked;
            Properties.Settings.Default.Tooltips = chkbxTooltips.Checked;
            Properties.Settings.Default.ShowNames = showNamesToolStripMenuItem.Checked;
            Properties.Settings.Default.Notify = chkbxNotify.Checked;
            Properties.Settings.Default.CtrlHide = chkbxCtrlHide.Checked;
            Properties.Settings.Default.ForeColorTheme = ForeColorTheme;
            Properties.Settings.Default.BackColorTheme = BackColorTheme;
            Properties.Settings.Default.AnimationSpeed = (byte)timer.Interval;
            Properties.Settings.Default.Save();
        }
        
        /*This method decrements the unread count smartly*/
        public void UnreadDecrement(object sender)
        {
            char tmp = (char)UnreadCount; //contains old value
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
                    this.Text = " Classroom Inquisition  |  Home (" + UnreadCount.ToString() + ")";
                }
                else
                {
                    picbxStatus.Visible = true;
                    tlstrplbl_Unread.Visible = false;
                    this.Text = " Classroom Inquisition  |  Home";
                }
            }
        }
        
        /*This allows the user to submit issues from the FAQ*/
        private void lnklblIssues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/dchriste/Classroom-Inquisition/issues");
            }
            catch { }
        }
        
        /*This method logs all questions and answers*/
        public bool LogQandA(string dialog, bool teacher, string recipient)
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
                logFiles[student] = @".logs\Student" + student.ToString() + "-log.txt"; //Student#-log.txt in hidden folder .logs
                if (Directory.Exists(@".logs"))
                {
                    if (File.Exists(logFiles[student]) == false)
                    {
                        using (FileStream fs = File.Create(logFiles[student]))
                        {
                            fs.Close();
                        }
                    }
                }
                else
                {
                    Directory.CreateDirectory(@".logs");
                    using (FileStream fs = File.Create(logFiles[student])) //if folder is gone so was the file
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
        
        /*This closes the conversation view*/
        private void btnCLS_ConvView_Click(object sender, EventArgs e)
        {
            //hide the conversation view...
            ConvViewClicked = true;
            timer.Enabled = true;
        }
        
        /*This opens and fills the conversation view*/
        private void btnConvView_Click(object sender, EventArgs e)
        {
            //show the conversation viewer for the selected student.....
            string[] tmpstring = new string[logHistoryLength];
            if (lstbxStudents.SelectedIndex == -1)
            {
                MessageBox.Show("You haven't selected a Student to read yet.");
            }
            else
            {
                try
                {
                    if (File.Exists(@".logs\Student" + lstbxStudents.SelectedIndex.ToString() + "-log.txt"))
                    {
                        i = 0;
                        using (StreamReader sr = File.OpenText(@".logs\Student" + lstbxStudents.SelectedIndex.ToString() + "-log.txt")) //dynamically pick the logfile
                        {
                            string tempstr = "";
                            while ((tempstr = sr.ReadLine()) != null)
                            {
                                tmpstring = tempstr.Split('\x0A'); //divvy it up by newline
                                logTmpString[i] = tmpstring[0]; //weird error where tmpstring is only using index 0...this fixes it
                                i++;
                            }
                            sr.Close();
                            lstbxConvView.Items.Clear(); //clean up last view
                            for (j = 0; j < (i - 1); j++)
                            {
                                lstbxConvView.Items.Add(logTmpString[j]); //repopulate the lstbx
                            }
                            i = j = 0;
                        }
                    }
                    else
                    {
                        lstbxConvView.Items.Clear();
                        lstbxConvView.Items.Add("There isn't a conversation to view with this student.");
                    }
                }
                catch (Exception E)
                {
                    MessageBox.Show("This is embarassing..there were Errors..." + Environment.NewLine + E);
                }
                ConvViewClicked = true;
                timer.Enabled = true;
            }
        }
        
        /*Allows for esc key to be able to exit the panel normally, see checkkeys()*/
        private void lstbxConvView_MouseLeave(object sender, EventArgs e)
        {
            PanelConvView.Focus();
        }
        
        /*This method allows for conversation view to be opened dynamically per question*/
        private void ConvViewPic_Click(object sender, EventArgs e)
        {
            /*Figure out who sent the message and select their student in mgmt for even call to work*/
            byte student = 0;
            for (int i = 0; i < picbx_ConvView_arr.Length - 1; i++) //loop through to find the clicked one
            {
                if (sender.Equals(picbx_ConvView_arr[i]))
                {
                    student = (byte) i; 
                    break;
                }
            }
            for (int i = 0; i < Students_ID.Length; i++) //loop to find student name by comparing addresses
            {
                if (String.CompareOrdinal(addr_tbl[i], Q_sender[student]) == 0) //CompareOrdinal Compares Numerical value
                {
                    lstbxStudents.SetSelected(i, true); //selects the correct item so the next event call succeeds
                    break;
                }
            }
            /****************************************/
            btnConvView_Click(sender, e);
        }
        
        /*This method dynamically shows tooltips for questions when the mouse enters the cv pic*/
        private void picbx_CV_MouseEnter(object sender, EventArgs e)
        {
            if (chkbxTooltips.Checked)
            {
                byte student = 0;
                for (int i = 0; i < picbx_ConvView_arr.Length - 1; i++) //loop through to find the clicked one
                {
                    if (sender.Equals(picbx_ConvView_arr[i]))
                    {
                        student = (byte)i;
                        break;
                    }
                }
                /*The below is a workaround for a tooltip bug which doesn't set the relative location until after showing*/
                tt_picbxCV_arr[student].Show("To open the Conversation Viewer", picbx_ConvView_arr[student], 29, 20, 1);
                tt_picbxCV_arr[student].Show("To open the Conversation Viewer", picbx_ConvView_arr[student], 29, 20, 5000);
                /*********************************************************************************************************/
            }
        }
        
        /*This method dynamically hides tooltips for questions when the mouse leaves the cv pic*/
        private void picbx_CV_MouseLeave(object sender, EventArgs e)
        {
            if (chkbxTooltips.Checked)
            {
                byte student = 0;
                for (int i = 0; i < picbx_ConvView_arr.Length - 1; i++) //loop through to find the clicked one
                {
                    if (sender.Equals(picbx_ConvView_arr[i]))
                    {
                        student = (byte)i;
                        break;
                    }
                }
                tt_picbxCV_arr[student].Hide(picbx_ConvView_arr[student]); //make it hide if your not hovering
            }
        }
        
        /*This method preps the printstream for the print page method use later*/
        private void btnCV_Print_Click(object sender, EventArgs e)
        {
            //helped by: http://msdn.microsoft.com/en-us/library/system.drawing.printing.printdocument(v=vs.100).aspx
            if (File.Exists(@".logs\Student" + lstbxStudents.SelectedIndex.ToString() + "-log.txt"))
            {
                try
                {
                    Stream2Print = new StreamReader(@".logs\Student" + lstbxStudents.SelectedIndex.ToString() + "-log.txt"); //dynamically pick the logfile

                    //print the currently showing log file
                    PrintFont = new Font("Arial", 12);
                    Printer.DocumentName = "ClassroomInq_Conversation";
                    Printer.OriginAtMargins = true;
                    Printer.Print();
                }
                catch (Exception E)
                {
                    MessageBox.Show("This is embarassing..there were Errors..." + Environment.NewLine + E);
                }
                finally
                {
                    Stream2Print.Close();
                }
            }
            else
            {
                lstbxConvView.Items.Clear();
                lstbxConvView.Items.Add("There isn't a conversation to print.");
            }
            
        }
        
        /*This method allows the refresh of the conversation view*/
        private void btnCV_Refresh_Click(object sender, EventArgs e)
        {
            //re-read the log file showing to see if there were changes and update
            string[] tmpstring = new string[logHistoryLength];
           
            try
            {
                if (File.Exists(@".logs\Student" + lstbxStudents.SelectedIndex.ToString() + "-log.txt"))
                {
                    i = 0;
                    using (StreamReader sr = File.OpenText(@".logs\Student" + lstbxStudents.SelectedIndex.ToString() + "-log.txt")) //dynamically pick the logfile
                    {
                        string tempstr = "";
                        while ((tempstr = sr.ReadLine()) != null)
                        {
                            tmpstring = tempstr.Split('\x0A'); //divvy it up by newline
                            logTmpString[i] = tmpstring[0]; //weird error where tmpstring is only using index 0...this fixes it
                            i++;
                        }
                        sr.Close();
                        lstbxConvView.Items.Clear(); //clean up last view
                        for (j = 0; j < (i - 1); j++)
                        {
                            lstbxConvView.Items.Add(logTmpString[j]); //repopulate the lstbx
                        }
                        lstbxConvView.Refresh();
                        i = j = 0;
                    }
                }
                else
                {
                    lstbxConvView.Items.Clear();
                    lstbxConvView.Items.Add("There isn't a conversation to view with this student.");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show("This is embarassing..there were Errors..." + Environment.NewLine + E);
            }
        }
        
        /*This method allows the teacher to print a conversation log file if he/she so chooses*/
        private void Printer_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //adapted from:  http://msdn.microsoft.com/en-us/library/system.drawing.printing.printdocument(v=vs.100).aspx
            float linesPerPage = 0;
            float yPosition = 0;
            int count = 0;
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top / 3;

            string line = null;

            
                try
                {                    
                    // Calculate the number of lines per page.
                    linesPerPage = e.MarginBounds.Height / PrintFont.GetHeight(e.Graphics);

                    // Print each line of the file.
                    while ((count < linesPerPage) && ((line = Stream2Print.ReadLine()) != null))
                    {
                        yPosition = topMargin + (count * PrintFont.GetHeight(e.Graphics));
                        e.Graphics.DrawString(line, PrintFont, Brushes.Black, leftMargin, yPosition, new StringFormat());
                        count++;
                    }
                    // If more lines exist, print another page.
                    if (line != null)
                    {
                        e.HasMorePages = true;
                    }
                    else
                    {
                        e.HasMorePages = false;
                    }
                }
                catch (Exception E)
                {
                    MessageBox.Show("Printing Failed!");
                }
    
        }
        
        /*This method allows the user to delete a question from the feed*/
        private void muItmDelete_Click(object sender, EventArgs e)
        {
            byte student = (byte)new_lblID;
            string caption = "";

            for (i = 0; i < Students_Name.Length - 1; i++)
            {
                if (String.CompareOrdinal(addr_tbl[i], Q_sender[student]) == 0) 
                {
                    student = (byte) i;//lock in the name by looking up address
                    break; //otherwise the loop runs too far
                }
            }
            

            string message = "Are you sure you want to Delete this?";

            caption = "About to Delete Question from " + Students_Name[student];

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show(this, message, caption, buttons, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                //delete the question
                DeleteQuestion = true;
                Del_ID = (byte)new_lblID;
                timer.Enabled = true;
            }
            else
            {
                //question still exists
            }
        }
        
        /*This method builds the context menu based on which control is desiring a context menu*/
        private void cntxtMenu_arr_Popup(object sender, EventArgs e)
        {
            cntxtMenu.MenuItems.Clear();//make clean for new context menu

           if (cntxtMenu.SourceControl == lbl_arr[new_lblID]) //if a question label is context clicked
           {
               cntxtMenu.MenuItems.Add(muItmName);//add the toggle name
               cntxtMenu.MenuItems.Add(muItmDelete); //add context menu for delete
           }
           else if (cntxtMenu.SourceControl == group_arr[new_lblID])
           {
               cntxtMenu.MenuItems.Add(muItmName);//add the toggle name
               cntxtMenu.MenuItems.Add(muItmDelete); //add context menu for delete
           }
           else if (cntxtMenu.SourceControl == grpbxFeed)
           {
               if (ModifierKeys == System.Windows.Forms.Keys.Shift)
               {
                   cntxtMenu.MenuItems.Add(muItmQuit); //hidden quit option ;)
               }
               else
               {
                   if (Question_Deleted)
                   {
                       cntxtMenu.MenuItems.Add(muItmUndo);
                   }
                   cntxtMenu.MenuItems.Add(muItmDM);
                   cntxtMenu.MenuItems.Add(muItmFAQ);
                   cntxtMenu.MenuItems.Add(muItmPrefs);
                   cntxtMenu.MenuItems.Add(muItmQuiz);
                   cntxtMenu.MenuItems.Add(muItmAttendance);
                   cntxtMenu.MenuItems.Add(muItmClassVote);
               }
           }
           else if (cntxtMenu.SourceControl == btnQMDel)
           {
               cntxtMenu.MenuItems.Add(muItmQuizDelete);
           }
           
        }
        
        /*This method is intended to be an extension of the mousedown for lbl_arr incase there is a mis-click*/
        private void group_arr_MouseDown(object sender, MouseEventArgs e)
        {
            bool rightClick = (e.Button == System.Windows.Forms.MouseButtons.Right);
            bool leftClick = (e.Button == System.Windows.Forms.MouseButtons.Left);

            byte student = 0;

            for (int i = 0; i < lbl_arr.Length - 1; i++) //loop through to find the clicked one
            {
                if (sender.Equals(lbl_arr[i]))
                {
                    student = (byte) i; 
                    break;
                }
            }

            if (rightClick)
            {
                //show context menu at mouse click location aligned right
                try
                {
                    cntxtMenu.Show(group_arr[student], e.Location, LeftRightAlignment.Right);
                }
                catch { }
            }
        }
        
        /*This method is detecting mouse clicks and show the context menus*/
        private void grpbxFeed_MouseDown(object sender, MouseEventArgs e)
        {
            bool rightClick = (e.Button == System.Windows.Forms.MouseButtons.Right);
            bool leftClick = (e.Button == System.Windows.Forms.MouseButtons.Left);

            if (rightClick)
            {
                //show context menu at mouse click location aligned right
                cntxtMenu.Show(grpbxFeed, e.Location, LeftRightAlignment.Right);
            }
        }
        
        /*This method allows for the showing of PanelPrefs from the home context menu*/
        private void muItmPrefs_Click(object sender, EventArgs e)
        {
            PrefsClicked = true;
            timer.Enabled = true;
        }
        
        /*This method allows for the showing of PanelDM from the home context menu*/
        private void muItmDM_Click(object sender, EventArgs e)
        {
            DMclicked = true;
            timer.Enabled = true;
        }
        
        /*This method allows for the showing of PanelFAQ from the home context menu*/
        private void muItmFAQ_Click(object sender, EventArgs e)
        {
            FAQClicked = true;
            timer.Enabled = true;
        }
        
        /*This method allows for a secret quit option availble only to those who know where tis*/
        private void muItmQuit_Click(object sender, EventArgs e)
        {
            sudo_kill = true; //bypass are you sure...
            this.Close(); //will be caught and confirmed on Form_closing
        }
        
        /*This method allows for the hiding of the window close control and, inseparably, the icon*/
        private void chkbxCtrlHide_CheckedChanged(object sender, EventArgs e)
        {
            this.ControlBox = !chkbxCtrlHide.Checked; //toggles control
            this.Refresh();
        }
        
        /*Method to call the Undo Delete Animation/Action*/
        private void muItmUndo_Click(object sender, EventArgs e)
        {
            //Del_ID is still the ID of the most recently deleted Item
            Request_Undo = true;
            timer.Enabled = true;
            group_arr[Del_ID].Show();
        }
        
        /*Method for individual name toggling on click of context menu item*/
        private void muItmName_Click(object sender, EventArgs e)
        {
            byte student = (byte)new_lblID;
            for (i = 0; i < Students_Name.Length - 1; i++)
            {
                if (String.CompareOrdinal(addr_tbl[i], Q_sender[student]) == 0)
                {
                    student = (byte)i;//lock in the name by looking up address
                    break; //otherwise the loop runs too far
                }
            }

            if (Q_NameShowing[new_lblID]) //toggle name off
            {
                //replace method returns the new string
                lbl_arr[new_lblID].Text = lbl_arr[new_lblID].Text.Replace((" -" + Students_Name[student]), "");
                Q_NameShowing[new_lblID] = false;
            }
            else
            {
                lbl_arr[new_lblID].Text += " -" + Students_Name[student]; //append name
                Q_NameShowing[new_lblID] = true;
            }

        }
        
        /*Method for the global toggling of name by view\ShowNames menu item*/
        private void showNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //toggle all name values to true or false and change those who need changed
            byte student = 0;
            showNamesToolStripMenuItem.Checked = !showNamesToolStripMenuItem.Checked; //toggle the value

            if (showNamesToolStripMenuItem.Checked)
            {
                for (int i = 0; i < NumQuestions; i++) //go through all the questions
                {
                    if (Q_NameShowing[i] == false) //toggle the false ones
                    {
                        for (j = 0; j < Students_Name.Length - 1; j++)
                        {
                            if (String.CompareOrdinal(addr_tbl[j], Q_sender[i]) == 0)
                            {
                                student = (byte)j;//lock in the name by looking up address
                                break; //otherwise the loop runs too far
                            }
                        }
                        lbl_arr[i].Text += " -" + Students_Name[student]; //append name
                        Q_NameShowing[i] = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < NumQuestions; i++) //go through all the questions
                {
                    if (Q_NameShowing[i] == true) //toggle the true ones
                    {
                        for (j = 0; j < Students_Name.Length - 1; j++)
                        {
                            if (String.CompareOrdinal(addr_tbl[j], Q_sender[i]) == 0)
                            {
                                student = (byte)j;//lock in the name by looking up address
                                break; //otherwise the loop runs too far
                            }
                        } 
                        lbl_arr[i].Text = lbl_arr[i].Text.Replace((" -" + Students_Name[student]), "");
                        Q_NameShowing[i] = false;
                    }
                }
            }
        }

        /*This method opens the quiz maker (via animation)*/
        private void btnQuizMaker_Click(object sender, EventArgs e)
        {
            //this should bring in QuizMaker
            string[] tmpstring = new string[logHistoryLength];
            try
            {
                if (File.Exists(@".data\Quiz\QuizMaker.txt"))
                {
                    i = 0;
                    using (StreamReader sr = File.OpenText(@".data\Quiz\QuizMaker.txt")) 
                    {
                        string tempstr = "";
                        while ((tempstr = sr.ReadLine()) != null)
                        {
                            tmpstring = tempstr.Split('\x0A'); //divvy it up by newline
                            logTmpString[i] = tmpstring[0]; //weird error where tmpstring is only using index 0...this fixes it
                            i++;
                        }
                        sr.Close();
                        lstbxQuizMaker.Items.Clear();
                        for (j = 0; j <= (i - 1); j++)
                        {
                            lstbxQuizMaker.Items.Add(logTmpString[j]); //repopulate the lstbx
                        }
                        i = j = 0;
                    }
                }
                else
                {
                    //file was deleted for some reason...
                    using (FileStream fs = File.Create(@".data\Quiz\QuizMaker.txt")) //make a file please
                    {
                        fs.Close();
                    }

                    lstbxQuizMaker.Items.Clear(); 
                    //maybe tell the teacher the quiz file was gone?
                }
            }
            catch (Exception E)
            {
                MessageBox.Show("This is embarassing..there were Errors..." + Environment.NewLine + E);
            }
            QuizMakerClicked = true;
            timer.Enabled = true;
        }

        /*This method closes the quiz maker (via animation)*/
        private void btnQMcls_Click(object sender, EventArgs e)
        {
            //hide quiz maker
            try
            {
                using (StreamWriter sw = new StreamWriter(@".data\Quiz\QuizMaker.txt"))
                {
                    //build and write the strings for the data file
                    for (int i = 0; i < lstbxQuizMaker.Items.Count; i++)
                    {
                        sw.WriteLine(lstbxQuizMaker.Items[i]);
                    }
                    sw.Flush();
                    sw.Close();
                }
            }
            catch 
            {
                MessageBox.Show("He's dead Jim!");
            }

            QuizMakerClicked = true;
            timer.Enabled = true;
        }

        /*This Allows for the editing of existing questions*/
        private void btnQMEdit_Click(object sender, EventArgs e)
        {
            //edit the currently selected question in Listbox, messagebox to select one if they haven't yet
            if (!EDTClicked)
            {
                if (lstbxQuizMaker.SelectedIndex == -1)
                {
                    MessageBox.Show("You haven't selected a Question to edit yet.");
                }
                else
                {
                    txtbxQM.Text = lstbxQuizMaker.Items[lstbxQuizMaker.SelectedIndex].ToString();
                    txtbxQM.TextAlign = HorizontalAlignment.Left;
                    txtbxQM.Focus();
                    btnQMEdit.Text = "Done"; //multipurpose the button
                    EDTClicked = true;
                }                
            }
            else
            {
                //second time clicked, they're done..
                if ((txtbxQM.Text.Trim() != "") && (txtbxQM.Text != "^^^^ Select a question to edit. ^^^^"))
                {
                    lstbxQuizMaker.Items.Insert(lstbxQuizMaker.SelectedIndex, txtbxQM.Text);
                    lstbxQuizMaker.Items.RemoveAt(lstbxQuizMaker.SelectedIndex);
                    lstbxQuizMaker.Refresh();
                }
                else
                {
                    MessageBox.Show("You must type a question!");                    
                }

                txtbxQM.Text = "^^^^ Select a question to edit. ^^^^";
                txtbxQM.TextAlign = HorizontalAlignment.Center;
                btnQMEdit.Text = "Edit";
                EDTClicked = false;
            }
        }

        /*Allows for the deleting of a question*/
        private void btnQMDel_MouseDown(object sender, MouseEventArgs e)
        {
            bool rightClick = (e.Button == System.Windows.Forms.MouseButtons.Right);
            bool leftClick = (e.Button == System.Windows.Forms.MouseButtons.Left);

            if (rightClick)
            {
                //show context menu for undo delete, if something has been deleted
                if (QuizQuestionDel)
                    cntxtMenu.Show(btnQMDel, e.Location, LeftRightAlignment.Right);
            }
            else
            {
                if (lstbxQuizMaker.SelectedIndex != -1) //something is actually selected
                {
                    QuizDelID = lstbxQuizMaker.SelectedIndex; //to save the ID but also to simplify the code
                    QzQuestionDel = lstbxQuizMaker.Items[QuizDelID].ToString();// save question
                    lstbxQuizMaker.Items.RemoveAt(QuizDelID);
                    if (EDTClicked) //we're in edit mode
                    {
                        //resets textbx in case you were editing the question you deleted
                        txtbxQM.Text = "^^^^ Select a question to edit. ^^^^";
                        txtbxQM.TextAlign = HorizontalAlignment.Center;
                        btnQMEdit.Text = "Edit";
                        EDTClicked = false; //reset flag
                    }
                    QuizQuestionDel = true;
                    trayICON.BalloonTipTitle = "Undo Delete by";
                    trayICON.BalloonTipText = "Right click Delete Button";
                    trayICON.ShowBalloonTip(1200);
                }
                else
                {
                    MessageBox.Show("Select a question to Delete!");
                }
            }
        }

        /*Clears the rename textbox*/
        private void btnQMclr_Click(object sender, EventArgs e)
        {
            txtbxQM.Clear();
            txtbxQM.TextAlign = HorizontalAlignment.Left;
        }

        /*Allows you to add a new question*/
        private void btnAddQM_Click(object sender, EventArgs e)
        {
            if (!EDTClicked)
            {
                if ((txtbxQM.Text.Trim() != "") && (txtbxQM.Text != "^^^^ Select a question to edit. ^^^^"))
                {
                    lstbxQuizMaker.Items.Add(txtbxQM.Text);
                    lstbxQuizMaker.Refresh();
                    txtbxQM.Text = "^^^^ Select a question to edit. ^^^^";

                    txtbxQM.TextAlign = HorizontalAlignment.Center;
                }
                else
                {
                    MessageBox.Show("You must type a question!");
                }
            }
            else
            {
                //add was accidentally clicked while in edit mode, perform edit action instead
                btnQMEdit_Click(sender, e);
            }
        }

        /*This method clears the textbox if it's clicked with nothing or default inside*/
        private void txtbxQM_Click(object sender, EventArgs e)
        {
            if ((txtbxQM.Text.Trim() != "") && (txtbxQM.Text != "^^^^ Select a question to edit. ^^^^"))
            {
                //there's something there
            }
            else
            {
                txtbxQM.Clear();
                txtbxQM.TextAlign = HorizontalAlignment.Left;
            }
        }

        /*Method to create questions on the quiz*/
        public bool MakeQuizQuestion(string question, int number)
        {
            bool success = false;
            int index = number;
            try
            {
                // 
                // lblQuestionNum_*
                // 
                lblQuestionNum_arr[index] = new Label();
                lblQuestionNum_arr[index].AutoSize = true;
                lblQuestionNum_arr[index].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblQuestionNum_arr[index].Location = lblQuestionNum_arr_tmp;
                lblQuestionNum_arr[index].Name = "lblQuestionNum_arr_" + number.ToString();
                lblQuestionNum_arr[index].Size = new System.Drawing.Size(23, 20);
                lblQuestionNum_arr[index].TabIndex = 0;
                lblQuestionNum_arr[index].Text = (number + 1).ToString() + ")";
                // 
                // txtbxQuestion_*
                // 
                txtbxQuestion_arr[index] = new TextBox();
                txtbxQuestion_arr[index].BackColor = System.Drawing.SystemColors.ControlDarkDark;
                txtbxQuestion_arr[index].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                txtbxQuestion_arr[index].ForeColor = ForeColorTheme;
                txtbxQuestion_arr[index].Location = txtbxQuestion_arr_tmp;
                txtbxQuestion_arr[index].Multiline = true;
                txtbxQuestion_arr[index].Name = "txtbxQuestion_arr_" + number.ToString();
                txtbxQuestion_arr[index].ReadOnly = true;
                txtbxQuestion_arr[index].Size = new System.Drawing.Size(252, 37);
                txtbxQuestion_arr[index].TabIndex = 1;
                txtbxQuestion_arr[index].Text = question;
                txtbxQuestion_arr[index].TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                // 
                // btnQuizQuestionSend_*
                // 
                btnQuizQuestionSend_arr[index] = new Button();
                btnQuizQuestionSend_arr[index].Location = btnQuizQuestionSend_arr_tmp;
                btnQuizQuestionSend_arr[index].Name = "btnQuizQuestionSend_arr_" + number.ToString();
                btnQuizQuestionSend_arr[index].Size = new System.Drawing.Size(36, 37);
                btnQuizQuestionSend_arr[index].TabIndex = 2;
                btnQuizQuestionSend_arr[index].Text = "Ask";
                btnQuizQuestionSend_arr[index].UseVisualStyleBackColor = true;
                btnQuizQuestionSend_arr[index].MouseClick += new MouseEventHandler(btnQuizAsk_Click);
                // 
                // PanelQuestion_arr_*
                // 
                PanelQuestion_arr[index] = new Panel();
                PanelQuestion_arr[index].BackColor = BackColorTheme;
                PanelQuestion_arr[index].Controls.Add(btnQuizQuestionSend_arr[number]);
                PanelQuestion_arr[index].Controls.Add(txtbxQuestion_arr[number]);
                PanelQuestion_arr[index].Controls.Add(lblQuestionNum_arr[number]);
                PanelQuestion_arr[index].ForeColor = ForeColorTheme;
                PanelQuestion_arr[index].Location = PanelQuestion_arr_tmp;
                PanelQuestion_arr[index].Name = "PanelQuestion_arr_" + number.ToString();
                PanelQuestion_arr[index].Size = new System.Drawing.Size(330, 63);
                PanelQuestion_arr[index].TabIndex = 0;

                pnlQuiz.Controls.Add(PanelQuestion_arr[index]); //add new question to container

                PanelQuestion_arr_tmp.Y += 63; //move down for next question
                success = true;
            }
            catch 
            {
                success = false;
            }

            return success;
        }

        /*Closes Quiz mode*/
        private void btnExitQuiz_Click(object sender, EventArgs e)
        {
            //exit quiz mode
            QuizModeClicked = true;
            timer.Enabled = true;
            quitToolStripMenuItem.Checked = false; //it won't uncheck itself
            StateMachine = "Normal";
            normalToolStripMenuItem.Checked = true;
        }

        /*Opens quiz mode*/
        private void btnQuizMode_Click(object sender, EventArgs e)
        {
            pnlQuiz.Controls.Clear(); //get rid of old questions
            PanelQuestion_arr_tmp.X = 7;//default coordinates
            PanelQuestion_arr_tmp.Y = 6;// ^^

            for (int i = 0; i < lstbxQuizMaker.Items.Count; i++)
            {
                MakeQuizQuestion(lstbxQuizMaker.Items[i].ToString(), i); //make new questions
            }

            //enter into quiz mode
            quizToolStripMenuItem_Click(sender, e);
        }

        /*Allows for Undoing the delete of a question*/
        private void muItmQuizDelete_Click(object sender, EventArgs e)
        {
            //Undo the delete of a question from quiz            
            lstbxQuizMaker.Items.Insert(QuizDelID, QzQuestionDel);
            lstbxQuizMaker.SelectedIndex = QuizDelID;

            QuizQuestionDel = false;
        }

        /*Method for the sending of questions via broadcast*/
        private void btnQuizAsk_Click(object sender, EventArgs e)
        {
            byte question = 0;
            
            for (int i = 0; i <  btnQuizQuestionSend_arr.Length - 1; i++) //loop through to find the clicked one
            {
                if (sender.Equals(btnQuizQuestionSend_arr[i]))
                {
                    question = (byte) i; 
                    break;
                }
            }

            if(txtbxQuestion_arr[question].Text.Trim() != "")
            {
                SendMsg(txtbxQuestion_arr[question].Text, brdcst_addr);
                RecordQuizData(txtbxQuestion_arr[question].Text, "Teacher");
            }
        }

        /*This allows the toggling of state in menubar, also shows the quiz*/
        private void quizToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file2write = @".data/Quiz/QuizData.txt";

            if (File.Exists(file2write) == true)
            {
                /*If Only File.Move was used the 2nd or 3rd time this ran it would barf, File.Replace Fixes that*/
                if(File.Exists(@".data/Quiz/QuizData-oldQuiz.txt"))
                {
                    File.Replace(file2write, @".data/Quiz/QuizData-oldQuiz.txt", @".data/Quiz/QuizData-bak"); // replace the file
                }
                else
                {
                    File.Move(file2write, @".data/Quiz/QuizData-oldQuiz.txt");//move to a backup              
                }
            }
            using (FileStream fs = File.Create(file2write))
            {
                fs.Close();
            }

            quitToolStripMenuItem.Checked = true;
            normalToolStripMenuItem.Checked = false;
            classVoteToolStripMenuItem.Checked = false;
            attedanceToolStripMenuItem.Checked = false;
            StateMachine = "Quiz"; //program state

            QuizModeClicked = true;
            timer.Enabled = true;
        }

        /*This method allows for the recording of questions and answers for the quiz*/
        public void RecordQuizData(string question, string name_sender)
        {
            string file2write = @".data/Quiz/QuizData.txt";
            try
            {      
                using (StreamWriter sw = File.AppendText(file2write))
                {
                    sw.NewLine = "\x0A"; //sets the writeline terminator
                    sw.WriteLine(name_sender + " ( " + DateTime.Now.ToString("HH:mm:ss") + "  |  " + DateTime.Today.Date.ToString("d") + " )" + ":" + "\x0A" + question + "\x0A");  //this will leave one blank line between each message
                    /*Example:
                     * Teacher (13:10:12  |  4/1/2012):
                     * What is the factorial of 100?
                     * ...
                     * */
                    sw.Flush();
                    sw.Close();
                }
            }
            catch
            {
                MessageBox.Show("Quiz Log Creation // Writing Failed!!!");
            }
        }

        /*Mode for the Attendance Keeping of the class*/
        private void attedanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show attendance panel            
            attedanceToolStripMenuItem.Checked = true;

            normalToolStripMenuItem.Checked = false;
            classVoteToolStripMenuItem.Checked = false;
            quizToolStripMenuItem.Checked = false;

            StateMachine = "Attendance";
            PanelAttendance.BringToFront();
            AttendanceClicked = true;
            timer.Enabled = true;
        }

        /*Closes the attendance mode*/
        private void btnExitAttendance_Click(object sender, EventArgs e)
        {
            attedanceToolStripMenuItem.Checked = false;
            normalToolStripMenuItem.Checked = true;
            classVoteToolStripMenuItem.Checked = false;
            quizToolStripMenuItem.Checked = false;

            StateMachine = "Normal";
            AttendanceClicked = true;
            timer.Enabled = true;
        }

        /*Closes the Class Vote Mode*/
        private void btnExitClassVote_Click(object sender, EventArgs e)
        {
            classVoteToolStripMenuItem.Checked = false;
            normalToolStripMenuItem.Checked = true;
            attedanceToolStripMenuItem.Checked = false;
            quizToolStripMenuItem.Checked = false;

            StateMachine = "Normal";
            ClassVoteClicked = true;
            timer.Enabled = true;
        }

        /*Mode for hosting Class Votes*/
        private void classVoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            attedanceToolStripMenuItem.Checked = false;
            normalToolStripMenuItem.Checked = false;
            classVoteToolStripMenuItem.Checked = true;
            quizToolStripMenuItem.Checked = false;

            StateMachine = "ClassVote";
            ClassVoteClicked = true;
            timer.Enabled = true;
        }

        /*Allows opening of Attendance mode from context menu at home screen*/
        private void muItmAttendance_Click(object sender, EventArgs e)
        {
            attedanceToolStripMenuItem_Click(sender, e);
        }

        /*Allows the entering of Class Vote mode from home screen context menu*/
        private void muItmClassVote_Click(object sender, EventArgs e)
        {
            classVoteToolStripMenuItem_Click(sender, e);
        }

        /*Allows entering Quiz mode from home screen context menu*/
        private void muItmQuiz_Click(object sender, EventArgs e)
        {
            quizToolStripMenuItem_Click(sender, e);
        }

        /*This method allows the choosing of a custom color for theming*/
        private void btnForeColor_Click(object sender, EventArgs e)
        {
            colorDlg.ShowDialog();
            ForeColorTheme = colorDlg.Color;

            ThemeApply(ForeColorTheme, BackColorTheme);
        }

        /*This method allows the choosing of a custom color for theming*/
        private void btnBkgrndColor_Click(object sender, EventArgs e)
        {
            colorDlg.ShowDialog();
            BackColorTheme = colorDlg.Color;

            ThemeApply(ForeColorTheme, BackColorTheme);
        }

        /*This method allows for Resetting the theme to default Purdue Colors*/
        private void btnDefaultTheme_Click(object sender, EventArgs e)
        {
            ForeColorTheme = Color.DarkGoldenrod;
            BackColorTheme = Color.Black;
            ThemeApply(Color.DarkGoldenrod, Color.Black); //default theme
        }

        /*Allows for the speed of animation preference*/
        private void cmbbxAnimationSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbbxAnimationSpeed.SelectedIndex)
            {
                case (-1): //the user selected the text which says speed...  
                    break;

                case (0): //slow
                    timer.Interval = 48; //twice as long as normal
                    break;

                case (1): //normal
                    timer.Interval = 24; //default setting
                    break;

                case (2): //fast
                    timer.Interval = 12; //twice as fast as normal
                    break;

                case (3): //blazing
                    timer.Interval = 6; //4 times faster than normal
                    break;

                default:
                    timer.Interval = 25;
                    break;
            }
        }

   } //end of partial class
} //end of namespace    