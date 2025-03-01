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
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.Threading;
using System.ComponentModel;
using System.Runtime.InteropServices;

using DomainObjects;

namespace DirectShow
{
    /// <summary>
    /// The class that describes a transport stream reader using a blocking collection.
    /// </summary>
    public class TSStreamReaderEx
    {
        /// <summary>
        /// Get the collection of sections waiting to be processed.
        /// </summary>
        public BlockingCollection<Mpeg2Section> Sections { get { return (sections); } }

        /// <summary>
        /// Get the total number of discontinuities.
        /// </summary>
        public int Discontinuities
        {
            get
            {
                int total = 0;

                for (int index = 0; index < pidHandlers.Length; index++)
                {
                    if (pidHandlers[index] != null)
                        total += pidHandlers[index].Discontinuities;
                }

                return (total);
            }
        }

        /// <summary>
        /// Get the count of sections processed.
        /// </summary>
        public int SectionCount { get; private set; }

        private BlockingCollection<Mpeg2Section> sections;
        private BackgroundWorker backgroundWorker;

        private AutoResetEvent resetEvent = new AutoResetEvent(false);
        private bool running;

        private bool[] tables = new bool[256];
        private int maxSections;

        private bool initialized;
        private IntPtr memoryPointer;
        private IntPtr currentPointer;

        private int maxOffset;
        private int currentOffset;

        private PidHandler[] pidHandlers = new PidHandler[8192];

        private TSStreamReaderEx() { }

        /// <summary>
        /// Initialize a new instance of the TSStreamReader class.
        /// </summary>
        /// <param name="maxSections">The maximum number of sections to be buffered by the reader.</param>
        /// <param name="bufferAddress">The address of the memory buffer holding the transport stream.</param>
        public TSStreamReaderEx(int maxSections, IntPtr bufferAddress)
        {
            this.maxSections = maxSections;
            memoryPointer = bufferAddress;

            for (int index = 0; index < tables.Length; index++)
                tables[index] = true;

            sections = new BlockingCollection<Mpeg2Section>();
        }

        /// <summary>
        /// Initialize a new instance of the TSStreamReader class filtering by table.
        /// </summary>
        /// <param name="table">The table ID to be filtered.</param>
        /// <param name="maxSections">The maximum number of sections to be buffered by the reader.</param>
        /// <param name="bufferAddress">The address of the memory buffer holding the transport stream.</param>
        public TSStreamReaderEx(byte table, int maxSections, IntPtr bufferAddress) : this(maxSections, bufferAddress)
        {
            for (int index = 0; index < tables.Length; index++)
                tables[index] = false;

            tables[table] = true;
        }

        /// <summary>
        /// Initialize a new instance of the TSStreamReader class filtering by a list of tables.
        /// </summary>
        /// <param name="tables">The tables to be filtered.</param>
        /// <param name="maxSections">The maximum number of sections to be buffered by the reader.</param>
        /// <param name="bufferAddress">The address of the memory buffer holding the transport stream.</param>
        public TSStreamReaderEx(Collection<byte> tables, int maxSections, IntPtr bufferAddress) : this(maxSections, bufferAddress)
        {
            for (int index = 0; index < this.tables.Length; index++)
                this.tables[index] = false;

            foreach (byte table in tables)
                this.tables[table] = true;            
        }

        /// <summary>
        /// Start the reader.
        /// </summary>
        public void Run()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(workerDoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerCompleted);
            backgroundWorker.RunWorkerAsync(new BackgroundParameters(maxSections));

            running = true;
        }

        private void workerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                throw new InvalidOperationException("TS Stream Reader background worker failed - see inner exception", e.Error);
        }

        /// <summary>
        /// Stop the reader.
        /// </summary>
        public void Stop()
        {
            if (running)
            {
                backgroundWorker.CancelAsync();
                bool reply = resetEvent.WaitOne(new TimeSpan(0, 0, 40));

                Mpeg2Section endSection = new Mpeg2Section();
                endSection.PID = int.MaxValue;
                Sections.Add(endSection);

                running = false;
            }
        }

        private void workerDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker == null)
                throw (new ArgumentException("Worker thread has been started with an incorrect sender"));

            BackgroundParameters parameters = e.Argument as BackgroundParameters;
            if (parameters == null)
                throw (new ArgumentException("Worker thread has been started with an incorrect parameter"));

            if (RunParameters.IsWindows)
                Thread.CurrentThread.Name = "TS Stream Reader";
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

            while (!worker.CancellationPending)
                getSection(worker, parameters);

            resetEvent.Set();
        }

        private void getSection(BackgroundWorker worker, BackgroundParameters parameters)
        {
            Collection<Mpeg2Section> sections = getNextSection();
            if (sections == null)
                return;

            try
            {
                foreach (Mpeg2Section mpeg2Section in sections)
                {
                    if (tables[mpeg2Section.Table])
                    {
                        Sections.Add(mpeg2Section);
                        SectionCount++;
                        if (TraceEntry.IsDefined(TraceName.Mpeg2SectionsStored))
                            Logger.Instance.Write("Section stored: PID 0x" + mpeg2Section.PID.ToString("X") + " table 0x" + mpeg2Section.Table.ToString("X"));
                    }
                    else
                        if (TraceEntry.IsDefined(TraceName.Mpeg2SectionsStored))
                            Logger.Instance.Write("Section rejected: PID 0x" + mpeg2Section.PID.ToString("X") + " table 0x" + mpeg2Section.Table.ToString("X"));
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
        }

        private Collection<Mpeg2Section> getNextSection()
        {
            if (!initialized)
                initialize();

            if (currentOffset >= maxOffset)
            {
                getMaxOffset();
                if (currentOffset >= maxOffset)
                    return (null);
            }

            byte[] buffer = new byte[188];
            Marshal.Copy(currentPointer, buffer, 0, 188);
            currentPointer += 188;
            currentOffset += 188;

            if (TraceEntry.IsDefined(TraceName.TransportPackets))
                Logger.Instance.Dump("Transport Packet", buffer, buffer.Length);

            TransportPacket transportPacket = new TransportPacket();
            try
            {
                transportPacket.Process(buffer);                
            }
            catch (ArgumentOutOfRangeException)
            {
                Logger.Instance.Write("Transport packet parsing failed at current offset  " + currentOffset +
                    " max offset: " + maxOffset);                
                return (null);
            }

            if (transportPacket.IsNullPacket || transportPacket.ErrorIndicator)
                return (null);

            /*Logger.Instance.Write("Processing PID 0x" + transportPacket.PID.ToString("x4") + " from offset " + currentOffset);*/
            PidHandler pidHandler = findPidHandler(transportPacket.PID);
            pidHandler.Process(buffer, transportPacket);

            /*Logger.Instance.Write("PID handler 0x" + pidHandler.Pid.ToString("x4") + " has created " + pidHandler.Sections.Count + " sections");*/

            return (pidHandler.Sections);
        }

        private void initialize()
        {
            initialized = true;

            getMaxOffset();

            currentPointer = memoryPointer + 136;
            currentOffset = 0;
        }

        private void getMaxOffset()
        {
            maxOffset = Marshal.ReadInt32(memoryPointer);            
        }

        private PidHandler findPidHandler(int pid)
        {
            if (pidHandlers[pid] == null)
            {
                pidHandlers[pid] = new PidHandler(pid);
                /*Logger.Instance.Write("Created PID handler for pid 0x" + pid.ToString("x4"));*/
            }

            return (pidHandlers[pid]);
        }

        internal class BackgroundParameters
        {
            internal int MaxSections { get { return (maxSections); } }

            private int maxSections;

            private BackgroundParameters() { }

            internal BackgroundParameters(int maxSections)
            {
                this.maxSections = maxSections;
            }
        }
    }
}

