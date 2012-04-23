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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SrP_ClassroomInq
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }
        byte i = 0;


        private void FatherTime_Tick(object sender, EventArgs e)
        {
            if (i < 3)
            {
                //tick is once a second
                i++;
            }
            else if (i < 10)
            {
                if (i == 3)
                {
                    FatherTime.Interval = 100; //change to 10 times faster
                }
                this.Opacity = this.Opacity - (double)0.1; //fade out by percent
                i++;
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK; //tell the main form we're good to go
            }
        }
        
    }
}
