/*******************************************************************************
 * Copyright (C) 2012  David V. Christensen
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 *********************************************************************************/
﻿using System;
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
    /****************************************************************************
     * 
     *  Classroom Inquisition Teacher Application 
     *  Programmer: David Christensen
     *  Bug Tracking: https://github.com/dchriste/Classroom-Inquisition/issues 
     *  
    *****************************************************************************/
    public partial class frmClassrromInq : Form
    {
        /*This method does all the animation for the application*/
        private void timer_Tick(object sender, EventArgs e)
        {
            #region BroadCast Panel

            if ((!BrdcstShowing) && (textbox1WASclicked == true) && (pnlBrdCst.Location.Y < 41))// if not visible
            {
                if (!chkbxLameMode.Checked) //animations
                {
                    if (i < 10)
                    {
                        if (i == 0)
                        {
                            pnlBrdCst.Hide();
                            pnlBrdCst.BringToFront();
                            menuStrip1.BringToFront();
                            pnlBrdCst.Show();
                            aPanelIsMoving = true;
                        }
                        pnlBrdCst.SetBounds(pnlBrdCst.Location.X, pnlBrdCst.Location.Y + 5,
                                             pnlBrdCst.Size.Width, pnlBrdCst.Size.Height);
                        i++;
                    }
                    else if (i < 20)
                    {
                        pnlBrdCst.SetBounds(pnlBrdCst.Location.X, pnlBrdCst.Location.Y + 2,
                                             pnlBrdCst.Size.Width, pnlBrdCst.Size.Height);
                        i++;
                    }/* //uncomment this for bounce effect
                    else if (i < 25)
                    {
                        pnlBrdCst.SetBounds(pnlBrdCst.Location.X, pnlBrdCst.Location.Y + 3,
                                             pnlBrdCst.Size.Width, pnlBrdCst.Size.Height);
                        i++;
                    }
                    else if (i < 30)
                    {
                        pnlBrdCst.SetBounds(pnlBrdCst.Location.X, pnlBrdCst.Location.Y - 3,
                                             pnlBrdCst.Size.Width, pnlBrdCst.Size.Height);
                        i++;
                    }*/
                    else
                    {
                        i = 0;
                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }
                        BrdcstShowing = true;
                        aPanelIsMoving = false;
                        textBox1.Clear();
                        this.Text = " Classroom Inquisition  |  Broadcast" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
                else //no animations
                {
                    //jump straight to whatever position..
                    pnlBrdCst.SetBounds(pnlBrdCst.Location.X, pnlBrdCst.Location.Y + 70,
                                         pnlBrdCst.Size.Width, pnlBrdCst.Size.Height);
                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }
                    BrdcstShowing = true;
                    aPanelIsMoving = false;
                    textBox1.Clear();
                    textbox1WASclicked = false;
                    this.Text = " Classroom Inquisition  |  Broadcast";
                }
            }

            if ((BrdcstShowing) && (textbox1WASclicked == true) && (pnlBrdCst.Location.Y > -46))// if visible, destroy
            {
                if (!chkbxLameMode.Checked)
                {
                    if (i < 10)
                    {
                        if (i == 0)
                        {
                            aPanelIsMoving = true;
                        }
                        pnlBrdCst.SetBounds(pnlBrdCst.Location.X, pnlBrdCst.Location.Y -2,
                                             pnlBrdCst.Size.Width, pnlBrdCst.Size.Height);
                        i++;
                    }
                    else if (i < 20)
                    {
                        pnlBrdCst.SetBounds(pnlBrdCst.Location.X, pnlBrdCst.Location.Y - 5,
                                             pnlBrdCst.Size.Width, pnlBrdCst.Size.Height);
                        i++;
                    }
                    else
                    {
                        i = 0;
                        
                        if (DesirePrefs)
                        {
                            if (!PrefsShowing && !DMPanelShowing)
                            {
                                PrefsClicked = true; //show prefs now      
                                DesirePrefs = false;
                            }
                            else if (DMPanelShowing && !PrefsShowing)
                            {
                                DMclicked = true; //hide
                                //leave desire prefs on..
                            }
                            else
                            {
                                DesirePrefs = false;
                            }
                            
                        }
                        else if (DesireDM)
                        {
                            if (!DMPanelShowing && !PrefsShowing)
                            {
                                DMclicked = true; //show dm now
                                DesireDM = false;
                            }
                            else if (PrefsShowing && !DMPanelShowing)
                            {
                                PrefsClicked = true; //hide this
                                //leaves desire DM on...
                            }
                            else
                            {
                                DesireDM = false;
                            }
                        }
                        else if (DesireFAQ)
                        {
                            if (!FAQShowing)
                            {
                                FAQClicked = true; //show FAQ now      
                            }
                            DesireFAQ = false;
                        }
                        else if (DesireAttendance)
                        {
                            if (!AttendanceShowing)
                            {
                                AttendanceClicked = true; //show Attendance Mode now      
                            }
                            DesireAttendance = false;
                        }
                        else if (DesireClassVote)
                        {
                            if (!ClassVoteShowing)
                            {
                                ClassVoteClicked = true; //show ClassVote Mode now      
                            }
                            DesireClassVote = false;
                        }
                        else if (DesireQuizMd)
                        {
                            if (!QuizModeShowing)
                            {
                                QuizModeClicked = true; //show Quiz Mode now      
                            }
                            DesireQuizMd = false;
                        }
                        else if (DesireQzMkr)
                        {
                            if (!QuizMakerShowing)
                            {
                                QuizMakerClicked = true; //show QuizMaker now      
                            }
                            DesireQzMkr = false;
                        }
                        else if (DesireStuMgmt)
                        {
                            if (!StuMgmtShowing)
                            {
                                StuMgmtClicked = true; //show Student Management now      
                            }
                            DesireStuMgmt = false;
                        }
                        else if (DesireConvView)
                        {
                            if (!ConvViewShowing)
                            {
                                ConvViewClicked = true; //show Conversation Viewer now      
                            }
                            DesireConvView = false;
                        }
                        else
                        {
                            if (!NotifyShowing && !DesireNotify)
                            {
                                timer.Enabled = false;
                            }
                        }
                        aPanelIsMoving = false;
                        textbox1WASclicked = false;
                        textbox1WASclicked = false;
                        BrdcstShowing = false;
                        if (FAQShowing)
                        {
                            this.Text = " Classroom Inquisition  |  FAQ" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                            PanelFAQ.Focus();
                        }
                        else if (ConvViewShowing)
                        {
                            this.Text = " Classroom Inquisition  |  Conversation Viewer" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                            PanelConvView.Focus();
                        }
                        else if (StuMgmtShowing)
                        {
                            this.Text = " Classroom Inquisition  |  Student Managment" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                            PanelStudents.Focus();
                        }
                        else if (PrefsShowing)
                        {
                            this.Text = " Classroom Inquisition  |  Prefs" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                            PanelPrefs.Focus();
                        }
                        else if (DMPanelShowing)
                        {
                            this.Text = " Classroom Inquisition  |  Direct Msg" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                            txtbxDM.Focus();
                        }
                        else
                        {
                            this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                            this.Focus();
                        }
                    }
                }
                else //no animations
                {
                    pnlBrdCst.SetBounds(pnlBrdCst.Location.X, pnlBrdCst.Location.Y - 70,
                                         pnlBrdCst.Size.Width, pnlBrdCst.Size.Height);

                    if (DesirePrefs)
                    {
                        if (!PrefsShowing && !DMPanelShowing)
                        {
                            PrefsClicked = true; //show prefs now      
                            DesirePrefs = false;
                        }
                        else if (DMPanelShowing && !PrefsShowing)
                        {
                            DMclicked = true; //hide
                            //leave desire prefs on..
                        }
                        else
                        {
                            DesirePrefs = false;
                        }

                    }
                    else if (DesireDM)
                    {
                        if (!DMPanelShowing && !PrefsShowing)
                        {
                            DMclicked = true; //show dm now
                            DesireDM = false;
                        }
                        else if (PrefsShowing && !DMPanelShowing)
                        {
                            PrefsClicked = true; //hide this
                            //leaves desire DM on...
                        }
                        else
                        {
                            DesireDM = false;
                        }
                    }
                    else if (DesireFAQ)
                    {
                        if (!FAQShowing)
                        {
                            FAQClicked = true; //show FAQ now      
                        }
                        DesireFAQ = false;
                    }
                    else if (DesireAttendance)
                    {
                        if (!AttendanceShowing)
                        {
                            AttendanceClicked = true; //show Attendance Mode now      
                        }
                        DesireAttendance = false;
                    }
                    else if (DesireClassVote)
                    {
                        if (!ClassVoteShowing)
                        {
                            ClassVoteClicked = true; //show ClassVote Mode now      
                        }
                        DesireClassVote = false;
                    }
                    else if (DesireQuizMd)
                    {
                        if (!QuizModeShowing)
                        {
                            QuizModeClicked = true; //show Quiz Mode now      
                        }
                        DesireQuizMd = false;
                    }
                    else if (DesireQzMkr)
                    {
                        if (!QuizMakerShowing)
                        {
                            QuizMakerClicked = true; //show QuizMaker now      
                        }
                        DesireQzMkr = false;
                    }
                    else if (DesireStuMgmt)
                    {
                        if (!StuMgmtShowing)
                        {
                            StuMgmtClicked = true; //show Student Management now      
                        }
                        DesireStuMgmt = false;
                    }
                    else if (DesireConvView)
                    {
                        if (!ConvViewShowing)
                        {
                            ConvViewClicked = true; //show Conversation Viewer now      
                        }
                        DesireConvView = false;
                    }
                    else
                    {
                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }
                    }
                    aPanelIsMoving = false;
                    textbox1WASclicked = false;
                    textbox1WASclicked = false;
                    BrdcstShowing = false;
                    if (FAQShowing)
                    {
                        this.Text = " Classroom Inquisition  |  FAQ" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        PanelFAQ.Focus();
                    }
                    else if (ConvViewShowing)
                    {
                        this.Text = " Classroom Inquisition  |  Conversation Viewer" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        PanelConvView.Focus();
                    }
                    else if (StuMgmtShowing)
                    {
                        this.Text = " Classroom Inquisition  |  Student Managment" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        PanelStudents.Focus();
                    }
                    else if (PrefsShowing)
                    {
                        this.Text = " Classroom Inquisition  |  Prefs" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        PanelPrefs.Focus();
                    }
                    else if (DMPanelShowing)
                    {
                        this.Text = " Classroom Inquisition  |  Direct Msg" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        txtbxDM.Focus();
                    }
                    else
                    {
                        this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        this.Focus();
                    }
                }
            }

            #endregion

            #region GrpBxRPL_Clicked
            if (grpbxFeed.Controls.Count > 0) //allows textbox click with no questions
            {
                if ((group_arr[lbl_ID].Height < 141) && (grpbxRPL_WASclicked == true))// if not visible
                {
                    Point tmp = new Point();
                    if (!chkbxLameMode.Checked)
                    {
                        if (k < 3) //use groupboxID..
                        {
                            if (k == 0)
                            {
                                aPanelIsMoving = true;
                            }
                            group_arr[lbl_ID].SetBounds(group_arr[lbl_ID].Location.X, group_arr[lbl_ID].Location.Y,
                                                        group_arr[lbl_ID].Size.Width, group_arr[lbl_ID].Size.Height + 2);
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
                            group_arr[lbl_ID].SetBounds(group_arr[lbl_ID].Location.X, group_arr[lbl_ID].Location.Y,
                                                        group_arr[lbl_ID].Size.Width, group_arr[lbl_ID].Size.Height + 3);
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
                            if (!NotifyShowing && !DesireNotify)
                            {
                                timer.Enabled = false;
                            }
                            aPanelIsMoving = false;
                            grpbxRPL_WASclicked = false;
                            txtbx_reply_arr[lbl_ID].Focus(); //places cursor in question reply box on open
                        }
                    }
                    else //no animations
                    {
                        group_arr[lbl_ID].SetBounds(group_arr[lbl_ID].Location.X, group_arr[lbl_ID].Location.Y,
                                                    group_arr[lbl_ID].Size.Width, group_arr[lbl_ID].Size.Height + 105);
                        if (lbl_ID != 0)
                        {
                            for (int i = lbl_ID - 1; i >= 0; i--)// to achieve LIFO
                            {
                                tmp.X = group_arr[i].Location.X;
                                tmp.Y = group_arr[i].Location.Y + 105;
                                group_arr[i].Location = tmp;
                            }
                        }
                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }
                        aPanelIsMoving = false;
                        grpbxRPL_WASclicked = false;
                        txtbx_reply_arr[lbl_ID].Focus(); //places cursor in question reply box on open
                    }
                }
            }

            #endregion

            #region Destroy Reply
            if (grpbxFeed.Controls.Count > 0) //allows textbox click with no questions
            {
                if ((group_arr[lbl_ID].Height > 31) && (btnCLS_WASclicked == true))
                {
                    Point tmp = new Point(); // for dynamic moving of all controls below the clicked one
                    if (!chkbxLameMode.Checked)
                    {

                        if (j < 3)
                        {
                            if (j == 0)
                            {
                                aPanelIsMoving = true;
                            }
                            group_arr[lbl_ID].SetBounds(group_arr[lbl_ID].Location.X, group_arr[lbl_ID].Location.Y,
                                                        group_arr[lbl_ID].Size.Width, group_arr[lbl_ID].Size.Height - 2);
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
                            group_arr[lbl_ID].SetBounds(group_arr[lbl_ID].Location.X, group_arr[lbl_ID].Location.Y,
                                                        group_arr[lbl_ID].Size.Width, group_arr[lbl_ID].Size.Height - 3);
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
                            aPanelIsMoving = false;
                            if (!DesireID)
                            {
                                if (!NotifyShowing && !DesireNotify)
                                {
                                    timer.Enabled = false;
                                }
                                timesClicked = 0; //reset
                            }
                            else
                            {
                                lbl_ID = new_lblID; //open other question
                                grpbxRPL_WASclicked = true;
                                DesireID = false;
                            }
                        }
                    }
                    else //no animations :(
                    {
                        group_arr[lbl_ID].SetBounds(group_arr[lbl_ID].Location.X, group_arr[lbl_ID].Location.Y,
                                                    group_arr[lbl_ID].Size.Width, group_arr[lbl_ID].Size.Height - 105);
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
                        aPanelIsMoving = false;
                        if (!DesireID)
                        {
                            if (!NotifyShowing && !DesireNotify)
                            {
                                timer.Enabled = false;
                            }
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
            }
            #endregion

            #region Delete Question
            //keeps things from being upset when Del_ID is intialized to 255 
            //(so as not to delete the first question that comes in...)
            if (DeleteQuestion)
            {
                if ((group_arr[Del_ID].Height > 0) && (DeleteQuestion == true))// if open and clicked
                {
                    Point tmp = new Point(); // for dynamic moving of all controls below the clicked one
                    if (!chkbxLameMode.Checked)
                    {
                        aPanelIsMoving = true;
                        group_arr[Del_ID].SetBounds(group_arr[Del_ID].Location.X, group_arr[Del_ID].Location.Y,
                                                    group_arr[Del_ID].Size.Width, group_arr[Del_ID].Size.Height - 5);
                        if (Del_ID != 0)
                        {
                            for (int x = Del_ID - 1; x >= 0; x--)
                            {
                                tmp.X = group_arr[x].Location.X;
                                tmp.Y = group_arr[x].Location.Y - 5;
                                group_arr[x].Location = tmp;
                            }
                        }
                    }
                    else
                    {
                        for (int x = Del_ID - 1; x >= 0; x--)
                        {
                            tmp.X = group_arr[x].Location.X;
                            tmp.Y = group_arr[x].Location.Y - group_arr[Del_ID].Size.Height;
                            group_arr[x].Location = tmp;
                        }
                        group_arr[Del_ID].SetBounds(group_arr[Del_ID].Location.X, group_arr[Del_ID].Location.Y,
                                                    group_arr[Del_ID].Size.Width, 0);
                    }
                }
                else if (DeleteQuestion)
                {
                    btnCLS_WASclicked = false; //needed if you delete an open question
                    timesClicked = 0; //reset
                    x = 0;
                    DeleteQuestion = false;
                    Question_Deleted = true;
                    aPanelIsMoving = false;
                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }
                    group_arr[Del_ID].Hide(); //real deleting would mess up a lot of numbers in status arrays etc..
                    trayICON.BalloonTipTitle = "Undo Delete by";
                    trayICON.BalloonTipText = "Right click Feed (grey) & choose Undo Delete";
                    trayICON.ShowBalloonTip(1200);
                }
            }
            #endregion

            #region Undo Delete Question
            //keeps things from being upset when Del_ID is intialized to 255 
            //(so as not to delete the first question that comes in...)
            if (Request_Undo && Question_Deleted)
            {
                if ((group_arr[Del_ID].Height < 32) && (Request_Undo == true))// if open and clicked
                {
                    Point tmp = new Point(); // for dynamic moving of all controls below the clicked one
                    if (!chkbxLameMode.Checked)
                    {
                        aPanelIsMoving = true;
                        group_arr[Del_ID].SetBounds(group_arr[Del_ID].Location.X, group_arr[Del_ID].Location.Y,
                                                    group_arr[Del_ID].Size.Width, group_arr[Del_ID].Size.Height + 4);
                        if (Del_ID != 0)
                        {
                            if (group_arr[Del_ID].Height < 32)
                            {
                                for (int x = Del_ID - 1; x >= 0; x--)
                                {
                                    tmp.X = group_arr[x].Location.X;
                                    tmp.Y = group_arr[x].Location.Y + 4;
                                    group_arr[x].Location = tmp;
                                }
                            }
                            else
                            {
                                for (int x = Del_ID - 1; x >= 0; x--)
                                {
                                    tmp.X = group_arr[x].Location.X;
                                    tmp.Y = group_arr[x].Location.Y + 6; // looks better with 6 than 4 for last run
                                    group_arr[x].Location = tmp;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Del_ID != 0)
                        {
                            group_arr[Del_ID].SetBounds(group_arr[Del_ID].Location.X, group_arr[Del_ID].Location.Y,
                                                        group_arr[Del_ID].Size.Width, 32);
                            for (int x = Del_ID - 1; x >= 0; x--)
                            {
                                tmp.X = group_arr[x].Location.X;
                                tmp.Y = group_arr[x].Location.Y + 32;
                                group_arr[x].Location = tmp;
                            }
                        }
                    }
                }
                else if (Request_Undo)
                {
                    Question_Deleted = false;
                    Request_Undo = false;
                    aPanelIsMoving = false;
                    x = 0;
                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }
                }
            }
            #endregion

            #region MoveCtrlsDown
            if ((NEW_grpbx == true))
            {
                Point TEMP = new Point();
                if (k < 3) //use groupboxID..
                {
                    if (k == 0)
                    {
                        aPanelIsMoving = true;
                    }
                    for (int i = NumQuestions - 1; i >= 0; i--) // to achieve LIFO
                    {
                        TEMP.X = group_arr[i].Location.X;
                        TEMP.Y = group_arr[i].Location.Y + 2;
                        group_arr[i].Location = TEMP;
                    }

                    k++;
                }
                else if ((k > 2) && (k < 12)) //was 19 with 3,6 pixel increments
                {
                    for (int i = NumQuestions - 1; i >= 0; i--)// to achieve LIFO
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
                    aPanelIsMoving = false;
                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }
                    NEW_grpbx = false;
                }
            }

            #endregion

            #region DMPanel Animate
            if ((DirectMsgPanel.Location.X < 16) && (DMclicked == true)
                    && (DMtimesClicked == 0) && (DMPanelShowing == false))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                        }
                        DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X + 16, DirectMsgPanel.Location.Y,
                                                 DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X + 16, grpbxFeed.Location.Y,
                                            grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X + 15, DirectMsgPanel.Location.Y,
                                                 DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X + 15, grpbxFeed.Location.Y,
                                            grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X + 10, DirectMsgPanel.Location.Y,
                                                 DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X + 10, grpbxFeed.Location.Y,
                                            grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        DMPanelShowing = true;
                        DMclicked = false;
                        aPanelIsMoving = false;
                        DMtimesClicked = 1;
                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }
                        DirectMsgPanel.Focus();
                        this.Text = " Classroom Inquisition  |  Direct Msg" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
                else //no animations, boo!
                {
                    DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X + 410, DirectMsgPanel.Location.Y,
                                             DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height);
                    grpbxFeed.SetBounds(grpbxFeed.Location.X + 410, grpbxFeed.Location.Y,
                                        grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                    DMPanelShowing = true;
                    DMclicked = false;
                    aPanelIsMoving = false;
                    DMtimesClicked = 1;
                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }
                    DirectMsgPanel.Focus();
                    this.Text = " Classroom Inquisition  |  Direct Msg" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                }
            }
            else if ((DirectMsgPanel.Location.X > -396) && (DMclicked == true)
                            && (DMtimesClicked == 1) && (DMPanelShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                        }
                        DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X - 10, DirectMsgPanel.Location.Y,
                                                 DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X - 10, grpbxFeed.Location.Y,
                                            grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X - 15, DirectMsgPanel.Location.Y,
                                                 DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X - 15, grpbxFeed.Location.Y,
                                            grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X - 16, DirectMsgPanel.Location.Y,
                                                 DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X - 16, grpbxFeed.Location.Y,
                                            grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        DMPanelShowing = false;
                        DMclicked = false;
                        aPanelIsMoving = false;
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
                            if (!NotifyShowing && !DesireNotify)
                            {
                                timer.Enabled = false;
                            }
                            this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        }
                    }
                }
                else //no animations, boo!
                {
                    DirectMsgPanel.SetBounds(DirectMsgPanel.Location.X - 410, DirectMsgPanel.Location.Y,
                                             DirectMsgPanel.Size.Width, DirectMsgPanel.Size.Height);
                    grpbxFeed.SetBounds(grpbxFeed.Location.X - 410, grpbxFeed.Location.Y,
                                        grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                    DMPanelShowing = false;
                    DMclicked = false;
                    aPanelIsMoving = false;
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
                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }
                        this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
            }


            #endregion

            #region Prefs Animate

            if ((PanelPrefs.Location.X > 14) && (PrefsClicked == true)
                && (PrefsTimesClicked == 0) && (PrefsShowing == false))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                        }
                        PanelPrefs.SetBounds(PanelPrefs.Location.X - 16, PanelPrefs.Location.Y,
                                             PanelPrefs.Size.Width, PanelPrefs.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X - 16, grpbxFeed.Location.Y,
                                            grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelPrefs.SetBounds(PanelPrefs.Location.X - 12, PanelPrefs.Location.Y,
                                             PanelPrefs.Size.Width, PanelPrefs.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X - 12, grpbxFeed.Location.Y,
                                            grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelPrefs.SetBounds(PanelPrefs.Location.X - 10, PanelPrefs.Location.Y,
                                             PanelPrefs.Size.Width, PanelPrefs.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X - 10, grpbxFeed.Location.Y,
                                            grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        PrefsShowing = true;
                        PrefsClicked = false;
                        aPanelIsMoving = false;
                        PrefsTimesClicked = 1;
                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }
                        else
                        {
                            PanelNotify.BringToFront();
                        }
                        PanelPrefs.Focus();
                        this.Text = " Classroom Inquisition  |  Prefs" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
                else //no animations
                {
                    PanelPrefs.SetBounds(PanelPrefs.Location.X - 380, PanelPrefs.Location.Y,
                                         PanelPrefs.Size.Width, PanelPrefs.Size.Height);
                    grpbxFeed.SetBounds(grpbxFeed.Location.X - 380, grpbxFeed.Location.Y,
                                        grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                    PrefsShowing = true;
                    PrefsClicked = false;
                    aPanelIsMoving = false;
                    PrefsTimesClicked = 1;
                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }
                    else
                    {
                        PanelNotify.BringToFront();
                    }
                    PanelPrefs.Focus();
                    this.Text = " Classroom Inquisition  |  Prefs" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                }
            }
            else if ((PanelPrefs.Location.X < 396) && (PrefsClicked == true)
                        && (PrefsTimesClicked == 1) && (PrefsShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                        }
                        PanelPrefs.SetBounds(PanelPrefs.Location.X + 10, PanelPrefs.Location.Y,
                                             PanelPrefs.Size.Width, PanelPrefs.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X + 10, grpbxFeed.Location.Y,
                                            grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelPrefs.SetBounds(PanelPrefs.Location.X + 12, PanelPrefs.Location.Y,
                                             PanelPrefs.Size.Width, PanelPrefs.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X + 12, grpbxFeed.Location.Y,
                                            grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelPrefs.SetBounds(PanelPrefs.Location.X + 16, PanelPrefs.Location.Y,
                                             PanelPrefs.Size.Width, PanelPrefs.Size.Height);
                        grpbxFeed.SetBounds(grpbxFeed.Location.X + 16, grpbxFeed.Location.Y,
                                            grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        PrefsShowing = false;
                        PrefsClicked = false;
                        PrefsTimesClicked = 0;
                        aPanelIsMoving = false;
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
                            if (!NotifyShowing && !DesireNotify)
                            {
                                timer.Enabled = false;//all done
                            } 
                            this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        }
                    }
                }
                else //no animations, boo!
                {
                    PanelPrefs.SetBounds(PanelPrefs.Location.X + 380, PanelPrefs.Location.Y,
                                         PanelPrefs.Size.Width, PanelPrefs.Size.Height);
                    grpbxFeed.SetBounds(grpbxFeed.Location.X + 380, grpbxFeed.Location.Y,
                                        grpbxFeed.Size.Width, grpbxFeed.Size.Height);
                    PrefsShowing = false;
                    PrefsClicked = false;
                    aPanelIsMoving = false;
                    PrefsTimesClicked = 0;
                    //SavePrefs(); //when hiding the panel save the preferences
                    this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
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
                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;//all done
                        } 
                    }
                }
            }


            #endregion

            #region FAQ Animate

            if ((PanelFAQ.Location.Y < 26) && (FAQClicked == true)
                && (FAQTimesClicked == 0) && (FAQShowing == false))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                            PanelFAQ.BringToFront();
                            menuStrip1.BringToFront();
                        }
                        PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y + 25,
                                           PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y + 15,
                                           PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y + 12,
                                           PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        FAQShowing = true;
                        FAQClicked = false;
                        aPanelIsMoving = false;
                        FAQTimesClicked = 1;
                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }
                        PanelFAQ.Focus();
                        this.Text = " Classroom Inquisition  |  FAQ" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
                else //no animations
                {
                    PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y + 520,
                                       PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                    FAQShowing = true;
                    FAQClicked = false;
                    aPanelIsMoving = false;
                    FAQTimesClicked = 1;
                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }
                    PanelFAQ.Focus();
                    this.Text = " Classroom Inquisition  |  FAQ" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                }
            }
            else if ((PanelFAQ.Location.Y > -496) && (FAQClicked == true)
                        && (FAQTimesClicked == 1) && (FAQShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                        }
                        PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y - 12,
                                           PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y - 15,
                                           PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y - 25,
                                           PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        FAQShowing = false;
                        FAQClicked = false;
                        aPanelIsMoving = false;
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
                                this.Text = " Classroom Inquisition  |  Direct Msg" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
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
                                this.Text = " Classroom Inquisition  |  Prefs" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                            }
                            DesirePrefs = false;
                        }
                        else
                        {
                            if (!NotifyShowing && !DesireNotify)
                            {
                                timer.Enabled = false;//all done
                            } 
                            if (StuMgmtShowing)
                                this.Text = " Classroom Inquisition  |  Student Management" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                            else if (DMPanelShowing)
                                this.Text = " Classroom Inquisition  |  Direct Msg" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                            else if (PrefsShowing)
                                this.Text = " Classroom Inquisition  |  Prefs" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                            else
                                this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        }
                    }
                }
                else //no animations
                {
                    PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y - 520,
                                       PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                    FAQShowing = false;
                    FAQClicked = false;
                    aPanelIsMoving = false;
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
                            this.Text = " Classroom Inquisition  |  Direct Msg" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
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
                            this.Text = " Classroom Inquisition  |  Prefs" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        }
                        DesirePrefs = false;
                    }
                    else
                    {
                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false; //all done
                        } 
                        if (StuMgmtShowing)
                            this.Text = " Classroom Inquisition  |  Student Management" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        else if (DMPanelShowing)
                            this.Text = " Classroom Inquisition  |  Direct Msg" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        else if (PrefsShowing)
                            this.Text = " Classroom Inquisition  |  Prefs" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        else
                            this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
            }


            #endregion

            #region Student Management Animate

            if ((PanelStudents.Location.Y > 24) && (StuMgmtClicked == true)
                 && (StuMgmtTimesClicked == 0) && (StuMgmtShowing == false))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                            PanelStudents.BringToFront();
                        }
                        PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y - 25,
                                                PanelStudents.Size.Width, PanelStudents.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y - 15,
                                                PanelStudents.Size.Width, PanelStudents.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y - 12,
                                                PanelStudents.Size.Width, PanelStudents.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        StuMgmtShowing = true;
                        StuMgmtClicked = false;
                        aPanelIsMoving = false;
                        StuMgmtTimesClicked = 1;
                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }
                        PanelStudents.Focus();
                        this.Text = " Classroom Inquisition  |  Student Management" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
                else //no animations, boo!
                {
                    aPanelIsMoving = true;
                    PanelStudents.BringToFront();
                    PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y - 520,
                                            PanelStudents.Size.Width, PanelStudents.Size.Height);
                    StuMgmtShowing = true;
                    StuMgmtClicked = false;
                    aPanelIsMoving = false;
                    StuMgmtTimesClicked = 1;
                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }
                    PanelStudents.Focus();
                    this.Text = " Classroom Inquisition  |  Student Management" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                }
            }
            else if ((PanelStudents.Location.Y < 546) && (StuMgmtClicked == true)
                        && (StuMgmtTimesClicked == 1) && (StuMgmtShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                        }
                        PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y + 12,
                                                PanelStudents.Size.Width, PanelStudents.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y + 15,
                                                PanelStudents.Size.Width, PanelStudents.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y + 25,
                                                PanelStudents.Size.Width, PanelStudents.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        StuMgmtShowing = false;
                        StuMgmtClicked = false;
                        aPanelIsMoving = false;
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
                            if (!NotifyShowing && !DesireNotify)
                            {
                                timer.Enabled = false;
                            }
                            if (PrefsShowing)
                                this.Text = " Classroom Inquisition  |  Prefs" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                            else
                                this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        }
                    }
                }
                else //no animations
                {
                    PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y + 520,
                                            PanelStudents.Size.Width, PanelStudents.Size.Height);
                    StuMgmtShowing = false;
                    StuMgmtClicked = false;
                    aPanelIsMoving = false;
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
                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }
                        if (PrefsShowing)
                            this.Text = " Classroom Inquisition  |  Prefs" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        else
                            this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
            }
            #endregion

            #region Conversation Viewer Animation
            if ((PanelConvView.Location.Y < 1) && (ConvViewClicked == true)
               && (ConvViewTimesClicked == 0) && (ConvViewShowing == false))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                            PanelConvView.BringToFront();
                        }
                        PanelConvView.SetBounds(PanelConvView.Location.X, PanelConvView.Location.Y + 25,
                                                PanelConvView.Size.Width, PanelConvView.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelConvView.SetBounds(PanelConvView.Location.X, PanelConvView.Location.Y + 15,
                                                PanelConvView.Size.Width, PanelConvView.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelConvView.SetBounds(PanelConvView.Location.X, PanelConvView.Location.Y + 13,
                                                PanelConvView.Size.Width, PanelConvView.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        ConvViewShowing = true;
                        ConvViewClicked = false;
                        aPanelIsMoving = false;
                        ConvViewTimesClicked = 1;

                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }

                        PanelConvView.Focus();
                        this.Text = " Classroom Inquisition  |  Conversation Viewer" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
                else //no animations
                {
                    aPanelIsMoving = true;
                    PanelConvView.BringToFront();
                    PanelConvView.SetBounds(PanelConvView.Location.X, PanelConvView.Location.Y + 530,
                                            PanelConvView.Size.Width, PanelConvView.Size.Height);
                    ConvViewShowing = true;
                    ConvViewClicked = false;
                    aPanelIsMoving = false;
                    ConvViewTimesClicked = 1;

                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }

                    PanelConvView.Focus();
                    this.Text = " Classroom Inquisition  |  Conversation Viewer" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                }
            }
            else if ((PanelConvView.Location.Y > -531) && (ConvViewClicked == true)
                        && (ConvViewTimesClicked == 1) && (ConvViewShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                        }
                        PanelConvView.SetBounds(PanelConvView.Location.X, PanelConvView.Location.Y - 13,
                                                PanelConvView.Size.Width, PanelConvView.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelConvView.SetBounds(PanelConvView.Location.X, PanelConvView.Location.Y - 15,
                                                PanelConvView.Size.Width, PanelConvView.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelConvView.SetBounds(PanelConvView.Location.X, PanelConvView.Location.Y - 25,
                                                PanelConvView.Size.Width, PanelConvView.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        ConvViewShowing = false;
                        ConvViewClicked = false;
                        aPanelIsMoving = false;
                        menuStrip1.BringToFront();
                        ConvViewTimesClicked = 0; 

                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }

                        if (StuMgmtShowing)
                            this.Text = " Classroom Inquisition  |  Student Management" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        else if (PrefsShowing)
                            this.Text = " Classroom Inquisition  |  Prefs" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        else
                            this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
                else //no animations
                {
                    PanelConvView.SetBounds(PanelConvView.Location.X, PanelConvView.Location.Y - 530,
                                            PanelConvView.Size.Width, PanelConvView.Size.Height);
                    ConvViewShowing = false;
                    ConvViewClicked = false;
                    aPanelIsMoving = false;
                    ConvViewTimesClicked = 0;
                    menuStrip1.BringToFront();

                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }

                    if (StuMgmtShowing)
                        this.Text = " Classroom Inquisition  |  Student Management" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    else if (PrefsShowing)
                        this.Text = " Classroom Inquisition  |  Prefs" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    else
                        this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                }
            }

            #endregion

            #region Quiz Maker Animate

            if ((PanelQuizMaker.Location.X > -1) && (QuizMakerClicked == true)
                && (QuizMakerTimesClicked == 0) && (QuizMakerShowing == false))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                            PanelQuizMaker.BringToFront();
                        }
                        PanelQuizMaker.SetBounds(PanelQuizMaker.Location.X - 20, PanelQuizMaker.Location.Y,
                                                 PanelQuizMaker.Size.Width, PanelQuizMaker.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelQuizMaker.SetBounds(PanelQuizMaker.Location.X - 12, PanelQuizMaker.Location.Y,
                                                 PanelQuizMaker.Size.Width, PanelQuizMaker.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelQuizMaker.SetBounds(PanelQuizMaker.Location.X - 8, PanelQuizMaker.Location.Y,
                                                 PanelQuizMaker.Size.Width, PanelQuizMaker.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        QuizMakerShowing = true;
                        QuizMakerClicked = false;
                        aPanelIsMoving = false;
                        QuizMakerTimesClicked = 1;

                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }

                        PanelQuizMaker.Focus();
                        this.Text = " Classroom Inquisition  |  Quiz Maker" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
                else //no animations
                {
                    aPanelIsMoving = true;
                    PanelQuizMaker.BringToFront();
                    PanelQuizMaker.SetBounds(PanelQuizMaker.Location.X - 400, PanelQuizMaker.Location.Y,
                                             PanelQuizMaker.Size.Width, PanelQuizMaker.Size.Height);
                    QuizMakerShowing = true;
                    QuizMakerClicked = false;
                    aPanelIsMoving = false;
                    QuizMakerTimesClicked = 1;
                    
                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }

                    PanelQuizMaker.Focus();
                    this.Text = " Classroom Inquisition  |  Quiz Maker" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                }
            }
            else if ((PanelQuizMaker.Location.X < 401) && (QuizMakerClicked == true)
                      && (QuizMakerTimesClicked == 1) && (QuizMakerShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                        }
                        PanelQuizMaker.SetBounds(PanelQuizMaker.Location.X + 8, PanelQuizMaker.Location.Y,
                                                 PanelQuizMaker.Size.Width, PanelQuizMaker.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelQuizMaker.SetBounds(PanelQuizMaker.Location.X + 12, PanelQuizMaker.Location.Y,
                                                 PanelQuizMaker.Size.Width, PanelQuizMaker.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelQuizMaker.SetBounds(PanelQuizMaker.Location.X + 20, PanelQuizMaker.Location.Y,
                                                 PanelQuizMaker.Size.Width, PanelQuizMaker.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        QuizMakerShowing = false;
                        QuizMakerClicked = false;
                        aPanelIsMoving = false;
                        QuizMakerTimesClicked = 0;

                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }

                        this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
                else //no animations, boo!
                {
                    PanelQuizMaker.SetBounds(PanelQuizMaker.Location.X + 400, PanelQuizMaker.Location.Y,
                                             PanelQuizMaker.Size.Width, PanelQuizMaker.Size.Height);
                    QuizMakerShowing = false;
                    QuizMakerClicked = false;
                    aPanelIsMoving = false;
                    QuizMakerTimesClicked = 0;
                    this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    
                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }
                }
            }


            #endregion

            #region Quiz Mode Animate
            if ((PanelQuizMode.Location.X < 1) && (QuizModeClicked == true)
                && (QuizModeTimesClicked == 0) && (QuizModeShowing == false))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                            PanelQuizMode.BringToFront();
                        }
                        PanelQuizMode.SetBounds(PanelQuizMode.Location.X + 20, PanelQuizMode.Location.Y,
                                                PanelQuizMode.Size.Width, PanelQuizMode.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelQuizMode.SetBounds(PanelQuizMode.Location.X + 12, PanelQuizMode.Location.Y,
                                                PanelQuizMode.Size.Width, PanelQuizMode.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelQuizMode.SetBounds(PanelQuizMode.Location.X + 8, PanelQuizMode.Location.Y,
                                                PanelQuizMode.Size.Width, PanelQuizMode.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        QuizModeShowing = true;
                        QuizModeClicked = false;
                        aPanelIsMoving = false;
                        QuizModeTimesClicked = 1;

                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }

                        PanelQuizMode.Focus();
                        this.Text = " Classroom Inquisition  |  Quiz Mode" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
                else //no animations
                {
                    aPanelIsMoving = true;
                    PanelQuizMode.BringToFront();
                    PanelQuizMode.SetBounds(PanelQuizMode.Location.X + 400, PanelQuizMode.Location.Y,
                                            PanelQuizMode.Size.Width, PanelQuizMode.Size.Height);
                    QuizModeShowing = true;
                    QuizModeClicked = false;
                    aPanelIsMoving = false;
                    QuizModeTimesClicked = 1;

                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }

                    PanelQuizMode.Focus();
                    this.Text = " Classroom Inquisition  |  Quiz Mode" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                }
            }
            else if ((PanelQuizMode.Location.X > -401) && (QuizModeClicked == true)
                        && (QuizModeTimesClicked == 1) && (QuizModeShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                        }
                        PanelQuizMode.SetBounds(PanelQuizMode.Location.X - 8, PanelQuizMode.Location.Y,
                                                PanelQuizMode.Size.Width, PanelQuizMode.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelQuizMode.SetBounds(PanelQuizMode.Location.X - 12, PanelQuizMode.Location.Y,
                                                PanelQuizMode.Size.Width, PanelQuizMode.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelQuizMode.SetBounds(PanelQuizMode.Location.X - 20, PanelQuizMode.Location.Y,
                                                PanelQuizMode.Size.Width, PanelQuizMode.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        QuizModeShowing = false;
                        QuizModeClicked = false;
                        aPanelIsMoving = false;
                        QuizModeTimesClicked = 0;
                        
                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }

                        this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
                else //no animations, boo!
                {
                    PanelQuizMode.SetBounds(PanelQuizMode.Location.X - 400, PanelQuizMode.Location.Y,
                                            PanelQuizMode.Size.Width, PanelQuizMode.Size.Height);
                    QuizModeShowing = false;
                    QuizModeClicked = false;
                    aPanelIsMoving = false;
                    QuizModeTimesClicked = 0;
                    this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");

                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }
                }
            }


            #endregion

            #region Attendance Mode Animation (android)
            if ((PanelAttendance.Location.Y <= 265) && (AttendanceClicked == true)
                  && (AttendanceTimesClicked == 0) && (AttendanceShowing == false))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                            PanelAttendance.Show();
                            PanelAttendance.SetBounds(190, PanelAttendance.Location.Y - 5,
                                                      PanelAttendance.Size.Width + 4, PanelAttendance.Size.Height + 2);
                        }
                        PanelAttendance.SetBounds(PanelAttendance.Location.X - 19, PanelAttendance.Location.Y,
                                                  PanelAttendance.Size.Width + 38, PanelAttendance.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelAttendance.SetBounds(PanelAttendance.Location.X, PanelAttendance.Location.Y - 10,
                                                  PanelAttendance.Size.Width, PanelAttendance.Size.Height + 23);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelAttendance.SetBounds(PanelAttendance.Location.X, PanelAttendance.Location.Y - 16,
                                                  PanelAttendance.Size.Width, PanelAttendance.Size.Height + 32);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        AttendanceShowing = true;
                        AttendanceClicked = false;
                        aPanelIsMoving = false;
                        AttendanceTimesClicked = 1;
                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }
                        else
                        {
                            PanelNotify.BringToFront();
                        }
                        PanelAttendance.Focus();
                        this.Text = " Classroom Inquisition  |  Attendance" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
                else //no animations
                {
                    PanelAttendance.SetBounds(0, 0, 384, 550);
                    PanelAttendance.Show();
                    AttendanceShowing = true;
                    AttendanceClicked = false;
                    aPanelIsMoving = false;
                    AttendanceTimesClicked = 1;
                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }
                    else
                    {
                        PanelNotify.BringToFront();
                    }
                    PanelAttendance.Focus();
                    this.Text = " Classroom Inquisition  |  Attendance" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                }
            }
            else if ((PanelAttendance.Location.Y >= 0) && (AttendanceClicked == true)
                     && (AttendanceTimesClicked == 1) && (AttendanceShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if(x==0)
                        {
                            aPanelIsMoving = true;
                        }
                        PanelAttendance.SetBounds(PanelAttendance.Location.X, PanelAttendance.Location.Y + 16,
                                                  PanelAttendance.Size.Width, PanelAttendance.Size.Height - 32);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelAttendance.SetBounds(PanelAttendance.Location.X, PanelAttendance.Location.Y + 10,
                                                  PanelAttendance.Size.Width, PanelAttendance.Size.Height - 23);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelAttendance.SetBounds(PanelAttendance.Location.X + 19, PanelAttendance.Location.Y,
                                                  PanelAttendance.Size.Width - 38, PanelAttendance.Size.Height);
                        if (x == 29)
                        {
                            PanelAttendance.SetBounds(PanelAttendance.Location.X, PanelAttendance.Location.Y + 5,
                                                      PanelAttendance.Size.Width - 4, PanelAttendance.Size.Height - 2);
                        }
                        x++;
                    }
                    else
                    {
                        x = 0;
                        AttendanceShowing = false;
                        AttendanceClicked = false;
                        aPanelIsMoving = false;
                        AttendanceTimesClicked = 0;
                        PanelAttendance.Hide();

                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }

                        if (StuMgmtShowing)
                            this.Text = " Classroom Inquisition  |  Student Management" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        else if (PrefsShowing)
                            this.Text = " Classroom Inquisition  |  Prefs" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        else
                            this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
                else //no animations
                {
                    PanelAttendance.SetBounds(190, 265, 0, 0);
                    PanelAttendance.Hide();
                    AttendanceShowing = false;
                    AttendanceClicked = false;
                    aPanelIsMoving = false;
                    AttendanceTimesClicked = 0;
                    
                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }

                    if (StuMgmtShowing)
                        this.Text = " Classroom Inquisition  |  Student Management" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    else if (PrefsShowing)
                        this.Text = " Classroom Inquisition  |  Prefs" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    else
                        this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                }
            }

            #endregion

            #region Class Vote Mode Animation (inverse android)
            if ((PanelClassVote.Location.Y <= 266) && (ClassVoteClicked == true)
                 && (ClassVoteTimesClicked == 0) && (ClassVoteShowing == false))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                            PanelClassVote.BringToFront();
                            PanelNotify.BringToFront();
                            PanelClassVote.Show();                           

                            PanelClassVote.SetBounds(190, PanelClassVote.Location.Y - 5,
                                                    PanelClassVote.Size.Width + 4, PanelClassVote.Size.Height+2);
                        }
                        PanelClassVote.SetBounds(PanelClassVote.Location.X, PanelClassVote.Location.Y - 26,
                                                 PanelClassVote.Size.Width, PanelClassVote.Size.Height + 55);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelClassVote.SetBounds(PanelClassVote.Location.X - 12, PanelClassVote.Location.Y,
                                                 PanelClassVote.Size.Width + 24, PanelClassVote.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelClassVote.SetBounds(PanelClassVote.Location.X - 7, PanelClassVote.Location.Y,
                                                 PanelClassVote.Size.Width + 14, PanelClassVote.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        ClassVoteShowing = true;
                        ClassVoteClicked = false;
                        aPanelIsMoving = false;
                        ClassVoteTimesClicked = 1;
                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }
                        pictureBox8.Show();
                        lblCVLeft.Show();
                        lblCVRight.Show();
                        lblOption1.Show();
                        lblOption2.Show();
                        lblCVPipe.Show();
                        txtbxCVStats.Show();
                        btnCVReset.Show();
                        btnExitClassVote.Show();
                        PanelClassVote.Focus();
                        this.Text = " Classroom Inquisition  |  ClassVote" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
                else //no animations
                {
                    PanelClassVote.SetBounds(0, 0, 384, 550);
                    aPanelIsMoving = true;
                    PanelClassVote.BringToFront();
                    PanelNotify.BringToFront();
                    PanelClassVote.Show();
                    ClassVoteShowing = true;
                    ClassVoteClicked = false;
                    aPanelIsMoving = false;
                    ClassVoteTimesClicked = 1;
                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }
                    pictureBox8.Show();
                    lblCVLeft.Show();
                    lblCVRight.Show();
                    lblOption1.Show();
                    lblOption2.Show();
                    lblCVPipe.Show();
                    txtbxCVStats.Show();
                    btnCVReset.Show();
                    btnExitClassVote.Show();
                    PanelClassVote.Focus();
                    this.Text = " Classroom Inquisition  |  ClassVote" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                }
            }
            else if ((PanelClassVote.Location.Y >= 0) && (ClassVoteClicked == true)
                     && (ClassVoteTimesClicked == 1) && (ClassVoteShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            aPanelIsMoving = true;
                            pictureBox8.Hide();
                            lblCVLeft.Hide();
                            lblCVRight.Hide();
                            lblOption1.Hide();
                            lblOption2.Hide();
                            lblCVPipe.Hide();
                            txtbxCVStats.Hide();
                            btnCVReset.Hide();
                            btnExitClassVote.Hide();
                        }
                        PanelClassVote.SetBounds(PanelClassVote.Location.X + 7, PanelClassVote.Location.Y,
                                                 PanelClassVote.Size.Width - 14, PanelClassVote.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelClassVote.SetBounds(PanelClassVote.Location.X + 12, PanelClassVote.Location.Y,
                                                 PanelClassVote.Size.Width - 24, PanelClassVote.Size.Height);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelClassVote.SetBounds(PanelClassVote.Location.X, PanelClassVote.Location.Y + 26,
                                                 PanelClassVote.Size.Width, PanelClassVote.Size.Height - 55);
                        if (x == 29)
                        {
                            PanelClassVote.SetBounds(PanelClassVote.Location.X, PanelClassVote.Location.Y + 5,
                                                     PanelClassVote.Size.Width - 4, PanelClassVote.Size.Height);
                        }
                        x++;
                    }
                    else
                    {
                        x = 0;
                        ClassVoteShowing = false;
                        ClassVoteClicked = false;
                        aPanelIsMoving = false;
                        ClassVoteTimesClicked = 0;
                        PanelClassVote.Hide();

                        if (!NotifyShowing && !DesireNotify)
                        {
                            timer.Enabled = false;
                        }

                        if (StuMgmtShowing)
                            this.Text = " Classroom Inquisition  |  Student Management" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        else if (PrefsShowing)
                            this.Text = " Classroom Inquisition  |  Prefs" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                        else
                            this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    }
                }
                else //no animations
                {
                    PanelClassVote.SetBounds(190, 265, 0, 0);
                    pictureBox8.Hide();
                    lblCVLeft.Hide();
                    lblCVRight.Hide();
                    lblOption1.Hide();
                    lblOption2.Hide();
                    lblCVPipe.Hide();
                    txtbxCVStats.Hide();
                    btnCVReset.Hide();
                    btnExitClassVote.Hide();
                    PanelClassVote.Hide();
                    ClassVoteShowing = false;
                    ClassVoteClicked = false;
                    aPanelIsMoving = false;
                    ClassVoteTimesClicked = 0;

                    if (!NotifyShowing && !DesireNotify)
                    {
                        timer.Enabled = false;
                    }

                    if (StuMgmtShowing)
                        this.Text = " Classroom Inquisition  |  Student Management" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    else if (PrefsShowing)
                        this.Text = " Classroom Inquisition  |  Prefs" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                    else
                        this.Text = " Classroom Inquisition  |  Home" + (UnreadCount > 0 ? " (" + UnreadCount.ToString() + ")" : "");
                }
            }

            #endregion

            #region Notify Panel
            if ((!NotifyShowing) && (DesireNotify == true) && (PanelNotify.Location.Y > 514))// if not visible
            {
                if (!chkbxLameMode.Checked) //animations
                {
                    if (notif_I < 5)
                    {
                        if (notif_I == 0)
                        {
                            PanelNotify.BringToFront();
                        }
                        PanelNotify.SetBounds(PanelNotify.Location.X, PanelNotify.Location.Y - 3,
                                             PanelNotify.Size.Width, PanelNotify.Size.Height);
                        notif_I++;
                    }
                    else if (notif_I < 15)
                    {
                        PanelNotify.SetBounds(PanelNotify.Location.X, PanelNotify.Location.Y - 2,
                                             PanelNotify.Size.Width, PanelNotify.Size.Height);
                        notif_I++;
                    }
                    else
                    {
                        //timer stays on to hide the panel soon..
                        DesireNotify = false;
                        NotifyShowing = true;
                    }
                }
                else //no animations
                {
                    //jump straight to whatever position..
                    PanelNotify.SetBounds(PanelNotify.Location.X, PanelNotify.Location.Y - 35,
                                         PanelNotify.Size.Width, PanelNotify.Size.Height);                    
                    NotifyShowing = true;
                    DesireNotify = false;
                }
            }

            if ((NotifyShowing) && (!DesireNotify) && (PanelNotify.Location.Y < 551))// if visible, destroy
            {
                if (!chkbxLameMode.Checked)
                {
                    if (notif_I < ((double)numUpDnNotify.Value*100))
                    {
                        notif_I++; //wait until timeout
                    }
                    else if (notif_I <((double)numUpDnNotify.Value * 100)+5)
                    {
                        PanelNotify.SetBounds(PanelNotify.Location.X, PanelNotify.Location.Y + 7,
                                             PanelNotify.Size.Width, PanelNotify.Size.Height);
                        notif_I++;
                    }
                    else
                    {
                        notif_I = 0;
                        if (!aPanelIsMoving)
                        {
                            timer.Enabled = false;
                        }
                        DesireNotify = false;
                        NotifyShowing = false;
                    }
                }
                else //no animations
                {
                    if (notif_I < ((double)numUpDnNotify.Value * 100))
                    {
                        notif_I++; //wait until timeout
                    }
                    else
                    {
                        PanelNotify.SetBounds(PanelNotify.Location.X, PanelNotify.Location.Y + 35,
                                             PanelNotify.Size.Width, PanelNotify.Size.Height);
                        if (!aPanelIsMoving)
                        {
                            timer.Enabled = false;
                        }
                        notif_I = 0;
                        DesireNotify = false;
                        NotifyShowing = false;
                    }
                    
                }
            }
            #endregion

        }//end of timer_Tick
    }
}
