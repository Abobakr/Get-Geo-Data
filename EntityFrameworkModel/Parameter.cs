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
    
    public partial class Parameter : IEntityObjectState
    {
        public EntityObjectState ObjectState { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
        public int FormulaId { get; set; }
    
        public virtual Formula Formula { get; set; }
    }
}
