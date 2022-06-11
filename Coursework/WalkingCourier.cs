/* 
 * Application name: ParcelTrack
 * Author: Julia Swietochowska, 40495101
 * Last modified: 27 / 11 / 2021
 */
namespace Coursework
{
    /*
     * Concrete class for creating and manipulating Walking Courier objects.
     * Extends Courier.
     */
    class WalkingCourier : Courier
    {
        // Constructors
        /*
         * Default empty constructor
         */
        public WalkingCourier()
        {
            ParcelLimit = 5;
        }

        /*
         * Creates a VanCourier object.
         * Specifies its parcelLimit.
         */
        public WalkingCourier(string name) : base(name)
        {
            ParcelLimit = 5;
        }
    }
}
