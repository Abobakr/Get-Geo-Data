//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace adsmap.EntityFrameworkModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class TerminalItem : IEntityObjectState
    {
        public EntityObjectState ObjectState { get; set; }
        public TerminalItem()
        {
            this.Activities = new HashSet<Activity>();
        }
    
        public int Id { get; set; }
        public Nullable<int> UnitItemId { get; set; }
        public string SerialNo { get; set; }
        public System.DateTime ConstructionDate { get; set; }
    
         
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual UnitItem UnitItem { get; set; }
    }
}
