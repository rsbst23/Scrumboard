using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scrumboard.Web.DAL;

namespace Scrumboard.Web.Controllers
{
    public class FeatureController : BaseController
    {
        private ScrumboardDB db = new ScrumboardDB();

        //
        // GET: /Feature/

        public ActionResult Index()
        {
            return View(db.Features.Include(x => x.Release).Include(x => x.BusinessValue).Where(x => x.ProjectId == ProjectId).ToList());
        }

        //
        // GET: /Feature/Details/5

        public ActionResult Details(int id = 0)
        {
            Feature feature = db.Features.Find(id);
            if (feature == null)
            {
                return HttpNotFound();
            }
            return View(feature);
        }

        //
        // GET: /Feature/Create

        public ActionResult Create()
        {
            PopulateReleaseDropDownList();
            PopulateBusinessValueDropDownList();

            return View();
        }

        //
        // POST: /Feature/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Feature feature)
        {
            if (ModelState.IsValid)
            {
                db.Features.Add(feature);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateReleaseDropDownList();
            PopulateBusinessValueDropDownList();

            return View(feature);
        }

        //
        // GET: /Feature/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Feature feature = db.Features.Find(id);
            if (feature == null)
            {
                return HttpNotFound();
            }

            PopulateReleaseDropDownList(feature.ReleaseId);
            PopulateBusinessValueDropDownList(feature.BusinessValueId);

            return View(feature);
        }

        //
        // POST: /Feature/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Feature feature)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feature).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateReleaseDropDownList(feature.ReleaseId);
            PopulateBusinessValueDropDownList(feature.BusinessValueId);

            return View(feature);
        }

        //
        // GET: /Feature/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Feature feature = db.Features.Find(id);
            if (feature == null)
            {
                return HttpNotFound();
            }
            return View(feature);
        }

        //
        // POST: /Feature/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Feature feature = db.Features.Find(id);
            db.Features.Remove(feature);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void PopulateReleaseDropDownList(object selectedRelease = null)
        {
            ViewBag.ReleaseId = new SelectList(db.Releases.Where(x => x.ProjectId == ProjectId).ToList(), "Id", "Title", selectedRelease);
        }

        private void PopulateBusinessValueDropDownList(object selectedBusinessValue = null)
        {
            ViewBag.BusinessValueId = new SelectList(db.BusinessValues.ToList(), "Id", "Title", selectedBusinessValue);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}