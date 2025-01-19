using System.Collections.Generic;
using System.Runtime.Serialization;

namespace  adsmap.Models
{
    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    
    public class FilterComponent : IdentityType
    {
        public IList<FilterComponent> SubFilterComponents { get; set; }

        public FilterComponent(int id, string name)
            : base(id, name)
        {

        }
    }
}
