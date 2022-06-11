/* 
 * Application name: ParcelTrack
 * Author: Julia Swietochowska, 40495101
 * Last modified: 27 / 11 / 2021
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace Coursework
{
    /*
     * Singleton Class for reading and writing from/to .csv file. 
     */ 
    class Storage
    {
        // private attributes
        // store the singleton Storage copy 
        private static string csv = @"data.csv";
        private static Storage instance;

        // Private constructor so that only one copy can be created
        private Storage() { }

        // public property for accessing the singleton instance of Storage
        internal static Storage Instance
        {
            get
            {
                if (instance == null)
                // If this is being called for the first time, instanciate a Logger object.
                {
                    instance = new Storage();
                }
                return instance;
            }
        }

        /*
         * Checks if a file with the specific filename exists.
         * If yes, deletes it.
         */ 
        public void CheckDeleteFile()
        {
            if (File.Exists(csv))
            {
                File.Delete(csv);
            }
        }

        /*
         * Returns true if a file with the specified filename exists.
         */ 
        public bool FileExists()
        {
            if (!File.Exists(csv))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /*
         * Reads the .csv file and creates Parcel objects stored in it.
         * Returns the Parcel objects in the list.
         */ 
        public List<Parcel> ReadParcels()
        {
            List<Parcel> parcels = new List<Parcel>();
            using (StreamReader reader = new StreamReader(csv))
            {
                // ignore the header
                string header = reader.ReadLine();

                // read and create all parcels
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] split = line.Split(",");
                    if (split[0].Equals("Parcel"))
                    {
                        string addressee = split[2] + "," + split[3];
                        // create a new parcel object
                        Parcel p = new Parcel(addressee, split[1]);
                        parcels.Add(p);
                    }
                }
            }
            return parcels;
        }

        /*
         * Reads the .csv file and creates Courier objects stored in it.
         * Returns the Courier objects in the list.
         */
        public List<Courier> ReadCouriers(List<Parcel> parcels)
        {
            List<Courier> couriers = new List<Courier>();
            using (StreamReader reader = new StreamReader(csv))
            {
                // ignore the header
                string header = reader.ReadLine();

                // read and create all couriers
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] split = line.Split(",");
                    if (split[0].Contains("Courier"))
                    {
                        Courier courier;
                        // create a courier object of the correct type
                        if (split[0].Contains("Van"))
                        {
                            courier = new VanCourier();
                            couriers.Add(courier);
                        }
                        else if (split[0].Contains("Cycle"))
                        {
                            courier = new CycleCourier();
                            couriers.Add(courier);
                        }
                        else
                        {
                            courier = new WalkingCourier();
                            couriers.Add(courier);
                        }

                        // assign the details
                        courier.Id = Int32.Parse(split[1]);
                        // update the last allocated ID to avoid duplication and errors 
                        AllocateID.LastID = courier.Id;
                        courier.Name = split[2];
                        string[] areas = split[3].Split(" ");
                        // assign areas
                        foreach (string a in areas)
                        {
                            if (!String.IsNullOrEmpty(a))
                            {
                                foreach (Area area in ParcelTrack.Areas)
                                {
                                    if (a.Equals(area.Code))
                                    {
                                        courier.Areas.Add(area);
                                        area.Couriers.Add(courier);
                                        break;
                                    }
                                }
                            }
                        }
                        // assign deliveries
                        string[] deliveries = split[4].Split(" ");
                        foreach (string d in deliveries)
                        {
                            if (!String.IsNullOrEmpty(d))
                            {
                                foreach (Parcel p in parcels)
                                {
                                    if (p.Postcode.Contains(d))
                                    {
                                        p.Courier = courier;
                                        courier.Deliveries.Add(p);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return couriers;
        }

        /*
         * Writes all Courier and Parcel objects currently in the system to the .csv file.
         */ 
        public void Write()
        {
            // get all couriers and parcels currently stored in the system
            List<Courier> couriers = ParcelTrack.Couriers;
            List<Parcel> parcels = ParcelTrack.Parcels;
            using (StreamWriter file = new StreamWriter(csv, true))
            {
                // write a header
                string header = "Parcel/Courier,Postcode/Courier ID,Addressee/Name,Address/Areas,CourierID/Deliveries";
                file.WriteLine(header);
                // write all parcels to the csv file
                foreach (Parcel p in parcels)
                {
                    string line = "Parcel," + p.Postcode + ',' + p.Addressee + "," + p.Courier.Id;
                    file.WriteLine(line);
                }
                // write all couriers to the csv file
                foreach (Courier c in couriers)
                {
                    string type = c.GetType().ToString().Split('.')[1];
                    string line = type + "," + c.Id + "," + c.Name + ',';
                    foreach (Area a in c.Areas)
                    {
                        line += a.Code;
                        line += " ";
                    }
                    line += ",";
                    foreach (Parcel p in c.Deliveries)
                    {
                        line += p.Postcode.Split(" ")[1];
                        line += " ";
                    }
                    file.WriteLine(line);
                }
            }
        }
    }
}
