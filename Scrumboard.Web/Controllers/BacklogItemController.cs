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
    public class BacklogItemController : BaseController
    {
        private ScrumboardDB db = new ScrumboardDB();

        //
        // GET: /BacklogItem/

        public ActionResult Index()
        {
            var backlogItems = db.BacklogItems.Include(s => s.Team).Include(s => s.BusinessValue).Include(s => s.Release).Include(s => s.Sprint).Include(s => s.Feature).Include(s => s.BacklogItemStatus).Include(s => s.Project).Where(x => x.ProjectId == ProjectId);
            return View(backlogItems.ToList());
        }

        //
        // GET: /BacklogItem/FeatureBacklogItems
        public ActionResult FeatureBacklogItems(int featureId)
        {
            var backlogItems = db.BacklogItems.Include(s => s.Team).Include(s => s.BusinessValue).Include(s => s.Release).Include(s => s.Sprint).Include(s => s.Feature).Include(s => s.BacklogItemStatus).Include(s => s.Project).Where(x => x.ProjectId == ProjectId).Where(x => x.FeatureId == featureId);
            return PartialView(backlogItems.ToList());
        }

        ////
        //// GET: /BacklogItem/Details/5

        public ActionResult Details(int id = 0)
        {
            BacklogItem backlogItem = db.BacklogItems.Find(id);
            if (backlogItem == null)
            {
                return HttpNotFound();
            }
            return View(backlogItem);
        }

        ////
        //// GET: /BacklogItem/Create

        public ActionResult Create(int backlogItemTypeId)
        {
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name");
            ViewBag.BusinessValueId = new SelectList(db.BusinessValues, "Id", "Title");
            ViewBag.ReleaseId = new SelectList(db.Releases.Where(x => x.ProjectId == ProjectId), "Id", "Title");
            ViewBag.SprintId = new SelectList(db.Sprints.Where(x => x.ProjectId == ProjectId), "Id", "Title");
            ViewBag.FeatureId = new SelectList(db.Features.Where(x => x.ProjectId == ProjectId), "Id", "Title");
            ViewBag.BacklogItemStatusId = new SelectList(db.BacklogItemStatus, "Id", "Title", 1);
            ViewBag.BacklogItemTypeId = backlogItemTypeId;

            return View();
        }

        ////
        //// POST: /BacklogItem/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BacklogItem backlogItem)
        {
            if (ModelState.IsValid)
            {
                db.BacklogItems.Add(backlogItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", backlogItem.TeamId);
            ViewBag.BusinessValueId = new SelectList(db.BusinessValues, "Id", "Title", backlogItem.BusinessValueId);
            ViewBag.ReleaseId = new SelectList(db.Releases.Where(x => x.ProjectId == ProjectId), "Id", "Title", backlogItem.ReleaseId);
            ViewBag.SprintId = new SelectList(db.Sprints.Where(x => x.ProjectId == ProjectId), "Id", "Title", backlogItem.SprintId);
            ViewBag.FeatureId = new SelectList(db.Features.Where(x => x.ProjectId == ProjectId), "Id", "Title", backlogItem.FeatureId);
            ViewBag.BacklogItemStatusId = new SelectList(db.BacklogItemStatus, "Id", "Title", backlogItem.BacklogItemStatusId);

            return View(backlogItem);
        }

        ////
        //// GET: /BacklogItem/Edit/5

        public ActionResult Edit(int id = 0)
        {
            BacklogItem backlogItem = db.BacklogItems.Find(id);
            if (backlogItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", backlogItem.TeamId);
            ViewBag.BusinessValueId = new SelectList(db.BusinessValues, "Id", "Title", backlogItem.BusinessValueId);
            ViewBag.ReleaseId = new SelectList(db.Releases.Where(x => x.ProjectId == ProjectId), "Id", "Title", backlogItem.ReleaseId);
            ViewBag.SprintId = new SelectList(db.Sprints.Where(x => x.ProjectId == ProjectId), "Id", "Title", backlogItem.SprintId);
            ViewBag.FeatureId = new SelectList(db.Features.Where(x => x.ProjectId == ProjectId), "Id", "Title", backlogItem.FeatureId);
            ViewBag.BacklogItemStatusId = new SelectList(db.BacklogItemStatus, "Id", "Title", backlogItem.BacklogItemStatusId);
            return View(backlogItem);
        }

        ////
        //// POST: /BacklogItem/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BacklogItem backlogItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(backlogItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", backlogItem.TeamId);
            ViewBag.BusinessValueId = new SelectList(db.BusinessValues, "Id", "Title", backlogItem.BusinessValueId);
            ViewBag.ReleaseId = new SelectList(db.Releases.Where(x => x.ProjectId == ProjectId), "Id", "Title", backlogItem.ReleaseId);
            ViewBag.SprintId = new SelectList(db.Sprints.Where(x => x.ProjectId == ProjectId), "Id", "Title", backlogItem.SprintId);
            ViewBag.FeatureId = new SelectList(db.Features.Where(x => x.ProjectId == ProjectId), "Id", "Title", backlogItem.FeatureId);
            ViewBag.BacklogItemStatusId = new SelectList(db.BacklogItemStatus, "Id", "Title", backlogItem.BacklogItemStatusId);
            return View(backlogItem);
        }

        ////
        //// GET: /BacklogItem/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BacklogItem backlogItem = db.BacklogItems.Find(id);
            if (backlogItem == null)
            {
                return HttpNotFound();
            }
            return View(backlogItem);
        }

        //
        // POST: /BacklogItem/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BacklogItem backlogItem = db.BacklogItems.Find(id);
            db.BacklogItems.Remove(backlogItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}