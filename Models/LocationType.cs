using System.Runtime.Serialization;

namespace  adsmap.Models
{
    // Use a data contract as illustrated in the sample below to add composite types to service operations.

    
    public class LocationType
    {
        public LocationType(double latitude, double longtitude)
        {
            this.Lat = latitude;
            this.Lng = longtitude;
        }


        public double Lat { get; set; } = 0;


        public double Lng { get; set; } = 0;
    }

}
