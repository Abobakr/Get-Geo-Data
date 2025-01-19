using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace  adsmap.Models
{
    
    public class UnitItemFilter
    {
        public DateTime LimitDate { get; set; }
        public IList<int> TerminalIdes { get; set; }
        public bool IsFirstRequest { get; set; } = false;


        public string Id { get; set; } = "";


        public int ComponentId { get; set; } = -1;


        public int RoleId { get; set; } = -1;


        public double MinValue { get; set; } = 0;


        public double MaxValue { get; set; } = 0;


        public DateTime DateTime { get; set; }


        public bool IsSubFilter { get; set; } = false;


        public LocationType SouthWest { get; set; }


        public LocationType NorthEast { get; set; }


        public UnitItemFilter()
        {

        }

        public UnitItemFilter(string id, int componentId, int roleId, double minValue, double maxValue, DateTime dateTime, bool isSubFilter, LocationType southWest, LocationType northEast)
        {
            // TODO: Complete member initialization
            Id = id;
            ComponentId = componentId;
            RoleId = roleId;
            MinValue = minValue;
            MaxValue = maxValue;
            DateTime = dateTime;
            IsSubFilter = isSubFilter;
            SouthWest = southWest;
            NorthEast = northEast;
            
        }

    }
}
