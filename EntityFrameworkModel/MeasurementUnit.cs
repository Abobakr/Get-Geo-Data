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
    
    public partial class MeasurementUnit : IEntityObjectState
    {
        public EntityObjectState ObjectState { get; set; }
        public MeasurementUnit()
        {
            this.Roles = new HashSet<Role>();
            this.MeasurementUnits1 = new HashSet<MeasurementUnit>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public float Multiplier { get; set; }
        public Nullable<int> BaseUnitId { get; set; }
    
         
        public virtual ICollection<Role> Roles { get; set; }
         
        public virtual ICollection<MeasurementUnit> MeasurementUnits1 { get; set; }
        public virtual MeasurementUnit MeasurementUnit1 { get; set; }
    }
}
