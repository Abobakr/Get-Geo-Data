using System;
using System.Runtime.Serialization;

namespace  adsmap.Models
{
    
    public class UnitItemDetails
    {
        public UnitItemDetails(string name, string modelNo,string roleName, double lastValue,double ?alertValue, DateTime date)
        {
            Name = name;
            ModelNo = modelNo;
            RoleName = roleName;
            LastValue = lastValue;
            if (alertValue != null)
                AlertValue =  (double)alertValue;
            else AlertValue = double.MaxValue;
            Date = date;
        }


        public double LastValue { get; set; } = 0;


        public DateTime Date { get; set; }


        public string Name { get; set; } = "";

        public string ModelNo { get; set; } = "";

        public string RoleName { get; set; } = "";

        public double AlertValue { get; set; } = 0;


        public bool IsOverflow { get; set; } = false;

    }
}
