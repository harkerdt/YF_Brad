using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YF_Brad.Models;
using YF_Brad.Models.ViewModels;
using System.Data.Entity.Validation;
using System.Diagnostics;
using PagedList;

namespace YF_Brad.Controllers
{
    public class CustomOrganizationController : Controller
    {
        private YF_NEWEntities db = new YF_NEWEntities();

        public ActionResult PostalInfo()
        {
            var model = new PostalInfo();

            return View();
        }


        // GET: CusotmOrganization
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? pageLength)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.OrgTypeSortParm = sortOrder == "Org Type" ? "orgtype" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            List<OrgAddView> OrgAddViewList = new List<OrgAddView>();
            var organizationlist = (from Org in db.Organizations.Include(o => o.OrgType)
                                    join Add in db.OrgAddresses on Org.ID equals Add.OrgID
                                    where Add.AddTypeID == 1
                                    select new { Org.ID, Org.Name, Org.Phone, Org.Fax, OrgType = Org.OrgType.Name, AddID = Add.ID, Add.AddTypeID, Add.AddType, Add.FirstName, Add.LastName, Add.Address, Add.Address2, Add.PostalID, Add.PostalInfo, Add.Longitude, Add.Latitude, Org.Active, Org.CreatedBy, Org.CreatedDate, Org.ModifiedBy, Org.ModifiedDate }).ToList();

            var finalList = (from org in organizationlist
                                 join add in db.OrgAddresses on org.ID equals add.OrgID
                                 where add.AddTypeID == 2
                                 select new { org.ID, org.Name, org.Phone, org.Fax, OrgType = org.OrgType, org.AddID, PAddTypeID = org.AddTypeID, org.AddType, add.FirstName, add.LastName, PAddress = org.Address, PAddress2 = org.Address2, PPostalID = org.PostalID, PPostalInfo = org.PostalInfo, PLongitude = org.Longitude, PLatitude = org.Latitude, org.Active, BAddress = add.Address, BAddress2 = add.Address2, BPostalID = add.PostalID, BPostalInfo = add.PostalInfo, BLongitude = add.Longitude, BLatitude = add.Latitude, org.CreatedBy, org.CreatedDate, org.ModifiedBy, org.ModifiedDate }).ToList();
            foreach (var item in finalList)
            {
                Debug.WriteLine("AddId - " + item.AddID);
                OrgAddView objoav = new OrgAddView()
                {
                    ID = item.ID,
                    AddID = item.AddID,
                    Name = item.Name,
                    Phone = item.Phone,
                    Fax = item.Fax,
                    OrgType = item.OrgType,
                    AddType = item.AddType.Name,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Address = item.PAddress,
                    Address2 = item.PAddress2,
                    PostalID = item.PPostalID,
                    City = item.PPostalInfo.City,
                    County = item.PPostalInfo.County,
                    State = item.PPostalInfo.State,
                    Zip = item.PPostalInfo.Zip,
                    Longitude = item.PLongitude,
                    Latitude = item.PLatitude,
                    BillingAddress = item.BAddress,
                    BillingAddress2 = item.BAddress2,
                    BillingPostalID = item.BPostalID,
                    BillingCity = item.BPostalInfo.City,
                    BillingCounty = item.BPostalInfo.County,
                    BillingState = item.BPostalInfo.State,
                    BillingZip = item.BPostalInfo.Zip,
                    BillingLongitude = item.BLongitude,
                    BillingLatitude = item.BLatitude,
                    Active = item.Active,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate
                };
                OrgAddViewList.Add(objoav);
            }

            IEnumerable<OrgAddView> results = new List<OrgAddView>();

            switch (sortOrder)
            {
                case "orgtype":
                    results = OrgAddViewList.OrderBy(s => s.OrgType);
                    break;
                case "name_desc":
                    results = OrgAddViewList.OrderByDescending(s => s.Name);
                    break;
                default:
                    results = OrgAddViewList.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = (pageLength ?? 10);
            int pageNumber = (page ?? 1);

            return View(results.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var organizationlist = (from Org in db.Organizations.Include(o => o.OrgType)
                                    join Add in db.OrgAddresses on Org.ID equals Add.OrgID
                                    where Add.AddTypeID == 1
                                    where Org.ID == id
                                    select new { Org.ID, Org.Name, Org.Phone, Org.Fax, OrgType = Org.OrgType.Name, AddID = Add.ID, Add.AddTypeID, Add.AddType, Add.FirstName, Add.LastName, Add.Address, Add.Address2, Add.PostalID, Add.PostalInfo, Add.Longitude, Add.Latitude, Org.Active, Org.CreatedBy, Org.CreatedDate, Org.ModifiedBy, Org.ModifiedDate }).ToList();

            var item = (from org in organizationlist
                             join add in db.OrgAddresses on org.ID equals add.OrgID
                             where add.AddTypeID == 2
                        select new { org.ID, org.Name, org.Phone, org.Fax, OrgType = org.OrgType, org.AddID, PAddTypeID = org.AddTypeID, org.AddType, add.FirstName, add.LastName, PAddress = org.Address, PAddress2 = org.Address2, PPostalID = org.PostalID, PPostalInfo = org.PostalInfo, PLongitude = org.Longitude, PLatitude = org.Latitude, org.Active, BAddress = add.Address, BAddress2 = add.Address2, BPostalID = add.PostalID, BPostalInfo = add.PostalInfo, BLongitude = add.Longitude, BLatitude = add.Latitude, org.CreatedBy, org.CreatedDate, org.ModifiedBy, org.ModifiedDate }).First();

            //var organization = db.Organizations.Find(id);
            //var orgAddress = (from Add in db.OrgAddresses
            //                  where Add.OrgID == id
            //                  select new { Add.ID, Add.OrgID, Add.AddTypeID, Add.AddType, Add.FirstName, Add.LastName, Add.Address, Add.Address2, Add.PostalID, Add.Longitude, Add.Latitude, Add.Active, Add.CreatedBy, Add.CreatedDate, Add.ModifiedBy, Add.ModifiedDate }).ToList().First();
            if(item == null)
            {
                return HttpNotFound();
            }
            OrgAddView orgAddView = new OrgAddView()
            {
                ID = item.ID,
                AddID = item.AddID,
                Name = item.Name,
                Phone = item.Phone,
                Fax = item.Fax,
                OrgType = item.OrgType,
                AddType = item.AddType.Name,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Address = item.PAddress,
                Address2 = item.PAddress2,
                PostalID = item.PPostalID,
                City = item.PPostalInfo.City,
                County = item.PPostalInfo.County,
                State = item.PPostalInfo.State,
                Zip = item.PPostalInfo.Zip,
                Longitude = item.PLongitude,
                Latitude = item.PLatitude,
                BillingAddress = item.BAddress,
                BillingAddress2 = item.BAddress2,
                BillingPostalID = item.BPostalID,
                BillingCity = item.BPostalInfo.City,
                BillingCounty = item.BPostalInfo.County,
                BillingState = item.BPostalInfo.State,
                BillingZip = item.BPostalInfo.Zip,
                BillingLongitude = item.BLongitude,
                BillingLatitude = item.BLatitude,
                Active = item.Active,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                ModifiedBy = item.ModifiedBy,
                ModifiedDate = item.ModifiedDate
            };
            return View(orgAddView);
        }

        public ActionResult Edit(int id)
        {
            

            var organizationlist = (from Org in db.Organizations.Include(o => o.OrgType)
                                    join Add in db.OrgAddresses on Org.ID equals Add.OrgID
                                    where Add.AddTypeID == 1
                                    where Org.ID == id
                                    select new { Org.ID, Org.Name, Org.Phone, Org.Fax, Org.OrgTypeID, OrgType = Org.OrgType, AddID = Add.ID, Add.AddTypeID, Add.AddType, Add.FirstName, Add.LastName, Add.Address, Add.Address2, Add.PostalID, Add.PostalInfo, Add.Longitude, Add.Latitude, Org.Active, Org.CreatedBy, Org.CreatedDate, Org.ModifiedBy, Org.ModifiedDate }).ToList();

            var item = (from org in organizationlist
                        join add in db.OrgAddresses on org.ID equals add.OrgID
                        where add.AddTypeID == 2
                        select new { org.ID, org.Name, org.Phone, org.Fax, org.OrgTypeID, OrgType = org.OrgType, org.AddID, PAddTypeID = org.AddTypeID, org.AddType, add.FirstName, add.LastName, PAddress = org.Address, PAddress2 = org.Address2, PPostalID = org.PostalID, PPostalInfo = org.PostalInfo, PLongitude = org.Longitude, PLatitude = org.Latitude, org.Active, BAddress = add.Address, BAddress2 = add.Address2, BPostalID = add.PostalID, BPostalInfo = add.PostalInfo, BLongitude = add.Longitude, BLatitude = add.Latitude, org.CreatedBy, org.CreatedDate, org.ModifiedBy, org.ModifiedDate }).First();
            OrgAddView orgAddView = new OrgAddView()
            {
                ID = item.ID,
                AddID = item.AddID,
                Name = item.Name,
                Phone = item.Phone,
                Fax = item.Fax,
                OrgType = item.OrgType.Name,
                OrgTypeID = item.OrgTypeID,
                AddType = item.AddType.Name,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Address = item.PAddress,
                Address2 = item.PAddress2,
                PostalID = item.PPostalID,
                City = item.PPostalInfo.City,
                County = item.PPostalInfo.County,
                State = item.PPostalInfo.State,
                Zip = item.PPostalInfo.Zip,
                Longitude = item.PLongitude,
                Latitude = item.PLatitude,
                BillingAddress = item.BAddress,
                BillingAddress2 = item.BAddress2,
                BillingPostalID = item.BPostalID,
                BillingCity = item.BPostalInfo.City,
                BillingCounty = item.BPostalInfo.County,
                BillingState = item.BPostalInfo.State,
                BillingZip = item.BPostalInfo.Zip,
                BillingLongitude = item.BLongitude,
                BillingLatitude = item.BLatitude,
                Active = item.Active,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                ModifiedBy = item.ModifiedBy,
                ModifiedDate = item.ModifiedDate
            };
            ViewBag.OrgTypeID = new SelectList(db.OrgTypes, "ID", "Name", orgAddView.OrgTypeID);
            return View(orgAddView);
        }

        // POST: Organizations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrgAddView orgAddView)
        {
            if (ModelState.IsValid)
            {
                Organization organization = db.Organizations.Where(x => x.ID == orgAddView.ID).First();
                OrgAddress orgAddress = db.OrgAddresses.Where(x => x.OrgID == orgAddView.ID && x.AddTypeID == 1).First();
                OrgAddress orgBillAddress = db.OrgAddresses.Where(x => x.OrgID == orgAddView.ID && x.AddTypeID == 2).First();
                OrgType orgtype = db.OrgTypes.Where(x => x.ID == orgAddView.OrgTypeID).First();
                

                organization.Name = orgAddView.Name;
                organization.Active = orgAddView.Active;
                organization.CreatedBy = orgAddView.CreatedBy;
                organization.CreatedDate = orgAddView.CreatedDate;
                organization.Fax = orgAddView.Fax;
                organization.ModifiedBy = orgAddView.ModifiedBy;
                organization.ModifiedDate = orgAddView.ModifiedDate;
                organization.Phone = orgAddView.Phone;
                organization.OrgTypeID = orgtype.ID;

                orgAddress.Address = orgAddView.Address;
                orgAddress.Address2 = orgAddView.Address2;

                orgBillAddress.Address = orgAddView.BillingAddress;
                orgBillAddress.Address2 = orgAddView.BillingAddress2;
                orgBillAddress.FirstName = orgAddView.BillingFirstName;
                orgBillAddress.LastName = orgAddView.BillingLastName;

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(orgAddView);
        }

        // GET: Organizations/Create
        public ActionResult Create()
        {
            ViewBag.OrgTypeID = new SelectList(db.OrgTypes, "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrgAddView orgAddView)
        {
                OrgType orgtype = db.OrgTypes.Where(x => x.ID == orgAddView.OrgTypeID).First();

                db.Organizations.Add(new Organization
                {
                    Name = orgAddView.Name,
                    Phone = orgAddView.Phone,
                    Fax = orgAddView.Fax,
                    OrgTypeID = orgtype.ID,
                    Active = orgAddView.Active,
                    CreatedBy = orgAddView.CreatedBy,
                    CreatedDate = orgAddView.CreatedDate
                });

                db.SaveChanges();

                

                db.OrgAddresses.Add(new OrgAddress
                {
                    OrgID = orgAddView.OrgID,
                    AddTypeID = 1,
                    Address = orgAddView.Address,
                    Address2 = orgAddView.Address2,
                    PostalID = orgAddView.PostalID,
                    Active = orgAddView.Active,
                    CreatedBy = orgAddView.CreatedBy,
                    CreatedDate = orgAddView.CreatedDate
                });

                db.SaveChanges();

                

                db.OrgAddresses.Add(new OrgAddress
                {
                    OrgID = orgAddView.OrgID,
                    AddTypeID = 2,
                    FirstName = orgAddView.BillingFirstName,
                    LastName = orgAddView.BillingLastName,
                    Address = orgAddView.BillingAddress,
                    Address2 = orgAddView.BillingAddress2,
                    PostalID = orgAddView.PostalID,
                    Active = orgAddView.Active,
                    CreatedBy = orgAddView.CreatedBy,
                    CreatedDate = orgAddView.CreatedDate
                });
                db.SaveChanges();

                Debug.WriteLine("jdklasfh");
                return RedirectToAction("Index");
        }
    }
}