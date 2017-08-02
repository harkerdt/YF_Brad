using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YF_Brad.Models;

namespace YF_Brad.Controllers
{
    public class OrgAddressesController : Controller
    {
        private YF_NEWEntities db = new YF_NEWEntities();

        // GET: OrgAddresses
        public ActionResult Index()
        {
            var orgAddresses = db.OrgAddresses.Include(o => o.AddType).Include(o => o.PostalInfo).Include(o => o.Organization);
            return View(orgAddresses.ToList());
        }

        // GET: OrgAddresses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrgAddress orgAddress = db.OrgAddresses.Find(id);
            if (orgAddress == null)
            {
                return HttpNotFound();
            }
            return View(orgAddress);
        }

        // GET: OrgAddresses/Create
        public ActionResult Create()
        {
            ViewBag.AddTypeID = new SelectList(db.AddTypes, "ID", "Name");
            ViewBag.PostalID = new SelectList(db.PostalInfoes, "ID", "City");
            ViewBag.OrgID = new SelectList(db.Organizations, "ID", "Name");
            return View();
        }

        // POST: OrgAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OrgID,AddTypeID,FirstName,LastName,Address,Address2,PostalID,Longitude,Latitude,Active,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] OrgAddress orgAddress)
        {
            if (ModelState.IsValid)
            {
                db.OrgAddresses.Add(orgAddress);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AddTypeID = new SelectList(db.AddTypes, "ID", "Name", orgAddress.AddTypeID);
            ViewBag.PostalID = new SelectList(db.PostalInfoes, "ID", "City", orgAddress.PostalID);
            ViewBag.OrgID = new SelectList(db.Organizations, "ID", "Name", orgAddress.OrgID);
            return View(orgAddress);
        }

        // GET: OrgAddresses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrgAddress orgAddress = db.OrgAddresses.Find(id);
            if (orgAddress == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddTypeID = new SelectList(db.AddTypes, "ID", "Name", orgAddress.AddTypeID);
            ViewBag.PostalID = new SelectList(db.PostalInfoes, "ID", "City", orgAddress.PostalID);
            ViewBag.OrgID = new SelectList(db.Organizations, "ID", "Name", orgAddress.OrgID);
            return View(orgAddress);
        }

        // POST: OrgAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrgID,AddTypeID,FirstName,LastName,Address,Address2,PostalID,Longitude,Latitude,Active,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] OrgAddress orgAddress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orgAddress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AddTypeID = new SelectList(db.AddTypes, "ID", "Name", orgAddress.AddTypeID);
            ViewBag.PostalID = new SelectList(db.PostalInfoes, "ID", "City", orgAddress.PostalID);
            ViewBag.OrgID = new SelectList(db.Organizations, "ID", "Name", orgAddress.OrgID);
            return View(orgAddress);
        }

        // GET: OrgAddresses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrgAddress orgAddress = db.OrgAddresses.Find(id);
            if (orgAddress == null)
            {
                return HttpNotFound();
            }
            return View(orgAddress);
        }

        // POST: OrgAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrgAddress orgAddress = db.OrgAddresses.Find(id);
            db.OrgAddresses.Remove(orgAddress);
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
