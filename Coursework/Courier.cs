/* 
 * Application name: ParcelTrack
 * Author: Julia Swietochowska, 40495101
 * Last modified: 27 / 11 / 2021
 */
using System.Collections.Generic;

namespace Coursework
{
    /*
     * Parent abstract class for courier objects 
     */
    public abstract class Courier
    {
        // private attributes
        private int id;
        private string name;
        private List<Parcel> deliveries = new List<Parcel>();
        private List<Area> areas = new List<Area>();
        private int parcelLimit;

        // Constructors
        public Courier() { }
        /*
         * Creates a new Courier objects and allocates the next free ID number to it.
         * Takes the courier name as a parameter.
         */ 
        public Courier(string name)
        {
            this.name = name;
            this.id = AllocateID.NextID();
        }

        // public properties for accessing the private attributes
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public List<Area> Areas { get => areas; set => areas = value; }
        public int ParcelLimit { get => parcelLimit; set => parcelLimit = value; }
        internal List<Parcel> Deliveries { get => deliveries; set => deliveries = value; }

        /*
         * Generates the delivery schedule for the courier object.
         * Returns the schedule as a list of strings.
         */
        public List<string> Display()
        {
            List<string> schedule = new List<string>();
            foreach (Parcel p in deliveries)
            {
                string line = p.Display();
                schedule.Add(line);
            }
            return schedule;
        }
    }
}
