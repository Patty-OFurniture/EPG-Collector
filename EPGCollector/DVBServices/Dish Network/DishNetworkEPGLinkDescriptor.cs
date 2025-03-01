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

using DomainObjects;

namespace DVBServices
{
    /// <summary>
    /// The class that describes a Dish Network EPG link descriptor.
    /// </summary>
    internal class DishNetworkEPGLinkDescriptor : DescriptorBase
    {
        /// <summary>
        /// Get the network ID.
        /// </summary>
        public int OriginalNetworkID { get { return (originalNetworkID); } }
        /// <summary>
        /// Get the transport stream ID.
        /// </summary>
        public int TransportStreamID { get { return (transportStreamID); } }
        /// <summary>
        /// Get the service ID.
        /// </summary>
        public int ServiceID { get { return (serviceID); } }
        /// <summary>
        /// Get the program time offset.
        /// </summary>
        public int TimeOffset { get { return (timeOffset); } }

        /// <summary>
        /// Get the index of the next byte in the section following this descriptor.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The descriptor has not been processed.
        /// </exception> 
        public override int Index
        {
            get
            {
                if (lastIndex == -1)
                    throw (new InvalidOperationException("DishNetworkEPGLinkDescriptor: Index requested before block processed"));
                return (lastIndex);
            }
        }

        private int originalNetworkID;
        private int transportStreamID;
        private int serviceID;
        private int timeOffset;

        private int lastIndex = -1;

        /// <summary>
        /// Initialize a new instance of the DishNetworkEPGLinkDescriptor class.
        /// </summary>
        internal DishNetworkEPGLinkDescriptor() { }

        /// <summary>
        /// Parse the descriptor.
        /// </summary>
        /// <param name="byteData">The MPEG2 section containing the descriptor.</param>
        /// <param name="index">Index of the byte in the MPEG2 section following the descriptor length.</param>
        internal override void Process(byte[] byteData, int index)
        {
            lastIndex = index;

            transportStreamID = Utils.Convert2BytesToInt(byteData, lastIndex);
            lastIndex += 2;

            originalNetworkID = Utils.Convert2BytesToInt(byteData, lastIndex);
            lastIndex += 2;

            serviceID = Utils.Convert2BytesToInt(byteData, lastIndex);
            lastIndex += 2;

            timeOffset = Utils.Convert2BytesToInt(byteData, lastIndex);
            lastIndex += 2;
        }

        /// <summary>
        /// Validate the descriptor fields.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// A descriptor field is not valid.
        /// </exception>
        internal override void Validate() { }

        /// <summary>
        /// Log the descriptor fields.
        /// </summary>
        internal override void LogMessage()
        {
            if (Logger.ProtocolLogger == null)
                return;

            Logger.ProtocolLogger.Write(Logger.ProtocolIndent + "DISH NETWORK EPG LINK DESCRIPTOR: ONID: " + originalNetworkID +
                " TID: " + transportStreamID +
                " SID: " + serviceID +
                " Time offset: " + timeOffset);
        }
    }
}

