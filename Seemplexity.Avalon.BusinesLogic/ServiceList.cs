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
    
    public partial class ServiceList
    {
        public int SL_SVKEY { get; set; }
        public int SL_KEY { get; set; }
        public string SL_NAME { get; set; }
        public string SL_NAMELAT { get; set; }
        public Nullable<System.DateTime> SL_TIMEBEG { get; set; }
        public Nullable<System.DateTime> SL_TIMEEND { get; set; }
        public string SL_TIME { get; set; }
        public Nullable<int> SL_CNKEY { get; set; }
        public Nullable<int> SL_CTKEY { get; set; }
        public string SL_PLACEFROM { get; set; }
        public string SL_PLACETO { get; set; }
        public string SL_CODE { get; set; }
        public byte[] ROWID { get; set; }
        public string SL_StdKey { get; set; }
        public Nullable<int> SL_DaysCountMin { get; set; }
        public string SL_Url { get; set; }
        public int SL_ShowOrder { get; set; }
        public Nullable<int> SL_quKey { get; set; }
    
        public virtual Service Service { get; set; }
        public virtual tbl_Country tbl_Country { get; set; }
    }
}
