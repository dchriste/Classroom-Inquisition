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
