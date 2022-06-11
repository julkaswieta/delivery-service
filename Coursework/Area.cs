/* 
 * Application name: ParcelTrack
 * Author: Julia Swietochowska, 40495101
 * Last modified: 27 / 11 / 2021
 */
using System.Collections.Generic;

namespace Coursework
{
    /*
     * Class for creating Area objects.
     */ 
    public class Area
    {
        // private attributes
        private string code;
        private List<Courier> couriers = new List<Courier>();

        // Constructors
        /*
         * Creates a new Area object.
         * Takes the area code as a parameter.
         */ 
        public Area(string code)
        {
            this.Code = code;
        }
        
        // public properties for accessing private attributes
        public string Code { get => code; set => code = value; }
        internal List<Courier> Couriers { get => couriers; set => couriers = value; }

        /*
         * Generates the summary for the area object (which couriers have what capacity left).
         * Returns the summary as a list of strings.
         */ 
        public List<string> Display()
        {
            List<string> lines = new List<string>();
            foreach (Courier c in couriers)
            {
                string type = null;
                if (c.GetType().ToString().Contains("Van"))
                {
                    type = "Van";
                }
                else if (c.GetType().ToString().Contains("Cycle"))
                {
                    type = "Cycle";
                }
                else if (c.GetType().ToString().Contains("Walking"))
                {
                    type = "Walking";
                }
                string line = Code + " - Courier (" + type + "): " + c.Deliveries.Count + "/" + c.ParcelLimit;
                lines.Add(line);
            }
            return lines;
        }
    }
}
