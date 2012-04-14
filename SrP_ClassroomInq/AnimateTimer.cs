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

            if ((!BrdcstShowing) && (textbox1WASclicked == true) && (pnlBrdCst.Location.Y  < 41))// if not visible
            {
                if (!chkbxLameMode.Checked) //animations
                {
                    if (i < 10)
                    {
                        pnlBrdCst.SetBounds(pnlBrdCst.Location.X, pnlBrdCst.Location.Y + 5,
                                             pnlBrdCst.Size.Width, pnlBrdCst.Size.Height);
                        i++;
                    }
                    else if (i <20)
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
                        timer.Enabled = false;
                        textbox1WASclicked = false;
                        BrdcstShowing = true;
                        textBox1.Clear();
                        this.Text = " Classroom Inquisition  |  Broadcast";
                    }
                }
                else //no animations
                {
                    //jump straight to whatever position..
                    pnlBrdCst.SetBounds(pnlBrdCst.Location.X, pnlBrdCst.Location.Y + 70,
                                         pnlBrdCst.Size.Width, pnlBrdCst.Size.Height);
                    timer.Enabled = false;
                    BrdcstShowing = true;
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
                        timer.Enabled = false;
                        textbox1WASclicked = false;
                        BrdcstShowing = false;
                        grpbxFeed.Focus(); //get cursor out of textbox if it's there...
                        if (PrefsShowing)
                            this.Text = " Classroom Inquisition  |  Prefs";
                        else if (DMPanelShowing)
                            this.Text = " Classroom Inquisition  |  Direct Msg";
                        else
                            this.Text = " Classroom Inquisition  |  Home";
                    }
                }
                else //no animations
                {
                    pnlBrdCst.SetBounds(pnlBrdCst.Location.X, pnlBrdCst.Location.Y - 70,
                                         pnlBrdCst.Size.Width, pnlBrdCst.Size.Height);
                   
                    timer.Enabled = false;
                    textbox1WASclicked = false;
                    BrdcstShowing = false;
                    grpbxFeed.Focus(); //get cursor out of textbox if it's there...
                    if (PrefsShowing)
                        this.Text = " Classroom Inquisition  |  Prefs";
                    else if (DMPanelShowing)
                        this.Text = " Classroom Inquisition  |  Direct Msg";
                    else
                        this.Text = " Classroom Inquisition  |  Home";
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
                            timer.Enabled = false;
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
                        timer.Enabled = false;
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
                            if (!DesireID)
                            {
                                timer.Enabled = false;
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
                    timer.Enabled = false;
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
                    x = 0;
                    timer.Enabled = false;
                }
            }
            #endregion

            #region MoveCtrlsDown
            if ((NEW_grpbx == true))
            {
                Point TEMP = new Point();
                if (k < 3) //use groupboxID..
                {
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
                    timer.Enabled = false;
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
                        DMtimesClicked = 1;
                        timer.Enabled = false;
                        DirectMsgPanel.Focus();
                        this.Text = " Classroom Inquisition  |  Direct Msg";
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
                    DMtimesClicked = 1;
                    timer.Enabled = false;
                    DirectMsgPanel.Focus();
                    this.Text = " Classroom Inquisition  |  Direct Msg";
                }
            }
            else if ((DirectMsgPanel.Location.X > -396) && (DMclicked == true)
                            && (DMtimesClicked == 1) && (DMPanelShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
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
                            this.Text = " Classroom Inquisition  |  Home";
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
                        this.Text = " Classroom Inquisition  |  Home";
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
                        PrefsTimesClicked = 1;
                        timer.Enabled = false;
                        PanelPrefs.Focus();
                        this.Text = " Classroom Inquisition  |  Prefs";
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
                    PrefsTimesClicked = 1;
                    timer.Enabled = false;
                    PanelPrefs.Focus();
                    this.Text = " Classroom Inquisition  |  Prefs";
                }
            }
            else if ((PanelPrefs.Location.X < 396) && (PrefsClicked == true)
                        && (PrefsTimesClicked == 1) && (PrefsShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
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
                            this.Text = " Classroom Inquisition  |  Home";
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
                    PrefsTimesClicked = 0;
                    //SavePrefs(); //when hiding the panel save the preferences
                    this.Text = " Classroom Inquisition  |  Home";
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

            if ((PanelFAQ.Location.Y < 26) && (FAQClicked == true)
                && (FAQTimesClicked == 0) && (FAQShowing == false))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y + 24,
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
                        FAQTimesClicked = 1;
                        timer.Enabled = false;
                        PanelFAQ.Focus();
                        this.Text = " Classroom Inquisition  |  FAQ";
                    }
                }
                else //no animations
                {
                    PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y + 510,
                                       PanelFAQ.Size.Width, PanelFAQ.Size.Height);
                    FAQShowing = true;
                    FAQClicked = false;
                    FAQTimesClicked = 1;
                    timer.Enabled = false;
                    PanelFAQ.Focus();
                    this.Text = " Classroom Inquisition  |  FAQ";
                }
            }
            else if ((PanelFAQ.Location.Y > -486) && (FAQClicked == true)
                        && (FAQTimesClicked == 1) && (FAQShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
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
                        PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y - 24,
                                           PanelFAQ.Size.Width, PanelFAQ.Size.Height);
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
                                this.Text = " Classroom Inquisition  |  Direct Msg";
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
                                this.Text = " Classroom Inquisition  |  Prefs";
                            }
                            DesirePrefs = false;
                        }
                        else
                        {
                            timer.Enabled = false; //all done
                            if (StuMgmtShowing)
                                this.Text = " Classroom Inquisition  |  Student Management";
                            else if (DMPanelShowing)
                                this.Text = " Classroom Inquisition  |  Direct Msg";
                            else if (PrefsShowing)
                                this.Text = " Classroom Inquisition  |  Prefs";
                            else
                                this.Text = " Classroom Inquisition  |  Home";
                        }
                    }
                }
                else //no animations
                {
                    PanelFAQ.SetBounds(PanelFAQ.Location.X, PanelFAQ.Location.Y - 510,
                                       PanelFAQ.Size.Width, PanelFAQ.Size.Height);
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
                            this.Text = " Classroom Inquisition  |  Direct Msg";
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
                            this.Text = " Classroom Inquisition  |  Prefs";
                        }
                        DesirePrefs = false;
                    }
                    else
                    {
                        timer.Enabled = false; //all done
                        if (StuMgmtShowing)
                            this.Text = " Classroom Inquisition  |  Student Management";
                        else if (DMPanelShowing)
                            this.Text = " Classroom Inquisition  |  Direct Msg";
                        else if (PrefsShowing)
                            this.Text = " Classroom Inquisition  |  Prefs";
                        else
                            this.Text = " Classroom Inquisition  |  Home";
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
                        PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y - 24,
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
                        StuMgmtTimesClicked = 1;
                        timer.Enabled = false;
                        PanelStudents.Focus();
                        this.Text = " Classroom Inquisition  |  Student Management";
                    }
                }
                else //no animations, boo!
                {
                    PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y - 510,
                                            PanelStudents.Size.Width, PanelStudents.Size.Height);
                    StuMgmtShowing = true;
                    StuMgmtClicked = false;
                    StuMgmtTimesClicked = 1;
                    timer.Enabled = false;
                    PanelStudents.Focus();
                    this.Text = " Classroom Inquisition  |  Student Management";
                }
            }
            else if ((PanelStudents.Location.Y < 536) && (StuMgmtClicked == true)
                        && (StuMgmtTimesClicked == 1) && (StuMgmtShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
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
                        PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y + 24,
                                                PanelStudents.Size.Width, PanelStudents.Size.Height);
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
                            if (PrefsShowing)
                                this.Text = " Classroom Inquisition  |  Prefs";
                            else
                                this.Text = " Classroom Inquisition  |  Home";
                        }
                    }
                }
                else //no animations
                {
                    PanelStudents.SetBounds(PanelStudents.Location.X, PanelStudents.Location.Y + 510,
                                            PanelStudents.Size.Width, PanelStudents.Size.Height);
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
                        if (PrefsShowing)
                            this.Text = " Classroom Inquisition  |  Prefs";
                        else
                            this.Text = " Classroom Inquisition  |  Home";
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
                        ConvViewTimesClicked = 1;
                        timer.Enabled = false;
                        PanelConvView.Focus();
                        this.Text = " Classroom Inquisition  |  Conversation Viewer";
                    }
                }
                else //no animations
                {
                    PanelConvView.SetBounds(PanelConvView.Location.X, PanelConvView.Location.Y + 530,
                                            PanelConvView.Size.Width, PanelConvView.Size.Height);
                    ConvViewShowing = true;
                    ConvViewClicked = false;
                    ConvViewTimesClicked = 1;
                    timer.Enabled = false;
                    PanelConvView.Focus();
                    this.Text = " Classroom Inquisition  |  Conversation Viewer";
                }
            }
            else if ((PanelConvView.Location.Y > -531) && (ConvViewClicked == true)
                        && (ConvViewTimesClicked == 1) && (ConvViewShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
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
                        ConvViewTimesClicked = 0;
                        timer.Enabled = false; //all done
                        if (StuMgmtShowing)
                            this.Text = " Classroom Inquisition  |  Student Management";
                        else if (PrefsShowing)
                            this.Text = " Classroom Inquisition  |  Prefs";
                        else
                            this.Text = " Classroom Inquisition  |  Home";
                    }
                }
                else //no animations
                {
                    PanelConvView.SetBounds(PanelConvView.Location.X, PanelConvView.Location.Y - 530,
                                            PanelConvView.Size.Width, PanelConvView.Size.Height);
                    ConvViewShowing = false;
                    ConvViewClicked = false;
                    ConvViewTimesClicked = 0;
                    timer.Enabled = false; //all done
                    if (StuMgmtShowing)
                        this.Text = " Classroom Inquisition  |  Student Management";
                    else if (PrefsShowing)
                        this.Text = " Classroom Inquisition  |  Prefs";
                    else
                        this.Text = " Classroom Inquisition  |  Home";
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
                        QuizMakerTimesClicked = 1;
                        timer.Enabled = false;
                        PanelQuizMaker.Focus();
                        this.Text = " Classroom Inquisition  |  Quiz Maker";
                    }
                }
                else //no animations
                {
                    PanelQuizMaker.SetBounds(PanelQuizMaker.Location.X - 400, PanelQuizMaker.Location.Y,
                                             PanelQuizMaker.Size.Width, PanelQuizMaker.Size.Height);
                    QuizMakerShowing = true;
                    QuizMakerClicked = false;
                    QuizMakerTimesClicked = 1;
                    timer.Enabled = false;
                    PanelQuizMaker.Focus();
                    this.Text = " Classroom Inquisition  |  Quiz Maker";
                }
            }
            else if ((PanelQuizMaker.Location.X < 401) && (QuizMakerClicked == true)
                      && (QuizMakerTimesClicked == 1) && (QuizMakerShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
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
                        QuizMakerTimesClicked = 0;
                        timer.Enabled = false; //all done
                        this.Text = " Classroom Inquisition  |  Home";
                    }
                }
                else //no animations, boo!
                {
                    PanelQuizMaker.SetBounds(PanelQuizMaker.Location.X + 400, PanelQuizMaker.Location.Y,
                                             PanelQuizMaker.Size.Width, PanelQuizMaker.Size.Height);
                    QuizMakerShowing = false;
                    QuizMakerClicked = false;
                    QuizMakerTimesClicked = 0;
                    this.Text = " Classroom Inquisition  |  Home";
                    timer.Enabled = false;
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
                        QuizModeTimesClicked = 1;
                        timer.Enabled = false;
                        PanelQuizMode.Focus();
                        this.Text = " Classroom Inquisition  |  Quiz Mode";
                    }
                }
                else //no animations
                {
                    PanelQuizMode.SetBounds(PanelQuizMode.Location.X + 400, PanelQuizMode.Location.Y,
                                            PanelQuizMode.Size.Width, PanelQuizMode.Size.Height);
                    QuizModeShowing = true;
                    QuizModeClicked = false;
                    QuizModeTimesClicked = 1;
                    timer.Enabled = false;
                    PanelQuizMode.Focus();
                    this.Text = " Classroom Inquisition  |  Quiz Mode";
                }
            }
            else if ((PanelQuizMode.Location.X > -401) && (QuizModeClicked == true)
                        && (QuizModeTimesClicked == 1) && (QuizModeShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
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
                        QuizModeTimesClicked = 0;
                        timer.Enabled = false; //all done
                        this.Text = " Classroom Inquisition  |  Home";
                    }
                }
                else //no animations, boo!
                {
                    PanelQuizMode.SetBounds(PanelQuizMode.Location.X - 400, PanelQuizMode.Location.Y,
                                            PanelQuizMode.Size.Width, PanelQuizMode.Size.Height);
                    QuizModeShowing = false;
                    QuizModeClicked = false;
                    QuizModeTimesClicked = 0;
                    this.Text = " Classroom Inquisition  |  Home";
                    timer.Enabled = false;
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
                            PanelAttendance.Show();
                            PanelAttendance.SetBounds(190, PanelAttendance.Location.Y - 5,
                                                      PanelAttendance.Size.Width + 4, PanelAttendance.Size.Height + 4);
                        }
                        PanelAttendance.SetBounds(PanelAttendance.Location.X - 19, PanelAttendance.Location.Y,
                                                  PanelAttendance.Size.Width + 38, PanelAttendance.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelAttendance.SetBounds(PanelAttendance.Location.X, PanelAttendance.Location.Y - 10,
                                                  PanelAttendance.Size.Width, PanelAttendance.Size.Height + 22);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelAttendance.SetBounds(PanelAttendance.Location.X, PanelAttendance.Location.Y - 16,
                                                  PanelAttendance.Size.Width, PanelAttendance.Size.Height + 30);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        AttendanceShowing = true;
                        AttendanceClicked = false;
                        AttendanceTimesClicked = 1;
                        timer.Enabled = false;
                        PanelAttendance.Focus();
                        this.Text = " Classroom Inquisition  |  Attendance";
                    }
                }
                else //no animations
                {
                    PanelAttendance.SetBounds(0, 0, 384, 530);
                    PanelAttendance.Show();
                    AttendanceShowing = true;
                    AttendanceClicked = false;
                    AttendanceTimesClicked = 1;
                    timer.Enabled = false;
                    PanelAttendance.Focus();
                    this.Text = " Classroom Inquisition  |  Attendance";
                }
            }
            else if ((PanelAttendance.Location.Y >= 0) && (AttendanceClicked == true)
                     && (AttendanceTimesClicked == 1) && (AttendanceShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        PanelAttendance.SetBounds(PanelAttendance.Location.X, PanelAttendance.Location.Y + 16,
                                                  PanelAttendance.Size.Width, PanelAttendance.Size.Height - 30);
                        x++;
                    }
                    else if (x < 20)
                    {
                        PanelAttendance.SetBounds(PanelAttendance.Location.X, PanelAttendance.Location.Y + 10,
                                                  PanelAttendance.Size.Width, PanelAttendance.Size.Height - 22);
                        x++;
                    }
                    else if (x < 30)
                    {
                        PanelAttendance.SetBounds(PanelAttendance.Location.X + 19, PanelAttendance.Location.Y,
                                                  PanelAttendance.Size.Width - 38, PanelAttendance.Size.Height);
                        if (x == 29)
                        {
                            PanelAttendance.SetBounds(PanelAttendance.Location.X, PanelAttendance.Location.Y + 5,
                                                      PanelAttendance.Size.Width - 4, PanelAttendance.Size.Height - 1);
                        }
                        x++;
                    }
                    else
                    {
                        x = 0;
                        AttendanceShowing = false;
                        AttendanceClicked = false;
                        AttendanceTimesClicked = 0;
                        PanelAttendance.Hide();
                        timer.Enabled = false; //all done
                        if (StuMgmtShowing)
                            this.Text = " Classroom Inquisition  |  Student Management";
                        else if (PrefsShowing)
                            this.Text = " Classroom Inquisition  |  Prefs";
                        else
                            this.Text = " Classroom Inquisition  |  Home";
                    }
                }
                else //no animations
                {
                    PanelAttendance.SetBounds(190, 265, 0, 0);
                    PanelAttendance.Hide();
                    AttendanceShowing = false;
                    AttendanceClicked = false;
                    AttendanceTimesClicked = 0;
                    timer.Enabled = false; //all done
                    if (StuMgmtShowing)
                        this.Text = " Classroom Inquisition  |  Student Management";
                    else if (PrefsShowing)
                        this.Text = " Classroom Inquisition  |  Prefs";
                    else
                        this.Text = " Classroom Inquisition  |  Home";
                }
            }

            #endregion

            #region Class Vote Mode Animation (inverse android)
            if ((PanelClassVote.Location.Y <= 265) && (ClassVoteClicked == true)
                 && (ClassVoteTimesClicked == 0) && (ClassVoteShowing == false))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            PanelClassVote.Show();
                            PanelClassVote.BringToFront();
                            PanelClassVote.SetBounds(190, PanelClassVote.Location.Y - 5,
                                                    PanelClassVote.Size.Width + 4, PanelClassVote.Size.Height+4);
                        }
                        PanelClassVote.SetBounds(PanelClassVote.Location.X, PanelClassVote.Location.Y - 26,
                                                 PanelClassVote.Size.Width, PanelClassVote.Size.Height + 52);
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
                        ClassVoteTimesClicked = 1;
                        timer.Enabled = false;
                        pictureBox8.Show();
                        btnExitClassVote.Show();
                        PanelClassVote.Focus();
                        this.Text = " Classroom Inquisition  |  ClassVote";
                    }
                }
                else //no animations
                {
                    PanelClassVote.SetBounds(0, 0, 384, 530);
                    PanelClassVote.Show();
                    ClassVoteShowing = true;
                    ClassVoteClicked = false;
                    ClassVoteTimesClicked = 1;
                    timer.Enabled = false;
                    pictureBox8.Show();
                    btnExitClassVote.Show();
                    PanelClassVote.Focus();
                    this.Text = " Classroom Inquisition  |  ClassVote";
                }
            }
            else if ((PanelClassVote.Location.Y >= 0) && (ClassVoteClicked == true)
                     && (ClassVoteTimesClicked == 1) && (ClassVoteShowing == true))
            {
                if (!chkbxLameMode.Checked)
                {
                    if (x < 10)
                    {
                        if (x == 1)
                        {
                            pictureBox8.Hide();
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
                                                 PanelClassVote.Size.Width, PanelClassVote.Size.Height - 53);
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
                        ClassVoteTimesClicked = 0;
                        PanelClassVote.Hide();
                        timer.Enabled = false; //all done
                        if (StuMgmtShowing)
                            this.Text = " Classroom Inquisition  |  Student Management";
                        else if (PrefsShowing)
                            this.Text = " Classroom Inquisition  |  Prefs";
                        else
                            this.Text = " Classroom Inquisition  |  Home";
                    }
                }
                else //no animations
                {
                    PanelClassVote.SetBounds(190, 265, 0, 0);
                    PanelClassVote.Hide();
                    ClassVoteShowing = false;
                    ClassVoteClicked = false;
                    ClassVoteTimesClicked = 0;
                    timer.Enabled = false; //all done
                    if (StuMgmtShowing)
                        this.Text = " Classroom Inquisition  |  Student Management";
                    else if (PrefsShowing)
                        this.Text = " Classroom Inquisition  |  Prefs";
                    else
                        this.Text = " Classroom Inquisition  |  Home";
                }
            }

            #endregion

        }//end of timer_Tick
        



    }
}