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

using System.Xml.Serialization;

namespace WMCUtility
{
    /// <summary>
    /// The class that describes a backup dynamic lineup.
    /// </summary>
    [XmlRoot(Namespace = "")]
    public class BackupDynamicLineup
    {
        /// <summary>
        /// Get or set the uid.
        /// </summary>
        [XmlAttribute("uid")]
        public string Uid { get; set; }

        /// <summary>
        /// Get or set the device type.
        /// </summary>
        [XmlAttribute("deviceType")]
        public string DeviceType { get; set; }

        /// <summary>
        /// Get or set the auto generated flag.
        /// </summary>
        [XmlAttribute("isAutoGenerated")]
        public bool IsAutoGenerated { get; set; }

        /// <summary>
        /// Get or set the name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Get or set the auto enabled flag.
        /// </summary>
        [XmlAttribute("isAutoEnabled")]
        public bool IsAutoEnabled { get; set; }

        /// <summary>
        /// Get or set the priority.
        /// </summary>
        [XmlAttribute("priority")]
        public int Priority { get; set; }

        /// <summary>
        /// Get or set the ONID.
        /// </summary>
        [XmlAttribute("onid")]
        public int Onid { get; set; }

        /// <summary>
        /// Initialize a new instance of the BackupDynamicLineup class.
        /// </summary>
        public BackupDynamicLineup() { }
    }
}
