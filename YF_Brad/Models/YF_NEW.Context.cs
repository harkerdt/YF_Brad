﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class YF_NEWEntities : DbContext
    {
        public YF_NEWEntities()
            : base("name=YF_NEWEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AddType> AddTypes { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<DoctorOrg> DoctorOrgs { get; set; }
        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<FIP> FIPS { get; set; }
        public virtual DbSet<HDAddress> HDAddresses { get; set; }
        public virtual DbSet<HealthDirector> HealthDirectors { get; set; }
        public virtual DbSet<HealthDistrict> HealthDistricts { get; set; }
        public virtual DbSet<JobTitle> JobTitles { get; set; }
        public virtual DbSet<OrgAddress> OrgAddresses { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<OrgType> OrgTypes { get; set; }
        public virtual DbSet<PostalInfo> PostalInfoes { get; set; }
        public virtual DbSet<StateRegion> StateRegions { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Temp_Doctor> Temp_Doctor { get; set; }
        public virtual DbSet<Temp_DoctorOrg> Temp_DoctorOrg { get; set; }
        public virtual DbSet<Temp_Emails> Temp_Emails { get; set; }
        public virtual DbSet<Temp_Org> Temp_Org { get; set; }
        public virtual DbSet<Temp_OrgAddress> Temp_OrgAddress { get; set; }
    }
}
