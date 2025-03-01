﻿////////////////////////////////////////////////////////////////////////////////// 
//                                                                              //
//      Copyright © 2005-2020 nzsjb                                           //
//                                                                              //
//  This Program is free software; you can redistribute it and/or modify        //
//  it under the terms of the GNU General Public License as published by        //
//  the Free Software Foundation; either version 2, or (at your option)         //
//  any later version.                                                          //
//                                                                              //
//  This Program is distributed in the hope that it will be useful,             //
//  but WITHOUT ANY WARRANTY; without even the implied warranty of              //
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                //
//  GNU General Public License for more details.                                //
//                                                                              //
//  You should have received a copy of the GNU General Public License           //
//  along with GNU Make; see the file COPYING.  If not, write to                //
//  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.       //
//  http://www.gnu.org/copyleft/gpl.html                                        //
//                                                                              //  
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace EPGCentre
{
    internal partial class InProgress : Form
    {
        internal delegate void CancelledHandler(object sender, EventArgs e);
        internal event CancelledHandler Cancelled;

        internal string Comment
        {
            get { return (this.Text.Replace("EPG Centre - ", "")); }
            set { this.Text = "EPG Centre - " + value; }
        }

        private bool inhibitEvents;

        internal InProgress(string comment)
        {
            InitializeComponent();

            this.Text = this.Text + comment;
            this.FormClosed += new FormClosedEventHandler(onClosed);
        }

        internal void CloseWithNoEvents()
        {
            inhibitEvents = true;
            Close();
        }

        private void onClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !inhibitEvents)
            {
                if (Cancelled != null)
                    Cancelled(this, new EventArgs());
            }
        }
    }
}

