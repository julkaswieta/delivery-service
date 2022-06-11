/* 
 * Application name: ParcelTrack
 * Author: Julia Swietochowska, 40495101
 * Last modified: 27 / 11 / 2021
 */
namespace Coursework
{
    /*
     * Class for creating and manipualting parcel objects 
     */ 
    public class Parcel
    {
        // private attributes
        private string addressee;
        private string postcode;
        private Courier courier;

        // Constructors
        public Parcel(string addressee, string postcode)
        {
            this.Addressee = addressee;
            this.Postcode = postcode;
        }

        // Public properties for accessing private attributes
        public string Addressee { get => addressee; set => addressee = value; }
        public string Postcode { get => postcode; set => postcode = value; }
        internal Courier Courier { get => courier; set => courier = value; }

        /*
         * Parcel object's string representation.
         * Returns a string with object's details.
         */ 
        public string Display()
        {
            string line = "Parcel, postcode: " + Postcode + ", \n\taddress: \"" + Addressee + "\"";
            return line;
        }
    }
}
