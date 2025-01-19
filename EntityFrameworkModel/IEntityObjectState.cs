using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace adsmap.EntityFrameworkModel
{

    public interface IEntityObjectState
    {
        EntityObjectState ObjectState { get; set; }
    }

    public enum EntityObjectState
    {
        Added,
        Modified,
        Deleted,
        Unchanged
    }
}
/*
 * :IEntityObjectState
 * 
 * 
 * public EntityObjectState ObjectState { get; set; }
*/
