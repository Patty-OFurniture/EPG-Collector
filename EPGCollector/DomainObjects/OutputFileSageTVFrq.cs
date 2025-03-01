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

using System.IO;
using System.Collections.ObjectModel;

namespace DomainObjects
{
    internal sealed class OutputFileSageTVFrq
    {
        private static string actualFileName;
        private static bool firstChannel = true;
        private static int recordCount;

        private OutputFileSageTVFrq() { }

        internal static void Process(string fileName)
        {
            if (!checkProcessValid())
                return;

            if (RunParameters.Instance.SageTVFileName != null)
                actualFileName = RunParameters.Instance.SageTVFileName;
            else
                actualFileName = Path.Combine(Path.GetDirectoryName(fileName), "SageTV.frq");

            string tempFileName = actualFileName + ".tmp";

            try
            {
                Logger.Instance.Write("Deleting temporary SageTV frequency file " + tempFileName);
                File.SetAttributes(tempFileName, FileAttributes.Normal);
                File.Delete(tempFileName);
            }
            catch (IOException e)
            {
                Logger.Instance.Write("File delete exception: " + e.Message);
            }

            Logger.Instance.Write("Creating temporary SageTV frequency file " + tempFileName);            

            using (StreamWriter streamWriter = new StreamWriter(tempFileName, false))
            {
                streamWriter.WriteLine("#");
                streamWriter.WriteLine("# Generated by EPG Collector " + RunParameters.SystemVersion);
                streamWriter.WriteLine("#");

                if (RunParameters.Instance.FrequencyCollection[0].AdvancedRunParamters.CountryCode != null)
                {
                    Country country = Country.FindCountryCode(RunParameters.Instance.FrequencyCollection[0].AdvancedRunParamters.CountryCode, Country.Countries);

                    string areaName = string.Empty;
                    if (RunParameters.Instance.FrequencyCollection.Count == 1 && Bouquet.Bouquets != null)
                    {
                        Bouquet bouquet = Bouquet.FindBouquet(RunParameters.Instance.FrequencyCollection[0].AdvancedRunParamters.ChannelBouquet);
                        if (bouquet != null)
                            areaName = "-" + bouquet.Name;
                    }

                    streamWriter.WriteLine("VERSION 3.0, country:" + country + areaName);
                }
                else
                    streamWriter.WriteLine("VERSION 3.0, country:undefined");

                Collection<TVStation> sortedStations = TVStation.GetUserChannelSortedStations(RunParameters.Instance.StationCollection);
                foreach (TVStation station in sortedStations)
                    processStation(streamWriter, station);

                streamWriter.Close();
                Logger.Instance.Write("Channels written to SageTV frequency file: " + recordCount);

                string actualBackupName = actualFileName + ".bak";

                try
                {
                    Logger.Instance.Write("Deleting current SageTV frequency file backup " + actualBackupName);
                    File.SetAttributes(actualBackupName, FileAttributes.Normal);
                    File.Delete(actualBackupName);
                }
                catch (IOException e)
                {
                    Logger.Instance.Write("File delete exception: " + e.Message);
                }

                try
                {
                    Logger.Instance.Write("Renaming current SageTV frequency file " + actualFileName + " to backup");
                    File.SetAttributes(actualFileName, FileAttributes.Normal);
                    File.Move(actualFileName, actualBackupName);
                }
                catch (IOException e)
                {
                    Logger.Instance.Write("File rename exception: " + e.Message);
                }

                try
                {
                    Logger.Instance.Write("Renaming temporary SageTV frequency file to " + tempFileName);
                    File.Move(tempFileName, actualFileName);
                    File.SetAttributes(actualFileName, FileAttributes.ReadOnly);
                }
                catch (IOException e)
                {
                    Logger.Instance.Write("File rename exception: " + e.Message);
                }
            }
        }

        private static bool checkProcessValid()
        {
            if (DebugEntry.IsDefined(DebugName.LogNetworkMap))
                NetworkMap.LogMapEntries();

            if (NetworkMap.NetworkMaps == null || NetworkMap.NetworkMaps.Count == 0)
            {
                Logger.Instance.Write("<e> No network map loaded - data not available to create SageTV frequency file");
                return (false);
            }

            return (true);
        }

        private static void processStation(StreamWriter streamWriter, TVStation station)
        {
            if (station.LogicalChannelNumber == 0 || station.LogicalChannelNumber == 65535)
                return;

            if (!station.Included)
                return;

            if (OptionEntry.IsDefined(OptionName.SageTvOmitNoEpg) && station.EPGCollection.Count == 0)
                return;

            TuningFrequency tuningFrequency = NetworkMap.FindFrequency(station.OriginalNetworkID, station.TransportStreamID);
            if (tuningFrequency == null)
                return;

            SatelliteFrequency satelliteFrequency = tuningFrequency as SatelliteFrequency;
            TerrestrialFrequency terrestrialFrequency = tuningFrequency as TerrestrialFrequency;
            CableFrequency cableFrequency = tuningFrequency as CableFrequency;

            if (satelliteFrequency == null && terrestrialFrequency == null && cableFrequency == null)
                return;

            if (cableFrequency != null && !NetworkMap.CheckForService(station.OriginalNetworkID, station.TransportStreamID, station.ServiceID))
                return;

            if (firstChannel)
            {
                if (satelliteFrequency != null)
                    streamWriter.WriteLine("TYPE DVB-S");
                else
                {
                    if (terrestrialFrequency != null)
                        streamWriter.WriteLine("TYPE DVB-T");
                    else
                        streamWriter.WriteLine("TYPE DVB-C");
                }
                streamWriter.WriteLine("#");

                firstChannel = false;
            }

            streamWriter.Write("CH:" + station.LogicalChannelNumber + " ");
            streamWriter.Write("onid:" + station.OriginalNetworkID + " ");
            streamWriter.Write("tsid:" + station.TransportStreamID + " ");
            streamWriter.Write("sid:" + station.ServiceID + " ");

            if (satelliteFrequency != null)
                processSatellite(streamWriter, station, satelliteFrequency);
            else
            {
                if (terrestrialFrequency != null)
                    processTerrestrial(streamWriter, station, terrestrialFrequency);
                else
                    processCable(streamWriter, station, cableFrequency);
            }

            if (!station.Encrypted)
                streamWriter.Write("ctrl:1 ");
            else
                streamWriter.Write("ctrl:3 ");

            if (string.IsNullOrWhiteSpace(station.NewName))
                streamWriter.WriteLine("#" + station.Name);
            else
                streamWriter.WriteLine("#" + station.NewName);

            recordCount++;
        }

        private static void processSatellite(StreamWriter streamWriter, TVStation station, SatelliteFrequency frequency)
        {
            streamWriter.Write("frq:" + frequency.Frequency + " ");
            streamWriter.Write("rate:" + (frequency.SymbolRate / 1000) + " ");
            streamWriter.Write("pol:" + (frequency.DVBPolarization + 1) + " ");

            switch (frequency.FEC.Rate)
            {
                case FECRate.FECRate12:
                    streamWriter.Write("fec_rate_in:1 ");
                    break;
                case FECRate.FECRate23:
                    streamWriter.Write("fec_rate_in:2 ");
                    break;
                case FECRate.FECRate34:
                    streamWriter.Write("fec_rate_in:3 ");
                    break;
                case FECRate.FECRate35:
                    streamWriter.Write("fec_rate_in:4 ");
                    break;
                case FECRate.FECRate45:
                    streamWriter.Write("fec_rate_in:5 ");
                    break;
                case FECRate.FECRate56:
                    streamWriter.Write("fec_rate_in:6 ");
                    break;
                case FECRate.FECRate511:
                    streamWriter.Write("fec_rate_in:7 ");
                    break;
                case FECRate.FECRate78:
                    streamWriter.Write("fec_rate_in:8 ");
                    break;
                case FECRate.FECRate89:
                    streamWriter.Write("fec_rate_in:9 ");
                    break;
                default:
                    streamWriter.Write("fec_rate_in:3 ");
                    break;
            }

            if (RunParameters.Instance.SageTVSatelliteNumber != -1)
                streamWriter.Write("sat:" + RunParameters.Instance.SageTVSatelliteNumber + " ");
            else
                streamWriter.Write("sat:0 ");
        }

        private static void processTerrestrial(StreamWriter streamWriter, TVStation station, TerrestrialFrequency frequency)
        {
            if (frequency.Frequency != 0)
                streamWriter.Write("frq:" + frequency.Frequency + " ");
            else
            {
                if (station.ActualFrequency != -1)
                    streamWriter.Write("frq:" + (station.ActualFrequency * 1000) + " ");
                else
                    streamWriter.Write("frq:" + (RunParameters.Instance.FrequencyCollection[0].Frequency * 1000) + " ");
            }

            streamWriter.Write("band:" + frequency.Bandwidth + " ");
        }

        private static void processCable(StreamWriter streamWriter, TVStation station, CableFrequency frequency)
        {
            streamWriter.Write("frq:" + (frequency.Frequency * 10) + " ");
            streamWriter.Write("rate:" + (frequency.SymbolRate / 1000) + " ");

            switch (frequency.Modulation)
            {                
                case SignalModulation.Modulation.QAM16:
                    streamWriter.Write("mod:1 ");
                    break;
                case SignalModulation.Modulation.QAM32:
                    streamWriter.Write("mod:2 ");
                    break;
                case SignalModulation.Modulation.QAM64:
                    streamWriter.Write("mod:3 ");
                    break;
                case SignalModulation.Modulation.QAM80:
                    streamWriter.Write("mod:4 ");
                    break;
                case SignalModulation.Modulation.QAM96:
                    streamWriter.Write("mod:5 ");
                    break;
                case SignalModulation.Modulation.QAM112:
                    streamWriter.Write("mod:6 ");
                    break;
                case SignalModulation.Modulation.QAM128:
                    streamWriter.Write("mod:7 ");
                    break;
                case SignalModulation.Modulation.QAM160:
                    streamWriter.Write("mod:8 ");
                    break;
                case SignalModulation.Modulation.QAM192:
                    streamWriter.Write("mod:9 ");
                    break;
                case SignalModulation.Modulation.QAM224:
                    streamWriter.Write("mod:10 ");
                    break;
                case SignalModulation.Modulation.QAM256:
                    streamWriter.Write("mod:11 ");
                    break;
                case SignalModulation.Modulation.QAM320:
                    streamWriter.Write("mod:12 ");
                    break;
                case SignalModulation.Modulation.QAM384:
                    streamWriter.Write("mod:13 ");
                    break;
                case SignalModulation.Modulation.QAM448:
                    streamWriter.Write("mod:14 ");
                    break;
                case SignalModulation.Modulation.QAM512:
                    streamWriter.Write("mod:15 ");
                    break;
                case SignalModulation.Modulation.QAM640:
                    streamWriter.Write("mod:16 ");
                    break;
                case SignalModulation.Modulation.QAM768:
                    streamWriter.Write("mod:17 ");
                    break;
                case SignalModulation.Modulation.QAM896:
                    streamWriter.Write("mod:18 ");
                    break;
                case SignalModulation.Modulation.QAM1024:
                    streamWriter.Write("mod:19 ");
                    break;
                case SignalModulation.Modulation.QPSK:
                    streamWriter.Write("mod:20 ");
                    break;
                case SignalModulation.Modulation.BPSK:
                    streamWriter.Write("mod:21 ");
                    break;
                case SignalModulation.Modulation.OQPSK:
                    streamWriter.Write("mod:22 ");
                    break;
                case SignalModulation.Modulation.VSB8:
                    streamWriter.Write("mod:23 ");
                    break;
                case SignalModulation.Modulation.VSB16:
                    streamWriter.Write("mod:24 ");
                    break;
                default:
                    streamWriter.Write("mod:3 ");
                    break;
            }

            switch (frequency.FEC.Rate)
            {
                case FECRate.FECRate12:
                    streamWriter.Write("fec_rate_in:1 ");
                    break;
                case FECRate.FECRate23:
                    streamWriter.Write("fec_rate_in:2 ");
                    break;
                case FECRate.FECRate34:
                    streamWriter.Write("fec_rate_in:3 ");
                    break;
                case FECRate.FECRate35:
                    streamWriter.Write("fec_rate_in:4 ");
                    break;
                case FECRate.FECRate45:
                    streamWriter.Write("fec_rate_in:5 ");
                    break;
                case FECRate.FECRate56:
                    streamWriter.Write("fec_rate_in:6 ");
                    break;
                case FECRate.FECRate511:
                    streamWriter.Write("fec_rate_in:7 ");
                    break;
                case FECRate.FECRate78:
                    streamWriter.Write("fec_rate_in:8 ");
                    break;
                case FECRate.FECRate89:
                    streamWriter.Write("fec_rate_in:9 ");
                    break;
                default:
                    streamWriter.Write("fec_rate_in:3 ");
                    break;
            }
        }
    }        
}

