/* 
 * Application name: ParcelTrack
 * Author: Julia Swietochowska, 40495101
 * Last modified: 27 / 11 / 2021
 */
using System;
using System.IO;

namespace Coursework
{
    /* 
     * Singleton Logger class for logging certain events
     */ 
    public class Logger
    {
        // private attributes
        // store the singleton Logger copy 
        private static Logger instance;
        private string filename = @"log.txt";

        // Private constructor so that only one copy can be created
        private Logger()
        {
            // check if file with the same filename exists
            // if yes - delete it
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        // public property for accessing the singleton instance of Logger
        public static Logger Instance
        {
            get
            {
                if (instance == null)
                // If this is being called for the first time, instanciate a Logger object.
                {
                    instance = new Logger();
                }
                return instance;
            }

        }

        /*
         * Logs the creation of a new courier object. 
         * Takes the object as a parameter.
         */ 
        public void LogCourier(Courier courier)
        {
            string timestamp = DateTime.Now.ToString("g");
            string date = timestamp.Split(" ")[0];
            string time = timestamp.Split(" ")[1];
            string cType;
            if (courier.GetType().ToString().Contains("Van"))
            {
                cType = "Van";
            }
            else if (courier.GetType().ToString().Contains("Cycle"))
            {
                cType = "Cycle";
            }
            else
            {
                cType = "Walking";
            }
            using (StreamWriter file = new StreamWriter(filename, true))
            {
                file.WriteLine(time + " " + date + " New Courier added: id = " + courier.Id + ", type = " + cType);
            }
        }

        /*
         * Logs the creation of a new parcel object. 
         * Takes the object as a parameter.
         */
        public void LogParcel(Parcel parcel)
        {
            string timestamp = DateTime.Now.ToString("g");
            string date = timestamp.Split(" ")[0];
            string time = timestamp.Split(" ")[1];

            using (StreamWriter file = new StreamWriter(filename, true))
            {
                if (parcel.Courier == null)
                {
                    file.WriteLine(time + " " + date + " New Parcel added (" + parcel.Postcode + ", \""
                    + parcel.Addressee + "\") Not allocated ");
                }
                else
                {
                    file.WriteLine(time + " " + date + " New Parcel added (" + parcel.Postcode + ", \""
                    + parcel.Addressee + "\") Allocated to Courier " + parcel.Courier.Id);
                } 
            }
        }

        /*
         * Logs the transfer of a parcel to a new courier.
         * Takes the parcel and new courier objects as a parameters.
         */
        public void LogTransfer(Parcel parcel, Courier newCourier)
        {
            string timestamp = DateTime.Now.ToString("g");
            string date = timestamp.Split(" ")[0];
            string time = timestamp.Split(" ")[1];

            using (StreamWriter file = new StreamWriter(filename, true))
            {
                file.WriteLine(time + " " + date + " Parcel (" + parcel.Postcode
                    + ") transferred to Courier " + newCourier.Id);
            }
        }

        /*
         * Logs the fact of displaying area summary.
         */
        public void LogSummary()
        {
            string timestamp = DateTime.Now.ToString("g");
            string date = timestamp.Split(" ")[0];
            string time = timestamp.Split(" ")[1];

            using (StreamWriter file = new StreamWriter(filename, true))
            {
                file.WriteLine(time + " " + date + " Areas Summary Displayed");
            }
        }

        /*
         * Logs the fact of displaying delivery schedule for a chosen courier. 
         * Takes the courier object as a parameter.
         */
        public void LogSchedule(Courier courier)
        {
            string timestamp = DateTime.Now.ToString("g");
            string date = timestamp.Split(" ")[0];
            string time = timestamp.Split(" ")[1];

            using (StreamWriter file = new StreamWriter(filename, true))
            {
                file.WriteLine(time + " " + date + " Courier " + courier.Id + " delivery schedule displayed");
            }
        }

        /*
         * Logs the read from the .csv file. 
         */
        public void LogReadCSV()
        {
            string timestamp = DateTime.Now.ToString("g");
            string date = timestamp.Split(" ")[0];
            string time = timestamp.Split(" ")[1];

            using (StreamWriter file = new StreamWriter(filename, true))
            {
                file.WriteLine(time + " " + date + " Data read from the CSV file");
            }

        }

        /*
         * Logs the write of couriers and parcels to the .csv file. 
         */
        public void LogWriteCSV()
        {
            string timestamp = DateTime.Now.ToString("g");
            string date = timestamp.Split(" ")[0];
            string time = timestamp.Split(" ")[1];

            using (StreamWriter file = new StreamWriter(filename, true))
            {
                file.WriteLine(time + " " + date + " Data written to the CSV file");
            }
        }
    }
}
