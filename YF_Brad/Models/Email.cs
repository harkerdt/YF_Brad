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
    
    public partial class Email
    {
        public int ID { get; set; }
        public int DoctorID { get; set; }
        public int OrgID { get; set; }
        public string Email1 { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    
        public virtual Doctor Doctor { get; set; }
        public virtual Organization Organization { get; set; }
    }
}