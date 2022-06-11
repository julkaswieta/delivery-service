/* 
 * Application name: ParcelTrack
 * Author: Julia Swietochowska, 40495101
 * Last modified: 27 / 11 / 2021
 */
namespace Coursework
{
    /*
     * Concrete class for creating and manipulating Van Courier objects.
     * Extends Courier.
     */
    class VanCourier : Courier
    {
        // Constructors
        /*
         * Default empty constructor
         */
        public VanCourier()
        {
            ParcelLimit = 100;
        }

        /*
         * Creates a VanCourier object.
         * Specifies its parcelLimit.
         */
        public VanCourier(string name) : base(name)
        {
            ParcelLimit = 100;
        }

    }
}
