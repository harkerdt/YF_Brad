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
using Newtonsoft.Json.Linq;

namespace YF_Brad.Controllers
{
    public class DoctorsController : Controller
    {
        private YF_NEWEntities db = new YF_NEWEntities();

        // GET: Doctors
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? pageLength)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var doctors = db.Doctors.Include(d => d.JobTitle);
            if (!String.IsNullOrEmpty(searchString))
            {
                doctors = doctors.Where(s => s.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    doctors = doctors.OrderByDescending(s => s.FirstName);
                    break;
                case "Date":
                    doctors = doctors.OrderBy(s => s.CreatedDate);
                    break;
                case "date_desc":
                    doctors = doctors.OrderByDescending(s => s.CreatedDate);
                    break;
                default:
                    doctors = doctors.OrderBy(s => s.FirstName);
                    break;
            }
            int pageSize = (pageLength ?? 10);
            int pageNumber = (page ?? 1);


            return View(doctors.ToPagedList(pageNumber, pageSize));
        }

        // GET: Doctors/Details/5
        public ActionResult Details(int? id)
        {
            string URL = "https://npiregistry.cms.hhs.gov/api";
            string url;
            string urlParameters = "?number=";
            string json;
            dynamic jsonData;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }

            //Generate api url with given name
            urlParameters = "?first_name=" + doctor.FirstName + "&last_name=" + doctor.LastName + "&state=VA";
            url = URL + urlParameters;
            try
            {
                json = new WebClient().DownloadString(url);
            }
            catch (WebException)
            {
                //Handles server errors
                return null;
            }
            //Parse json string and put info int dynamic variable "jsonData"
            jsonData = JObject.Parse(json);
            if (jsonData.Errors != null)
            {
                return null;
            }
            else if (jsonData.results.Count == 1)
            {
                List<string> results = new List<string>();
                if (jsonData.results[0].taxonomies.Count >= 1)
                {
                    foreach (var ln in jsonData.results[0].taxonomies)
                    {
                        string license = ln.license.ToString();
                        results.Add(license);
                    }
                }
                else
                {
                    List<string> nr = new List<string>
                        {
                            "NR"
                        };
                    ViewBag.findNameMLN = nr;
                }
                ViewBag.findNameMLN = results;
            }
            else if (jsonData.results.Count > 1)
            {
                List<string> tmr = new List<string>
                    {
                        "TMR"
                    };
                ViewBag.findNameMLN = tmr;
            }
            else
            {
                List<string> nr = new List<string>
                    {
                        "NR"
                    };
                ViewBag.findNameMLN = nr;
            }

            ViewBag.FirstName = doctor.FirstName.ToString();
            ViewBag.LastName = doctor.LastName.ToString();
            ViewBag.LicNum = doctor.LicNum.ToString();

            return View(doctor);
        }

        // GET: Doctors/Create
        public ActionResult Create()
        {
            ViewBag.JobID = new SelectList(db.JobTitles, "ID", "Name");
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LastName,FirstName,JobID,LicNum,Active,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Doctors.Add(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobID = new SelectList(db.JobTitles, "ID", "Name", doctor.JobID);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobID = new SelectList(db.JobTitles, "ID", "Name", doctor.JobID);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LastName,FirstName,JobID,LicNum,Active,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobID = new SelectList(db.JobTitles, "ID", "Name", doctor.JobID);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            db.Doctors.Remove(doctor);
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

static class LevenshteinDistance
{
    /// <summary>
    /// Compute the distance between two strings.
    /// </summary>
    public static int Compute(string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        // Step 1
        if (n == 0)
        {
            return m;
        }

        if (m == 0)
        {
            return n;
        }

        // Step 2
        for (int i = 0; i <= n; d[i, 0] = i++)
        {
        }

        for (int j = 0; j <= m; d[0, j] = j++)
        {
        }

        // Step 3
        for (int i = 1; i <= n; i++)
        {
            //Step 4
            for (int j = 1; j <= m; j++)
            {
                // Step 5
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                // Step 6
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }
        // Step 7
        return d[n, m];
    }
}
