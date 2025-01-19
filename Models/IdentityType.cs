using System.Runtime.Serialization;

namespace  adsmap.Models
{
    // Use a data contract as illustrated in the sample below to add composite types to service operations.

    
    public class IdentityType
    {
        int id = -1;
        string name = "";

        public IdentityType(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

       
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        
       
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

}
