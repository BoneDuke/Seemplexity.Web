//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Seemplexity.Avalon.BusinesLogic
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vehicle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vehicle()
        {
            this.VehiclePlans = new HashSet<VehiclePlan>();
        }
    
        public int VH_KEY { get; set; }
        public Nullable<int> VH_HOSTKEY { get; set; }
        public Nullable<int> VH_SVKEY { get; set; }
        public Nullable<short> VH_NUMOFCOLUMN { get; set; }
        public Nullable<short> VH_NUMOFROW { get; set; }
        public Nullable<short> VH_NUMOFAREA { get; set; }
        public string VH_TITLE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VehiclePlan> VehiclePlans { get; set; }
    }
}