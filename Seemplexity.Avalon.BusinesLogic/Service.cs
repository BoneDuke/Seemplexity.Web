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
    
    public partial class Service
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Service()
        {
            this.AddDescript1 = new HashSet<AddDescript1>();
            this.AddDescript2 = new HashSet<AddDescript2>();
            this.ServiceLists = new HashSet<ServiceList>();
            this.TurServices = new HashSet<TurService>();
        }
    
        public int SV_KEY { get; set; }
        public string SV_NAME { get; set; }
        public string SV_NAMELAT { get; set; }
        public Nullable<short> SV_TYPE { get; set; }
        public Nullable<int> SV_CONTROL { get; set; }
        public Nullable<short> SV_ISCITY { get; set; }
        public Nullable<short> SV_ISSUBCODE1 { get; set; }
        public Nullable<short> SV_ISSUBCODE2 { get; set; }
        public Nullable<short> SV_ROUNDBRUTTO { get; set; }
        public byte[] ROWID { get; set; }
        public string SV_StdKey { get; set; }
        public string SV_Code { get; set; }
        public short SV_IsDuration { get; set; }
        public short SV_UseManualInput { get; set; }
        public short SV_QUOTED { get; set; }
        public Nullable<short> SV_IsIndividual { get; set; }
        public Nullable<decimal> SV_LittlePercent { get; set; }
        public Nullable<short> SV_LittlePlace { get; set; }
        public Nullable<bool> SV_LittleAnd { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AddDescript1> AddDescript1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AddDescript2> AddDescript2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceList> ServiceLists { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TurService> TurServices { get; set; }
    }
}
