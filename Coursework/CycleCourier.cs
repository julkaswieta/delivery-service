/* 
 * Application name: ParcelTrack
 * Author: Julia Swietochowska, 40495101
 * Last modified: 27 / 11 / 2021
 */
namespace Coursework
{
    /*
     * Concrete class for creating and manipulating Cycle Courier objects.
     * Extends Courier.
     */
    class CycleCourier : Courier
    {
        // Constructors
        /*
         * Default empty constructor
         */ 
        public CycleCourier()
        {
            ParcelLimit = 10;
        }
        /*
         * Creates a CycleCourier object.
         * Specifies its parcelLimit.
         */ 
        public CycleCourier(string name) : base(name)
        {
            ParcelLimit = 10;
        }
    }
}
