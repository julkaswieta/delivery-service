/* 
 * Application name: ParcelTrack
 * Author: Julia Swietochowska, 40495101
 * Last modified: 27/11/2021 
 * Application for managing parcel delivery system. 
 * Supports storage of objects in a .csv file. Provides a log file.
 */
using System;
using System.Collections.Generic;

namespace Coursework
{
    /* 
     * Main class for controlling the whole ParcelTrack system (Facade)
     */
    static class ParcelTrack
    {
        // initialise logger and storage singletons 
        private static Logger logger = Logger.Instance;
        private static Storage storage = Storage.Instance;

        // private attributes
        private static List<Courier> couriers = new List<Courier>();
        private static List<Parcel> parcels = new List<Parcel>();
        private static List<Area> areas = new List<Area>();

        // properties for accessing private attributes
        internal static List<Courier> Couriers { get => couriers; set => couriers = value; }
        internal static List<Parcel> Parcels { get => parcels; set => parcels = value; }
        internal static List<Area> Areas { get => areas; set => areas = value; }

        /* 
         * Creates a new Courier object of the correct subclass based on the type specified
         * Adds the object to the courier list and logs its creation.
         * Takes the courier's name, type and list of areas served as parameters.
         * Returns the newly created Courier object
         */
        public static Courier AddCourier(string name, string type, List<Area> areas)
        {
            Courier c = null;
            // create the correct subtype of the courier
            if (type.Equals("Van"))
            {
                VanCourier vc = new VanCourier(name);
                vc.Areas = areas;
                c = vc;
                couriers.Add(vc);
                // add the courier to the list of couriers of each area
                foreach (Area a in areas)
                {
                    a.Couriers.Add(vc);
                }
            }
            else if (type.Equals("Cycle"))
            {
                CycleCourier cc = new CycleCourier(name);
                cc.Areas = areas;
                c = cc;
                couriers.Add(cc);
                // add the courier to the list of couriers of each area
                foreach (Area a in areas)
                {
                    a.Couriers.Add(cc);
                }
            }
            else if (type.Equals("Walking"))
            {
                WalkingCourier wc = new WalkingCourier(name);
                wc.Areas = areas;
                c = wc;
                couriers.Add(wc);
                // add the courier to the list of couriers of each area
                foreach (Area a in areas)
                {
                    a.Couriers.Add(wc);
                }
            }
            // log the addition
            logger.LogCourier(c);
            return c;
        }

        /* 
         * Creates a new Parcel object and assigns it to the first courier of the area with capacity left.
         * Adds the object to the parcel list and logs its creation.
         * Takes the addressee and postcode as parameters.
         * Returns an error message if anything is wrong or a success message if a parcel is created and allocated.
         */
        public static string AddParcel(String addressee, String postcode)
        {
            Parcel p = new Parcel(addressee, postcode);
            // Allocate the parcel to the first courier that works the area and has capacity left
            // check the area of the parcel
            string code = postcode.Split(" ")[0];
            Area area = null;
            foreach (Area a in areas)
            {
                if (code.Equals(a.Code))
                {
                    area = a;
                    break;
                }
            }
            bool allocated = false;
            // find the first courier of the are with capacity left
            foreach (Courier c in area.Couriers)
            {
                if (c.Deliveries.Count < c.ParcelLimit)
                {
                    c.Deliveries.Add(p);
                    p.Courier = c;
                    allocated = true;
                    break;
                }
            }
            parcels.Add(p);
            // log the addition
            logger.LogParcel(p);

            string message = "";
            // generate return messages depending on whether the allocation was successful or not
            if (allocated)
            {
                message = "Parcel added successfully. \nAllocated to " + p.Courier.GetType().ToString().Split(".")[1] + ", ID = " + p.Courier.Id;
            }
            else
            {
                message = "Parcel couldn't be allocated.";
            }

            return message;
        }

        /* 
         * Transfers a Parcel to a new specified Courier. First, it validates the transition.
         * Adds the Parcel to the courier list and logs its creation.
         * Takes the Parcel object and the new courier's ID as parameters.
         * Returns message regarding the outcome of the transfer (success or specific error).
         */
        public static string TransferParcel(string postcode, int newCourierID)
        {
            string message = null;
            Parcel parcel = null;
            Courier courier = null;

            // check if the parcel and courier exist
            bool found1 = false;
            foreach (Parcel p in parcels)
            {
                if (p.Postcode.Equals(postcode))
                {
                    parcel = p;
                    found1 = true;
                    break;
                }
            }
            bool found2 = false;
            foreach (Courier c in couriers)
            {
                if (c.Id == newCourierID)
                {
                    courier = c;
                    found2 = true;
                    break;
                }
            }
            // If one of them wasn't found, return the message 
            if (!found1)
            {
                message = "No such parcel found";
            }
            else if (!found2)
            {
                message = "No such courier found";
            }

            bool correctArea = false;
            bool sameToFrom = false;
            // If they were both identified, check if the transfer is not to and from the same courier
            // then, check if the courier operates in the correct area and has capacity
            if (found1 && found2)
            {
                // check the currently allocated and new courier
                if (parcel.Courier != null)
                {
                    if(parcel.Courier.Id == newCourierID)
                    {
                        sameToFrom = true;
                    }
                }
                // if it's not the same one, continue the checks
                if (!sameToFrom)
                {
                    // check the area
                    foreach (Area a in courier.Areas)
                    {
                        if (a.Code.Equals(parcel.Postcode.Split(" ")[0]))
                        {
                            correctArea = true;
                            break;
                        }
                    }

                    // if no area matched, return an error message 
                    if (!correctArea)
                    {
                        message = "Courier " + courier.Id + " does not operate in this area";
                    }
                    // if area is correct, check the capacity
                    else
                    {
                        if (courier.Deliveries.Count < courier.ParcelLimit)
                        {
                            // remove the parcel from the previous courier (if existed)
                            if (parcel.Courier != null)
                            {
                                parcel.Courier.Deliveries.Remove(parcel);
                            }
                            // change the courier assignment to the parcel as well
                            parcel.Courier = courier;
                            courier.Deliveries.Add(parcel);
                            logger.LogTransfer(parcel, courier);
                            message = "Parcel successfully reallocated to courier with ID = " + courier.Id;
                        }
                        // if no capacity left, return an error message
                        else
                        {
                            message = "Courier " + courier.Id + " does not have capacity left";
                        }
                    }
                }
                else
                {
                    message = "Cannot transfer to and from the same courier";
                }

            }
            return message;
        }

        /* 
         * Generates a delivery schedule for the courier chosen.
         * Takes the courier's ID as a parameter.
         * Returns the delivery schedule as a list of strings.
         */
        public static List<string> GetCourierSchedule(int courierID)
        {
            List<string> schedule = new List<string>();
            // find the courier chosen in the list of all couriers
            foreach (Courier c in couriers)
            {
                if (c.Id == courierID)
                {
                    // get the courier's schedule
                    schedule = c.Display();
                    // log the display
                    logger.LogSchedule(c);
                    // break when the chosen courier was found 
                    break;
                }
            }
            return schedule;
        }

        /* 
         * Generates area summary (which courier in area has what number of parcels allocated)
         * Returns the summary as a list of strings.
         */
        public static List<string> GetAreaSummary()
        {
            List<string> allAreas = new List<string>();
            foreach (Area area in areas)
            {
                allAreas.AddRange(area.Display());
            }
            logger.LogSummary();
            return allAreas;
        }

        /* 
         * Reads the .csv file and created Courier and Parcel objects. 
         * Adds them to the system.
         * Assumes that the data in the .csv file is in the correct format 
         * and conforms to any established rules (e.g. unique IDs among themselves).
         * Returns a message regarding the outcome of the read (error, fully or partially successful).
         */
        public static string ReadCSV()
        {
            string readSuccessful;
            List<Parcel> parcels = new List<Parcel>();
            List<Courier> couriers = new List<Courier>();

            // check if the file to read exists 
            if (!storage.FileExists())
            {
                readSuccessful = "No file to read";
            }
            else
            {
                // read the courier and parcel objects from the .csv file
                parcels.AddRange(storage.ReadParcels());
                couriers.AddRange(storage.ReadCouriers(parcels));

                // get the lists of all couriers and parcels already in the system
                List<Parcel> parcelTrackParcels = new List<Parcel>(ParcelTrack.Parcels);
                List<Courier> parcelTrackCouriers = new List<Courier>(ParcelTrack.Couriers);

                readSuccessful = "Data read successfully";
                // log the read (no matter if fully or partially successful)
                logger.LogReadCSV();

                // check if the newly created couriers are unique
                foreach (Courier c in couriers)
                {
                    if (parcelTrackCouriers.Count > 0)
                    {
                        foreach (Courier courier in parcelTrackCouriers)
                        {
                            // if the newly read courier's id is not unique, generate an error message
                            if (c.Id == courier.Id)
                            {
                                readSuccessful = "Some of the couriers and/or parcels did not have a unique ID. Read partially successful.";
                                foreach (Parcel p in c.Deliveries)
                                {
                                    // delete the duplicate courier from the system
                                    p.Courier = null;
                                }
                                break;
                            }
                            else
                            {
                                // add the unique courier to the system
                                ParcelTrack.Couriers.Add(c);
                                // log the creation
                                logger.LogCourier(c);
                            }
                        }
                    }
                    // if there are no couriers in the system yet, just add the ones read to it
                    else
                    {
                        ParcelTrack.Couriers.Add(c);
                        // log the creation
                        logger.LogCourier(c);
                    }

                }

                // check if the newly created parcels are unique
                foreach (Parcel p in parcels)
                {
                    if (parcelTrackParcels.Count > 0)
                    {
                        foreach (Parcel parcel in parcelTrackParcels)
                        {
                            // if the newly created parcel's delivery ID's not unique, generate an error message
                            if (p.Postcode.Split(" ")[1].Equals(parcel.Postcode.Split(" ")[1]))
                            {
                                readSuccessful = "Some of the couriers and/or parcels did not have a unique ID. Read partially successful.";
                                // remove the duplicate parcel from the system
                                if(p.Courier != null)
                                {
                                    p.Courier.Deliveries.Remove(p);
                                }
                                break;
                            }
                            else
                            {
                                // add the unique parcel to the system
                                ParcelTrack.Parcels.Add(p);
                                // log the addition
                                logger.LogParcel(p);
                            }
                        }
                    }
                    // if there are no parcels in the system yet, just add the new parcels
                    else
                    {
                        ParcelTrack.Parcels.Add(p);
                        // log the addition
                        logger.LogParcel(p);
                    }
                }
            }
            return readSuccessful;
        }

        /* 
         * Writes to the .csv file to persists the created courier and parcel objects.
         * Returns a message regarding the outcome of the write (error or successful).
         */
        public static string WriteToCSV()
        {
            string message = "";
            // if there are no couriers, nor parcels to persist, return an error message
            if (couriers.Count == 0 && parcels.Count == 0)
            {
                message = "No data to write";
            }
            else
            {
                // delete the current .csv file to create a new one 
                storage.CheckDeleteFile();
                storage.Write();
                // log the write to .csv
                logger.LogWriteCSV();
                message = "Data saved successfully.";
            }
            return message;
        }

        /*
         * Generates Area objects from the areas' codes.
         * Returns a list of all Area objects.
         */
        public static List<Area> GenerateAreas()
        {
            List<String> codes = new List<string>() { "EH1", "EH2", "EH3", "EH4", "EH5", "EH6",
                "EH7", "EH8", "EH9", "EH10", "EH11", "EH12", "EH13", "EH14",
                "EH15", "EH16", "EH17", "EH18", "EH19", "EH20", "EH21", "EH22" };
            foreach (string code in codes)
            {
                Area a = new Area(code);
                areas.Add(a);
            }
            return areas;
        }
    }
}
