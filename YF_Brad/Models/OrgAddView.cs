using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YF_Brad.Models.ViewModels
{
    public class OrgAddView
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Fax { get; set; }
        [Display(Name = "Org Type")]
        public string OrgType { get; set; }
        [Required]
        public int OrgTypeID { get; set; }

        public int AddID { get; set; }
        [Required]
        public int OrgID { get; set; }
        [Required]
        public int AddTypeID { get; set; }
        [Display(Name = "Address Type")]
        public string AddType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        [Required]
        public int PostalID { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
        public string County { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<decimal> Latitude { get; set; }

        public int BillingAddID { get; set; }
        [Required]
        public int BillingOrgID { get; set; }
        [Required]
        public int BillingAddTypeID { get; set; }
        [Display(Name = "Billing Address Type")]
        public string BillingAddType { get; set; }
        [Display(Name = "Firstname")]
        public string BillingFirstName { get; set; }
        [Display(Name = "Lastname")]
        public string BillingLastName { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string BillingAddress { get; set; }
        [Display(Name = "Address 2")]
        public string BillingAddress2 { get; set; }
        [Required]
        public int BillingPostalID { get; set; }
        [Display(Name = "City")]
        public string BillingCity { get; set; }
        [Display(Name = "State")]
        public string BillingState { get; set; }
        [Display(Name = "Zip")]
        public int BillingZip { get; set; }
        [Display(Name = "County")]
        public string BillingCounty { get; set; }
        [Display(Name = "Longitude")]
        public Nullable<decimal> BillingLongitude { get; set; }
        [Display(Name = "Latitude")]
        public Nullable<decimal> BillingLatitude { get; set; }

        [Required]
        public bool Active { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}