//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace YF_Brad.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HDAddress
    {
        public int ID { get; set; }
        public int HDID { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public Nullable<int> ZIPCodeID { get; set; }
        public byte[] SSMA_TimeStamp { get; set; }
    
        public virtual HealthDistrict HealthDistrict { get; set; }
        public virtual PostalInfo PostalInfo { get; set; }
    }
}