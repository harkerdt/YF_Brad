using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YF_Brad.Models;

namespace YF_Brad.Controllers
{
    public class PostalInfoController : Controller
    {
        YF_NEWEntities db = new YF_NEWEntities();

        // GET: PostalInfo
        public ActionResult ViewPostalList(int id)
        {
            ViewBag.Zip = id;
            var model = db.PostalInfoes.Where(r =>  r.Zip == id ).ToList();
            return PartialView("_PostalList", model.ToList());
        }

        // GET: PostalInfo
        public ActionResult ViewBusinessPostalList(int id)
        {
            ViewBag.Zip = id;
            var model = db.PostalInfoes.Where(r => r.Zip == id).ToList();
            return PartialView("_BusinessPostalList", model.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if(db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}