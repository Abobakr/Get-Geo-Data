using System.Runtime.Serialization;

namespace  adsmap.Models
{
    
    public class UnitItemData
    {
        public UnitItemData(int id, int activeModeId, double lastValue, double alertValue, double latitude, double longtitude)
        {
            Id = id;
            ActiveModeId = activeModeId;
            LastValue = lastValue;
            AlertValue = alertValue;
            Location = new LocationType(latitude, longtitude);
        }
        public int Id { get; set; } = 0;
        public int ActiveModeId { get; set; } = 0;
        public double LastValue { get; set; } = 0;
        public double[] SubLastValues { get; set; } = null;
        public int[] SubItemIdes { get; set; } = null;
        public LocationType Location { get; set; }
        public double AlertValue { get; set; } = 0;
        public bool IsOverflow { get; set; } = false;
    }
}
