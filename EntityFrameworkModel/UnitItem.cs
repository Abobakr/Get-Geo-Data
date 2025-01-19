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
    
    public partial class UnitItem : IEntityObjectState
    {
        public EntityObjectState ObjectState { get; set; }
        public UnitItem()
        {
            this.TerminalItems = new HashSet<TerminalItem>();
            this.UnitItems1 = new HashSet<UnitItem>();
        }
    
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public int UnitTypetId { get; set; }
        public Nullable<int> MotherUnitItemId { get; set; }
    
         
        public virtual ICollection<TerminalItem> TerminalItems { get; set; }
        public virtual UnitType UnitType { get; set; }
         
        public virtual ICollection<UnitItem> UnitItems1 { get; set; }
        public virtual UnitItem UnitItem1 { get; set; }
    }
}
