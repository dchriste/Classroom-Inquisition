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
          * 
          *  Classroom Inquisition Teacher Application 
          *  Programmer: David Christensen
          *  Bug Tracking: https://github.com/dchriste/Classroom-Inquisition/issues 
          *  
         *****************************************************************************/

        /*This Method allows for the theming of all controls globally*/
        public void ThemeApply(Color Fore, Color Back)
        {
            this.Hide(); //hide while we repaint everything
            this.SuspendLayout();

            #region Prefs Panel Controls
            btnForeColor.ForeColor = Fore;
            btnForeColor.BackColor = Back;

            btnBkgrndColor.ForeColor = Fore;
            btnBkgrndColor.BackColor = Back;

            btnDefaultTheme.ForeColor = Fore;
            btnDefaultTheme.BackColor = Back;

            btnPrefs_Cls.ForeColor = Fore;
            btnPrefs_Cls.BackColor = Back;

            btnStuMgmt_Prefs.ForeColor = Fore;
            btnStuMgmt_Prefs.BackColor = Back;

            btnQuizMaker.ForeColor = Fore;
            btnQuizMaker.BackColor = Back;

            chkbxCtrlHide.ForeColor = Fore;
            chkbxCtrlHide.BackColor = Back;

            chkbxLameMode.ForeColor = Fore;
            chkbxLameMode.BackColor = Back;

            chkbxNotify.ForeColor = Fore;
            chkbxNotify.BackColor = Back;

            chkbxRXSound.ForeColor = Fore;
            chkbxRXSound.BackColor = Back;

            chkbxTooltips.ForeColor = Fore;
            chkbxTooltips.BackColor = Back;

            chkbxTXSound.ForeColor = Fore;
            chkbxTXSound.BackColor = Back;

            grpbxTheme.ForeColor = Fore;
            grpbxTheme.BackColor = Back;

            grpbxUnread.ForeColor = Fore;
            grpbxUnread.BackColor = Back;

            lblAnimationPrefs.BackColor = Back;
            lblAnimationPrefs.ForeColor = Fore;

            lblNotifyTime.BackColor = Back;
            lblNotifyTime.ForeColor = Fore;

            lblSerPt.BackColor = Back;
            lblSerPt.ForeColor = Fore;

            if (Fore == Color.Lime)
            {
                cmbbxAnimationSpeed.BackColor = SystemColors.ControlDarkDark;
                cmbbxAnimationSpeed.ForeColor = Fore;
            }
            else
            {
                cmbbxAnimationSpeed.BackColor = Color.White;
                cmbbxAnimationSpeed.ForeColor = Color.Black;
            }

            PanelPrefs.ForeColor = Fore;
            PanelPrefs.BackColor = Back;
            #endregion

            #region Home Controls
            this.ForeColor = Fore;
            this.BackColor = Back;

            if (Fore == Color.Lime)
            {
                textBox1.BackColor = SystemColors.ControlDarkDark;
                textBox1.ForeColor = Fore;

                serialCOMcmbbx.BackColor = SystemColors.ControlDarkDark;
                serialCOMcmbbx.ForeColor = Fore;
            }
            else
            {
                textBox1.BackColor = Color.White;
                textBox1.ForeColor = Color.Black;

                serialCOMcmbbx.BackColor = Color.White;
                serialCOMcmbbx.ForeColor = Color.Black;
            }
            
            menuStrip1.BackColor = Fore; //for contrast between form back
            menuStrip1.ForeColor = Back;

            viewToolStripMenuItem.BackColor = Fore;
            viewToolStripMenuItem.ForeColor = Back;

            broadcastToolStripMenuItem.BackColor = Fore;
            broadcastToolStripMenuItem.ForeColor = Back;

            directMessageToolStripMenuItem.BackColor = Fore;
            directMessageToolStripMenuItem.ForeColor = Back;

            modesToolStripMenuItem.BackColor = Fore;
            modesToolStripMenuItem.ForeColor = Back;

            showNamesToolStripMenuItem.BackColor = Fore;
            showNamesToolStripMenuItem.ForeColor = Back;

            quizToolStripMenuItem.BackColor = Fore;
            quizToolStripMenuItem.ForeColor = Back;

            normalToolStripMenuItem.BackColor = Fore;
            normalToolStripMenuItem.ForeColor = Back;

            attedanceToolStripMenuItem.BackColor = Fore;
            attedanceToolStripMenuItem.ForeColor = Back;

            classVoteToolStripMenuItem.BackColor = Fore;
            classVoteToolStripMenuItem.ForeColor = Back;

            generalFAQToolStripMenuItem.BackColor = Fore;
            generalFAQToolStripMenuItem.ForeColor = Back;

            aboutToolStripMenuItem.BackColor = Fore;
            aboutToolStripMenuItem.ForeColor = Back;

            quitToolStripMenuItem.BackColor = Fore;
            quitToolStripMenuItem.ForeColor = Back;

            for (int i = 0; i < NumQuestions; i++) //re-skin the questions present
            {
                reply_arr[i].BackColor = BackColorTheme;
                reply_arr[i].ForeColor = ForeColorTheme;

                close_arr[i].BackColor = BackColorTheme;
                close_arr[i].ForeColor = ForeColorTheme;

                clear_arr[i].BackColor = BackColorTheme;
                clear_arr[i].ForeColor = ForeColorTheme;

                if (Fore == Color.Lime)
                {
                    txtbx_reply_arr[i].BackColor = SystemColors.ControlDarkDark;
                    txtbx_reply_arr[i].ForeColor = Fore;
                }
                else
                {
                    txtbx_reply_arr[i].BackColor = Color.White;
                    txtbx_reply_arr[i].ForeColor = Color.Black;
                }

                group_arr[i].BackColor = BackColorTheme;
                group_arr[i].ForeColor = ForeColorTheme;

                lbl_arr[i].BackColor = BackColorTheme;
                lbl_arr[i].ForeColor = ForeColorTheme;

                picbx_arr[i].BackColor = BackColorTheme;

                picbx_ConvView_arr[i].BackColor = BackColorTheme;

                tt_picbxCV_arr[i].BackColor = BackColorTheme;
                tt_picbxCV_arr[i].ForeColor = ForeColorTheme;
            }

            #endregion

            #region Direct Message Controls
            DirectMsgPanel.BackColor = Back;
            DirectMsgPanel.ForeColor = Fore;

            if (Fore == Color.Lime)
            {
                txtbxDM.BackColor = SystemColors.ControlDarkDark;
                txtbxDM.ForeColor = Fore;

                cmbxDM.BackColor = SystemColors.ControlDarkDark;
                cmbxDM.ForeColor = Fore;
            }
            else
            {
                txtbxDM.BackColor = Color.White;
                txtbxDM.ForeColor = Color.Black;

                cmbxDM.BackColor = Color.White;
                cmbxDM.ForeColor = Color.Black;
            }

            btnDM_Clr.BackColor = Back;
            btnDM_Clr.ForeColor = Fore;

            btnDM_Cls.BackColor = Back;
            btnDM_Cls.ForeColor = Fore;

            btnDM_Send.BackColor = Back;
            btnDM_Send.ForeColor = Fore;
            #endregion

            #region BroadCast Controls
            pnlBrdCst.BackColor = Fore;
            pnlBrdCst.ForeColor = Back;

            btnSend.ForeColor = Fore;
            btnSend.BackColor = Back;
            #endregion

            #region FAQ Panel Controls
            PanelFAQ.BackColor = Back;
            PanelFAQ.ForeColor = Fore;

            txtbxFAQ.BackColor = Back;
            txtbxFAQ.ForeColor = Fore;

            lnklblIssues.BackColor = Back;
            lnklblIssues.ForeColor = Fore;

            btnFAQcls.BackColor = Back;
            btnFAQcls.ForeColor = Fore;
            #endregion

            #region StuMgmt Controls
            PanelStudents.BackColor = Back;
            PanelStudents.ForeColor = Fore;

            lstbxStudents.BackColor = Back;
            lstbxStudents.ForeColor = Fore;

            if (Fore == Color.Lime)
            {
                txtbxStudentsName.BackColor = SystemColors.ControlDarkDark;
                txtbxStudentsName.ForeColor = Fore;
            }
            else
            {
                txtbxStudentsName.BackColor = Color.White;
                txtbxStudentsName.ForeColor = Color.Black;
            }

            btnCLRStudents.BackColor = Back;
            btnCLRStudents.ForeColor = Fore;

            btnEDTStudents.BackColor = Back;
            btnEDTStudents.ForeColor = Fore;

            btnDoneStudents.BackColor = Back;
            btnDoneStudents.ForeColor = Fore;

            btnCLSStudents.BackColor = Back;
            btnCLSStudents.ForeColor = Fore;

            btnConvView.BackColor = Back;
            btnConvView.ForeColor = Fore;
            #endregion 

            #region Quiz Maker Controls
            PanelQuizMaker.BackColor = Back;
            PanelQuizMaker.ForeColor = Fore;

            lstbxQuizMaker.BackColor = Back;
            lstbxQuizMaker.ForeColor = Fore;

            if (Fore == Color.Lime)
            {
                txtbxQM.BackColor = SystemColors.ControlDarkDark;
                txtbxQM.ForeColor = Fore;
            }
            else
            {
                txtbxQM.BackColor = Color.White;
                txtbxQM.ForeColor = Color.Black;
            }

            btnQMclr.BackColor = Back;
            btnQMclr.ForeColor = Fore;

            btnQMcls.BackColor = Back;
            btnQMcls.ForeColor = Fore;

            btnQMDel.BackColor = Back;
            btnQMDel.ForeColor = Fore;

            btnQMEdit.BackColor = Back;
            btnQMEdit.ForeColor = Fore;

            btnAddQM.BackColor = Back;
            btnAddQM.ForeColor = Fore;

            btnQuizMode.BackColor = Back;
            btnQuizMode.ForeColor = Fore;
            #endregion

            #region Quiz Mode Controls
            PanelQuizMode.BackColor = Back;
            PanelQuizMode.ForeColor = Fore;

            btnExitQuiz.BackColor = Back;
            btnExitQuiz.ForeColor = Fore;

            //dynamically themed when opened
            #endregion

            #region Conversation Viewer Controls
            PanelConvView.BackColor = Back;
            PanelConvView.ForeColor = Fore;

            lstbxConvView.BackColor = Back;
            lstbxConvView.ForeColor = Fore;

            btnCLS_ConvView.BackColor = Back;
            btnCLS_ConvView.ForeColor = Fore;

            btnCV_Print.BackColor = Back;
            btnCV_Print.ForeColor = Fore;

            btnCV_Refresh.BackColor = Back;
            btnCV_Refresh.ForeColor = Fore;
            #endregion

            #region Attendance Mode Controls
            PanelAttendance.BackColor = Back;
            PanelAttendance.ForeColor = Fore;

            btnExitAttendance.BackColor = Back;
            btnExitAttendance.ForeColor = Fore;

            lstbxAttendance.BackColor = Fore;
            lstbxAttendance.ForeColor = Back;
            #endregion

            #region Class Vote Mode Controls
            PanelClassVote.BackColor = Back;
            PanelClassVote.ForeColor = Fore;

            lblCVLeft.BackColor = Back;
            lblCVLeft.ForeColor = Fore;

            lblCVRight.BackColor = Back;
            lblCVRight.ForeColor = Fore;

            lblOption1.BackColor = Back;
            lblOption1.ForeColor = Fore;

            lblOption2.BackColor = Back;
            lblOption2.ForeColor = Fore;

            lblCVPipe.BackColor = Back;
            lblCVPipe.ForeColor = Fore;

            txtbxCVStats.BackColor = Fore;
            txtbxCVStats.ForeColor = Back;

            btnCVReset.BackColor = Back;
            btnCVReset.ForeColor = Fore;

            btnExitClassVote.BackColor = Back;
            btnExitClassVote.ForeColor = Fore;
            #endregion

            #region About Dialog
            About.BackColor = Back;
            About.ForeColor = Fore;
            #endregion

            #region Notify Panel
            PanelNotify.BackColor = Fore;
            PanelNotify.ForeColor = Back;

            pictureBoxNotify.BackColor = Fore;

            lblNotify.BackColor = Fore;
            lblNotify.ForeColor = Back;
            #endregion

            this.ResumeLayout();
            this.Show(); //tada!!
        }
	}
}