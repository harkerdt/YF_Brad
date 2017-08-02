using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YF_Brad.Models;
using PagedList;
using System.Data.Entity.Validation;

namespace YF_Brad.Controllers
{
    public class OrganizationsController : Controller
    {
        private YF_NEWEntities db = new YF_NEWEntities();

        // GET: Organizations
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

            var organizations = db.Organizations.Include(o => o.OrgType).Include(o => o.OrgAddresses);
            if (!String.IsNullOrEmpty(searchString))
            {
                organizations = organizations.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "orgtype":
                    organizations = organizations.OrderBy(s => s.OrgType.Name);
                    break;
                case "name_desc":
                    organizations = organizations.OrderByDescending(s => s.Name);
                    break;
                default:
                    organizations = organizations.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = (pageLength ?? 10);
            int pageNumber = (page ?? 1);


            return View(organizations.ToPagedList(pageNumber, pageSize));
        }

        // GET: Organizations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = db.Organizations.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }

        // GET: Organizations/Create
        public ActionResult Create()
        {
            ViewBag.OrgTypeID = new SelectList(db.OrgTypes, "ID", "Name");
            return View();
        }

        // POST: Organizations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Phone,Fax,OrgTypeID,Active,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                db.Organizations.Add(organization);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrgTypeID = new SelectList(db.OrgTypes, "ID", "Name", organization.OrgTypeID);
            return View(organization);
        }

        // GET: Organizations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = db.Organizations.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrgTypeID = new SelectList(db.OrgTypes, "ID", "Name", organization.OrgTypeID);
            return View(organization);
        }

        // POST: Organizations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Phone,Fax,OrgTypeID,Active,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] Organization organization, OrgAddress orgAddress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(organization).State = EntityState.Modified;
                
                OrgAddress data = db.OrgAddresses.Where(x => x.ID == orgAddress.ID).First();

                data.ID = orgAddress.ID;
                data.Address = orgAddress.Address;
                data.Address2 = orgAddress.Address2;
                data.Active = orgAddress.Active;
                data.FirstName = orgAddress.FirstName;
                data.LastName = orgAddress.LastName;
                data.Latitude = orgAddress.Latitude;
                data.Longitude = orgAddress.Longitude;
                data.AddTypeID = orgAddress.AddTypeID;
                data.OrgID = orgAddress.OrgID;
                data.PostalID = orgAddress.PostalID;
                data.CreatedBy = orgAddress.CreatedBy;
                data.CreatedDate = orgAddress.CreatedDate;
                data.ModifiedBy = orgAddress.ModifiedBy;
                data.ModifiedDate = orgAddress.ModifiedDate;

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                return RedirectToAction("Index");
            }
            ViewBag.OrgTypeID = new SelectList(db.OrgTypes, "ID", "Name", organization.OrgTypeID);
            return View(organization);
        }

        // GET: Organizations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = db.Organizations.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }

        // POST: Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Organization organization = db.Organizations.Find(id);
            db.Organizations.Remove(organization);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
