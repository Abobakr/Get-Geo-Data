using System.Runtime.Serialization;

namespace  adsmap.Models
{
    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    
    public class TerminalType : IdentityType
    {
        public TerminalType(int id,string name):base(id, name)
        {
            
        }       
    }

}
