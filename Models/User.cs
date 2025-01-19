using System.Collections.Generic;

namespace adsmap.Models
{
    public class User
    {
        public string UserId { get; set; }

        public LocationType Center { get; set; }

        public IList<FilterComponent> FilterComponents { get; set; }
    }
}