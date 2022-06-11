/* Application name: ParcelTrack
 * Author: Julia Swietochowska, 40495101
 * Last modified: 27 / 11 / 2021
 */
namespace Coursework
{
    /* 
     * Static class for allocating IDs to the couriers. 
     */ 
    static class AllocateID
    {
        // private attributes
        private static int lastID = 0;

        // public properties for accessing the private attributes
        public static int LastID { get => lastID; set => lastID = value; }

        /*
         * Returns the next ID based on the value of the lastID attribute.
         */
        public static int NextID()
        {
            LastID++;
            return LastID;
        }

    }
}
